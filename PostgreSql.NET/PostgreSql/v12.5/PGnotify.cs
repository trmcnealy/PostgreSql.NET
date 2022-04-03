using System;
using System.Runtime.InteropServices;

namespace PostgreSql
{
    /// <summary>
    /// PGnotify represents the occurrence of a NOTIFY message.
    /// Ideally this would be an opaque typedef, but it's so simple that it's
    /// unlikely to change.
    /// NOTE: in Postgres 6.4 and later, the be_pid is the notifying backend's,
    /// whereas in earlier versions it was always your own backend's PID.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public readonly struct PGnotify : IEquatable<PGnotify>
    {
        public PGnotify(pgNotify value)
        {
            Value = value;
        }

        public readonly pgNotify Value;

        public bool Equals(PGnotify other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is PGnotify other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string? ToString()
        {
            return Value.ToString();
        }

        public static implicit operator pgNotify(PGnotify from)
        {
            return @from.Value;
        }

        public static implicit operator PGnotify(pgNotify from)
        {
            return new PGnotify(@from);
        }

        public static bool operator ==(PGnotify left,
                                       PGnotify right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PGnotify left,
                                       PGnotify right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// PGnotify represents the occurrence of a NOTIFY message.
    /// Ideally this would be an opaque typedef, but it's so simple that it's
    /// unlikely to change.
    /// NOTE: in Postgres 6.4 and later, the be_pid is the notifying backend's,
    /// whereas in earlier versions it was always your own backend's PID.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct pgNotify
    {
        /// <summary>
        /// notification condition name
        /// </summary>
        public nuint relname;

        /// <summary>
        /// process ID of notifying server process
        /// </summary>
        public int be_pid;

        /// <summary>
        /// notification parameter
        /// </summary>
        public nuint extra;

        /// <summary>
        /// list link
        /// </summary>
        public nuint next;
    }
}