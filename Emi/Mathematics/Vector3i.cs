using System;
using System.Numerics;

namespace Emi.Mathematics;

/// <summary>
/// Integer three-dimensional vector.
/// </summary>
public readonly record struct Vector3i : IVector3<Vector3i, int> {
    /// <inheritdoc />
    public int X { get; init; }

    /// <inheritdoc />
    public int Y { get; init; }

    /// <inheritdoc />
    public int Z { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3i"/> struct.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    public Vector3i(int x, int y, int z) {
        X = x;
        Y = y;
        Z = z;
    }

    /// <inheritdoc cref="Vector3.Zero" />
    public static Vector3i Zero { get; } = new Vector3i(0, 0, 0);

    /// <inheritdoc cref="Vector3.One" />
    public static Vector3i One { get; } = new Vector3i(1, 1, 1);

    /// <inheritdoc cref="Vector3.UnitX" />
    public static Vector3i UnitX { get; } = new Vector3i(1, 0, 0);

    /// <inheritdoc cref="Vector3.UnitY" />
    public static Vector3i UnitY { get; } = new Vector3i(0, 1, 0);

    /// <inheritdoc cref="Vector3.UnitZ" />
    public static Vector3i UnitZ { get; } = new Vector3i(0, 0, 1);

    /// <inheritdoc />
    public static Vector3i Create(int x, int y, int z) => new Vector3i(x, y, z);

    /// <inheritdoc cref="Vector3.Magnitude" />
    public double Magnitude => this.Length();

    /// <summary>
    /// Converts this vector to single precision.
    /// </summary>
    /// <returns>Single-precision vector</returns>
    public Vector3 ToSingle() => new Vector3(X, Y, Z);

    /// <summary>
    /// Converts this vector to double precision.
    /// </summary>
    /// <returns>Double-precision vector</returns>
    public Vector3d ToDouble() => new Vector3d(X, Y, Z);

    /// <summary>
    /// Deconstructs the vector into individual components.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    public void Deconstruct(out int x, out int y, out int z) {
        x = X;
        y = Y;
        z = Z;
    }

    /// <summary>
    /// Provides a rounded normalization that preserves integer output.
    /// </summary>
    /// <returns>Approximately normalized integer vector</returns>
    public Vector3i Normalize() {
        double length = this.Length();
        if (length == 0d) {
            return this;
        }

        double scale = 1d / length;
        return new Vector3i(
            (int)Math.Round(X * scale),
            (int)Math.Round(Y * scale),
            (int)Math.Round(Z * scale));
    }

    /// <inheritdoc cref="Vector3.operator +(Vector3,Vector3)" />
    public static Vector3i operator +(Vector3i left, Vector3i right) => left.Add(right);

    /// <inheritdoc cref="Vector3.Cross(Vector3,Vector3)" />
    public static Vector3i Cross(Vector3i left, Vector3i right) => left.Cross(right);

    /// <inheritdoc cref="Vector3.operator -(Vector3,Vector3)" />
    public static Vector3i operator -(Vector3i left, Vector3i right) => left.Subtract(right);

    /// <inheritdoc cref="Vector3.operator *(Vector3,Vector3)" />
    public static Vector3i operator *(Vector3i left, Vector3i right) => left.Cross(right);

    /// <inheritdoc cref="Vector3.operator *(Vector3,float)" />
    public static Vector3i operator *(Vector3i left, int scalar) => left.Multiply(scalar);

    /// <inheritdoc cref="Vector3.operator *(float,Vector3)" />
    public static Vector3i operator *(int scalar, Vector3i right) => right.Multiply(scalar);

    /// <inheritdoc cref="Vector3.operator /(Vector3,float)" />
    public static Vector3i operator /(Vector3i left, int scalar) => left.Divide(scalar);

    /// <inheritdoc cref="Vector3.operator %(Vector3,float)" />
    public static Vector3i operator %(Vector3i left, int scalar) => left.Modulus(scalar);

    /// <inheritdoc cref="Vector3.operator -(Vector3)" />
    public static Vector3i operator -(Vector3i value) => value.Negate();

    /// <inheritdoc />
    public override string ToString() => $"Vector3i({X}, {Y}, {Z})";
}
