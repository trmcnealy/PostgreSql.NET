using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// PGresult encapsulates the result of a query (or more precisely, of a single
    /// SQL command --- a query string given to PQsendQuery can contain multiple
    /// commands and thus return multiple PGresult objects).
    /// The contents of this struct are not supposed to be known to applications.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct PGresult : IEquatable<PGresult>
    {
        public PGresult(pg_result value)
        {
            Value = value;
        }

        public readonly pg_result Value;

        public bool Equals(PGresult other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is PGresult other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator pg_result(PGresult from)
        {
            return @from.Value;
        }

        public static implicit operator PGresult(pg_result from)
        {
            return new PGresult(@from);
        }

        public static bool operator ==(PGresult left,
                                       PGresult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PGresult left,
                                       PGresult right)
        {
            return !left.Equals(right);
        }
    }
}