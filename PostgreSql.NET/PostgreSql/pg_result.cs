using System.Runtime.InteropServices;

namespace PostgreSql
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct pg_result : IEquatable<pg_result>
    {
        private readonly nuint _handle;

        public pg_result(nuint handle)
        {
            _handle = handle;
        }

        public nuint Handle { get { return _handle; } }

        public bool Equals(pg_result other)
        {
            return _handle.Equals(other._handle);
        }

        public override bool Equals(object? obj)
        {
            return obj is pg_result other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }

        public override string ToString()
        {
            unsafe
            {
                return "0x" + (sizeof(nuint) == 8 ? _handle.ToString("X16") : _handle.ToString("X8"));
            }
        }

        public static bool operator ==(pg_result left,
                                       pg_result right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(pg_result left,
                                       pg_result right)
        {
            return !left.Equals(right);
        }
    }
}
