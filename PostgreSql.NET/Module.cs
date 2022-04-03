using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.System32)]

public static class Module
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        PostgreSql.NativeLibrary.Load();
    }
}