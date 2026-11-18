// SPDX-License-Identifier: Apache-2.0 OR MIT

namespace Jspassgen.Tests;

public class CryptoRandomSourceTests
{
    private readonly CryptoRandomSource _source = new();

    [Fact]
    public void IsTheRandomPort()
    {
        Assert.IsAssignableFrom<IRandomSource>(_source);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(16)]
    [InlineData(4096)]
    public void GetBytesReturnsTheRequestedLength(int count)
    {
        Assert.Equal(count, _source.GetBytes(count).Length);
    }

    [Fact]
    public void GetBytesReturnsFreshBytesOnEachCall()
    {
        // Two equal 32-byte draws from a CSPRNG have probability 2^-256.
        Assert.NotEqual(_source.GetBytes(32), _source.GetBytes(32));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void GetBytesRejectsACountBelowOne(int count)
    {
        var error = Assert.Throws<ArgumentOutOfRangeException>(() => _source.GetBytes(count));
        Assert.Equal("count", error.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void NextIntRejectsAMaxBelowOne(int max)
    {
        var error = Assert.Throws<ArgumentOutOfRangeException>(() => _source.NextInt(max));
        Assert.Equal("max", error.ParamName);
    }

    [Fact]
    public void NextIntOfOneIsAlwaysZero()
    {
        for (var i = 0; i < 100; i++)
        {
            Assert.Equal(0, _source.NextInt(1));
        }
    }

    [Theory]
    [InlineData(2)]
    [InlineData(7)]
    [InlineData(94)]
    [InlineData(int.MaxValue)]
    public void NextIntStaysInRange(int max)
    {
        for (var i = 0; i < 1000; i++)
        {
            Assert.InRange(_source.NextInt(max), 0, max - 1);
        }
    }

    [Fact]
    public void NextIntIsUniformAcrossASmallRange()
    {
        // Pearson's chi-squared over 10 buckets (9 degrees of freedom).
        // 46.0 is the critical value at p = 1e-6, so a correct source
        // fails this about once in a million runs.
        const int max = 10;
        const int draws = 100_000;
        var counts = new int[max];
        for (var i = 0; i < draws; i++)
        {
            counts[_source.NextInt(max)]++;
        }

        const double expected = (double)draws / max;
        var chiSquared = counts.Sum(observed => Math.Pow(observed - expected, 2) / expected);
        Assert.True(chiSquared < 46.0, $"chi-squared {chiSquared:F2} over {string.Join(',', counts)}");
    }

    [Fact]
    public void NextIntHasNoModuloBias()
    {
        // max = 3 * 2^29 does not divide 2^31 or 2^32. A modulo of 31 raw
        // bits puts half the draws in the lowest third, 32 bits 3/8; an
        // unbiased draw puts a third there. Expected 10 000 of 30 000,
        // standard deviation about 82; the window is six of them.
        const int max = 3 << 29;
        const int draws = 30_000;
        var lowThird = 0;
        for (var i = 0; i < draws; i++)
        {
            if (_source.NextInt(max) < 1 << 29)
            {
                lowThird++;
            }
        }

        Assert.InRange(lowThird, 9_510, 10_490);
    }
}
