using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// ----------------
    /// PGresAttDesc -- Data about a single attribute (column) of a query result
    /// ----------------
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct PGresAttDesc : IEquatable<PGresAttDesc>
    {
        public PGresAttDesc(pgresAttDesc value)
        {
            Value = value;
        }

        public readonly pgresAttDesc Value;

        public bool Equals(PGresAttDesc other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is PGresAttDesc other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string? ToString()
        {
            return Value.ToString();
        }

        public static implicit operator pgresAttDesc(PGresAttDesc from)
        {
            return @from.Value;
        }

        public static implicit operator PGresAttDesc(pgresAttDesc from)
        {
            return new PGresAttDesc(@from);
        }

        public static bool operator ==(PGresAttDesc left,
                                       PGresAttDesc right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PGresAttDesc left,
                                       PGresAttDesc right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// ----------------
    /// PGresAttDesc -- Data about a single attribute (column) of a query result
    /// ----------------
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct pgresAttDesc
    {
        /// <summary>
        /// column name
        /// </summary>
        public nuint name;

        /// <summary>
        /// source table, if known
        /// </summary>
        public Oid tableid;

        /// <summary>
        /// source column, if known
        /// </summary>
        public int columnid;

        /// <summary>
        /// format code for value (text/binary)
        /// </summary>
        public int format;

        /// <summary>
        /// type id
        /// </summary>
        public Oid typid;

        /// <summary>
        /// type size
        /// </summary>
        public int typlen;

        /// <summary>
        /// type-specific modifier info
        /// </summary>
        public int atttypmod;
    }
}