using PacketDotNet;

internal class Program
{
    public static void Main(string[] args)
    {
        MyNetAnalyzer net = new MyNetAnalyzer();
        net.Run();
    }
}