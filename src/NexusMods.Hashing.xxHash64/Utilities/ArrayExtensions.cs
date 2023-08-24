using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#if NET5_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace NexusMods.Hashing.xxHash64.Utilities;

/// <summary>
///     Internal extension methods tied to arrays.
/// </summary>
[SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
internal static class ArrayExtensions
{
    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <typeparamref name="T" /> array, with no
    ///     bounds checks.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <typeparamref name="T" /> array instance.</typeparam>
    /// <param name="array">The input <typeparamref name="T" /> array instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="array" />.</param>
    /// <returns>A reference to the element within <paramref name="array" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [ExcludeFromCodeCoverage] // forked from CommunityToolkit
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T DangerousGetReferenceAt<T>(this T[] array, int i)
    {
#if NET5_0_OR_GREATER
        ref T r0 = ref MemoryMarshal.GetArrayDataReference(array);
        ref T ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
#else
        // Fallback for older runtimes APIs
        return ref array[i];
#endif
    }
}
