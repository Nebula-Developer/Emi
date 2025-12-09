using Emi.Mathematics;

using Xunit;

namespace Emi.Tests.Mathematics;

public class Vector3_Tests {
    [Fact]
    public void Cross_UnitAxes_ReturnsPerpendicular() {
        var a = Vector3.UnitX;
        var b = Vector3.UnitY;

        var result = a * b;
        var reverse = b * a;

        Assert.Equal(Vector3.UnitZ, result);
        Assert.Equal(new Vector3(0f, 0f, -1f), reverse);
    }

    [Fact]
    public void Cross_IntVectors_ReturnsExpected() {
        var a = new Vector3i(2, 3, 4);
        var b = new Vector3i(5, 6, 7);

        var result = a * b;

        Assert.Equal(new Vector3i(-3, 6, -3), result);
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
}
