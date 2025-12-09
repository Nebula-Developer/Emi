using System;
using System.Numerics;

namespace Emi.Mathematics;

/// <summary>
/// Extension helpers for <see cref="IVector3{TSelf, TValue}"/> implementations.
/// </summary>
public static class Vector3Extensions {
    /// <summary>
    /// Creates a copy of the vector with a new X component.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to copy</param>
    /// <param name="x">Replacement X component</param>
    /// <returns>Vector with the updated X component</returns>
    public static TSelf WithX<TSelf, TValue>(this IVector3<TSelf, TValue> vector, TValue x)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(x, vector.Y, vector.Z);

    /// <summary>
    /// Creates a copy of the vector with a new Y component.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to copy</param>
    /// <param name="y">Replacement Y component</param>
    /// <returns>Vector with the updated Y component</returns>
    public static TSelf WithY<TSelf, TValue>(this IVector3<TSelf, TValue> vector, TValue y)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(vector.X, y, vector.Z);

    /// <summary>
    /// Creates a copy of the vector with a new Z component.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to copy</param>
    /// <param name="z">Replacement Z component</param>
    /// <returns>Vector with the updated Z component</returns>
    public static TSelf WithZ<TSelf, TValue>(this IVector3<TSelf, TValue> vector, TValue z)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(vector.X, vector.Y, z);

    /// <summary>
    /// Adds two vectors component-wise.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Left operand</param>
    /// <param name="other">Vector to add</param>
    /// <returns>Component-wise sum</returns>
    public static TSelf Add<TSelf, TValue>(this IVector3<TSelf, TValue> vector, IVector3<TSelf, TValue> other)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(vector.X + other.X, vector.Y + other.Y, vector.Z + other.Z);

    /// <summary>
    /// Subtracts another vector component-wise.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Left operand</param>
    /// <param name="other">Vector to subtract</param>
    /// <returns>Component-wise difference</returns>
    public static TSelf Subtract<TSelf, TValue>(this IVector3<TSelf, TValue> vector, IVector3<TSelf, TValue> other)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(vector.X - other.X, vector.Y - other.Y, vector.Z - other.Z);

