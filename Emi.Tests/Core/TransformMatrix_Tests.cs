using Emi.Core;
using Emi.Mathematics;
using Emi.Mathematics.Vectors;

using Xunit;

namespace Emi.Tests.Core;

public class TransformMatrix_Tests {
    [Fact]
    public void LocalMatrix_ReflectsTranslation() {
        var element = new Element();
        element.Position = new Vector3(3f, 4f, 5f);

        var matrix = element.Transform.LocalMatrix;

        Assert.Equal(3f, matrix.M41);
        Assert.Equal(4f, matrix.M42);
        Assert.Equal(5f, matrix.M43);
        Assert.Equal(1f, matrix.M11);
        Assert.Equal(1f, matrix.M22);
        Assert.Equal(1f, matrix.M33);
    }

    [Fact]
    public void WorldMatrix_IncludesParentTranslation() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);

        parent.Position = new Vector3(1f, 0f, -10f);
        child.Position = new Vector3(1f, 2f, 10f);

        var world = child.Transform.WorldMatrix;

        Assert.Equal(2f, world.M41, 5);
        Assert.Equal(2f, world.M42, 5);
        Assert.Equal(0f, world.M43, 5);
    }

    [Fact]
    public void ChildTransform_InvalidatedWhenParentChanges() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);

        var handler = child.Transform.MatrixHandler;
        handler.Validate();

        parent.Position = new Vector3(7f, 8f, 9f);

        Assert.True(handler.Dirty);
    }

    [Fact]
    public void InvalidateAndValidate_ToggleDirtyState() {
        var element = new Element();
        var handler = element.Transform.MatrixHandler;

        handler.Validate();
        Assert.False(handler.Dirty);

        handler.Invalidate();
        Assert.True(handler.Dirty);
    }

    [Fact]
    public void WorldPosition_RespectsParentRotationAndTranslation_WhenUsingEulerDegrees() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);

        parent.Position = new Vector3(20f);
        parent.EulerDegrees = new Vector3(0f, 0f, 30f);

        child.Position = new Vector3(10f);
        child.EulerDegrees = new Vector3(0f, 0f, 15f);

        var world = child.Transform.WorldPosition;

        Assert.Equal(23.660254f, world.X, 5);
        Assert.Equal(33.660254f, world.Y, 5);
        Assert.Equal(30f, world.Z, 5);
    }

    [Fact]
    public void WorldPosition_EqualsEulerDegrees_WhenUsingQuaternions() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);

        parent.Position = new Vector3(20f);
        parent.Rotation = Quaternion.FromAxisAngle(new Vector3(0f, 0f, 1f), MathF.PI / 6f); // 30 degrees

        child.Position = new Vector3(10f);
        child.Rotation = Quaternion.FromAxisAngle(new Vector3(0f, 0f, 1f), MathF.PI / 12f); // 15 degrees

        var worldQuaternion = child.Transform.WorldPosition;

        // Recompute using Euler Degrees setter to confirm equivalence
        parent.EulerDegrees = new Vector3(0f, 0f, 30f);
        child.EulerDegrees = new Vector3(0f, 0f, 15f);

        var worldEuler = child.Transform.WorldPosition;

        Assert.Equal(worldQuaternion.X, worldEuler.X, 5);
        Assert.Equal(worldQuaternion.Y, worldEuler.Y, 5);
        Assert.Equal(worldQuaternion.Z, worldEuler.Z, 5);
    }
}
