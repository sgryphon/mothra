using System;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        // dotnet run . -- --boot-nodes $(cat ~/.mothra/network/enr.dat) --listen-address 127.0.0.1 --port 9002 --datadir /tmp/.netcore
        static async Task Main(string[] args)
        {
            MothraLibp2p mothra = new MothraLibp2p();
            mothra.Start(args);

            Console.WriteLine("Press CTRL+C to exit");
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                string message = string.Format("Hello libp2p from .NET {0:s}", DateTimeOffset.Now);
                Console.WriteLine($"Sending {message}");
                byte[] data = Encoding.UTF8.GetBytes(message);
                mothra.SendGossip("/eth2/beacon_block/ssz", data);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
