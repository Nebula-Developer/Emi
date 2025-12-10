using Emi.Mathematics;

namespace Emi.Mathematics.Vectors;

/// <summary>
/// Double-precision two-dimensional vector.
/// </summary>
public readonly record struct Vector2d : IVector2<Vector2d, double> {
    /// <inheritdoc />
    public double X { get; init; }

    /// <inheritdoc />
    public double Y { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2d"/> struct.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    public Vector2d(double x, double y) {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2d"/> struct with both
    /// components set to the same scale value.
    /// </summary>
    /// <param name="scale">Value to assign to X and Y.</param>
    public Vector2d(double scale) {
        X = scale;
        Y = scale;
    }

    /// <inheritdoc cref="Vector2.Zero" />
    public static Vector2d Zero { get; } = new Vector2d(0d, 0d);

    /// <inheritdoc cref="Vector2.One" />
    public static Vector2d One { get; } = new Vector2d(1d, 1d);

    /// <inheritdoc cref="Vector2.UnitX" />
    public static Vector2d UnitX { get; } = new Vector2d(1d, 0d);

    /// <inheritdoc cref="Vector2.UnitY" />
    public static Vector2d UnitY { get; } = new Vector2d(0d, 1d);

    /// <inheritdoc />
    public static Vector2d Create(double x, double y) => new Vector2d(x, y);

    /// <inheritdoc cref="Vector2.Magnitude" />
    public double Magnitude => this.Length();

    /// <inheritdoc cref="Vector2.Deconstruct" />
    public void Deconstruct(out double x, out double y) {
        x = X;
        y = Y;
    }

    /// <inheritdoc cref="Vector2.operator +(Vector2,Vector2)" />
    public static Vector2d operator +(Vector2d left, Vector2d right) => left.Add(right);

    /// <inheritdoc cref="Vector2.operator -(Vector2,Vector2)" />
    public static Vector2d operator -(Vector2d left, Vector2d right) => left.Subtract(right);

    /// <inheritdoc cref="Vector2.operator *(Vector2,float)" />
    public static Vector2d operator *(Vector2d left, double scalar) => left.Multiply(scalar);

    /// <inheritdoc cref="Vector2.operator *(float,Vector2)" />
    public static Vector2d operator *(double scalar, Vector2d right) => right.Multiply(scalar);

    /// <inheritdoc cref="Vector2.operator /(Vector2,float)" />
    public static Vector2d operator /(Vector2d left, double scalar) => left.Divide(scalar);

    /// <inheritdoc cref="Vector2.operator %(Vector2,float)" />
    public static Vector2d operator %(Vector2d left, double scalar) => left.Modulus(scalar);

    /// <inheritdoc cref="Vector2.operator -(Vector2)" />
    public static Vector2d operator -(Vector2d value) => value.Negate();

    /// <inheritdoc />
    public override string ToString() => $"Vector2d({X}, {Y})";

    public static implicit operator Vector2d(Vector2i value) => new Vector2d(value.X, value.Y);
    public static implicit operator Vector2d(Vector2 value) => new Vector2d(value.X, value.Y);

    /// <inheritdoc cref="Vector2.Dot(Vector2)" />
    public double Dot(Vector2d other) => this.Dot<Vector2d, double>(other);

    /// <inheritdoc cref="Vector2.Normalize()" />
    public Vector2d Normalized => this.Normalize<Vector2d, double>();

    /// <inheritdoc cref="Vector2.IsFinite" />
    public bool IsFinite => double.IsFinite(X) && double.IsFinite(Y);

    #region Math Bridges
    /// <inheritdoc cref="MathE.Min{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2d Min(Vector2d left, Vector2d right) => left.Min(right);

    /// <inheritdoc cref="MathE.Max{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2d Max(Vector2d left, Vector2d right) => left.Max(right);

    /// <inheritdoc cref="MathE.Lerp{TSelf,TValue}(TSelf,TSelf,TValue)"/>
    public static Vector2d Lerp(Vector2d a, Vector2d b, double t) => a.Lerp(b, t);

    /// <inheritdoc cref="MathE.Clamp{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2d Clamp(Vector2d value, Vector2d min, Vector2d max) => value.Clamp(min, max);
    #endregion
}
