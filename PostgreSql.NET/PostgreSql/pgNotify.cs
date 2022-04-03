using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct pgNotify
    {
        [NativeTypeName("char *")]
        public utf8string relname;

        public int be_pid;

        [NativeTypeName("char *")]
        public utf8string extra;

        [NativeTypeName("struct pgNotify *")]
        public pgNotify* next;
    }
}
