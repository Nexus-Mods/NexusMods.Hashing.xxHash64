using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace NexusMods.Hashing.xxHash64;

/// <summary>
/// Extensions related to <see cref="Span{T}"/>(s) and their heap sibling <see cref="Memory{T}"/>.
/// </summary>
[PublicAPI]
public static class SpanExtensions
{
    /// <summary>
    /// Hashes the given <see cref="Span{T}"/> of bytes using xxHash64.
    /// </summary>
    /// <param name="data">The data to hash.</param>
    /// <returns>Hash for the given data.</returns>
    public static Hash XxHash64(this Span<byte> data) => XxHash64((ReadOnlySpan<byte>)data);

    /// <summary>
    /// Hashes the given <see cref="Span{T}"/> of bytes using xxHash64.
    /// </summary>
    /// <param name="data">The data to hash.</param>
    /// <returns>Hash for the given data.</returns>
    public static Hash XxHash64(this ReadOnlySpan<byte> data) => Hash.From(XxHash64Algorithm.HashBytes(data));

    /// <summary>
    /// Hashes the given <see cref="Memory{T}"/> of bytes using xxHash64.
    /// </summary>
    /// <param name="data">The data to hash.</param>
    /// <returns>Hash for the given data.</returns>
    public static Hash XxHash64(this Memory<byte> data) => XxHash64(data.Span);

    /// <summary>
    /// Reads a <see cref="ulong"/> from the given <see cref="ReadOnlySpan{T}"/> of bytes,
    /// without any bounds checks!
    /// </summary>
    /// <param name="value">The value to convert to <see cref="ulong"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64Fast(this ReadOnlySpan<byte> value)
    {
        return Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(value));
    }

    /// <summary>
    /// Reads a <see cref="ulong"/> from the given <see cref="ReadOnlySpan{T}"/> of bytes,
    /// without any bounds checks!
    /// </summary>
    /// <param name="value">The value to convert to <see cref="uint"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32Fast(this ReadOnlySpan<byte> value) => Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(value));
}
