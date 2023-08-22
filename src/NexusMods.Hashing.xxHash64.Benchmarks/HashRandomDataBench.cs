using System;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace NexusMods.Hashing.xxHash64.Benchmarks;

[MemoryDiagnoser, DisassemblyDiagnoser]
public class HashRandomDataBench
{
    // ReSharper disable once InconsistentNaming
    private readonly byte[] _data;

    public HashRandomDataBench()
    {
        _data = new byte[1024 * 1024 * 1024];
        Random.Shared.NextBytes(_data);
    }

    [Params(1024)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int Size { get; set; }

    [Benchmark]
    public ulong NexusHash()
    {
        var algo = new XxHash64Algorithm(0);
        return algo.HashBytes(_data.AsSpan()[..Size]);
    }

    [Benchmark]
    public unsafe ulong ExtensionsHashingBench()
    {
        Span<byte> hash = stackalloc byte[sizeof(ulong)];
        XxHash64.Hash(_data.AsSpan()[..Size], hash);
        return Unsafe.As<byte, ulong>(ref MemoryMarshal.GetReference(hash));
    }
}

