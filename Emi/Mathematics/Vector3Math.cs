using System;
using System.Numerics;

namespace Emi.Mathematics;

/// <summary>
/// Shared math helpers for <see cref="IVector3{TSelf, TValue}"/>.
/// </summary>
public static class Vector3Math {
    /// <summary>
    /// Returns the component-wise minimum of two vectors.
    /// </summary>
    public static TSelf Min<TSelf, TValue>(TSelf a, TSelf b)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(
        TValue.Min(a.X, b.X),
        TValue.Min(a.Y, b.Y),
        TValue.Min(a.Z, b.Z));

    /// <summary>
    /// Returns the component-wise maximum of two vectors.
    /// </summary>
    public static TSelf Max<TSelf, TValue>(TSelf a, TSelf b)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => TSelf.Create(
        TValue.Max(a.X, b.X),
        TValue.Max(a.Y, b.Y),
        TValue.Max(a.Z, b.Z));

    /// <summary>
    /// Computes the squared distance between two vectors.
    /// </summary>
    public static double DistanceSquared<TSelf, TValue>(TSelf a, TSelf b)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> {
        TSelf delta = a.Subtract<TSelf, TValue>(b);
        return delta.LengthSquared<TSelf, TValue>();
    }

    /// <summary>
    /// Computes the distance between two vectors.
    /// </summary>
    public static double Distance<TSelf, TValue>(TSelf a, TSelf b)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> => Math.Sqrt(DistanceSquared<TSelf, TValue>(a, b));

    /// <summary>
    /// Linearly interpolates between two vectors with amount clamped between zero and one.
    /// </summary>
    public static TSelf Lerp<TSelf, TValue>(TSelf start, TSelf end, TValue amount)
        where TSelf : struct, IVector3<TSelf, TValue>
        where TValue : INumber<TValue> {
        TValue clamped = TValue.Clamp(amount, TValue.Zero, TValue.One);
        TSelf delta = end.Subtract(start);
        return start.Add(delta.Multiply(clamped));
    }
}
