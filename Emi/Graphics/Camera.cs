using Silk.NET.Maths;

namespace Emi.Graphics;

// TODO: introduce camera types (orthographic, perspective, etc.)
public class Camera {
    private Matrix4X4<float> _projection;
    public Matrix4X4<float> Projection => _projection;

    public void UpdateProjection(float width, float height) {
        float aspectRatio = width / height;

        _projection = Matrix4X4<float>.Identity;
        _projection.M11 = 1.0f / aspectRatio;
        _projection.M22 = 1.0f;
        _projection.M33 = 0.5f;
        _projection.M43 = 0.5f;
    }

    public float[] GetProjectionMatrixFlat() {
        return [
            _projection.M11, _projection.M12, _projection.M13, _projection.M14,
            _projection.M21, _projection.M22, _projection.M23, _projection.M24,
            _projection.M31, _projection.M32, _projection.M33, _projection.M34,
            _projection.M41, _projection.M42, _projection.M43, _projection.M44
        ];
    }
}
