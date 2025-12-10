using System.Numerics;

namespace Emi.Mathematics.Vectors;

/// <summary>
/// Contract for three-dimensional vectors.
/// </summary>
/// <typeparam name="TSelf">Self referencing vector type</typeparam>
/// <typeparam name="TValue">Numeric component of the vector</typeparam>
public interface IVector3<TSelf, TValue>
    where TSelf : struct, IVector3<TSelf, TValue>
    where TValue : INumber<TValue> {
    /// <summary>
    /// Component along the X axis.
    /// </summary>
    TValue X { get; }

    /// <summary>
    /// Component along the Y axis.
    /// </summary>
    TValue Y { get; }

    /// <summary>
    /// Component along the Z axis.
    /// </summary>
    TValue Z { get; }

    /// <summary>
    /// Creates a new vector instance from the provided components.
    /// </summary>
    /// <param name="x">Component along the X axis</param>
    /// <param name="y">Component along the Y axis</param>
    /// <param name="z">Component along the Z axis</param>
    /// <returns>Newly created vector</returns>
    static abstract TSelf Create(TValue x, TValue y, TValue z);
}
