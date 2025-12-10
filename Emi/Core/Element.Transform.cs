
using Emi.Mathematics;

namespace Emi.Core;

public partial class Element {
    /// <summary>
    /// The transform associated with this element.
    /// </summary>
    public Transform Transform { get; }

    /// <summary>
    /// Creates an instance of the appropriate transform for this element.
    /// </summary>
    protected virtual Transform CreateTransform() => new(this);

    #region Transform Properties

    #endregion
}
