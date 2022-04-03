using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct PGEventConnDestroy
    {
        [NativeTypeName("PGconn *")]
        public pg_conn* conn;
    }
}
