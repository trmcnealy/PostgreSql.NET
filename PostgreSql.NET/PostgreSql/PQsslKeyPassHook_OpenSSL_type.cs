using System.Runtime.InteropServices;

namespace PostgreSql
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int PQsslKeyPassHook_OpenSSL_type([NativeTypeName("char *")] utf8string buf, int size, [NativeTypeName("PGconn *")] pg_conn* conn);
}
