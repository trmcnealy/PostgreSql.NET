using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct PGEventConnReset
    {
        [NativeTypeName("PGconn *")]
        public pg_conn* conn;
    }
}
