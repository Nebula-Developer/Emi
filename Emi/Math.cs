using System;

public static class MathHelpers {
    public static void MtxRotateXY(float[] result, float ax, float ay) {
        if (result.Length != 16)
            throw new ArgumentException("Matrix must be a 16-element float array.", nameof(result));


        float sx = MathF.Sin(ax);
        float cx = MathF.Cos(ax);
        float sy = MathF.Sin(ay);
        float cy = MathF.Cos(ay);

        // zero the matrix
        for (int i = 0; i < 16; i++)
            result[i] = 0f;

        result[0] = cy;
        result[2] = sy;
        result[4] = sx * sy;
        result[5] = cx;
        result[6] = -sx * cy;
        result[8] = -cx * sy;
        result[9] = sx;
        result[10] = cx * cy;
        result[15] = 1.0f;
    }


}
