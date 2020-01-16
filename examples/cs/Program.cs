using System;
using System.Text;

namespace Example
{
    class Program
    {
        // dotnet run . -- --boot-nodes $(cat ~/.mothra/network/enr.dat) --listen-address 127.0.0.1 --port 9002 --datadir /tmp/.artemis
        static void Main(string[] args)
        {
            string message = "Hello libp2p from .NET";
            byte[] data = Encoding.UTF8.GetBytes(message);

            MothraLibp2p mothra = new MothraLibp2p();
            mothra.Start(args);
            mothra.SendGossip("topic1", data);

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
