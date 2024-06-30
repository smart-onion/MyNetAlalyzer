using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class NetConf
{
    ICaptureDevice device;
    RuleManager ruleManager;
    public string FilePath { get; set; }
    public RuleManager RuleManager { get => this.ruleManager; }

    public ICaptureDevice Device
    {
        get => this.device;
        set
        {
            SelectNetworkDevice();
        }
    }

    public NetConf()
    {
        this.ruleManager = new RuleManager();
        LoadConfiguration();
    }

    public void LoadConfiguration()
    {
        LoadDeviceConfiguration("NetDevice.json");
        this.ruleManager.ReadRules();
    }

    public void LoadDeviceConfiguration(string filename)
    {
        string jsonString = File.ReadAllText(filename);
        string? deviceName = JsonSerializer.Deserialize<string>(jsonString);

        if (deviceName == null)
        {
            SelectNetworkDevice();
            return;
        }

        foreach (ICaptureDevice dev in CaptureDeviceList.Instance)
        {
            if (dev.Name == deviceName)
            {
                this.device = dev;
                return;
            }
        }

    }

    public void ConfigureRules()
    {
        int select;
        Console.WriteLine("Choose option:");
        Console.WriteLine("1 - Add rules");
        Console.WriteLine("2 - Remove rules");
        Console.WriteLine("3 - Show All Rules");

        select = Convert.ToInt32(Console.ReadLine());

        switch (select)
        {
            case 1:
                this.ruleManager.AddRule();
                break;
            case 2:
                this.ruleManager.RemoveRules();
                break;
            case 3:
                this.ruleManager.ShowRules();
                break;
            default:
                break;
        }
    }

   

    public void SelectNetworkDevice()
    {
        Console.Clear();
        CaptureDeviceList devices = CaptureDeviceList.Instance;

        if (devices.Count < 1)
        {
            Console.WriteLine("No devices were found on this machine!");
            return;
        }

        Console.WriteLine("\nThe following devices are available on this machine:");
        Console.WriteLine("----------------------------------------------------\n");

        // Print out the available network devices
        for (int i = 0; i < devices.Count; i++)
        {
            Console.WriteLine("{0} - {1}\n", i, devices[i].Description);
        }

        int select = Convert.ToInt32(Console.ReadLine());

        this.device = devices[select];

        SaveConfigurations(this.device.Name, this.FilePath);
    }

    public static void SaveConfigurations(object obj, string filename)
    {
        string jsonString = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filename, jsonString);
    }
}