using System;
using System.Numerics;

namespace Emi.Mathematics;

/// <summary>
/// Double-precision three-dimensional vector.
/// </summary>
public readonly record struct Vector3d : IVector3<Vector3d, double> {
    /// <inheritdoc />
    public double X { get; init; }

    /// <inheritdoc />
    public double Y { get; init; }

    /// <inheritdoc />
    public double Z { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3d"/> struct.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    public Vector3d(double x, double y, double z) {
        X = x;
        Y = y;
        Z = z;
    }

    /// <inheritdoc cref="Vector3.Zero" />
    public static Vector3d Zero { get; } = new Vector3d(0d, 0d, 0d);

    /// <inheritdoc cref="Vector3.One" />
    public static Vector3d One { get; } = new Vector3d(1d, 1d, 1d);

    /// <inheritdoc cref="Vector3.UnitX" />
    public static Vector3d UnitX { get; } = new Vector3d(1d, 0d, 0d);

    /// <inheritdoc cref="Vector3.UnitY" />
    public static Vector3d UnitY { get; } = new Vector3d(0d, 1d, 0d);

    /// <inheritdoc cref="Vector3.UnitZ" />
    public static Vector3d UnitZ { get; } = new Vector3d(0d, 0d, 1d);

    /// <inheritdoc />
    public static Vector3d Create(double x, double y, double z) => new Vector3d(x, y, z);

    /// <inheritdoc cref="Vector3.Magnitude" />
    public double Magnitude => this.Length();

    /// <summary>
    /// Converts this vector to single precision.
    /// </summary>
    /// <returns>Single-precision vector</returns>
    public Vector3 ToSingle() => new Vector3((float)X, (float)Y, (float)Z);

    /// <summary>
    /// Converts this vector to an integer vector by rounding components.
    /// </summary>
    /// <returns>Integer vector with rounded components</returns>
    public Vector3i ToInt32() => new Vector3i((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Z));

    /// <summary>
    /// Deconstructs the vector into individual components.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    public void Deconstruct(out double x, out double y, out double z) {
        x = X;
        y = Y;
        z = Z;
    }

    /// <summary>
    /// Implicitly converts an integer vector to double precision.
    /// </summary>
    /// <param name="value">Integer vector to convert</param>
    public static implicit operator Vector3d(Vector3i value) => new Vector3d(value.X, value.Y, value.Z);

    /// <summary>
    /// Implicitly converts a single-precision vector to double precision.
    /// </summary>
    /// <param name="value">Single-precision vector to convert</param>
    public static implicit operator Vector3d(Vector3 value) => new Vector3d(value.X, value.Y, value.Z);

    /// <inheritdoc cref="Vector3.operator +(Vector3,Vector3)" />
    public static Vector3d operator +(Vector3d left, Vector3d right) => left.Add(right);

    /// <inheritdoc cref="Vector3.Cross(Vector3,Vector3)" />
    public static Vector3d Cross(Vector3d left, Vector3d right) => left.Cross(right);

    /// <inheritdoc cref="Vector3.operator -(Vector3,Vector3)" />
    public static Vector3d operator -(Vector3d left, Vector3d right) => left.Subtract(right);

    /// <inheritdoc cref="Vector3.operator *(Vector3,Vector3)" />
    public static Vector3d operator *(Vector3d left, Vector3d right) => left.Cross(right);

    /// <inheritdoc cref="Vector3.operator *(Vector3,float)" />
    public static Vector3d operator *(Vector3d left, double scalar) => left.Multiply(scalar);

    /// <inheritdoc cref="Vector3.operator *(float,Vector3)" />
    public static Vector3d operator *(double scalar, Vector3d right) => right.Multiply(scalar);

    /// <inheritdoc cref="Vector3.operator /(Vector3,float)" />
    public static Vector3d operator /(Vector3d left, double scalar) => left.Divide(scalar);

    /// <inheritdoc cref="Vector3.operator %(Vector3,float)" />
    public static Vector3d operator %(Vector3d left, double scalar) => left.Modulus(scalar);

    /// <inheritdoc cref="Vector3.operator -(Vector3)" />
    public static Vector3d operator -(Vector3d value) => value.Negate();

    /// <inheritdoc />
    public override string ToString() => $"Vector3d({X}, {Y}, {Z})";
}
