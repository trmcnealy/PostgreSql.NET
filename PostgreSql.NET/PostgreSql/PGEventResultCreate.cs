using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct PGEventResultCreate
    {
        [NativeTypeName("PGconn *")]
        public pg_conn* conn;

        [NativeTypeName("PGresult *")]
        public pg_result* result;
    }
}
