using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PQprintOpt
    {
        /// <summary>
        /// print output field headings and row count
        /// </summary>
        public pqbool header;

        /// <summary>
        /// fill align the fields
        /// </summary>
        public pqbool align;

        /// <summary>
        /// old brain dead format
        /// </summary>
        public pqbool standard;

        /// <summary>
        /// output html tables
        /// </summary>
        public pqbool html3;

        /// <summary>
        /// expand tables
        /// </summary>
        public pqbool expanded;

        /// <summary>
        /// use pager for output if needed
        /// </summary>
        public pqbool pager;

        /// <summary>
        /// field separator
        /// </summary>
        public nuint fieldSep;

        /// <summary>
        /// insert to HTML &lt;table&gt;...&gt;
        /// </summary>
        public nuint tableOpt;

        /// <summary>
        /// HTML &lt;caption&gt;
        /// </summary>
        public nuint caption;

        /// <summary>
        /// null terminated array of replacement field
        /// names
        /// </summary>
        public nuint fieldName;
    }
}