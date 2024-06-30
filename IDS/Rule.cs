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

public class Rule : IComparable<Rule>
{
    IPAddress? ip;
    int? port;
    string? macAddress;
    bool isAllowed;

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

    public string? MACAddress
    {
        get => this.macAddress;
        set
        {
            SetMACAddress(value);
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

    public Rule() : this(null, null, null, false) { }

    public Rule(string? ip, int? port, string? mac, bool allowed)
    {
        SetIPAddress(ip);
        SetPort(port);
        SetMACAddress(mac);
        SetIsAllowed(allowed);
     }
    /*
    public Rule(IPAddress? ip, int? port, string? mac, bool allowed)
    {
        SetIPAddress(ip);
        SetPort(port);
        SetMACAddress(mac);
        SetIsAllowed(allowed);
    }*/

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
        this.macAddress = mac;
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
    public int CompareTo(Rule? other)
    {
        if (this == other) return 1;
        return -1;
    }

    public class CompareByIP : IComparer<IPAddress>
    {
        public int Compare(IPAddress? x, IPAddress? y)
        {
            if (x == null) return -1;
            return x.ToString().CompareTo(y.ToString());
        }
    }

    public class CompareByPort : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }
    }

    public class CompareByMACAddress : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            return x.CompareTo(y);
        }
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