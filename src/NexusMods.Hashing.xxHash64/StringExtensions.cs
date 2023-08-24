using System;
using System.Buffers;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using NexusMods.Hashing.xxHash64.Utilities;

namespace NexusMods.Hashing.xxHash64;

/// <summary>
/// Hashing related extensions for strings that might come in handy down the road.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    private static readonly char[] HexLookup = "0123456789ABCDEF".ToArray();

    /// <summary>
    /// Converts the given bytes to a hexadecimal string.
    /// </summary>
    /// <param name="bytes">The bytes to convert.</param>
    /// <returns>The string in question.</returns>
    public static unsafe string ToHex(this ReadOnlySpan<byte> bytes)
    {
        Span<char> outputBuf = stackalloc char[bytes.Length * 2];
        ToHex(bytes, outputBuf);
        fixed (char* outputPtr = outputBuf)
            return new string(outputPtr, 0, outputBuf.Length);
    }

    /// <summary>
    /// Converts the given bytes to a hexadecimal string.
    /// </summary>
    /// <param name="bytes">The bytes to convert.</param>
    /// <param name="outputBuf">The buffer where the data should be output.</param>
    /// <returns>The string in question.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToHex(this ReadOnlySpan<byte> bytes, Span<char> outputBuf)
    {
        for (var x = 0; x < bytes.Length; x++)
        {
            outputBuf.DangerousGetReferenceAt(x * 2) = HexLookup.DangerousGetReferenceAt((bytes[x] >> 4));
            outputBuf.DangerousGetReferenceAt((x * 2) + 1) = HexLookup.DangerousGetReferenceAt(bytes[x] & 0xF);
        }
    }

    /// <summary>
    /// Converts a hex string back to its corresponding bytes.
    /// </summary>
    /// <param name="hex">The hex string itself.</param>
    /// <param name="bytes">The bytes for the hex string.</param>
    public static void FromHex(this string hex, Span<byte> bytes) => hex.AsSpan().FromHex(bytes);

    /// <summary>
    /// Converts a hex string span of characters back to its corresponding bytes.
    /// </summary>
    /// <param name="hex">The hex string itself.</param>
    /// <param name="bytes">The bytes for the hex string.</param>
    public static unsafe void FromHex(this ReadOnlySpan<char> hex, Span<byte> bytes)
    {
        fixed (char* hexBytes = hex)
        {
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = ParseHexByte(&hexBytes[i * 2]);
        }
    }

    /// <summary>
    /// Returns the xxHash64 of the given string using UTF8 encoding.
    /// </summary>
    /// <param name="text">The string to hash.</param>
    /// <returns>Hash of the given string.</returns>
    public static Hash XxHash64AsUtf8(this string text)
    {
        // This is only used in tests right now.
        var utf8 = Encoding.UTF8;
        var numBytes = utf8.GetByteCount(text);

        if (numBytes >= 1024)
        {

            using var mem = MemoryPool<byte>.Shared.Rent(numBytes);
            var dataSpan = mem.Memory.Span[..numBytes];
            utf8.GetBytes(text.AsSpan(), dataSpan);
            return dataSpan.XxHash64();
        }
        else
        {
            Span<byte> dataSpan = stackalloc byte[numBytes];
            utf8.GetBytes(text.AsSpan(), dataSpan);
            return dataSpan.XxHash64();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe byte ParseHexByte(char* hex)
    {
        return (byte)((GetHexValue(hex[0]) << 4) + GetHexValue(hex[1]));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetHexValue(char hexChar)
    {
        // Do not refactor into switch, this gets optimised better.
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (hexChar is >= '0' and <= '9')
            return hexChar - '0';

        if (hexChar is >= 'a' and <= 'f')
            return hexChar - 'a' + 10;

        if (hexChar is >= 'A' and <= 'F')
            return hexChar - 'A' + 10;

        return 0;
    }
}
