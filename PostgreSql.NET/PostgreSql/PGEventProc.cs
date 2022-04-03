using System.Runtime.InteropServices;

namespace PostgreSql
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int PGEventProc(PGEventId evtId, nint evtInfo, nint passThrough);
}
