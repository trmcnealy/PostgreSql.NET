using System.Runtime.InteropServices;

namespace PostgreSql
{
    public partial struct pgresAttDesc
    {
        [NativeTypeName("char *")]
        public utf8string name;

        [NativeTypeName("Oid")]
        public uint tableid;

        public int columnid;

        public int format;

        [NativeTypeName("Oid")]
        public uint typid;

        public int typlen;

        public int atttypmod;
    }
}
