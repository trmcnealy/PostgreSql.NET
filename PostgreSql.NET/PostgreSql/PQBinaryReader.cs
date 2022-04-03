#region

using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using PlatformApi;

#endregion

namespace PostgreSql
{
    public static unsafe class PQBinaryReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static sbyte ReadSByte(sbyte* bytes)
        {
            return ReadSByte(new ReadOnlySpan<byte>((void*)bytes, 1));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool ReadBoolean(sbyte* bytes)
        {
            return ReadBoolean(new ReadOnlySpan<byte>((void*)bytes, 1));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static char ReadChar(sbyte* bytes)
        {
            return ReadChar(new ReadOnlySpan<byte>((void*)bytes, 2));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static short ReadInt16(sbyte* bytes)
        {
            return ReadInt16(new ReadOnlySpan<byte>((void*)bytes, 2));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ushort ReadUInt16(sbyte* bytes)
        {
            return ReadUInt16(new ReadOnlySpan<byte>((void*)bytes, 2));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int ReadInt32(sbyte* bytes)
        {
            return ReadInt32(new ReadOnlySpan<byte>((void*)bytes, 4));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint ReadUInt32(sbyte* bytes)
        {
            return ReadUInt32(new ReadOnlySpan<byte>((void*)bytes, 4));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static long ReadInt64(sbyte* bytes)
        {
            return ReadInt64(new ReadOnlySpan<byte>((void*)bytes, 8));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong ReadUInt64(sbyte* bytes)
        {
            return ReadUInt64(new ReadOnlySpan<byte>((void*)bytes, 8));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Half ReadHalf(sbyte* bytes)
        {
            return ReadHalf(new ReadOnlySpan<byte>((void*)bytes, 2));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe float ReadSingle(sbyte* bytes)
        {
            return ReadSingle(new ReadOnlySpan<byte>((void*)bytes, 4));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe double ReadDouble(sbyte* bytes)
        {
            return ReadDouble(new ReadOnlySpan<byte>((void*)bytes, 8));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static decimal ReadDecimal(sbyte* bytes)
        {
            return ReadDecimal(new ReadOnlySpan<byte>((void*)bytes, 16));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static utf8string ReadString(sbyte* bytes)
        {
            int    length = 0;
            sbyte* value  = bytes;

            while(*value++ != char.MinValue)
            {
                ++length;
            }

            return ReadString(new ReadOnlySpan<byte>((void*)bytes, length));
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static sbyte ReadSByte(ReadOnlySpan<byte> bytes)
        {
            return (sbyte)bytes[0];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool ReadBoolean(ReadOnlySpan<byte> bytes)
        {
            return bytes[0] != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static char ReadChar(ReadOnlySpan<byte> bytes)
        {
            Span<char> singleChar = new(Unsafe.AsPointer(ref Unsafe.AsRef(bytes[0])),
                                        2);

            return (char)singleChar[0];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static short ReadInt16(ReadOnlySpan<byte> bytes)
        {
            return BinaryPrimitives.ReadInt16BigEndian(bytes);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ushort ReadUInt16(ReadOnlySpan<byte> bytes)
        {
            return BinaryPrimitives.ReadUInt16BigEndian(bytes);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int ReadInt32(ReadOnlySpan<byte> bytes)
        {
            return BinaryPrimitives.ReadInt32BigEndian(bytes);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint ReadUInt32(ReadOnlySpan<byte> bytes)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(bytes);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static long ReadInt64(ReadOnlySpan<byte> bytes)
        {
            return BinaryPrimitives.ReadInt64BigEndian(bytes);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong ReadUInt64(ReadOnlySpan<byte> bytes)
        {
            return BinaryPrimitives.ReadUInt64BigEndian(bytes);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Half ReadHalf(ReadOnlySpan<byte> bytes)
        {
            return BitConverter.Int16BitsToHalf(BinaryPrimitives.ReadInt16BigEndian(bytes));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe float ReadSingle(ReadOnlySpan<byte> bytes)
        {
            return BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32BigEndian(bytes));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe double ReadDouble(ReadOnlySpan<byte> bytes)
        {
            return BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64BigEndian(bytes));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static decimal ReadDecimal(ReadOnlySpan<byte> bytes)
        {
            int lo    = BinaryPrimitives.ReadInt32BigEndian(bytes);
            int mid   = BinaryPrimitives.ReadInt32BigEndian(bytes.Slice(4));
            int hi    = BinaryPrimitives.ReadInt32BigEndian(bytes.Slice(8));
            int flags = BinaryPrimitives.ReadInt32BigEndian(bytes.Slice(12));
            return new decimal(lo,
                               mid,
                               hi,
                               false,
                               (byte)(flags >> 16));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static utf8string ReadString(ReadOnlySpan<byte> bytes)
        {
            return new utf8string((sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(bytes[0])));
        }
    }
}
