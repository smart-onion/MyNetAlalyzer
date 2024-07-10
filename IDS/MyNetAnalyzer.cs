using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MyNetAnalyzer
{
    Component component;

    public MyNetAnalyzer() { }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("MyNetAnalyzer running");
            Console.WriteLine("----------------------------------------");
            DisplayComponents();
            SelectComponent();
            if (component != null) SelectActionOnComponent();

        }

    }

    public void DisplayComponents()
    {
        Console.Clear();
        Console.WriteLine("Available Components:");
        Console.WriteLine("1 - IDS");
        Console.WriteLine("2 - IP Scanner");
        Console.WriteLine("3 - Port Scanner");
    }

    public void SelectComponent()
    {
        Console.Write("Select component: ");
        IntegerInput select = new IntegerInput();
        switch (select)
        {
            case 1:
                this.component = new IDS();
                break;
            case 2:
                this.component = new IPScanner();
                break;
            case 3:
                this.component = new PortScanner();
                break;
            default:
                Console.WriteLine("Wrong component!");
                break;
        }
    }

    public void SelectActionOnComponent()
    {
        Console.Clear();
        Console.WriteLine("+---- {0} ----+", component);
        Console.WriteLine("1 - Start");
        Console.WriteLine("2 - Configure");
        Console.WriteLine("3 - Stop");
        Console.Write("Select an action: ");
        IntegerInput select = new IntegerInput();
        Console.WriteLine(select); 
        switch (select)
        {
            case 1:
                this.component.Start();
                break;
            case 2:
                this.component.Configure();
                break;
            case 3:
                this.component.Stop();
                break;
            default:
                break;
        }
    }
}
