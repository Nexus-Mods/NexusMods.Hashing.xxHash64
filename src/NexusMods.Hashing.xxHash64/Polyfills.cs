using System.Runtime.CompilerServices;
// ReSharper disable CheckNamespace

#if !NET7_0_OR_GREATER
namespace System.Numerics
{
    internal static class BitOperations
    {
        /// <summary>Rotates the specified value left by the specified number of bits.</summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by. Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <returns>The rotated value.</returns>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, int offset) => value << offset | value >> 64 - offset;
    }
}
#endif
