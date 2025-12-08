using Xunit;
using Emi.Graphics;
using Silk.NET.Maths;

namespace Emi.Tests;

public class CameraTests {
    [Fact]
    public void ProjectionMatrix_ShouldBeCorrectForSquareAspectRatio() {
        var camera = new Camera();
        camera.UpdateProjection(100, 100); // 1/1 ar

        var flat = camera.GetProjectionMatrixFlat();

        Assert.Equal(1.0f, flat[0]);
        Assert.Equal(1.0f, flat[5]);
        Assert.Equal(0.5f, flat[10]);
        Assert.Equal(0.5f, flat[14]);
    }
}
