using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct PGEventResultDestroy
    {
        [NativeTypeName("PGresult *")]
        public pg_result* result;
    }
}
