using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using PacketDotNet;
using PacketDotNet.Ieee80211;
using System.Net;
using System.Net.NetworkInformation;
using SharpPcap.LibPcap;
using System.Data;

public class TrafficMonitor
{
    NetConf config;

    List<Packet> packets;

    List<Packet> Packets { get => this.packets; }

    private static CaptureFileWriterDevice captureFileWriter = new CaptureFileWriterDevice("./test.txt");

    public delegate void FilterHandler(Packet packet);

    public event FilterHandler OnFilter;

    public TrafficMonitor(NetConf conf)
    {
        this.config = conf;
    }

    private void OnPacketArrival(object sender, PacketCapture e)
    {
        // var device = (ICaptureDevice)sender;
        DateTime time = e.Header.Timeval.Date;
        int len = e.Data.Length;
        RawCapture rawPacket = e.GetPacket();
        captureFileWriter.Write(rawPacket);
        Packet packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
        TcpPacket tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
        if (tcpPacket != null)
        {
            var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
            System.Net.IPAddress srcIp = ipPacket.SourceAddress;
            System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
            int srcPort = tcpPacket.SourcePort;
            int dstPort = tcpPacket.DestinationPort;
            Console.WriteLine($"TCP packet: {packet}");
            //FilterTraffic(packet);
        }
        UdpPacket udpPacket = packet.Extract<PacketDotNet.UdpPacket>();

        if (udpPacket != null)
        {
            IPPacket ipPacket = (PacketDotNet.IPPacket)udpPacket.ParentPacket;

            System.Net.IPAddress srcIp = ipPacket.SourceAddress;
            System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
            int srcPort = udpPacket.SourcePort;
            int dstPort = udpPacket.DestinationPort;
            //Console.WriteLine($"UDP packet: {udpPacket}");
        }
    }

    public void CaptureTraffic() 
    {
        config.Device.Open(DeviceModes.Promiscuous | DeviceModes.NoCaptureLocal, 1000);

        config.Device.OnPacketArrival += new PacketArrivalEventHandler(OnPacketArrival);
        captureFileWriter.Open(config.Device);

        config.Device.Filter = FilterIPv4Traffic();

        config.Device.StartCapture();
        Console.WriteLine("Capturing... Press 'Enter' to stop.");
        Console.ReadLine();
        Console.WriteLine(config.Device.Statistics.ToString());
        config.Device.StopCapture();
        config.Device.Close();
    }

    private string FilterIPv4Traffic() 
    {
        StringBuilder filter = new StringBuilder();

        foreach (Rule rule in config.RuleManager.Rules)
        {
            if (filter.Length > 0)
            {
                filter.Append(" or ");
            }

            if (rule.IP != null && rule.Port != null && rule.MACAddress != null)
            {
                filter.AppendFormat("(host {0} and port {1} and ether host {2})", rule.IP, rule.Port, rule.MACAddress);
            }
            else if (rule.IP != null && rule.Port != null)
            {
                filter.AppendFormat("(host {0} and port {1})", rule.IP, rule.Port);
            }
            else if (rule.IP != null && rule.MACAddress != null)
            {
                Console.WriteLine(rule.MACAddress);
                filter.AppendFormat("(host {0} and ether host {1})", rule.IP, rule.MACAddress);
            }
            else if (rule.Port != null && rule.MACAddress != null)
            {
                filter.AppendFormat("(port {0} and ether host {1})", rule.Port, rule.MACAddress);
            }
            else if (rule.IP != null)
            {
                filter.AppendFormat("host {0}", rule.IP);
            }
            else if (rule.Port != null)
            {
                filter.AppendFormat("port {0}", rule.Port);
            }
            else if (rule.MACAddress != null)
            {
                filter.AppendFormat("ether host {0}", rule.MACAddress);
            }
        }

        string resultFilter = filter.ToString();
        Console.WriteLine(resultFilter);
        return resultFilter;
    }


}

