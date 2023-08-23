using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace NexusMods.Hashing.xxHash64.Utilities;

// ReSharper disable MemberCanBePrivate.Global

/// <summary>
///     Extension methods tied to spans.
/// </summary>
[SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
[ExcludeFromCodeCoverage] // Reused from other projects.
internal static class SpanExtensions
{
    /// <summary>
    ///     Slices a span without any bounds checks, using a start index and length.
    /// </summary>
    /// <param name="data">The input span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <param name="length">The number of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> SliceFast<T>(this Span<T> data, int start, int length)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start), length);
#else
        return data.Slice(start, length);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a start index and length.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <param name="length">The number of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, int start, int length)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start), length);
#else
        return data.Slice(start, length);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a range.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="range">The range of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    /// <remarks>
    ///     This is a fallback. Normally the compiler will lower this to the overload with index and length.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, Range range)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(
            ref Unsafe.Add(ref MemoryMarshal.GetReference(data), range.Start.GetOffset(data.Length)),
            range.End.Value - range.Start.Value);
#else
        return data.Slice(range.Start.GetOffset(data.Length), range.End.Value - range.Start.Value);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a range.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="range">The range of elements to include in the sliced span.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    /// <remarks>
    ///     This is a fallback. Normally the compiler will lower this to the overload with index and length.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> SliceFast<T>(this Span<T> data, Range range)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(
            ref Unsafe.Add(ref MemoryMarshal.GetReference(data), range.Start.GetOffset(data.Length)),
            range.End.Value - range.Start.Value);
#else
        return data.Slice(range.Start.GetOffset(data.Length), range.End.Value - range.Start.Value);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a start index.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> SliceFast<T>(this Span<T> data, int start)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start),
            data.Length - start);
#else
        return data.Slice(start);
#endif
    }

    /// <summary>
    ///     Slices a read-only span without any bounds checks, using a start index.
    /// </summary>
    /// <param name="data">The input read-only span to slice.</param>
    /// <param name="start">The zero-based start index of the range to slice.</param>
    /// <typeparam name="T">The type of elements in the span.</typeparam>
    /// <returns>A sliced span without bounds checks.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, int start)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(data), start),
            data.Length - start);
#else
        return data.Slice(start);
#endif
    }

        /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="Span{T}" />, with no bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="Span{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this Span<T> span, int i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="ReadOnlySpan{T}" />, with no
    ///     bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="ReadOnlySpan{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this ReadOnlySpan<T> span, int i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="Span{T}" />, with no bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="Span{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="Span{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this Span<T> span, nint i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, i);

        return ref ri;
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="ReadOnlySpan{T}" />, with no
    ///     bounds
    ///     checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}" /> instance.</typeparam>
    /// <param name="span">The input <see cref="ReadOnlySpan{T}" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="span" />.</param>
    /// <returns>A reference to the element within <paramref name="span" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this ReadOnlySpan<T> span, nint i)
    {
        ref T r0 = ref MemoryMarshal.GetReference(span);
        ref T ri = ref Unsafe.Add(ref r0, i);

        return ref ri;
    }
}
