using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;

public abstract class Component
{
    protected NetConf config;

    public Component()
    {
        this.config = new NetConf();
    }

    public abstract void Start(params object[] parameters);

    public abstract void Stop();

    public abstract void Configure();
}