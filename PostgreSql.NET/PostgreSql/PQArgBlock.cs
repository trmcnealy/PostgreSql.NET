using System.Runtime.InteropServices;

namespace PostgreSql
{
    public partial struct PQArgBlock
    {
        public int len;

        public int isint;

        [NativeTypeName("union (anonymous union at E:/Github/postgres/postgres/src/interfaces/libpq/libpq-fe.h:250:2)")]
        public _u_e__Union u;

        [StructLayout(LayoutKind.Explicit)]
        public unsafe partial struct _u_e__Union
        {
            [FieldOffset(0)]
            public int* ptr;

            [FieldOffset(0)]
            public int integer;
        }
    }
}
