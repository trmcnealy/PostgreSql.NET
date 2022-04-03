using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// PGcancel encapsulates the information needed to cancel a running
    /// query on an existing connection.
    /// The contents of this struct are not supposed to be known to applications.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct PGcancel : IEquatable<PGcancel>
    {
        public PGcancel(pg_cancel value)
        {
            Value = value;
        }

        public readonly pg_cancel Value;

        public bool Equals(PGcancel other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is PGcancel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator pg_cancel(PGcancel from)
        {
            return @from.Value;
        }

        public static implicit operator PGcancel(pg_cancel from)
        {
            return new PGcancel(@from);
        }

        public static bool operator ==(PGcancel left,
                                       PGcancel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PGcancel left,
                                       PGcancel right)
        {
            return !left.Equals(right);
        }
    }
}