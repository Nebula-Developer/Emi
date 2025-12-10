using Emi.Mathematics;
using Emi.Mathematics.Vectors;

using Xunit;

namespace Emi.Tests.Mathematics;

public class Vector3_Tests {
    [Fact]
    public void Cross_UnitAxes_ReturnsPerpendicular() {
        var a = Vector3.UnitX;
        var b = Vector3.UnitY;

        var result = a.Cross(b);
        var reverse = b.Cross(a);

        Assert.Equal(Vector3.UnitZ, result);
        Assert.Equal(new Vector3(0f, 0f, -1f), reverse);
    }

    [Fact]
    public void Multiply_WithScalar_PreservesScalarOverload() {
        var value = new Vector3(1.5f, -2f, 0.5f);

        var scaled = value * 2f;

        Assert.Equal(new Vector3(3f, -4f, 1f), scaled);
    }

    [Fact]
    public void Cross_StaticMethod_UsesInstanceLogic() {
        var a = new Vector3d(1d, 2d, 3d);
        var b = new Vector3d(4d, 5d, 6d);

        var result = Vector3d.Cross(a, b);

        Assert.Equal(new Vector3d(-3d, 6d, -3d), result);
    }

    [Fact]
    public void Cross_ParallelVectors_ReturnsZero() {
        var a = new Vector3(1f, 2f, 3f);
        var b = new Vector3(2f, 4f, 6f); // parallel to a

        var result = a.Cross(b);

        Assert.Equal(Vector3.Zero, result);
    }

    [Fact]
    public void Cross_WithZeroVector_ReturnsZero() {
        var a = new Vector3(1f, 2f, 3f);
        var b = Vector3.Zero;

        var result = a.Cross(b);

        Assert.Equal(Vector3.Zero, result);
    }

    [Fact]
    public void Divide_ByScalar_ScalesCorrectly() {
        var v = new Vector3(2f, -4f, 6f);
        var result = v / 2f;

        Assert.Equal(new Vector3(1f, -2f, 3f), result);
    }

    [Fact]
    public void Negate_Vector_ReturnsNegative() {
        var v = new Vector3(1f, -2f, 3f);
        var neg = -v;

        Assert.Equal(new Vector3(-1f, 2f, -3f), neg);
    }

    [Fact]
    public void Lerp_Vector_ReturnsInterpolated() {
        var a = new Vector3(0f, 0f, 0f);
        var b = new Vector3(2f, 4f, 6f);

        var result = a.Lerp(b, 0.5f);

        Assert.Equal(new Vector3(1f, 2f, 3f), result);
    }

    [Fact]
    public void Abs_Vector_ReturnsPositiveComponents() {
        var v = new Vector3(-1f, -2f, 3f);
        var abs = v.Abs();

        Assert.Equal(new Vector3(1f, 2f, 3f), abs);
    }
}
