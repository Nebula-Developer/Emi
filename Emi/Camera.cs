using System;
using System.Numerics;

using Emi.Graphics.BgfxCS;

namespace Emi.Graphics;

public class Camera {
    public Vector3 Position = new(0f, 0f, -10f);
    public Vector3 Rotation = Vector3.Zero; // pitch, yaw, roll in radians


    public float Fov = 60f;
    public float Near = 0.1f;
    public float Far = 1000f;
    public float Aspect = 16f / 9f;
    public bool Orthographic = false;
    public float OrthoHeight = 10f;

    public float[] _view = new float[16];
    public float[] _proj = new float[16];

    // Ported from bx::mtxLookAt
    public static float[] MtxLookAt(Vector3 eye, Vector3 at, Vector3 up, bool rightHanded) {
        Vector3 view = rightHanded ? Vector3.Normalize(eye - at) : Vector3.Normalize(at - eye);

        Vector3 right;
        Vector3 upVec;

        Vector3 uxv = Vector3.Cross(up, view);
        if (Math.Abs(Vector3.Dot(uxv, uxv)) < float.Epsilon)
            right = new Vector3(rightHanded ? 1.0f : -1.0f, 0.0f, 0.0f);
        else
            right = Vector3.Normalize(uxv);

        upVec = Vector3.Cross(view, right);

        float[] m = new float[16];
        m[0] = right.X; m[1] = upVec.X; m[2] = view.X; m[3] = 0f;
        m[4] = right.Y; m[5] = upVec.Y; m[6] = view.Y; m[7] = 0f;
        m[8] = right.Z; m[9] = upVec.Z; m[10] = view.Z; m[11] = 0f;
        m[12] = -Vector3.Dot(right, eye);
        m[13] = -Vector3.Dot(upVec, eye);
        m[14] = -Vector3.Dot(view, eye);
        m[15] = 1f;

        return m;
    }

    // Ported from bx::mtxProj (fovy/aspect)
    public static float[] MtxProj(float fovy, float aspect, float near, float far) {
        float f = 1f / MathF.Tan(fovy * 0.5f); // cotangent of half FOV
        float nf = 1f / (near - far);

        float[] m = new float[16];
        m[0] = f / aspect; m[1] = 0; m[2] = 0; m[3] = 0;
        m[4] = 0; m[5] = f; m[6] = 0; m[7] = 0;
        m[8] = 0; m[9] = 0; m[10] = far * nf; m[11] = -1f;
        m[12] = 0; m[13] = 0; m[14] = near * far * nf; m[15] = 0;
        return m;
    }


    private static float[] MtxProjXYWH(float x, float y, float width, float height, float near, float far, bool homogeneousNdc, bool rightHanded) {
        float diff = far - near;
        float aa = homogeneousNdc ? (far + near) / diff : far / diff;
        float bb = homogeneousNdc ? (2f * far * near) / diff : near * aa;

        float[] m = new float[16];
        m[0] = width; m[5] = height;
        m[8] = rightHanded ? x : -x;
        m[9] = rightHanded ? y : -y;
        m[10] = rightHanded ? -aa : aa;
        m[11] = rightHanded ? -1f : 1f;
        m[14] = -bb;
        // rest already zero
        return m;
    }

    public void UpdateView() {
        Vector3 forward = new(
            MathF.Sin(Rotation.Y) * MathF.Cos(Rotation.X),
            MathF.Sin(Rotation.X),
            MathF.Cos(Rotation.Y) * MathF.Cos(Rotation.X)
        );

        Vector3 lookAt = Position + forward;
        _view = MtxLookAt(Position, lookAt, Vector3.UnitY, true);
    }

    public void UpdateProj() {
        if (Orthographic) {
            float halfHeight = OrthoHeight * 0.5f;
            float halfWidth = halfHeight * Aspect;
            _proj = MtxProjXYWH(-halfWidth, -halfHeight, 2f * halfWidth, 2f * halfHeight, Near, Far, false, true);
        } else {
            _proj = MtxProj(Fov * MathF.PI / 180f, Aspect, Near, Far);
        }
    }

    public void Apply(byte viewId) {
        UpdateView();
        UpdateProj();

        unsafe {
            fixed (float* viewPtr = &_view[0])
            fixed (float* projPtr = &_proj[0]) {
                Bgfx.SetViewTransform(viewId, viewPtr, projPtr);
            }
        }
    }
}
