using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct PGEventRegister
    {
        [NativeTypeName("PGconn *")]
        public pg_conn* conn;
    }
}
