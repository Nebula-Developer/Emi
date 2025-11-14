using System;
using System.Numerics;

using Emi.Graphics.BgfxCS;

namespace Emi.Graphics;

public static class MeshFactory {
    public unsafe static Mesh CreateCube(float size) {
        float half = size * 0.5f;

        // Define vertices (position + normal)
        float[] vertices = new float[] {
        // Front face
        -half, -half,  half,  0, 0, 1,
         half, -half,  half,  0, 0, 1,
         half,  half,  half,  0, 0, 1,
        -half,  half,  half,  0, 0, 1,

        // Back face
        -half, -half, -half,  0, 0, -1,
        -half,  half, -half,  0, 0, -1,
         half,  half, -half,  0, 0, -1,
         half, -half, -half,  0, 0, -1,

        // Left face
        -half, -half, -half, -1, 0, 0,
        -half, -half,  half, -1, 0, 0,
        -half,  half,  half, -1, 0, 0,
        -half,  half, -half, -1, 0, 0,

        // Right face
         half, -half, -half, 1, 0, 0,
         half,  half, -half, 1, 0, 0,
         half,  half,  half, 1, 0, 0,
         half, -half,  half, 1, 0, 0,

        // Top face
        -half,  half, -half, 0, 1, 0,
        -half,  half,  half, 0, 1, 0,
         half,  half,  half, 0, 1, 0,
         half,  half, -half, 0, 1, 0,

        // Bottom face
        -half, -half, -half, 0, -1, 0,
         half, -half, -half, 0, -1, 0,
         half, -half,  half, 0, -1, 0,
        -half, -half,  half, 0, -1, 0,
    };

        // Define indices
        ushort[] indices = new ushort[] {
        0, 1, 2, 2, 3, 0,       // Front
        4, 5, 6, 6, 7, 4,       // Back
        8, 9,10,10,11, 8,       // Left
       12,13,14,14,15,12,       // Right
       16,17,18,18,19,16,       // Top
       20,21,22,22,23,20        // Bottom
    };

        // Create vertex layout
        VertexLayout* layout = stackalloc VertexLayout[1];
        Bgfx.VertexLayoutBegin(layout, RendererType.Noop);
        Bgfx.VertexLayoutAdd(layout, Attrib.Position, 3, AttribType.Float, false, false);
        Bgfx.VertexLayoutAdd(layout, Attrib.Normal, 3, AttribType.Float, false, false);
        Bgfx.VertexLayoutEnd(layout);

        // Create vertex buffer
        fixed (float* v = vertices)
        fixed (ushort* i = indices) {
            Memory* indexMemory = Bgfx.Copy(i, (uint)(indices.Length * sizeof(ushort)));
            Memory* vertexMemory = Bgfx.Copy(v, (uint)(vertices.Length * sizeof(float)));

            VertexBufferHandle vbh = Bgfx.CreateVertexBuffer(vertexMemory, layout, (ushort)(vertices.Length / 6));
            IndexBufferHandle ibh = Bgfx.CreateIndexBuffer(indexMemory, 0);
            
            return new Mesh(vbh, ibh, layout, (uint)(vertices.Length / 6), (uint)indices.Length);
        }
    }
}
