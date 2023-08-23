using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
}
