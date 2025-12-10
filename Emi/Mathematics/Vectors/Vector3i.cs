using System;

using Emi.Mathematics;

namespace Emi.Mathematics.Vectors;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3i"/> struct with all
    /// components set to the same scale value.
    /// </summary>
    /// <param name="scale">Value to assign to X, Y and Z.</param>
    public Vector3i(int scale) {
        X = scale;
        Y = scale;
        Z = scale;
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

    public static implicit operator Vector3i(Vector3 value) => new Vector3i(
        (int)MathF.Round(value.X, MidpointRounding.AwayFromZero),
        (int)MathF.Round(value.Y, MidpointRounding.AwayFromZero),
        (int)MathF.Round(value.Z, MidpointRounding.AwayFromZero));
    public static implicit operator Vector3i(Vector3d value) => new Vector3i(
        (int)Math.Round(value.X, MidpointRounding.AwayFromZero),
        (int)Math.Round(value.Y, MidpointRounding.AwayFromZero),
        (int)Math.Round(value.Z, MidpointRounding.AwayFromZero));

    /// <summary>
    /// Computes the dot product with another vector.
    /// </summary>
    /// <param name="other">Vector to dot against</param>
    /// <returns>Dot product</returns>
    public int Dot(Vector3i other) => this.Dot<Vector3i, int>(other);

    /// <summary>
    /// Returns a normalized version of this vector.
    /// </summary>
    public Vector3i Normalized => this.Normalize();

    /// <summary>
    /// Returns whether all components are finite. Always true for integers.
    /// </summary>
    public bool IsFinite => true;

    #region Math Bridges
    /// <inheritdoc cref="MathE.Min{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3i Min(Vector3i left, Vector3i right) => left.Min(right);

    /// <inheritdoc cref="MathE.Max{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3i Max(Vector3i left, Vector3i right) => left.Max(right);

    /// <inheritdoc cref="MathE.Clamp{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3i Clamp(Vector3i value, Vector3i min, Vector3i max) => value.Clamp(min, max);
    #endregion
}
