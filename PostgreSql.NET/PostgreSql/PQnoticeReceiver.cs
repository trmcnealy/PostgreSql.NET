using System.Runtime.InteropServices;

namespace PostgreSql
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void PQnoticeReceiver(nint arg, [NativeTypeName("const PGresult *")] pg_result* res);
}
