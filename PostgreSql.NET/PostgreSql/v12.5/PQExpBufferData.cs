using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PQExpBufferData
    {
        public nint data;
            
        public ulong len;
            
        public ulong maxlen;
    }
}