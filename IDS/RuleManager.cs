using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class RuleManager
{
    List<Rule> rules;
    string filePath;

    public List<Rule> Rules { get => this.rules; }

    string FilePath
    {
        get => this.filePath;
        set
        {
            this.filePath = value;
        }
    }

    public RuleManager(string filePath = "rules.json")
    {
        this.rules = new List<Rule>();
        this.filePath = filePath;
        this.rules = ReadRules();
    }

    private class IPAddressConverter : JsonConverter<IPAddress>
    {
        public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var ipString = reader.GetString();
            return IPAddress.Parse(ipString);
        }

        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }


    public List<Rule> ReadRules()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new IPAddressConverter());

        string jsonString = File.ReadAllText(this.filePath);
        return JsonSerializer.Deserialize<List<Rule>>(jsonString, options);
    }

    public void WriteRules()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new IPAddressConverter());
        options.WriteIndented = true;

        string jsonString = JsonSerializer.Serialize(this.rules, options);
        File.WriteAllText(this.filePath, jsonString);
    }

    public void AddRule()
    {
        string output = "yes";

        while (output != "no")
        {
            Rule rule = new Rule();
            Console.Clear();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Set rule example:");
            Console.WriteLine("Is Allowed: true/false");
            Console.WriteLine("IPAddress: 192.168.1.0 / null");
            Console.WriteLine("Port number: 444 / null");
            Console.WriteLine("MAC Address: 00:00:00:00:00 / null");
            Console.WriteLine("------------------------------------");
            Console.Write("IscAllowed? (Yes/No)");
            output = Console.ReadLine();

            if (output.ToLower() == "yes") rule.IsAllowed = true;
            else rule.IsAllowed = false;

            Console.Write("IP Address: ");
            output = Console.ReadLine();

            rule.SetIPAddress(output);

            Console.Write("Port number: ");
            output = Console.ReadLine();

            if (output == "") rule.SetPort(null);
            else rule.SetPort(Convert.ToInt32(output));


            Console.Write("MAC Address: ");
            output = Console.ReadLine();

            rule.SetMACAddress(output);

            this.rules.Add(rule);

            WriteRules();

            Console.WriteLine("Add more rules?(yes/no)");
            output = Console.ReadLine();
        }
    }

    public void RemoveRules()
    {
        string output = "yes";
        while (output.ToLower() != "no")
        {
            int index = 0;
            ShowRules();
            Console.WriteLine("-----------------------------------");
            Console.Write("Select rule number to remove: ");
            try
            {
                index = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.FormatException)
            {
                break;
            }

            this.rules.RemoveAt(index);
            WriteRules();

            Console.Write("Remove more rules? (yes/no) ");
            output = Console.ReadLine();
        }
    }

    public void ShowRules()
    {
        int index = 0;
        foreach (Rule rule in this.rules)
        {
            Console.Write("{0} - {1}", index, rule);
            index++;
        }
    }

}