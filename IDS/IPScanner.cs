using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IPScanner : Component
{

    public IPScanner()
    {
        this.config = new NetConf();
    }
    public override void Start(params object[] parameters)
    {
        Console.WriteLine("IP Scanner started");
    }

    public override void Stop()
    {
        Console.WriteLine("IP Scanner stopped");
    }

    public override void Configure()
    {
        this.config.FilePath = "NetDevice.json";
        this.config.SelectNetworkDevice();
    }
}
