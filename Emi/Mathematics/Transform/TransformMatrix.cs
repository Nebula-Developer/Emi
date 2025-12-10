
using Emi.Core;
using Emi.Mathematics;

namespace Emi.Mathematics.Transform;

/// <summary>
/// Responsible for the compute of an <see cref="Element"/>'s transform matrices.
/// </summary>
/// <remarks>
/// This class can be extended to provide custom transform behavior for different element types.
/// </remarks>
public class TransformMatrix(Transform transform) {
    public readonly Transform Transform = transform;
    public Element Element => Transform.Element;
    public Element? Parent => Element.Parent;
    public Transform? ParentTransform => Parent?.Transform;
    public IReadOnlyCollection<Element> Children => Element.Children;

    private enum MatrixType {
        Local,
        World
    }

    public Matrix4x4 LocalMatrix => GetMatrix(MatrixType.Local);
    public Matrix4x4 WorldMatrix => GetMatrix(MatrixType.World);

    private Matrix4x4 _localMatrix = Matrix4x4.Identity;
    private Matrix4x4 _worldMatrix = Matrix4x4.Identity;

    public bool Dirty { get; protected set; } = true;
    public void Invalidate() => Dirty = true;
    public void Validate() => Dirty = false;

    private Matrix4x4 GetMatrix(MatrixType type) {
        if (Dirty) {
            _localMatrix = ComputeLocalMatrix();
            _worldMatrix = ComputeWorldMatrix(_localMatrix);
            Dirty = false;
        }

        return type == MatrixType.Local ? _localMatrix : _worldMatrix;
    }

    protected virtual Matrix4x4 ComputeLocalMatrix() {
        var scaleMat = Matrix4x4.CreateScale(Transform.Scale);

        var rot = Transform.Rotation;
        var rotationMat = Matrix4x4.CreateFromQuaternion(rot);

        var translationMat = Matrix4x4.CreateTranslation(Transform.Position);

        return scaleMat * rotationMat * translationMat;
    }

    protected virtual Matrix4x4 ComputeWorldMatrix(Matrix4x4 localMatrix) {
        var parent = ParentTransform;

        return parent is null
            ? localMatrix
            : localMatrix * parent.WorldMatrix;
    }
}
