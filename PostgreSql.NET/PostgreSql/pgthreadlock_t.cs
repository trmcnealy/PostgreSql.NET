using System.Runtime.InteropServices;

namespace PostgreSql
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void pgthreadlock_t(int acquire);
}
