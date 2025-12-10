using System.Numerics;

using Emi.Mathematics;

namespace Emi.Mathematics.Vectors;

/// <summary>
/// Single-precision three-dimensional vector.
/// </summary>
public readonly record struct Vector3 : IVector3<Vector3, float> {
    /// <inheritdoc />
    public float X { get; init; }

    /// <inheritdoc />
    public float Y { get; init; }

    /// <inheritdoc />
    public float Z { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    public Vector3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3"/> struct with all
    /// components set to the same scale value.
    /// </summary>
    /// <param name="scale">Value to assign to X, Y and Z.</param>
    public Vector3(float scale) {
        X = scale;
        Y = scale;
        Z = scale;
    }

    /// <summary>
    /// Zero vector.
    /// </summary>
    public static Vector3 Zero { get; } = new Vector3(0f, 0f, 0f);

    /// <summary>
    /// Vector with all components set to one.
    /// </summary>
    public static Vector3 One { get; } = new Vector3(1f, 1f, 1f);

    /// <summary>
    /// Unit vector along the X axis.
    /// </summary>
    public static Vector3 UnitX { get; } = new Vector3(1f, 0f, 0f);

    /// <summary>
    /// Unit vector along the Y axis.
    /// </summary>
    public static Vector3 UnitY { get; } = new Vector3(0f, 1f, 0f);

    /// <summary>
    /// Unit vector along the Z axis.
    /// </summary>
    public static Vector3 UnitZ { get; } = new Vector3(0f, 0f, 1f);

    /// <inheritdoc />
    public static Vector3 Create(float x, float y, float z) => new Vector3(x, y, z);

    /// <summary>
    /// Magnitude computed in double precision.
    /// </summary>
    public double Magnitude => this.Length();

    /// <summary>
    /// Deconstructs the vector into individual components.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    public void Deconstruct(out float x, out float y, out float z) {
        x = X;
        y = Y;
        z = Z;
    }

    #region Operators
    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">Left operand</param>
    /// <param name="right">Right operand</param>
    /// <returns>Component-wise sum</returns>
    public static Vector3 operator +(Vector3 left, Vector3 right) => left.Add(right);

    /// <summary>
    /// Computes the cross product of two vectors.
    /// </summary>
    /// <param name="left">Left operand</param>
    /// <param name="right">Right operand</param>
    /// <returns>Cross product result</returns>
    public static Vector3 Cross(Vector3 left, Vector3 right) => left.Cross(right);

    /// <summary>
    /// Subtracts one vector from another.
    /// </summary>
    /// <param name="left">Left operand</param>
    /// <param name="right">Right operand</param>
    /// <returns>Component-wise difference</returns>
    public static Vector3 operator -(Vector3 left, Vector3 right) => left.Subtract(right);

    /// <summary>
    /// Scales a vector by a scalar.
    /// </summary>
    /// <param name="left">Vector to scale</param>
    /// <param name="scalar">Scaling factor</param>
    /// <returns>Scaled vector</returns>
    public static Vector3 operator *(Vector3 left, float scalar) => left.Multiply(scalar);

    /// <summary>
    /// Scales a vector by a scalar.
    /// </summary>
    /// <param name="scalar">Scaling factor</param>
    /// <param name="right">Vector to scale</param>
    /// <returns>Scaled vector</returns>
    public static Vector3 operator *(float scalar, Vector3 right) => right.Multiply(scalar);

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="left">Vector to scale</param>
    /// <param name="scalar">Divisor</param>
    /// <returns>Scaled vector</returns>
    public static Vector3 operator /(Vector3 left, float scalar) => left.Divide(scalar);

    /// <summary>
    /// Applies component-wise modulus using a scalar divisor.
    /// </summary>
    /// <param name="left">Vector to reduce</param>
    /// <param name="scalar">Scalar divisor</param>
    /// <returns>Vector with modulus applied</returns>
    public static Vector3 operator %(Vector3 left, float scalar) => left.Modulus(scalar);

    /// <summary>
    /// Negates the vector.
    /// </summary>
    /// <param name="value">Vector to negate</param>
    /// <returns>Negated vector</returns>
    public static Vector3 operator -(Vector3 value) => value.Negate();

    public static implicit operator Vector3(Vector3i value) => new Vector3(value.X, value.Y, value.Z);
    public static implicit operator Vector3(Vector3d value) => new Vector3((float)value.X, (float)value.Y, (float)value.Z);
    #endregion

    /// <summary>
    /// Computes the dot product with another vector.
    /// </summary>
    /// <param name="other">Vector to dot against</param>
    /// <returns>Dot product</returns>
    public float Dot(Vector3 other) => this.Dot<Vector3, float>(other);

    /// <summary>
    /// Returns a normalized version of this vector.
    /// </summary>
    public Vector3 Normalized => this.Normalize<Vector3, float>();

    /// <summary>
    /// Returns whether all components are finite (not NaN or Infinity).
    /// </summary>
    public bool IsFinite => float.IsFinite(X) && float.IsFinite(Y) && float.IsFinite(Z);

    #region Math Bridges
    /// <inheritdoc cref="MathE.Min{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3 Min(Vector3 left, Vector3 right) => left.Min(right);

    /// <inheritdoc cref="MathE.Max{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3 Max(Vector3 left, Vector3 right) => left.Max(right);

    /// <inheritdoc cref="MathE.Lerp{TSelf,TValue}(TSelf,TSelf,TValue)"/>
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a.Lerp(b, t);

    /// <inheritdoc cref="MathE.Clamp{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) => value.Clamp(min, max);
    #endregion

    /// <inheritdoc />
    public override string ToString() => $"Vector3({X}, {Y}, {Z})";
}
