using System;
using System.Threading;
using System.Threading.Tasks;
using NexusMods.Paths;

namespace NexusMods.Hashing.xxHash64.Paths;


/// <summary>
/// Extensions tied to <see cref="AbsolutePath"/>(s).
/// </summary>
public static class AbsolutePathExtensions
{
    /// <summary>
    /// Asynchronously hashes a given file.
    /// </summary>
    /// <param name="path">Path to the file to be hashed.</param>
    /// <param name="token">Allows you to cancel the operation.</param>
    /// <param name="reportFn">If not null, will be called with each block of data read from the input stream</param>
    public static async Task<Hash> XxHash64Async(this AbsolutePath path, CancellationToken? token = null,
        Func<Memory<byte>, Task>? reportFn = null)
    {
        await using var stream = path.Read();
        return await stream.XxHash64Async(token ?? CancellationToken.None, reportFn);
    }
}
