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
using System.Runtime.InteropServices;

namespace Example
{
    class Program
    {
        // Build the c-bindings, then run single node to check it works:
        //  make c
        //  dotnet run --project examples/dotnet

        private static GCHandle s_discoveredPeerHandle;
        private static GCHandle s_receiveGossipHandle;
        private static GCHandle s_receiveRpcHandle;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Press CTRL+C to exit");
            Start(args);
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                
                string message = string.Format("Hello libp2p from .NET {0:s}", DateTimeOffset.Now);
                Console.WriteLine($"dotnet: Sending {message}");
                
                byte[] data = Encoding.UTF8.GetBytes(message);
                
                SendGossip("/eth2/beacon_block/ssz", data);
            }
        }
        
        public static unsafe void Start(string[] args)
        {
            MothraInterop.DiscoveredPeer discoveredPeer = new MothraInterop.DiscoveredPeer(OnDiscoveredPeer);
            MothraInterop.ReceiveGossip receiveGossip = new MothraInterop.ReceiveGossip(OnReceiveGossip);
            MothraInterop.ReceiveRpc receiveRpc = new MothraInterop.ReceiveRpc(OnReceiveRpc);

            // as delegates
            s_discoveredPeerHandle = GCHandle.Alloc(discoveredPeer);
            s_receiveGossipHandle = GCHandle.Alloc(receiveGossip);            
            s_receiveRpcHandle = GCHandle.Alloc(receiveRpc);
            
            // as pointers
            //IntPtr discoveredPeerPtr = Marshal.GetFunctionPointerForDelegate(discoveredPeer);
            //IntPtr receiveGossipPtr = Marshal.GetFunctionPointerForDelegate(receiveGossip);
            //IntPtr receiveRpcPtr = Marshal.GetFunctionPointerForDelegate(receiveRpc);
            //s_discoveredPeerHandle = GCHandle.Alloc(discoveredPeerPtr, GCHandleType.Pinned);
            //s_receiveGossipHandle = GCHandle.Alloc(receiveGossipPtr, GCHandleType.Pinned);            
            //s_receiveRpcHandle = GCHandle.Alloc(receiveRpcPtr, GCHandleType.Pinned);
            
            MothraInterop.RegisterHandlers(discoveredPeer, receiveGossip, receiveRpc);
            //MothraInterop.RegisterHandlers(discoveredPeerPtr, receiveGossipPtr, receiveRpcPtr);
            MothraInterop.Start(args, args.Length);
        }
        
        public static unsafe void SendGossip(string topic, ReadOnlySpan<byte> data)
        {
            byte[] topicUtf8 = Encoding.UTF8.GetBytes(topic);
            fixed (byte* topicUtf8Ptr = topicUtf8)
            fixed (byte* dataPtr = data)
            {
                MothraInterop.SendGossip(topicUtf8Ptr, topicUtf8.Length, dataPtr, data.Length);
            }
        }

        private static unsafe void OnDiscoveredPeer(byte* peerUtf8, int peerLength)
        {
            Console.Write("dotnet: peer");
            string peer = new String((sbyte*)peerUtf8, 0, peerLength, Encoding.UTF8);
            Console.WriteLine($" discovered {peer}");
        }

        private static unsafe void OnReceiveGossip(byte* topicUtf8, int topicLength, byte* data, int dataLength)
        {
            Console.Write("dotnet: receive");
            string topic = new String((sbyte*)topicUtf8, 0, topicLength, Encoding.UTF8);
            string dataString = new String((sbyte*)data, 0, dataLength, Encoding.UTF8);
            Console.WriteLine($" gossip={topic},data={dataString}");
        }

        private static unsafe void OnReceiveRpc(byte* methodUtf8, int methodLength, int requestResponseFlag, byte* peerUtf8,
            int peerLength, byte* data, int dataLength)
        {
            // Nothing
        }
    }
}
