using Emi.Mathematics;
using Emi.Mathematics.Vectors;

using Xunit;

namespace Emi.Tests.Mathematics;

public class Vector2_Tests {
    [Fact]
    public void Multiply_WithScalar_ScalesFloat() {
        var value = new Vector2(1.5f, -2f);

        var scaled = value * 2f;

        Assert.Equal(new Vector2(3f, -4f), scaled);
    }

    [Fact]
    public void Dot_WithPerpendicularVectors_ReturnsZeroSum() {
        var a = new Vector2(1f, 2f);
        var b = new Vector2(2f, -1f);

        var dot = a.Dot(b);

        Assert.Equal(0f, dot);
    }

    [Fact]
    public void Lerp_Vector_ReturnsInterpolated() {
        var a = Vector2.Zero;
        var b = new Vector2(4f, 6f);

        var result = a.Lerp(b, 0.5f);

        Assert.Equal(new Vector2(2f, 3f), result);
    }

    [Fact]
    public void Abs_Vector_ReturnsPositiveComponents() {
        var v = new Vector2(-1f, -2f);
        var abs = v.Abs();

        Assert.Equal(new Vector2(1f, 2f), abs);
    }
}
