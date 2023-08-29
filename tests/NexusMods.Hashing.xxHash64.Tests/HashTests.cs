namespace NexusMods.Hashing.xxHash64.Tests;

public class HashTests
{
    private const string KnownString = "Something clever should go here";
    private static readonly Hash KnownHash = Hash.FromHex("F4C92BE058F432D0");

    [Fact]
    public void CanConvertHashBetweenFormats()
    {
        var hash = Hash.FromULong(0xDEADBEEFDECAFBAD);

        ((ulong)hash).Should().Be(0xDEADBEEFDECAFBAD);
        hash.ToHex().Should().Be("DEADBEEFDECAFBAD");
        Hash.FromHex("DEADBEEFDECAFBAD").Should().Be(hash);

        Hash.FromLong(KnownHash).Should().Be(KnownHash);
        Hash.FromULong((ulong)KnownHash).Should().Be(KnownHash);
        KnownHash.ToString().Should().Be("0x" + KnownHash.ToHex());
    }

    [Fact]
    public void CanCompareHashes()
    {
        var hash1 = Hash.FromULong(0);
        var hash2 = Hash.FromULong(1);
        var hash3 = Hash.FromULong(2);

        hash1.Should().BeLessThan(hash2);
        hash2.Should().BeLessThan(hash3);

        hash1.Should().Be(hash1);
        hash1.Should().NotBe(hash2);

        hash1.Should().BeRankedEquallyTo(hash1);

        Assert.True(hash1 != hash2);
        Assert.False(hash1 == hash2);
    }

    [Fact]
    public void CanHashStrings()
    {
        KnownString.XxHash64AsUtf8()
            .Should().Be(KnownHash);
    }

    [Fact]
    public async Task CanHashFile()
    {
        var file = Path.Combine(Environment.CurrentDirectory, $"tempFile{Guid.NewGuid()}");
        await File.WriteAllTextAsync(file, KnownString);

        (await file.XxHash64Async()).Should().Be(KnownHash);
        (await file.MSXxHash64()).Should().Be(KnownHash); // sanity test against MSFT hasher.
    }

    [Fact]
    public async Task CanHashLargeFile()
    {
        var file = Path.Combine(Environment.CurrentDirectory, $"tempFile{Guid.NewGuid()}");
        var emptyArray = CreateTestArray();

        await File.WriteAllBytesAsync(file, emptyArray);
        (await file.XxHash64Async()).Should().Be(Hash.FromULong(0x54AC7E8D1810EC9D));
        (await file.MSXxHash64()).Should().Be(Hash.FromULong(0x54AC7E8D1810EC9D)); // sanity test against MSFT hasher.
        File.Delete(file);
    }

    [Fact]
    public async Task CanHashLargeFileWithReporting()
    {
        var file = Path.Combine(Environment.CurrentDirectory, $"tempFile{Guid.NewGuid()}");
        var emptyArray = CreateTestArray();

        await File.WriteAllBytesAsync(file, emptyArray);
        var ms = new MemoryStream();
        var expectedHash = Hash.FromULong(0x54AC7E8D1810EC9D);

        await using var stream = new FileStream(file, FileMode.OpenOrCreate);
        (await stream.XxHash64Async(reportFn: async m => await ms.WriteAsync(m))).Should().Be(expectedHash);
        (await file.MSXxHash64()).Should().Be(expectedHash);
        ms.ToArray().AsSpan().XxHash64().Should().Be(expectedHash);
        File.Delete(file);
    }

    [Fact]
    public unsafe void HashIs8Bytes()
    {
        sizeof(Hash).Should().Be(8);
    }

    private static byte[] CreateTestArray()
    {
        var emptyArray = new byte[1024 * 1024 * 10];
        for (var x = 0; x < emptyArray.Length; x++)
            emptyArray[x] = (byte)(x % 256);

        return emptyArray;
    }
}
