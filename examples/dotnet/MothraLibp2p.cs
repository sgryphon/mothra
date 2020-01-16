using System;
using System.Text;
using System.Threading;

namespace Example
{
    public class MothraLibp2p
    {
        public void Start(string[] args)
        {
            MothraInterop.Start(args, args.Length);
        }
        
        public void SendGossip(string topic, ReadOnlySpan<byte> data)
        {
            byte[] topicUtf8 = Encoding.UTF8.GetBytes(topic);
            unsafe
            {
                fixed (byte* topicUtf8Ptr = topicUtf8)
                fixed (byte* dataPtr = data)
                {
                    MothraInterop.SendGossip(topicUtf8Ptr, topicUtf8.Length, dataPtr, data.Length);
                }
            }
        }

        public void SendRequest(string method, string peer, ReadOnlySpan<byte> data)
        {
            byte[] methodUtf8 = Encoding.UTF8.GetBytes(method);
            byte[] peerUtf8 = Encoding.UTF8.GetBytes(peer);
            unsafe
            {
                fixed (byte* methodUtf8Ptr = methodUtf8)
                fixed (byte* peerUtf8Ptr = peerUtf8)
                fixed (byte* dataPtr = data)
                {
                    MothraInterop.SendRequest(methodUtf8Ptr, methodUtf8.Length, peerUtf8Ptr, peer.Length, dataPtr,
                        data.Length);
                }
            }
        }

        public void SendResponse(string method, string peer, ReadOnlySpan<byte> data)
        {
            byte[] methodUtf8 = Encoding.UTF8.GetBytes(method);
            byte[] peerUtf8 = Encoding.UTF8.GetBytes(peer);
            unsafe
            {
                fixed (byte* methodUtf8Ptr = methodUtf8)
                fixed (byte* peerUtf8Ptr = peerUtf8)
                fixed (byte* dataPtr = data)
                {
                    MothraInterop.SendResponse(methodUtf8Ptr, methodUtf8.Length, peerUtf8Ptr, peer.Length, dataPtr,
                        data.Length);
                }
            }
        }
    }
}