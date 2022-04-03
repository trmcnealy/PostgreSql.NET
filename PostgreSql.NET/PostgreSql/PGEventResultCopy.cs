using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct PGEventResultCopy
    {
        [NativeTypeName("const PGresult *")]
        public pg_result* src;

        [NativeTypeName("PGresult *")]
        public pg_result* dest;
    }
}
