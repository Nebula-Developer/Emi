using Silk.NET.WebGPU;

using Buffer = Silk.NET.WebGPU.Buffer;

namespace Emi.Graphics;

public unsafe class UniformBuffer : IDisposable {
    private readonly GPUContext _context;
    public Buffer* Buffer { get; private set; }
    public ulong Size { get; private set; }

    public UniformBuffer(GPUContext context, ulong size) {
        _context = context;
        Size = size;

        // Align size to 16 bytes as per WebGPU spec for uniforms
        ulong alignedSize = (size + 15) & ~15ul;

        BufferDescriptor desc = new() {
            Usage = BufferUsage.Uniform | BufferUsage.CopyDst,
            Size = (nuint)alignedSize,
            MappedAtCreation = false
        };

        Buffer = _context.WebGPU.DeviceCreateBuffer(_context.Device, &desc);
    }

    public void Update<T>(T data) where T : unmanaged {
        _context.WebGPU.QueueWriteBuffer(_context.Queue, Buffer, 0, &data, (nuint)sizeof(T));
    }

    public void Dispose() {
        if (Buffer != null) {
            _context.WebGPU.BufferRelease(Buffer);
            Buffer = null;
        }
        GC.SuppressFinalize(this);
    }
}
