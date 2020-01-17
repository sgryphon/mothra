// Copyright 2020 Sly Gryphon
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
