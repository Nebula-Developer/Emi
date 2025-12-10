using System;

using Emi.Mathematics;

namespace Emi.Mathematics.Vectors;

/// <summary>
/// Integer two-dimensional vector.
/// </summary>
public readonly record struct Vector2i : IVector2<Vector2i, int> {
    /// <inheritdoc />
    public int X { get; init; }

    /// <inheritdoc />
    public int Y { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2i"/> struct.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    public Vector2i(int x, int y) {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2i"/> struct with both
    /// components set to the same scale value.
    /// </summary>
    /// <param name="scale">Value to assign to X and Y.</param>
    public Vector2i(int scale) {
        X = scale;
        Y = scale;
    }

    /// <inheritdoc cref="Vector2.Zero" />
    public static Vector2i Zero { get; } = new Vector2i(0, 0);

    /// <inheritdoc cref="Vector2.One" />
    public static Vector2i One { get; } = new Vector2i(1, 1);

    /// <inheritdoc cref="Vector2.UnitX" />
    public static Vector2i UnitX { get; } = new Vector2i(1, 0);

    /// <inheritdoc cref="Vector2.UnitY" />
    public static Vector2i UnitY { get; } = new Vector2i(0, 1);

    /// <inheritdoc />
    public static Vector2i Create(int x, int y) => new Vector2i(x, y);

    /// <inheritdoc cref="Vector2.Magnitude" />
    public double Magnitude => this.Length();

    /// <inheritdoc cref="Vector2.Deconstruct" />
    public void Deconstruct(out int x, out int y) {
        x = X;
        y = Y;
    }

    /// <inheritdoc cref="Vector2.Normalize()" />
    public Vector2i Normalize() {
        double length = this.Length();
        if (length == 0d) {
            return this;
        }

        double scale = 1d / length;
        return new Vector2i(
            (int)Math.Round(X * scale),
            (int)Math.Round(Y * scale));
    }

    /// <inheritdoc cref="Vector2.operator +(Vector2,Vector2)" />
    public static Vector2i operator +(Vector2i left, Vector2i right) => left.Add(right);

    /// <inheritdoc cref="Vector2.operator -(Vector2,Vector2)" />
    public static Vector2i operator -(Vector2i left, Vector2i right) => left.Subtract(right);

    /// <inheritdoc cref="Vector2.operator *(Vector2,float)" />
    public static Vector2i operator *(Vector2i left, int scalar) => left.Multiply(scalar);

    /// <inheritdoc cref="Vector2.operator *(float,Vector2)" />
    public static Vector2i operator *(int scalar, Vector2i right) => right.Multiply(scalar);

    /// <inheritdoc cref="Vector2.operator /(Vector2,float)" />
    public static Vector2i operator /(Vector2i left, int scalar) => left.Divide(scalar);

    /// <inheritdoc cref="Vector2.operator %(Vector2,float)" />
    public static Vector2i operator %(Vector2i left, int scalar) => left.Modulus(scalar);

    /// <inheritdoc cref="Vector2.operator -(Vector2)" />
    public static Vector2i operator -(Vector2i value) => value.Negate();

    /// <inheritdoc />
    public override string ToString() => $"Vector2i({X}, {Y})";

    public static implicit operator Vector2i(Vector2 value) => new Vector2i(
        (int)MathF.Round(value.X, MidpointRounding.AwayFromZero),
        (int)MathF.Round(value.Y, MidpointRounding.AwayFromZero));
    public static implicit operator Vector2i(Vector2d value) => new Vector2i(
        (int)Math.Round(value.X, MidpointRounding.AwayFromZero),
        (int)Math.Round(value.Y, MidpointRounding.AwayFromZero));

    /// <inheritdoc cref="Vector2.Dot(Vector2)" />
    public int Dot(Vector2i other) => this.Dot<Vector2i, int>(other);

    /// <inheritdoc cref="Vector2.Normalize()" />
    public Vector2i Normalized => this.Normalize();

    /// <inheritdoc cref="Vector2.IsFinite" />
    public bool IsFinite => true;

    #region Math Bridges
    /// <inheritdoc cref="MathE.Min{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2i Min(Vector2i left, Vector2i right) => left.Min(right);

    /// <inheritdoc cref="MathE.Max{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2i Max(Vector2i left, Vector2i right) => left.Max(right);

    /// <inheritdoc cref="MathE.Clamp{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2i Clamp(Vector2i value, Vector2i min, Vector2i max) => value.Clamp(min, max);
    #endregion
}
