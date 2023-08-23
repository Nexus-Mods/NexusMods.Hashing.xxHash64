using NexusMods.Paths;

namespace NexusMods.Hashing.xxHash64.Paths.Tests;

public class HashTests
{
    private const string KnownString = "Something clever should go here";
    private static readonly Hash KnownHash = Hash.FromHex("F4C92BE058F432D0");
    private readonly IFileSystem _fileSystem = FileSystem.Shared;

    [Fact]
    public async Task CanHashFile()
    {
        var file = _fileSystem.GetKnownPath(KnownPath.CurrentDirectory).Combine($"tempFile{Guid.NewGuid()}");
        await file.WriteAllTextAsync(KnownString);
        (await file.XxHash64Async()).Should().Be(KnownHash);
        (await file.MSXxHash64()).Should().Be(KnownHash); // sanity test against MSFT hasher.
    }

    [Fact]
    public async Task CanHashLargeFile()
    {
        var file = _fileSystem.GetKnownPath(KnownPath.CurrentDirectory).Combine($"tempFile{Guid.NewGuid()}");
        var testArray = CreateTestArray();

        await file.WriteAllBytesAsync(testArray);
        (await file.XxHash64Async()).Should().Be(Hash.FromULong(0x54AC7E8D1810EC9D));
        (await file.MSXxHash64()).Should().Be(Hash.FromULong(0x54AC7E8D1810EC9D)); // sanity test against MSFT hasher.
        file.Delete();
    }

    [Fact]
    public async Task CanHashLargeFileWithReporting()
    {
        var file = _fileSystem.GetKnownPath(KnownPath.CurrentDirectory).Combine($"tempFile{Guid.NewGuid()}");
        var testArray = CreateTestArray();

        await file.WriteAllBytesAsync(testArray);

        var ms = new MemoryStream();
        var expectedHash = Hash.FromULong(0x54AC7E8D1810EC9D);

        (await file.XxHash64Async(reportFn: async m => await ms.WriteAsync(m))).Should().Be(expectedHash);
        (await file.MSXxHash64()).Should().Be(expectedHash);
        ms.ToArray().AsSpan().XxHash64().Should().Be(expectedHash);
        file.Delete();
    }

    private static byte[] CreateTestArray()
    {
        var emptyArray = new byte[1024 * 1024 * 10];
        for (var x = 0; x < emptyArray.Length; x++)
            emptyArray[x] = (byte)(x % 256);
        return emptyArray;
    }
}
