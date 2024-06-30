using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using PacketDotNet;
using PacketDotNet.Ieee80211;
using System.Net;

public class TrafficMonitor
{
    NetConf config;

    List<Packet> packets;

    List<Packet> Packets { get => this.packets; }

    public delegate void FilterHandler(IPPacket packet);

    public event FilterHandler OnFilter;

    public TrafficMonitor(NetConf conf)
    {
        this.config = conf;
    }

    private void OnPacketArrival(object sender, PacketCapture e)
    {
        DateTime time = e.Header.Timeval.Date;
        int len = e.Data.Length;
        RawCapture rawPacket = e.GetPacket();

        Packet packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

        TcpPacket tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
        UdpPacket udpPacket = packet.Extract<PacketDotNet.UdpPacket>();

        if (tcpPacket != null)
        {
            var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
            System.Net.IPAddress srcIp = ipPacket.SourceAddress;
            System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
            int srcPort = tcpPacket.SourcePort;
            int dstPort = tcpPacket.DestinationPort;

            //Console.WriteLine("{0}:{1}:{2},{3} Len={4} {5}:{6} -> {7}:{8}",
            //    time.Hour, time.Minute, time.Second, time.Millisecond, len,
            //   srcIp, srcPort, dstIp, dstPort);

            FilterTraffic(ipPacket);
        }

        if (udpPacket != null)
        {
            IPPacket ipPacket = (PacketDotNet.IPPacket)udpPacket.ParentPacket;

            System.Net.IPAddress srcIp = ipPacket.SourceAddress;
            System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
            int srcPort = udpPacket.SourcePort;
            int dstPort = udpPacket.DestinationPort;
        }
    }

    public void CaptureTraffic() 
    {
        config.Device.Open(DeviceModes.Promiscuous, 1000);

        config.Device.OnPacketArrival += new PacketArrivalEventHandler(OnPacketArrival);

        config.Device.StartCapture();

        Console.WriteLine("Capturing... Press 'Enter' to stop.");
        Console.ReadLine();

        config.Device.StopCapture();
        config.Device.Close();
    }

    private void FilterTraffic(IPPacket packet) 
    {
        Rule.CompareByIP ipComparer = new Rule.CompareByIP();
        foreach (Rule rule in this.config.RuleManager.Rules)
        {
            if (ipComparer.Compare(rule.IP, packet.SourceAddress) == 0) Console.WriteLine(packet.ToString());
        }
    }

}

