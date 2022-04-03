using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// Object ID is a fundamental type in Postgres.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct Oid : IEquatable<Oid>
    {
        public Oid(uint value)
        {
            Value = value;
        }

        public readonly uint Value;

        public bool Equals(Oid other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is Oid other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator uint(Oid from)
        {
            return @from.Value;
        }

        public static implicit operator Oid(uint from)
        {
            return new Oid(@from);
        }

        public static bool operator ==(Oid left,
                                       Oid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Oid left,
                                       Oid right)
        {
            return !left.Equals(right);
        }
    }
}