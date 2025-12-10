using System;
using Emi.Mathematics;
using Vector3 = Emi.Mathematics.Vectors.Vector3;

using Xunit;

namespace Emi.Tests.Mathematics;

public class Quaternion_Tests {
    [Fact]
    public void FromAxisAngle_RotatesUnitXToUnitY() {
        var rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, MathF.PI / 2f);
        var rotated = rotation.Rotate(Vector3.UnitX);

        Assert.True(MathF.Abs(rotated.X) < 1e-5f);
        Assert.True(MathF.Abs(rotated.Y - 1f) < 1e-5f);
        Assert.True(MathF.Abs(rotated.Z) < 1e-5f);
    }

    [Fact]
    public void Lerp_PreservesUnitMagnitude() {
        var start = Quaternion.Identity;
        var end = Quaternion.FromAxisAngle(Vector3.UnitX, MathF.PI);

        var lerp = Quaternion.Lerp(start, end, 0.5f);

        Assert.InRange(lerp.LengthSquared, 0.9999, 1.0001);
    }

    [Fact]
    public void Slerp_BetweenIdentities_ReturnsIdentity() {
        var start = Quaternion.Identity;
        var end = Quaternion.Identity;

        var slerp = Quaternion.Slerp(start, end, 0.5f);

        Assert.Equal(Quaternion.Identity, slerp);
    }
}
