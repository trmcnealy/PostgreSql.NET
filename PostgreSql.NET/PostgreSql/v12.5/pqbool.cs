using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// Print options for PQprint()
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct pqbool : IEquatable<pqbool>
    {
        public pqbool(sbyte value)
        {
            Value = value;
        }

        public readonly sbyte Value;

        public bool Equals(pqbool other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is pqbool other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator sbyte(pqbool from)
        {
            return @from.Value;
        }

        public static implicit operator pqbool(sbyte from)
        {
            return new pqbool(@from);
        }

        public static bool operator ==(pqbool left,
                                       pqbool right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(pqbool left,
                                       pqbool right)
        {
            return !left.Equals(right);
        }
    }
}