using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// ----------------
    /// Structure for the conninfo parameter definitions returned by PQconndefaults
    /// or PQconninfoParse.
    /// </summary>
    /// <remarks>
    /// All fields except "val" point at static strings which must not be altered.
    /// "val" is either NULL or a malloc'd current-value string.  PQconninfoFree()
    /// will release both the val strings and the PQconninfoOption array itself.
    /// ----------------
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PQconninfoOption
    {
        /// <summary>
        /// The keyword of the option
        /// </summary>
        public nuint keyword;

        /// <summary>
        /// Fallback environment variable name
        /// </summary>
        public nuint envvar;

        /// <summary>
        /// Fallback compiled in default value
        /// </summary>
        public nuint compiled;

        /// <summary>
        /// Option's current value, or NULL
        /// </summary>
        public nuint val;

        /// <summary>
        /// Label for field in connect dialog
        /// </summary>
        public nuint label;

        /// <summary>
        /// Indicates how to display this field in a
        /// connect dialog. Values are: "" Display
        /// entered value as is "*" Password field -
        /// hide value "D"  Debug option - don't show
        /// by default
        /// </summary>
        public nuint dispchar;

        /// <summary>
        /// Field size in characters for dialog
        /// </summary>
        public int dispsize;
    }
}