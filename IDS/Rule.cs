using PacketDotNet.Lldp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

public class Rule
{
    string name;
    IPAddress? ip;
    int? port;
    PhysicalAddress? macAddress;
    bool isAllowed;
    
    public string Name { get => this.name; set { this.name = value; } }

    public IPAddress? IP
    { 
        get => this.ip; 
        set
        {
            this.ip = value;
        }
    }

    public int? Port
    {
        get => port;
        set
        {
            SetPort(value);
        }
    }

    public PhysicalAddress? MACAddress
    {
        get => this.macAddress;
        set
        {
            this.macAddress = value;
        }
    }

    public bool IsAllowed
    {
        get => this.isAllowed;
        set
        {
            this.isAllowed = value;
        }
    }

    // c-tors
    [JsonConstructor]
    public Rule(string name) : this(name, null, null, null, false) { }

    public Rule(string name, string? ip, int? port, string? mac, bool allowed)
    {
        Name = name;
        SetIPAddress(ip);
        SetPort(port);
        SetMACAddress(mac);
        SetIsAllowed(allowed);
     }

    // setters
    public void SetIPAddress(string? ipAddress)
    {
        if (ipAddress == null || ipAddress == "") return;
        this.ip = IPAddress.Parse(ipAddress);
    }

    public void SetIPAddress(IPAddress? ipAddress)
    {
        this.ip = ipAddress;
    }

    public void SetPort(int? port)
    {
        if (port <= 65535 && port > 0 || port == null)
        {
            this.port = port;
            return;
        }

        throw new Exception("Port range from 0 to 65535");
    }

    public void SetMACAddress(string? mac)
    {
        if (mac.IsNullOrEmpty())
        {
            return;
        }
        this.macAddress = PhysicalAddress.Parse(mac);
    }

    public void SetIsAllowed(bool allowed)
    {
        this.isAllowed = allowed;
    }

    // methods

    public override string ToString()
    {
        return "Is allowed: " + isAllowed + " IPAddress: " + ip + " Port: " + port + " MACAddress: " + macAddress + "\n";
    }

   
    // comparers
    public override bool Equals(object? obj)
    {
        if (this == obj) return true;
        return false;
    }

    // operator overloading 
    public static bool operator ==(Rule left, Rule right)
    {
        if (left.IP == right.IP && left.Port == right.Port && left.MACAddress == right.MACAddress) return true;
        return false;
    }

    public static bool operator !=(Rule left, Rule right)
    {
        return !(left == right);
    }
}