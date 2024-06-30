using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using PacketDotNet;
using System.Text.Json;
public class IDS : Component
{
    // fields
    TrafficMonitor trafficMonitor;
    // Analyzer analyzer;
    // AlertManager alertManager;

    // c-tor
    public IDS() 
    {
        trafficMonitor = new TrafficMonitor(this.config);
    }

    // methods

    

    public override void Start(params object[] param)
    {
        Console.Clear();
        config.LoadConfiguration();
        Console.WriteLine("IDS started");
        trafficMonitor.CaptureTraffic();

    }

    public override void Stop() 
    {
        Console.WriteLine("IDS stopped");
    }

    public override void Configure()
    {
        this.config.FilePath = "NetDevice.json";

        Console.Clear();       
        Console.WriteLine("Choose option:");
        Console.WriteLine("1 - Select Network Device");
        Console.WriteLine("2 - Configure rules");

        int select = Convert.ToInt32(Console.ReadLine());

        switch (select)
        {
            case 1:
                config.SelectNetworkDevice();
                break;
            case 2:
                config.ConfigureRules();
                break;
            default:
                break;
        }

    }

   
}

