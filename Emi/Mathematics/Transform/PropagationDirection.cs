
namespace Emi.Mathematics.Transform;

public enum PropagationDirection {
    /// <summary>
    /// No propagation.
    /// </summary>
    None,
    
    /// <summary>
    /// Invalidation should propagate upwards to parent elements.
    /// </summary>
    Upwards,

    /// <summary>
    /// Invalidation should propagate downwards to child elements.
    /// </summary>
    Downwards
}
