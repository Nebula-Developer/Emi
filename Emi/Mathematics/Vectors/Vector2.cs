using System.Numerics;

using Emi.Mathematics;

namespace Emi.Mathematics.Vectors;

/// <summary>
/// Single-precision two-dimensional vector.
/// </summary>
public readonly record struct Vector2 : IVector2<Vector2, float> {
    /// <inheritdoc />
    public float X { get; init; }

    /// <inheritdoc />
    public float Y { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2"/> struct.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    public Vector2(float x, float y) {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2"/> struct with both
    /// components set to the same scale value.
    /// </summary>
    /// <param name="scale">Value to assign to X and Y.</param>
    public Vector2(float scale) {
        X = scale;
        Y = scale;
    }

    /// <summary>
    /// Zero vector.
    /// </summary>
    public static Vector2 Zero { get; } = new Vector2(0f, 0f);

    /// <summary>
    /// Vector with all components set to one.
    /// </summary>
    public static Vector2 One { get; } = new Vector2(1f, 1f);

    /// <summary>
    /// Unit vector along the X axis.
    /// </summary>
    public static Vector2 UnitX { get; } = new Vector2(1f, 0f);

    /// <summary>
    /// Unit vector along the Y axis.
    /// </summary>
    public static Vector2 UnitY { get; } = new Vector2(0f, 1f);

    /// <inheritdoc />
    public static Vector2 Create(float x, float y) => new Vector2(x, y);

    /// <summary>
    /// Magnitude computed in double precision.
    /// </summary>
    public double Magnitude => this.Length();

    /// <summary>
    /// Deconstructs the vector into individual components.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    public void Deconstruct(out float x, out float y) {
        x = X;
        y = Y;
    }

    #region Operators
    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">Left operand</param>
    /// <param name="right">Right operand</param>
    /// <returns>Component-wise sum</returns>
    public static Vector2 operator +(Vector2 left, Vector2 right) => left.Add(right);

    /// <summary>
    /// Subtracts one vector from another.
    /// </summary>
    /// <param name="left">Left operand</param>
    /// <param name="right">Right operand</param>
    /// <returns>Component-wise difference</returns>
    public static Vector2 operator -(Vector2 left, Vector2 right) => left.Subtract(right);

    /// <summary>
    /// Scales a vector by a scalar.
    /// </summary>
    /// <param name="left">Vector to scale</param>
    /// <param name="scalar">Scaling factor</param>
    /// <returns>Scaled vector</returns>
    public static Vector2 operator *(Vector2 left, float scalar) => left.Multiply(scalar);

    /// <summary>
    /// Scales a vector by a scalar.
    /// </summary>
    /// <param name="scalar">Scaling factor</param>
    /// <param name="right">Vector to scale</param>
    /// <returns>Scaled vector</returns>
    public static Vector2 operator *(float scalar, Vector2 right) => right.Multiply(scalar);

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="left">Vector to scale</param>
    /// <param name="scalar">Divisor</param>
    /// <returns>Scaled vector</returns>
    public static Vector2 operator /(Vector2 left, float scalar) => left.Divide(scalar);

    /// <summary>
    /// Applies component-wise modulus using a scalar divisor.
    /// </summary>
    /// <param name="left">Vector to reduce</param>
    /// <param name="scalar">Scalar divisor</param>
    /// <returns>Vector with modulus applied</returns>
    public static Vector2 operator %(Vector2 left, float scalar) => left.Modulus(scalar);

    /// <summary>
    /// Negates the vector.
    /// </summary>
    /// <param name="value">Vector to negate</param>
    /// <returns>Negated vector</returns>
    public static Vector2 operator -(Vector2 value) => value.Negate();

    public static implicit operator Vector2(Vector2i value) => new Vector2(value.X, value.Y);
    public static implicit operator Vector2(Vector2d value) => new Vector2((float)value.X, (float)value.Y);
    #endregion

    /// <summary>
    /// Computes the dot product with another vector.
    /// </summary>
    /// <param name="other">Vector to dot against</param>
    /// <returns>Dot product</returns>
    public float Dot(Vector2 other) => this.Dot<Vector2, float>(other);

    /// <summary>
    /// Returns a normalized version of this vector.
    /// </summary>
    public Vector2 Normalized => this.Normalize<Vector2, float>();

    /// <summary>
    /// Returns whether all components are finite (not NaN or Infinity).
    /// </summary>
    public bool IsFinite => float.IsFinite(X) && float.IsFinite(Y);

    #region Math Bridges
    /// <inheritdoc cref="MathE.Min{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2 Min(Vector2 left, Vector2 right) => left.Min(right);

    /// <inheritdoc cref="MathE.Max{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2 Max(Vector2 left, Vector2 right) => left.Max(right);

    /// <inheritdoc cref="MathE.Lerp{TSelf,TValue}(TSelf,TSelf,TValue)"/>
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => a.Lerp(b, t);

    /// <inheritdoc cref="MathE.Clamp{TSelf,TValue}(IVector2{TSelf,TValue},IVector2{TSelf,TValue},IVector2{TSelf,TValue})"/>
    public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) => value.Clamp(min, max);
    #endregion

    /// <inheritdoc />
    public override string ToString() => $"Vector2({X}, {Y})";
}
