using System;
using SystemNumerics = System.Numerics;
using SystemMatrix4x4 = System.Numerics.Matrix4x4;
using SystemVector3 = System.Numerics.Vector3;
using Vector3 = Emi.Mathematics.Vectors.Vector3;

namespace Emi.Mathematics;

/// <summary>
/// Represents a 4x4 transformation matrix.
/// </summary>
/// <remarks>
/// This struct wraps <see cref="System.Numerics.Matrix4x4"/> to provide fluid integration with the Emi mathematics library.
/// This wrapping behaviour is subject to change.
/// </remarks>
public readonly record struct Matrix4x4 {
    private readonly SystemMatrix4x4 _matrix;

    /// <summary>
    /// Component at row 1, column 1.
    /// </summary>
    public float M11 => _matrix.M11;

    /// <summary>
    /// Component at row 1, column 2.
    /// </summary>
    public float M12 => _matrix.M12;

    /// <summary>
    /// Component at row 1, column 3.
    /// </summary>
    public float M13 => _matrix.M13;

    /// <summary>
    /// Component at row 1, column 4.
    /// </summary>
    public float M14 => _matrix.M14;

    /// <summary>
    /// Component at row 2, column 1.
    /// </summary>
    public float M21 => _matrix.M21;

    /// <summary>
    /// Component at row 2, column 2.
    /// </summary>
    public float M22 => _matrix.M22;

    /// <summary>
    /// Component at row 2, column 3.
    /// </summary>
    public float M23 => _matrix.M23;

    /// <summary>
    /// Component at row 2, column 4.
    /// </summary>
    public float M24 => _matrix.M24;

    /// <summary>
    /// Component at row 3, column 1.
    /// </summary>
    public float M31 => _matrix.M31;

    /// <summary>
    /// Component at row 3, column 2.
    /// </summary>
    public float M32 => _matrix.M32;

    /// <summary>
    /// Component at row 3, column 3.
    /// </summary>
    public float M33 => _matrix.M33;

    /// <summary>
    /// Component at row 3, column 4.
    /// </summary>
    public float M34 => _matrix.M34;

    /// <summary>
    /// Component at row 4, column 1.
    /// </summary>
    public float M41 => _matrix.M41;

    /// <summary>
    /// Component at row 4, column 2.
    /// </summary>
    public float M42 => _matrix.M42;

    /// <summary>
    /// Component at row 4, column 3.
    /// </summary>
    public float M43 => _matrix.M43;

    /// <summary>
    /// Component at row 4, column 4.
    /// </summary>
    public float M44 => _matrix.M44;

    /// <summary>
    /// Identity matrix.
    /// </summary>
    public static Matrix4x4 Identity { get; } = new(SystemMatrix4x4.Identity);

    private Matrix4x4(SystemMatrix4x4 matrix) {
        _matrix = matrix;
    }

    /// <summary>
    /// Returns the transpose of this matrix.
    /// </summary>
    public Matrix4x4 Transpose() => new(SystemMatrix4x4.Transpose(_matrix));

    /// <summary>
    /// Computes the determinant of the matrix.
    /// </summary>
    public double Determinant => _matrix.GetDeterminant();

    /// <summary>
    /// Returns the inverse of this matrix, or <see cref="Identity"/> if inversion fails.
    /// </summary>
    public Matrix4x4 Inverse() {
        if (SystemMatrix4x4.Invert(_matrix, out SystemMatrix4x4 inverted)) {
            return new(inverted);
        }

        return Identity;
    }

    /// <summary>
    /// Attempts to invert the matrix.
    /// </summary>
    /// <param name="inverse">Inverted matrix if successful</param>
    /// <returns>Whether inversion succeeded</returns>
    public bool TryInvert(out Matrix4x4 inverse) {
        if (SystemMatrix4x4.Invert(_matrix, out SystemMatrix4x4 inverted)) {
            inverse = new(inverted);
            return true;
        }

        inverse = default;
        return false;
    }

    /// <summary>
    /// Transforms a point using this matrix.
    /// </summary>
    /// <param name="point">Point to transform</param>
    /// <returns>Transformed point</returns>
    public Vector3 TransformPoint(Vector3 point) {
        var systemPoint = ToSystem(point);
        var transformed = SystemVector3.Transform(systemPoint, _matrix);
        return FromSystem(transformed);
    }

    /// <summary>
    /// Transforms a direction using this matrix (ignores translation).
    /// </summary>
    /// <param name="direction">Direction to transform</param>
    /// <returns>Transformed direction</returns>
    public Vector3 TransformDirection(Vector3 direction) {
        var systemDirection = ToSystem(direction);
        var transformed = SystemVector3.TransformNormal(systemDirection, _matrix);
        return FromSystem(transformed);
    }

    /// <summary>
    /// Adds another matrix component-wise.
    /// </summary>
    /// <param name="other">Matrix to add</param>
    /// <returns>Summed matrix</returns>
    public Matrix4x4 Add(Matrix4x4 other) => new(SystemMatrix4x4.Add(_matrix, other._matrix));

    /// <summary>
    /// Subtracts another matrix component-wise.
    /// </summary>
    /// <param name="other">Matrix to subtract</param>
    /// <returns>Resulting matrix</returns>
    public Matrix4x4 Subtract(Matrix4x4 other) => new(SystemMatrix4x4.Subtract(_matrix, other._matrix));

    /// <summary>
    /// Multiplies this matrix by another.
    /// </summary>
    /// <param name="other">Matrix to multiply</param>
    /// <returns>Product matrix</returns>
    public Matrix4x4 Multiply(Matrix4x4 other) => new(SystemMatrix4x4.Multiply(_matrix, other._matrix));

    /// <summary>
    /// Multiplies the matrix by a scalar.
    /// </summary>
    /// <param name="scalar">Scalar multiplier</param>
    /// <returns>Scaled matrix</returns>
    public Matrix4x4 Multiply(float scalar) => new(SystemMatrix4x4.Multiply(_matrix, scalar));

    /// <summary>
    /// Adds two matrices component-wise.
    /// </summary>
    public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right) => left.Add(right);

    /// <summary>
    /// Subtracts one matrix from another.
    /// </summary>
    public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right) => left.Subtract(right);

    /// <summary>
    /// Multiplies two matrices.
    /// </summary>
    public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right) => left.Multiply(right);

    /// <summary>
    /// Scales a matrix by a scalar.
    /// </summary>
    public static Matrix4x4 operator *(Matrix4x4 matrix, float scalar) => matrix.Multiply(scalar);

    /// <summary>
    /// Scales a matrix by a scalar.
    /// </summary>
    public static Matrix4x4 operator *(float scalar, Matrix4x4 matrix) => matrix.Multiply(scalar);

    /// <summary>
    /// Returns a string representation of the matrix.
    /// </summary>
    public override string ToString() => _matrix.ToString();

    /// <summary>
    /// Constructs a matrix from scaling components.
    /// </summary>
    public static Matrix4x4 CreateScale(Vector3 scale) => new(SystemMatrix4x4.CreateScale(scale.X, scale.Y, scale.Z));

    /// <summary>
    /// Constructs a matrix from translation components.
    /// </summary>
    public static Matrix4x4 CreateTranslation(Vector3 translation) => new(SystemMatrix4x4.CreateTranslation(translation.X, translation.Y, translation.Z));

    /// <summary>
    /// Constructs a rotation matrix around the X axis.
    /// </summary>
    public static Matrix4x4 CreateRotationX(float radians) => new(SystemMatrix4x4.CreateRotationX(radians));

    /// <summary>
    /// Constructs a rotation matrix around the Y axis.
    /// </summary>
    public static Matrix4x4 CreateRotationY(float radians) => new(SystemMatrix4x4.CreateRotationY(radians));

    /// <summary>
    /// Constructs a rotation matrix around the Z axis.
    /// </summary>
    public static Matrix4x4 CreateRotationZ(float radians) => new(SystemMatrix4x4.CreateRotationZ(radians));

    /// <summary>
    /// Constructs a rotation matrix from a quaternion.
    /// </summary>
    public static Matrix4x4 CreateFromQuaternion(Quaternion quaternion) =>
        new(SystemMatrix4x4.CreateFromQuaternion(new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)));

    /// <summary>
    /// Creates a perspective projection matrix.
    /// </summary>
    public static Matrix4x4 CreatePerspectiveFieldOfView(float fovY, float aspectRatio, float nearPlane, float farPlane) =>
        new(SystemMatrix4x4.CreatePerspectiveFieldOfView(fovY, aspectRatio, nearPlane, farPlane));

    /// <summary>
    /// Creates an orthographic projection matrix.
    /// </summary>
    public static Matrix4x4 CreateOrthographic(float width, float height, float nearPlane, float farPlane) =>
        new(SystemMatrix4x4.CreateOrthographic(width, height, nearPlane, farPlane));

    private static SystemVector3 ToSystem(Vector3 vector) => new(vector.X, vector.Y, vector.Z);

    private static Vector3 FromSystem(SystemVector3 vector) => new(vector.X, vector.Y, vector.Z);
}
