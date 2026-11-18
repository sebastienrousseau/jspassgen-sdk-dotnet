// SPDX-License-Identifier: Apache-2.0 OR MIT

using System.Security.Cryptography;

namespace Jspassgen;

/// <summary>
/// An <see cref="IRandomSource"/> backed by the platform CSPRNG,
/// <see cref="RandomNumberGenerator"/>.
/// </summary>
/// <remarks>
/// It holds no state and is safe to share across threads. It makes no
/// network call and records nothing.
/// </remarks>
public sealed class CryptoRandomSource : IRandomSource
{
    /// <inheritdoc />
    public byte[] GetBytes(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        return RandomNumberGenerator.GetBytes(count);
    }

    /// <inheritdoc />
    /// <remarks>
    /// <see cref="RandomNumberGenerator.GetInt32(int)"/> draws by
    /// rejection sampling, never by a modulo of raw bytes, so every
    /// value in the range is equally likely.
    /// </remarks>
    public int NextInt(int max)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(max, 1);
        return RandomNumberGenerator.GetInt32(max);
    }
}
