

using Emi.Graphics.BgfxCS;

namespace Emi.Graphics;

public unsafe class Mesh {
    public VertexBufferHandle VertexBuffer;
    public IndexBufferHandle IndexBuffer;
    public VertexLayout* Layout;
    public uint VertexCount;
    public uint IndexCount;

    public Mesh(VertexBufferHandle vertexBuffer, IndexBufferHandle indexBuffer, VertexLayout* layout, uint vertexCount, uint indexCount) {
        VertexBuffer = vertexBuffer;
        IndexBuffer = indexBuffer;
        Layout = layout;
        VertexCount = vertexCount;
        IndexCount = indexCount;
    }

    public void Render(ProgramHandle program) {
        Bgfx.SetVertexBuffer(0, VertexBuffer, 0, VertexCount);
        Bgfx.SetIndexBuffer(IndexBuffer, 0, IndexCount);
        Bgfx.Submit(0, program, 0, 0);
    }
}
