
using Emi.Mathematics;

namespace Emi.Core;

/// <summary>
/// Represents the transform of an element.
/// </summary>
/// <remarks>
/// This class is intended to encapsulate transform-related properties (position, rotation, scale, etc.)
/// and also to manage the computation associated with these properties, such as local and world matrices.
/// </remarks>
/// <param name="owner">The element that owns this transform</param>
public class Transform(Element owner) {
    /// <summary>
    /// The element that owns this transform.
    /// </summary>
    public readonly Element Element = owner;    
}