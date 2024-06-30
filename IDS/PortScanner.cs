using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PortScanner : Component
{

    public override void Start(params object[] parameters)
    {
        Console.WriteLine("Port scanner started");
    }

    public override void Stop()
    {
        Console.WriteLine("Port scanner stopped");

    }

    public override void Configure()
    {
        this.config.FilePath = "NetDevice.json";
        this.config.SelectNetworkDevice();
    }

    public override string ToString()
    {
        return "Port scanner";
    }
}
