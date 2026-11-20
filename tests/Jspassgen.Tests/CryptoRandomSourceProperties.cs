// SPDX-License-Identifier: Apache-2.0 OR MIT

using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace Jspassgen.Tests;

/// <summary>
/// Property-based tests of the <see cref="IRandomSource"/> contract:
/// FsCheck generates the arguments, over the whole valid and invalid
/// ranges, where the example tests pin a few.
/// </summary>
public class CryptoRandomSourceProperties
{
    private readonly CryptoRandomSource _source = new();

    // Valid counts, kept small enough that a run allocates little.
    private static readonly Arbitrary<int> ValidCounts = Gen.Choose(1, 1 << 16).ToArbitrary();

    // Every valid bound, from 1 to int.MaxValue. Small bounds are drawn
    // often, so a draw at the top of the range, max - 1, comes up.
    private static readonly Arbitrary<int> ValidMaxima = Gen.Frequency(
        (1, Gen.Constant(1)),
        (2, Gen.Choose(2, 16)),
        (1, Gen.Choose(17, int.MaxValue))).ToArbitrary();

    // Below 1: zero and one, the boundary, are drawn often; the rest
    // of the negative range is drawn too.
    private static readonly Arbitrary<int> BelowOne = Gen.Frequency(
        (1, Gen.Constant(0)),
        (1, Gen.Constant(-1)),
        (2, Gen.Choose(int.MinValue, -1))).ToArbitrary();

    [Property]
    public Property GetBytesReturnsExactlyCountBytes() =>
        Prop.ForAll(ValidCounts, count => _source.GetBytes(count).Length == count);

    [Property]
    public Property GetBytesRejectsEveryCountBelowOne() =>
        Prop.ForAll(BelowOne, count => ParamNameOf(() => _source.GetBytes(count)) == "count");

    [Property]
    public Property NextIntStaysInZeroToMax() =>
        Prop.ForAll(ValidMaxima, max =>
        {
            var value = _source.NextInt(max);
            return value >= 0 && value < max;
        });

    [Property]
    public Property NextIntRejectsEveryMaxBelowOne() =>
        Prop.ForAll(BelowOne, max => ParamNameOf(() => _source.NextInt(max)) == "max");

    // The parameter an ArgumentOutOfRangeException names, or null when
    // the call returns or throws anything else.
    private static string? ParamNameOf(Action call) =>
        (Record.Exception(call) as ArgumentOutOfRangeException)?.ParamName;
}
