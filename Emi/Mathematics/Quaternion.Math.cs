

using Emi.Mathematics.Vectors;

namespace Emi.Mathematics;

public readonly partial record struct Quaternion {
    /// <summary>
    /// Constructs a quaternion from Euler angles given in radians.
    /// The rotation is applied in the Z (yaw), Y (pitch), X (roll) order.
    /// </summary>
    /// <param name="eulerRadians">Euler angles in radians</param>
    /// <returns>Quaternion representing the rotation</returns>
    public static Quaternion FromEulerRadians(Vector3 eulerRadians) {
        var cx = MathF.Cos(eulerRadians.X * 0.5f);
        var sx = MathF.Sin(eulerRadians.X * 0.5f);
        var cy = MathF.Cos(eulerRadians.Y * 0.5f);
        var sy = MathF.Sin(eulerRadians.Y * 0.5f);
        var cz = MathF.Cos(eulerRadians.Z * 0.5f);
        var sz = MathF.Sin(eulerRadians.Z * 0.5f);

        // Quaternion composition for yaw/z, pitch/y, roll/x.
        // Using Tait-Bryan ZYX order: q = qz * qy * qx.
        float w = (cz * cy * cx) + (sz * sy * sx);
        float x = (cz * cy * sx) - (sz * sy * cx);
        float y = (cz * sy * cx) + (sz * cy * sx);
        float z = (sz * cy * cx) - (cz * sy * sx);

        return new Quaternion(x, y, z, w).Normalize();
    }

    /// <summary>
    /// Constructs a quaternion from Euler angles given in degrees.
    /// The rotation is applied in the Z (yaw), Y (pitch), X (roll) order.
    /// </summary>
    /// <param name="eulerDegrees">Euler angles in degrees</param>
    /// <returns>Quaternion representing the rotation</returns>
    public static Quaternion FromEulerDegrees(Vector3 eulerDegrees) {
        const float ToRadians = MathF.PI / 180f;
        var radians = new Vector3(eulerDegrees.X * ToRadians, eulerDegrees.Y * ToRadians, eulerDegrees.Z * ToRadians);
        return FromEulerRadians(radians);
    }

    /// <summary>
    /// Constructs a quaternion from an axis-angle representation.
    /// </summary>
    /// <param name="axis">Rotation axis</param>
    /// <param name="angleRadians">Rotation angle in radians</param>
    /// <returns>Quaternion representing the rotation</returns>
    public static Quaternion FromAxisAngle(Vector3 axis, float angleRadians) {
        if (axis == Vector3.Zero) {
            return Quaternion.Identity;
        }

        float halfAngle = angleRadians * 0.5f;
        float sinHalf = MathF.Sin(halfAngle);
        Vector3 normalized = axis.Normalized;
        return new Quaternion(normalized * sinHalf, MathF.Cos(halfAngle));
    }

    /// <summary>
    /// Linearly interpolates between two quaternions.
    /// </summary>
    /// <param name="start">Start quaternion</param>
    /// <param name="end">End quaternion</param>
    /// <param name="amount">Interpolation amount</param>
    /// <returns>Interpolated quaternion</returns>
    public static Quaternion Lerp(Quaternion start, Quaternion end, float amount) {
        float t = Math.Clamp(amount, 0f, 1f);
        Quaternion result = new Quaternion(
            start.X + ((end.X - start.X) * t),
            start.Y + ((end.Y - start.Y) * t),
            start.Z + ((end.Z - start.Z) * t),
            start.W + ((end.W - start.W) * t));
        return result.Normalize();
    }

    /// <summary>
    /// Spherically interpolates between two quaternions.
    /// </summary>
    /// <param name="start">Start quaternion</param>
    /// <param name="end">End quaternion</param>
    /// <param name="amount">Interpolation amount</param>
    /// <returns>Interpolated quaternion</returns>
    public static Quaternion Slerp(Quaternion start, Quaternion end, float amount) {
        float t = Math.Clamp(amount, 0f, 1f);
        float dot = start.Dot(end);
        if (MathF.Abs(dot) > 0.9995f) {
            return Lerp(start, end, t);
        }

        float theta = MathF.Acos(Math.Clamp(dot, -1f, 1f));
        float sinTheta = MathF.Sin(theta);
        float scaleStart = MathF.Sin((1f - t) * theta) / sinTheta;
        float scaleEnd = MathF.Sin(t * theta) / sinTheta;
        return new Quaternion(
            (start.X * scaleStart) + (end.X * scaleEnd),
            (start.Y * scaleStart) + (end.Y * scaleEnd),
            (start.Z * scaleStart) + (end.Z * scaleEnd),
            (start.W * scaleStart) + (end.W * scaleEnd)).Normalize();
    }
}
