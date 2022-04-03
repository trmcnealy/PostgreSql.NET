using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// Define a signed 64-bit integer type for use in client API declarations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct pg_int64 : IEquatable<pg_int64>
    {
        public pg_int64(long value)
        {
            Value = value;
        }

        public readonly long Value;

        public bool Equals(pg_int64 other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is pg_int64 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator long(pg_int64 from)
        {
            return @from.Value;
        }

        public static implicit operator pg_int64(long from)
        {
            return new pg_int64(@from);
        }

        public static bool operator ==(pg_int64 left,
                                       pg_int64 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(pg_int64 left,
                                       pg_int64 right)
        {
            return !left.Equals(right);
        }
    }
}