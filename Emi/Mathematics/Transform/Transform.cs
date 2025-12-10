
using Emi.Core;
using Emi.Mathematics;
using Emi.Mathematics.Vectors;

namespace Emi.Mathematics.Transform;

/// <summary>
/// Represents the transform of an element.
/// </summary>
/// <remarks>
/// This class is intended to encapsulate transform-related properties (position, rotation, scale, etc.)
/// and also to manage the computation associated with these properties, such as local and world matrices.
/// </remarks>
public class Transform {
    /// <summary>
    /// Initializes a new instance of the <see cref="Transform"/> class.
    /// </summary>
    /// <param name="owner">The element that owns this transform</param>
    public Transform(Element owner) {
        Element = owner;
        MatrixHandler = new TransformMatrix(this);
    }
    
    /// <summary>
    /// The element that owns this transform.
    /// </summary>
    public readonly Element Element;

    /// <summary>
    /// Matrix computation handler.
    /// </summary>
    public TransformMatrix MatrixHandler { get; }

    /// <summary>
    /// The <see cref="TransformMatrix.WorldMatrix"/> of this transform.
    /// </summary>
    public Matrix4x4 WorldMatrix => MatrixHandler.WorldMatrix;

    /// <summary>
    /// The <see cref="TransformMatrix.LocalMatrix"/> of this transform.
    /// </summary>
    public Matrix4x4 LocalMatrix => MatrixHandler.LocalMatrix;

    #region Transform Properties
    /// <summary>
    /// The local position of the element.
    /// </summary>
    public Vector3 Position {
        get => _position;
        set {
            _position = value;
            Invalidate(TransformProperties.Position);
        }
    }
    private Vector3 _position = Vector3.Zero;

    /// <summary>
    /// The local rotation of the element as a quaternion.
    /// </summary>
    public Quaternion Rotation {
        get => _rotation;
        set {
            // Normalize incoming quaternion to avoid accidental invalid rotations set by callers.
            // This helps prevent unexpected transforms when users try to set Euler-like values using the quaternion constructor.
            var q = value;
            double diff = Math.Abs(q.Length() - 1d);
            if (diff > 1e-3) {
                q = q.Normalize();
            }
            _rotation = q;
            Invalidate(TransformProperties.Rotation);
        }
    }
    private Quaternion _rotation = Quaternion.Identity;

    /// <summary>
    /// The local scale of the element.
    /// </summary>
    public Vector3 Scale {
        get => _scale;
        set {
            _scale = value;
            Invalidate(TransformProperties.Scale);
        }
    }
    private Vector3 _scale = Vector3.One;
    #endregion

    #region World Properties
    /// <summary>
    /// The world position of the element.
    /// </summary>
    public Vector3 WorldPosition {
        get => WorldMatrix.TransformPoint(Vector3.Zero);
    }

    /// <summary>
    /// The local rotation of the element represented as Euler angles in degrees.
    /// Setting this will convert the provided Euler angles into a quaternion and assign it to <see cref="Rotation"/>.
    /// </summary>
    public Vector3 EulerDegrees {
        set => Rotation = Quaternion.FromEulerDegrees(value);
    }
    #endregion

    // defaults to downwards propagation for now until we have invalidation control
    public void Invalidate(TransformProperties properties, PropagationDirection direction = PropagationDirection.Downwards) {
        // TODO: fine-grained invalidation based on dependencies
        MatrixHandler.Invalidate();

        if (direction == PropagationDirection.Upwards && Element.Parent is not null)
            Element.Parent.Transform.Invalidate(properties, direction);
        if (direction == PropagationDirection.Downwards)
            Element.ForEachChild(child => child.Transform.Invalidate(properties, direction));
    }
}
