using Emi.Mathematics;
using Vector3 = Emi.Mathematics.Vectors.Vector3;

using Xunit;

namespace Emi.Tests.Mathematics;

public class Matrix4x4_Tests {
    [Fact]
    public void Multiply_WithIdentity_ReturnsOriginal() {
        var original = Matrix4x4.CreateTranslation(new Vector3(2f, 3f, 4f));
        var result = original * Matrix4x4.Identity;

        Assert.Equal(original, result);
    }

    [Fact]
    public void TransformPoint_WithTranslation_ShiftsPoint() {
        var translation = Matrix4x4.CreateTranslation(new Vector3(1f, 2f, 3f));
        var point = new Vector3(3f, 5f, 7f);

        var transformed = translation.TransformPoint(point);

        Assert.Equal(new Vector3(4f, 7f, 10f), transformed);
    }

    [Fact]
    public void TransformDirection_IgnoresTranslation() {
        var matrix = Matrix4x4.CreateTranslation(new Vector3(1f, 2f, 3f)) * Matrix4x4.CreateScale(new Vector3(2f, 3f, 4f));
        var direction = new Vector3(1f, 0f, 0f);

        var transformed = matrix.TransformDirection(direction);

        Assert.Equal(new Vector3(2f, 0f, 0f), transformed);
    }

    [Fact]
    public void Inverse_MultipliesToIdentity() {
        var matrix = Matrix4x4.CreateRotationY(MathF.PI / 3f) * Matrix4x4.CreateTranslation(new Vector3(1f, 2f, 3f));
        var inverse = matrix.Inverse();

        var product = matrix * inverse;

        Assert.True(IsApproximatelyIdentity(product));
    }

    private static bool IsApproximatelyIdentity(Matrix4x4 matrix, float epsilon = 1e-5f) {
        return MathF.Abs(matrix.M11 - 1f) < epsilon
            && MathF.Abs(matrix.M22 - 1f) < epsilon
            && MathF.Abs(matrix.M33 - 1f) < epsilon
            && MathF.Abs(matrix.M44 - 1f) < epsilon
            && MathF.Abs(matrix.M12) < epsilon
            && MathF.Abs(matrix.M13) < epsilon
            && MathF.Abs(matrix.M14) < epsilon
            && MathF.Abs(matrix.M21) < epsilon
            && MathF.Abs(matrix.M23) < epsilon
            && MathF.Abs(matrix.M24) < epsilon
            && MathF.Abs(matrix.M31) < epsilon
            && MathF.Abs(matrix.M32) < epsilon
            && MathF.Abs(matrix.M34) < epsilon
            && MathF.Abs(matrix.M41) < epsilon
            && MathF.Abs(matrix.M42) < epsilon
            && MathF.Abs(matrix.M43) < epsilon;
    }
}
