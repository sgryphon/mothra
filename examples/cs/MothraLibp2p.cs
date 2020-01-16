using System;
using System.Text;
using System.Threading;

namespace Example
{
    public class MothraLibp2p
    {
        public void Start(string[] args)
        {
            int result = MothraInterop.Start(args, args.Length);
            if (result != 0)
            {
                throw new Exception($"Error initialising libp2p [{result}]");
            }
        }
        
        public void SendGossip(string topic, ReadOnlySpan<byte> data)
        {
            byte[] topicUtf8 = Encoding.UTF8.GetBytes(topic);
            int result;
            unsafe
            {
                fixed (byte* topicUtf8Ptr = topicUtf8)
                fixed (byte* dataPtr = data)
                {
                    result = MothraInterop.SendGossip(topicUtf8Ptr, topicUtf8.Length, dataPtr, data.Length);
                }
            }

            if (result != 0)
            {
                throw new Exception($"Error sending libp2p gossip [{result}]");
            }
        }

        public void SendRequest(string method, string peer, ReadOnlySpan<byte> data)
        {
            byte[] methodUtf8 = Encoding.UTF8.GetBytes(method);
            byte[] peerUtf8 = Encoding.UTF8.GetBytes(peer);
            int result;
            unsafe
            {
                fixed (byte* methodUtf8Ptr = methodUtf8)
                fixed (byte* peerUtf8Ptr = peerUtf8)
                fixed (byte* dataPtr = data)
                {
                    result = MothraInterop.SendRequest(methodUtf8Ptr, methodUtf8.Length, peerUtf8Ptr, peer.Length, dataPtr, data.Length);
                }
            }

            if (result != 0)
            {
                throw new Exception($"Error sending libp2p request [{result}]");
            }
        }

        public void SendResponse(string method, string peer, ReadOnlySpan<byte> data)
        {
            byte[] methodUtf8 = Encoding.UTF8.GetBytes(method);
            byte[] peerUtf8 = Encoding.UTF8.GetBytes(peer);
            int result;
            unsafe
            {
                fixed (byte* methodUtf8Ptr = methodUtf8)
                fixed (byte* peerUtf8Ptr = peerUtf8)
                fixed (byte* dataPtr = data)
                {
                    result = MothraInterop.SendResponse(methodUtf8Ptr, methodUtf8.Length, peerUtf8Ptr, peer.Length, dataPtr, data.Length);
                }
            }

            if (result != 0)
            {
                throw new Exception($"Error sending libp2p response [{result}]");
            }
        }
        
        private void EnsureInitialised()
        {
        }

    }
}