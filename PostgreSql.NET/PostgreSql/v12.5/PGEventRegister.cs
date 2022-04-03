using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PGEventRegister
    {
        public nuint conn;
    }
        
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PGEventConnReset
    {
        public nuint conn;
    }
        
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PGEventConnDestroy
    {
        public nuint conn;
    }
        
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PGEventResultCreate
    {
        public nuint conn;
            
        public nuint result;
    }
        
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PGEventResultCopy
    {
        public nuint src;
            
        public nuint dest;
    }
        
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PGEventResultDestroy
    {
        public nuint result;
    }

}