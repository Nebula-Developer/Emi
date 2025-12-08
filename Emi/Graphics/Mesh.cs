using Silk.NET.WebGPU;
using Buffer = Silk.NET.WebGPU.Buffer;

namespace Emi.Graphics;

public unsafe class Mesh : IDisposable {
    private readonly GPUContext _context;
    public Buffer* VertexBuffer { get; private set; }
    public uint VertexCount { get; private set; }

    public Mesh(GPUContext context, float[] vertices) {
        _context = context;
        CreateVertexBuffer(vertices);
        VertexCount = (uint)(vertices.Length / 3); 
    }

    public Mesh(GPUContext context, float[] vertices, uint vertexCount) {
        _context = context;
        CreateVertexBuffer(vertices);
        VertexCount = vertexCount;
    }

    private void CreateVertexBuffer(float[] vertices) {
        BufferDescriptor vertexBufferDesc = new() {
            Usage = BufferUsage.Vertex | BufferUsage.CopyDst,
            Size = (nuint)(vertices.Length * sizeof(float)),
            MappedAtCreation = true
        };

        VertexBuffer = _context.WebGPU.DeviceCreateBuffer(_context.Device, &vertexBufferDesc);
        
        void* mappedPtr = _context.WebGPU.BufferGetMappedRange(VertexBuffer, 0, (nuint)vertexBufferDesc.Size);
        for (int i = 0; i < vertices.Length; i++)
            ((float*)mappedPtr)[i] = vertices[i];
        
        _context.WebGPU.BufferUnmap(VertexBuffer);
    }

    public void Draw(RenderPassEncoder* pass) {
        _context.WebGPU.RenderPassEncoderSetVertexBuffer(pass, 0, VertexBuffer, 0, nuint.MaxValue);
        _context.WebGPU.RenderPassEncoderDraw(pass, VertexCount, 1, 0, 0);
    }

    public void Dispose() {
        if (VertexBuffer != null) {
            _context.WebGPU.BufferRelease(VertexBuffer);
            VertexBuffer = null;
        }
        GC.SuppressFinalize(this);
    }
}
