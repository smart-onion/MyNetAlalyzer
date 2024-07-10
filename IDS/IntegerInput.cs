using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct IntegerInput
{
    int integer;
    public int Integer { get => this.integer; }

    public IntegerInput()
    {
        SetInteger();
    }

    public void SetInteger()
    {
        try
        {
            this.integer = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine("Wrong Input!");
            this.integer = -1;
        }
    }

    public static implicit operator int(IntegerInput intInput)
    {
        return intInput.Integer;
    }

}