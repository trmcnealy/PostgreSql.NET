using System.Runtime.InteropServices;

namespace PostgreSql
{
    public unsafe partial struct PQprintOpt
    {
        [NativeTypeName("pqbool")]
        public sbyte header;

        [NativeTypeName("pqbool")]
        public sbyte align;

        [NativeTypeName("pqbool")]
        public sbyte standard;

        [NativeTypeName("pqbool")]
        public sbyte html3;

        [NativeTypeName("pqbool")]
        public sbyte expanded;

        [NativeTypeName("pqbool")]
        public sbyte pager;

        [NativeTypeName("char *")]
        public utf8string fieldSep;

        [NativeTypeName("char *")]
        public utf8string tableOpt;

        [NativeTypeName("char *")]
        public utf8string caption;

        [NativeTypeName("char **")]
        public utf8string* fieldName;
    }
}
