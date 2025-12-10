using System;
using Vector3 = Emi.Mathematics.Vectors.Vector3;

namespace Emi.Mathematics;

/// <summary>
/// Represents a rotation stored as a vector and scalar component.
/// </summary>
public readonly partial record struct Quaternion {
    /// <summary>
    /// Vector component along the X axis.
    /// </summary>
    public float X { get; init; }

    /// <summary>
    /// Vector component along the Y axis.
    /// </summary>
    public float Y { get; init; }

    /// <summary>
    /// Vector component along the Z axis.
    /// </summary>
    public float Z { get; init; }

    /// <summary>
    /// Scalar component.
    /// </summary>
    public float W { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quaternion"/> struct.
    /// </summary>
    /// <param name="x">Vector component along the X axis</param>
    /// <param name="y">Vector component along the Y axis</param>
    /// <param name="z">Vector component along the Z axis</param>
    /// <param name="w">Scalar component</param>
    public Quaternion(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quaternion"/> struct from a vector part and scalar part.
    /// </summary>
    /// <param name="vectorPart">Vector portion of the quaternion</param>
    /// <param name="scalarPart">Scalar portion of the quaternion</param>
    public Quaternion(Vector3 vectorPart, float scalarPart) {
        X = vectorPart.X;
        Y = vectorPart.Y;
        Z = vectorPart.Z;
        W = scalarPart;
    }

    /// <summary>
    /// Identity rotation quaternion.
    /// </summary>
    public static Quaternion Identity { get; } = new Quaternion(0f, 0f, 0f, 1f);

    /// <summary>
    /// Quaternion with all components zero.
    /// </summary>
    public static Quaternion Zero { get; } = new Quaternion(0f, 0f, 0f, 0f);

    /// <summary>
    /// Creates a new quaternion with the provided components.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    /// <param name="w">Scalar component</param>
    /// <returns>Quaternion instance</returns>
    public static Quaternion Create(float x, float y, float z, float w) => new Quaternion(x, y, z, w);

    /// <summary>
    /// Squared length of the quaternion.
    /// </summary>
    public double LengthSquared => (double)(X * X + Y * Y + Z * Z + W * W);

    /// <summary>
    /// Length of the quaternion.
    /// </summary>
    /// <returns>Magnitude</returns>
    public double Length() => Math.Sqrt(LengthSquared);

    /// <summary>
    /// Magnitude of the quaternion.
    /// </summary>
    public double Magnitude => Length();

    /// <summary>
    /// Returns a normalized quaternion.
    /// </summary>
    public Quaternion Normalized => Normalize();

    /// <summary>
    /// Computes the dot product with another quaternion.
    /// </summary>
    /// <param name="other">Quaternion to dot against</param>
    /// <returns>Dot product</returns>
    public float Dot(Quaternion other) => (X * other.X) + (Y * other.Y) + (Z * other.Z) + (W * other.W);

    /// <summary>
    /// Returns the conjugate of this quaternion.
    /// </summary>
    public Quaternion Conjugate() => new Quaternion(-X, -Y, -Z, W);

    /// <summary>
    /// Returns the inverse of this quaternion.
    /// </summary>
    public Quaternion Inverse() {
        double lengthSq = LengthSquared;
        if (lengthSq == 0d) {
            return Identity;
        }

        double inv = 1d / lengthSq;
        return new Quaternion(
            (float)(-X * inv),
            (float)(-Y * inv),
            (float)(-Z * inv),
            (float)(W * inv));
    }

    /// <summary>
    /// Normalizes the quaternion.
    /// </summary>
    /// <returns>Normalized quaternion</returns>
    public Quaternion Normalize() {
        double length = Length();
        if (length == 0d) {
            return Identity;
        }

        float scale = (float)(1d / length);
        return Multiply(scale);
    }

    /// <summary>
    /// Adds two quaternions.
    /// </summary>
    /// <param name="other">Quaternion to add</param>
    /// <returns>Component-wise sum</returns>
    public Quaternion Add(Quaternion other) => new Quaternion(X + other.X, Y + other.Y, Z + other.Z, W + other.W);

    /// <summary>
    /// Subtracts another quaternion.
    /// </summary>
    /// <param name="other">Quaternion to subtract</param>
    /// <returns>Component-wise difference</returns>
    public Quaternion Subtract(Quaternion other) => new Quaternion(X - other.X, Y - other.Y, Z - other.Z, W - other.W);

    /// <summary>
    /// Multiplies the quaternion by a scalar.
    /// </summary>
    /// <param name="scalar">Scaling factor</param>
    /// <returns>Scaled quaternion</returns>
    public Quaternion Multiply(float scalar) => new Quaternion(X * scalar, Y * scalar, Z * scalar, W * scalar);

    /// <summary>
    /// Multiplies this quaternion with another quaternion (composition of rotations).
    /// </summary>
    /// <param name="other">Quaternion to multiply by</param>
    /// <returns>Resulting quaternion</returns>
    public Quaternion Multiply(Quaternion other) => new Quaternion(
        (W * other.X) + (X * other.W) + (Y * other.Z) - (Z * other.Y),
        (W * other.Y) - (X * other.Z) + (Y * other.W) + (Z * other.X),
        (W * other.Z) + (X * other.Y) - (Y * other.X) + (Z * other.W),
        (W * other.W) - (X * other.X) - (Y * other.Y) - (Z * other.Z));

    /// <summary>
    /// Divides the quaternion by a scalar.
    /// </summary>
    /// <param name="scalar">Divisor</param>
    /// <returns>Scaled quaternion</returns>
    public Quaternion Divide(float scalar) {
        float inv = 1f / scalar;
        return Multiply(inv);
    }

    /// <summary>
    /// Negates the quaternion.
    /// </summary>
    /// <returns>Negated quaternion</returns>
    public Quaternion Negate() => new Quaternion(-X, -Y, -Z, -W);

    /// <summary>
    /// Rotates a vector by this quaternion.
    /// </summary>
    /// <param name="vector">Vector to rotate</param>
    /// <returns>Rotated vector</returns>
    public Vector3 Rotate(Vector3 vector) {
        Quaternion vectorQuat = new Quaternion(vector, 0f);
        Quaternion rotated = this.Multiply(vectorQuat).Multiply(this.Conjugate());
        return new Vector3(rotated.X, rotated.Y, rotated.Z);
    }

    #region Operators
    /// <summary>
    /// Adds two quaternions.
    /// </summary>
    public static Quaternion operator +(Quaternion left, Quaternion right) => left.Add(right);

    /// <summary>
    /// Subtracts one quaternion from another.
    /// </summary>
    public static Quaternion operator -(Quaternion left, Quaternion right) => left.Subtract(right);

    /// <summary>
    /// Multiplies two quaternions.
    /// </summary>
    public static Quaternion operator *(Quaternion left, Quaternion right) => left.Multiply(right);

    /// <summary>
    /// Scales a quaternion by a scalar.
    /// </summary>
    public static Quaternion operator *(Quaternion quaternion, float scalar) => quaternion.Multiply(scalar);

    /// <summary>
    /// Scales a quaternion by a scalar.
    /// </summary>
    public static Quaternion operator *(float scalar, Quaternion quaternion) => quaternion.Multiply(scalar);

    /// <summary>
    /// Divides a quaternion by a scalar.
    /// </summary>
    public static Quaternion operator /(Quaternion quaternion, float scalar) => quaternion.Divide(scalar);

    /// <summary>
    /// Negates the quaternion.
    /// </summary>
    public static Quaternion operator -(Quaternion value) => value.Negate();
    #endregion

    /// <summary>
    /// Returns a string representation of the quaternion.
    /// </summary>
    public override string ToString() => $"Quaternion({X}, {Y}, {Z}, {W})";
}
