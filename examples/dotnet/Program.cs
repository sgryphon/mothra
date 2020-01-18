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

        // HACK: Working when debugger attached (on Linux)
        // 1. build c-bindings: make c
        // 2. run C native in one console: bin/example
        // 3. open Example.csproj in Jetbrains Rider
        // 4. set program arguments in debug configuration:
        //     -- --boot-nodes enr:-Iu4QOcRj-KivlPmJ8FNyYGCV7Kkub3j8OzMwXCL-iZijl8kEg4nz2J3xTP5ENqMr5QgExjP9bzI7hOHZuDWhOjsPcUBgmlkgnY0gmlwhH8AAAGJc2VjcDI1NmsxoQKVrVQHZsUqntitqKx6o6cQBmwvA78SzeCb8jTLcHY_iYN0Y3CCIyiDdWRwgiMo --listen-address 127.0.0.1 --port 9001 --datadir /tmp/.artemis
        //     (this should match the enr of the running C native; yes, it starts with two dashes to separate program input)
        // 5. run in debug mode; it works fine (is stable)
        // 6. run without debugger: crashes on first message
        
        private static GCHandle s_discoveredPeerHandle;
        private static GCHandle s_receiveGossipHandle;
        private static GCHandle s_receiveRpcHandle;
        
        private static MothraInterop.DiscoveredPeer s_discoveredPeer;
        private static MothraInterop.ReceiveGossip s_receiveGossip;
        private static MothraInterop.ReceiveRpc s_receiveRpc;

        private static IntPtr s_discoveredPeerPtr;
        private static IntPtr s_receiveGossipPtr;
        private static IntPtr s_receiveRpcPtr;

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
            // MothraInterop.DiscoveredPeer discoveredPeer = new MothraInterop.DiscoveredPeer(OnDiscoveredPeer);
            // MothraInterop.ReceiveGossip receiveGossip = new MothraInterop.ReceiveGossip(OnReceiveGossip);
            // MothraInterop.ReceiveRpc receiveRpc = new MothraInterop.ReceiveRpc(OnReceiveRpc);
            s_discoveredPeer = new MothraInterop.DiscoveredPeer(OnDiscoveredPeer);
            s_receiveGossip = new MothraInterop.ReceiveGossip(OnReceiveGossip);
            s_receiveRpc = new MothraInterop.ReceiveRpc(OnReceiveRpc);

            // as delegates
            // s_discoveredPeerHandle = GCHandle.Alloc(discoveredPeer);
            // s_receiveGossipHandle = GCHandle.Alloc(receiveGossip);            
            // s_receiveRpcHandle = GCHandle.Alloc(receiveRpc);
            s_discoveredPeerHandle = GCHandle.Alloc(s_discoveredPeer);
            s_receiveGossipHandle = GCHandle.Alloc(s_receiveGossip);            
            s_receiveRpcHandle = GCHandle.Alloc(s_receiveRpc);
            
            // as pointers
            // IntPtr discoveredPeerPtr = Marshal.GetFunctionPointerForDelegate(discoveredPeer);
            // IntPtr receiveGossipPtr = Marshal.GetFunctionPointerForDelegate(receiveGossip);
            // IntPtr receiveRpcPtr = Marshal.GetFunctionPointerForDelegate(receiveRpc);
            // s_discoveredPeerHandle = GCHandle.Alloc(discoveredPeerPtr, GCHandleType.Pinned);
            // s_receiveGossipHandle = GCHandle.Alloc(receiveGossipPtr, GCHandleType.Pinned);            
            // s_receiveRpcHandle = GCHandle.Alloc(receiveRpcPtr, GCHandleType.Pinned);
            
            // as static field pointers
            // s_discoveredPeerPtr = Marshal.GetFunctionPointerForDelegate(discoveredPeer);
            // s_receiveGossipPtr = Marshal.GetFunctionPointerForDelegate(receiveGossip);
            // s_receiveRpcPtr = Marshal.GetFunctionPointerForDelegate(receiveRpc);
            // s_discoveredPeerHandle = GCHandle.Alloc(s_discoveredPeerPtr, GCHandleType.Pinned);
            // s_receiveGossipHandle = GCHandle.Alloc(s_receiveGossipPtr, GCHandleType.Pinned);            
            // s_receiveRpcHandle = GCHandle.Alloc(s_receiveRpcPtr, GCHandleType.Pinned);
            
            //MothraInterop.RegisterHandlers(discoveredPeer, receiveGossip, receiveRpc);
            MothraInterop.RegisterHandlers(s_discoveredPeer, s_receiveGossip, s_receiveRpc);
            //MothraInterop.RegisterHandlers(discoveredPeerPtr, receiveGossipPtr, receiveRpcPtr);
            // MothraInterop.RegisterHandlers(s_discoveredPeerPtr, s_receiveGossipPtr, s_receiveRpcPtr);
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
