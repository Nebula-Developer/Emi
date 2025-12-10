namespace Emi.Mathematics.Transform;

/// <summary>
/// Flags for transform properties, primarily used for tracking changes.
/// </summary>
[Flags]
public enum TransformProperties {
    /// <summary>
    /// No properties have changed.
    /// </summary>
    None = 0,

    /// <summary>
    /// Position has changed.
    /// </summary>
    Position = 1 << 0,

    /// <summary>
    /// Rotation has changed.
    /// </summary>
    Rotation = 1 << 1,

    /// <summary>
    /// Scale has changed.
    /// </summary>
    Scale = 1 << 2
}
