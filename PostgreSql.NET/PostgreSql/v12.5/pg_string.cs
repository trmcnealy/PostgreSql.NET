// ReSharper disable InconsistentNaming

using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct pg_string : IEquatable<pg_string>
    {
        private readonly nuint _handle;

        public pg_string(nuint handle)
        {
            _handle = handle;
        }

        public nuint Handle { get { return _handle; } }

        public static implicit operator pg_string(nuint from)
        {
            return new pg_string(from);
        }

        public static implicit operator string?(pg_string from)
        {
            return Marshal.PtrToStringAnsi(from.Handle);
        }

        public bool Equals(pg_string other)
        {
            return _handle.Equals(other._handle);
        }

        public override bool Equals(object? obj)
        {
            return obj is pg_string other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }

        public override string? ToString()
        {
            return Marshal.PtrToStringAnsi(_handle);
        }

        public static bool operator ==(pg_string left,
                                       pg_string right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(pg_string left,
                                       pg_string right)
        {
            return !left.Equals(right);
        }
    }
}
