using System.Buffers.Binary;
using NexusMods.Paths;

namespace NexusMods.Hashing.xxHash64.Tests;

/// <summary>
/// Contains various utility functions.
/// </summary>
public static class Utility
{
    /// <summary>
    /// Runs the microsoft's System.IO.Hashing algorithm returning in our format
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static Hash MSHash(byte[] data)
    {
        var bytes = System.IO.Hashing.XxHash64.Hash(data);
        return Hash.From(BinaryPrimitives.ReadUInt64BigEndian(bytes));
    }

    /// <summary>
    /// Asynchronously hashes a given file.
    /// </summary>
    /// <param name="path">Path to the file to be hashed.</param>
    public static async Task<Hash> MSXxHash64(this AbsolutePath path)
    {
        var data = await path.ReadAllBytesAsync();
        return MSHash(data);
    }
}
