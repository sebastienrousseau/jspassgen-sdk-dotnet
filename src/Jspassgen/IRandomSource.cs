// SPDX-License-Identifier: Apache-2.0 OR MIT

namespace Jspassgen;

/// <summary>
/// The source of randomness every generator draws from: the .NET form
/// of jspassgen's <c>RandomGeneratorPort</c>
/// (<c>generateRandomBytes</c>, <c>generateRandomInt</c>).
/// </summary>
/// <remarks>
/// Implementations used to make real secrets must be cryptographically
/// secure. <see cref="CryptoRandomSource"/> is the platform CSPRNG; a
/// test may supply a deterministic implementation to replay a fixed
/// sequence of draws.
/// </remarks>
public interface IRandomSource
{
    /// <summary>Returns <paramref name="count"/> random bytes.</summary>
    /// <param name="count">How many bytes to return; at least 1.</param>
    /// <returns>A new array of exactly <paramref name="count"/> bytes.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
    byte[] GetBytes(int count);

    /// <summary>
    /// Returns a uniformly distributed integer in [0, <paramref name="max"/>),
    /// without modulo bias.
    /// </summary>
    /// <param name="max">The exclusive upper bound; at least 1.</param>
    /// <returns>An integer greater than or equal to 0 and less than <paramref name="max"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than 1.</exception>
    int NextInt(int max);
}
