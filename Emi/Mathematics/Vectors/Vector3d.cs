using Emi.Mathematics;

namespace Emi.Mathematics.Vectors;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3d"/> struct with all
    /// components set to the same scale value.
    /// </summary>
    /// <param name="scale">Value to assign to X, Y and Z.</param>
    public Vector3d(double scale) {
        X = scale;
        Y = scale;
        Z = scale;
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

    /// <inheritdoc cref="Vector3.Deconstruct" />
    public void Deconstruct(out double x, out double y, out double z) {
        x = X;
        y = Y;
        z = Z;
    }

    /// <inheritdoc cref="Vector3.operator +(Vector3,Vector3)" />
    public static Vector3d operator +(Vector3d left, Vector3d right) => left.Add(right);

    /// <inheritdoc cref="Vector3.Cross(Vector3,Vector3)" />
    public static Vector3d Cross(Vector3d left, Vector3d right) => left.Cross(right);

    /// <inheritdoc cref="Vector3.operator -(Vector3,Vector3)" />
    public static Vector3d operator -(Vector3d left, Vector3d right) => left.Subtract(right);

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

    public static implicit operator Vector3d(Vector3i value) => new Vector3d(value.X, value.Y, value.Z);
    public static implicit operator Vector3d(Vector3 value) => new Vector3d(value.X, value.Y, value.Z);

    /// <summary>
    /// Computes the dot product with another vector.
    /// </summary>
    /// <param name="other">Vector to dot against</param>
    /// <returns>Dot product</returns>
    public double Dot(Vector3d other) => this.Dot<Vector3d, double>(other);

    /// <summary>
    /// Returns a normalized version of this vector.
    /// </summary>
    public Vector3d Normalized => this.Normalize<Vector3d, double>();

    /// <summary>
    /// Returns whether all components are finite (not NaN or Infinity).
    /// </summary>
    public bool IsFinite => double.IsFinite(X) && double.IsFinite(Y) && double.IsFinite(Z);

    #region Math Bridges
    /// <inheritdoc cref="MathE.Min{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3d Min(Vector3d left, Vector3d right) => left.Min(right);

    /// <inheritdoc cref="MathE.Max{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3d Max(Vector3d left, Vector3d right) => left.Max(right);

    /// <inheritdoc cref="MathE.Lerp{TSelf,TValue}(TSelf,TSelf,TValue)"/>
    public static Vector3d Lerp(Vector3d a, Vector3d b, double t) => a.Lerp(b, t);

    /// <inheritdoc cref="MathE.Clamp{TSelf,TValue}(IVector3{TSelf,TValue},IVector3{TSelf,TValue},IVector3{TSelf,TValue})"/>
    public static Vector3d Clamp(Vector3d value, Vector3d min, Vector3d max) => value.Clamp(min, max);
    #endregion
}