    /// <summary>
    /// Multiplies all components by a scalar.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to scale</param>
    /// <param name="scalar">Scalar multiplier</param>
    /// <returns>Scaled vector</returns>
    public static TSelf Multiply<TSelf, TValue>(this IVector3<TSelf, TValue> vector, TValue scalar)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);

    /// <summary>
    /// Divides all components by a scalar.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to scale</param>
    /// <param name="scalar">Scalar divisor</param>
    /// <returns>Scaled vector</returns>
    public static TSelf Divide<TSelf, TValue>(this IVector3<TSelf, TValue> vector, TValue scalar)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);

    /// <summary>
    /// Applies component-wise modulus using a scalar.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to reduce</param>
    /// <param name="scalar">Scalar divisor for modulus</param>
    /// <returns>Vector with component-wise modulus applied</returns>
    public static TSelf Modulus<TSelf, TValue>(this IVector3<TSelf, TValue> vector, TValue scalar)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(vector.X % scalar, vector.Y % scalar, vector.Z % scalar);

    /// <summary>
    /// Component-wise negation.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to negate</param>
    /// <returns>Negated vector</returns>
    public static TSelf Negate<TSelf, TValue>(this IVector3<TSelf, TValue> vector)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(-vector.X, -vector.Y, -vector.Z);

    /// <summary>
    /// Computes the cross product with another vector.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Left operand</param>
    /// <param name="other">Right operand</param>
    /// <returns>Cross product result</returns>
    public static TSelf Cross<TSelf, TValue>(this IVector3<TSelf, TValue> vector, IVector3<TSelf, TValue> other)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(
        (vector.Y * other.Z) - (vector.Z * other.Y),
        (vector.Z * other.X) - (vector.X * other.Z),
        (vector.X * other.Y) - (vector.Y * other.X));

    /// <summary>
    /// Computes the dot product with another vector.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Left operand</param>
    /// <param name="other">Vector to dot against</param>
    /// <returns>Dot product scalar</returns>
    public static TValue Dot<TSelf, TValue>(this IVector3<TSelf, TValue> vector, IVector3<TSelf, TValue> other)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => (vector.X * other.X) + (vector.Y * other.Y) + (vector.Z * other.Z);

    /// <summary>
    /// Returns a vector with absolute component values.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to transform</param>
    /// <returns>Vector with absolute components</returns>
    public static TSelf Abs<TSelf, TValue>(this IVector3<TSelf, TValue> vector)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(TValue.Abs(vector.X), TValue.Abs(vector.Y), TValue.Abs(vector.Z));

    /// <summary>
    /// Clamps each component between matching components of the minimum and maximum vectors.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to clamp</param>
    /// <param name="min">Component-wise minimum</param>
    /// <param name="max">Component-wise maximum</param>
    /// <returns>Clamped vector</returns>
    public static TSelf Clamp<TSelf, TValue>(this IVector3<TSelf, TValue> vector, IVector3<TSelf, TValue> min, IVector3<TSelf, TValue> max)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(
        TValue.Clamp(vector.X, min.X, max.X),
        TValue.Clamp(vector.Y, min.Y, max.Y),
        TValue.Clamp(vector.Z, min.Z, max.Z));

    /// <summary>
    /// Computes the squared length of the vector.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to measure</param>
    /// <returns>Squared magnitude</returns>
    public static double LengthSquared<TSelf, TValue>(this IVector3<TSelf, TValue> vector)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => double.CreateChecked((vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z));

    /// <summary>
    /// Computes the length of the vector.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to measure</param>
    /// <returns>Vector magnitude</returns>
    public static double Length<TSelf, TValue>(this IVector3<TSelf, TValue> vector)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => Math.Sqrt(vector.LengthSquared<TSelf, TValue>());

    /// <summary>
    /// Returns a normalized vector, or the zero vector if the length is zero.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to normalize</param>
    /// <returns>Normalized vector</returns>
    public static TSelf Normalize<TSelf, TValue>(this IVector3<TSelf, TValue> vector)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> {
        double length = vector.Length<TSelf, TValue>();
        if (length == 0d) {
            return TSelf.Create(TValue.Zero, TValue.Zero, TValue.Zero);
        }

        TValue scale = TValue.CreateTruncating(1d / length);
        return vector.Multiply(scale);
    }

    /// <summary>
    /// Reflects this vector around a provided normal.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to reflect</param>
    /// <param name="normal">Normal to reflect around</param>
    /// <returns>Reflected vector</returns>
    public static TSelf Reflect<TSelf, TValue>(this IVector3<TSelf, TValue> vector, IVector3<TSelf, TValue> normal)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> {
        double dot = double.CreateChecked(vector.Dot<TSelf, TValue>(normal));
        TValue factor = TValue.CreateTruncating(dot + dot);
        return vector.Subtract(normal.Multiply(factor));
    }

    /// <summary>
    /// Projects this vector onto the provided normal.
    /// </summary>
    /// <typeparam name="TSelf">Vector type</typeparam>
    /// <typeparam name="TValue">Component type</typeparam>
    /// <param name="vector">Vector to project</param>
    /// <param name="normal">Normal to project onto</param>
    /// <returns>Projected vector</returns>
    public static TSelf ProjectOnto<TSelf, TValue>(this IVector3<TSelf, TValue> vector, IVector3<TSelf, TValue> normal)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> {
        double lengthSq = normal.LengthSquared<TSelf, TValue>();
        if (lengthSq == 0d) {
            return TSelf.Create(TValue.Zero, TValue.Zero, TValue.Zero);
        }

        double ratio = double.CreateChecked(vector.Dot<TSelf, TValue>(normal)) / lengthSq;
        TValue scale = TValue.CreateTruncating(ratio);
        return normal.Multiply(scale);
    }

    /// <inheritdoc cref="Vector3Math.Lerp{TSelf, TValue}(TSelf, TSelf, TValue)"/>
    public static TSelf LerpTo<TSelf, TValue>(this TSelf vector, TSelf target, TValue amount)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => Vector3Math.Lerp(vector, target, amount);
}
