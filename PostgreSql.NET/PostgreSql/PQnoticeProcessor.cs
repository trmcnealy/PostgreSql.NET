using System.Runtime.InteropServices;

namespace PostgreSql
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PQnoticeProcessor(nint arg, [NativeTypeName("const char *")] utf8string message);
}
