// ReSharper disable RedundantUsingDirective
using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace

#if !NETCOREAPP2_0_OR_GREATER
namespace System.Numerics
{
    internal static class BitOperations
    {
        /// <summary>Rotates the specified value left by the specified number of bits.</summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by. Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, int offset) => value << offset | value >> 64 - offset;
    }
}
#endif

#if !NETCOREAPP3_1_OR_GREATER
/// <summary>
/// Polyfills for missing methods in older versions of .NET.
/// </summary>
internal static class PolyfillExtensions
{
    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAsync(
        this Stream stream,
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        if (MemoryMarshal.TryGetArray(buffer, out var segment))
            return stream.WriteAsync(segment.Array!, segment.Offset, segment.Count, cancellationToken);

        var numArray = ArrayPool<byte>.Shared.Rent(buffer.Length);
        buffer.Span.CopyTo((Span<byte>) numArray);
        return FinishWriteAsync(stream.WriteAsync(numArray, 0, buffer.Length, cancellationToken), numArray);

        static async Task FinishWriteAsync(Task writeTask, byte[] localBuffer)
        {
            try
            {
                await writeTask.ConfigureAwait(false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(localBuffer);
            }
        }
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of its <see cref="P:System.Threading.Tasks.ValueTask`1.Result" /> property contains the total number of bytes read into the buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public static Task<int> ReadAsync(
        this Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment))
            return stream.ReadAsync(segment.Array, segment.Offset, segment.Count, cancellationToken);

        var numArray = ArrayPool<byte>.Shared.Rent(buffer.Length);
        return FinishReadAsync(stream.ReadAsync(numArray, 0, buffer.Length, cancellationToken), numArray, buffer);

#nullable disable
        static async Task<int> FinishReadAsync(
            Task<int> readTask,
            byte[] localBuffer,
            Memory<byte> localDestination)
        {
            int num;
            try
            {
                var length = await readTask.ConfigureAwait(false);
                new ReadOnlySpan<byte>(localBuffer, 0, length).CopyTo(localDestination.Span);
                num = length;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(localBuffer);
            }
            return num;
        }
    }

    /// <summary>When overridden in a derived class, encodes into a span of bytes a set of characters from the specified read-only span.</summary>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="chars">The span containing the set of characters to encode.</param>
    /// <param name="bytes">The byte span to hold the encoded bytes.</param>
    /// <returns>The number of encoded bytes.</returns>
    public static unsafe int GetBytes(this Encoding encoding, ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        fixed (char* chars1 = &MemoryMarshal.GetReference(chars))
        fixed (byte* bytes1 = &MemoryMarshal.GetReference(bytes))
            return encoding.GetBytes(chars1, chars.Length, bytes1, bytes.Length);
    }
}
#endif
