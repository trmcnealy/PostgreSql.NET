using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// ----------------
    /// PQArgBlock -- structure for PQfn() arguments
    /// ----------------
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PQArgBlock
    {
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
        public struct PQArgBlock_union_u
        {
            /// <summary>
            /// can't use void (dec compiler barfs)
            /// </summary>
            [FieldOffset(0)]
            public nuint ptr;

            [FieldOffset(0)]
            public int integer;
        }

        public int len;

        public int isint;

        public PQArgBlock_union_u u;
    }
}