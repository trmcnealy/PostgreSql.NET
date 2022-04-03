using System.Runtime.InteropServices;

namespace PostgreSql
{
    public partial struct PQconninfoOption
    {
        [NativeTypeName("char *")]
        public utf8string keyword;

        [NativeTypeName("char *")]
        public utf8string envvar;

        [NativeTypeName("char *")]
        public utf8string compiled;

        [NativeTypeName("char *")]
        public utf8string val;

        [NativeTypeName("char *")]
        public utf8string label;

        [NativeTypeName("char *")]
        public utf8string dispchar;

        public int dispsize;
    }
}
