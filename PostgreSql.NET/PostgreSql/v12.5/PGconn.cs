using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// PGconn encapsulates a connection to the backend.
    /// The contents of this struct are not supposed to be known to applications.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct PGconn : IEquatable<PGconn>
    {
        public PGconn(pg_conn value)
        {
            Value = value;
        }

        public readonly pg_conn Value;

        public bool Equals(PGconn other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is PGconn other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator pg_conn(PGconn from)
        {
            return @from.Value;
        }

        public static implicit operator PGconn(pg_conn from)
        {
            return new PGconn(@from);
        }

        public static bool operator ==(PGconn left,
                                       PGconn right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PGconn left,
                                       PGconn right)
        {
            return !left.Equals(right);
        }
    }
}