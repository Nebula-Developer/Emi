
using Emi.Mathematics;
using Emi.Mathematics.Transform;
using Emi.Mathematics.Vectors;

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
    /// <inheritdoc cref="Transform.Position"/>
    public Vector3 Position {
        get => Transform.Position;
        set => Transform.Position = value;
    }

    /// <inheritdoc cref="Transform.Rotation"/>
    public Quaternion Rotation {
        get => Transform.Rotation;
        set => Transform.Rotation = value;
    }

    /// <inheritdoc cref="Transform.Scale"/>
    public Vector3 Scale {
        get => Transform.Scale;
        set => Transform.Scale = value;
    }
    #endregion

    /// <summary>
    /// Convenience helper to set this element's rotation via Euler angles in degrees.
    /// </summary>
    public Vector3 EulerDegrees {
        set => Transform.EulerDegrees = value;
    }
}
