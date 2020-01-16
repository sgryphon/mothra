using System.Runtime.InteropServices;

namespace Example
{
    internal static class MothraInterop
    {
        // This will search and load mothra.dll on Windows, and libmothra.so on Linux
        private const string DllName = "mothra";
        
        [DllImport(DllName, EntryPoint = "libp2p_start", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Start([In, Out] string[] args, int length);

        [DllImport(DllName, EntryPoint = "libp2p_send_gossip")]
        public static extern unsafe void SendGossip(byte* topicUtf8, int topicLength, byte* data, int dataLength);

        [DllImport(DllName, EntryPoint = "libp2p_send_rpc_request")]
        public static extern unsafe void SendRequest(byte* methodUtf8, int methodLength, byte* peerUtf8, int peerLength, byte* data, int dataLength);

        [DllImport(DllName, EntryPoint = "libp2p_send_rpc_response")]
        public static extern unsafe void SendResponse(byte* methodUtf8, int methodLength, byte* peerUtf8, int peerLength, byte* data, int dataLength);

        // Java
        //extern void libp2p_start(char**, int);
        //extern void libp2p_send_gossip(jbyte*, int, jbyte*, int);
        //extern void libp2p_send_rpc_request(jbyte*, int, jbyte*, int, jbyte*, int);
        //extern void libp2p_send_rpc_response(jbyte*, int, jbyte*, int, jbyte*, int);
        //void discovered_peer(const unsigned char*, int);
        //void receive_gossip(const unsigned char*, int, unsigned char*, int);
        //void receive_rpc(const unsigned char*, int, int, const unsigned char*, int, unsigned char*, int);

        // C
        //extern void libp2p_start(char**, int length);
        //extern void libp2p_send_gossip(unsigned char*, int length);
        //void receive_gossip(unsigned char*);

    }
}