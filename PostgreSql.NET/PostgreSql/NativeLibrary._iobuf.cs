using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    public static partial class NativeLibrary
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct _iobuf
        {
            public nuint _ptr;

            public int _cnt;

            public nuint _base;

            public int _flag;

            public int _file;

            public int _charbuf;

            public int _bufsiz;

            public nuint _tmpfname;
        }
    }
}