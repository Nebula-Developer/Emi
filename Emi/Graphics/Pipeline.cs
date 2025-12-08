using Silk.NET.Core.Native;
using Silk.NET.WebGPU;

namespace Emi.Graphics;

public unsafe class Pipeline : IDisposable {
    private readonly GPUContext _context;
    public RenderPipeline* RenderPipeline { get; private set; }

    public Pipeline(GPUContext context, GraphicsPipelineDescription description) {
        _context = context;
        CreatePipeline(description);
    }

    private void CreatePipeline(GraphicsPipelineDescription desc) {
        // Convert VertexLayout to WebGPU format
        var attributes = stackalloc VertexAttribute[desc.VertexLayout.Attributes.Count];
        for (int i = 0; i < desc.VertexLayout.Attributes.Count; i++) {
            attributes[i] = desc.VertexLayout.Attributes[i];
        }

        VertexBufferLayout layout = new() {
            ArrayStride = desc.VertexLayout.Stride,
            StepMode = VertexStepMode.Vertex,
            AttributeCount = (uint)desc.VertexLayout.Attributes.Count,
            Attributes = attributes
        };

        byte* vsEntryPoint = (byte*)SilkMarshal.StringToPtr(desc.VertexEntryPoint);
        byte* fsEntryPoint = (byte*)SilkMarshal.StringToPtr(desc.FragmentEntryPoint);

        VertexState vertexState = new() {
            Module = desc.Shader.Module,
            EntryPoint = vsEntryPoint,
            Buffers = &layout,
            BufferCount = 1
        };

        BlendState blendState = new() {
            Color = new() {
                SrcFactor = BlendFactor.One,
                DstFactor = BlendFactor.OneMinusSrcAlpha,
                Operation = BlendOperation.Add
            },
            Alpha = new() {
                SrcFactor = BlendFactor.One,
                DstFactor = BlendFactor.OneMinusSrcAlpha,
                Operation = BlendOperation.Add
            }
        };

        ColorTargetState colorTargetState = new() {
            Format = _context.PreferredSurfaceFormat,
            Blend = &blendState,
            WriteMask = ColorWriteMask.All
        };

        FragmentState fragmentState = new() {
            Module = desc.Shader.Module,
            EntryPoint = fsEntryPoint,
            Targets = &colorTargetState,
            TargetCount = 1
        };

        RenderPipelineDescriptor descriptor = new() {
            Vertex = vertexState,
            Fragment = &fragmentState,
            Primitive = new() {
                Topology = desc.Topology,
                StripIndexFormat = IndexFormat.Undefined,
                FrontFace = desc.FrontFace,
                CullMode = desc.CullMode
            },
            Multisample = new() {
                Count = 1,
                Mask = uint.MaxValue,
                AlphaToCoverageEnabled = false
            },
        };

        RenderPipeline = _context.WebGPU.DeviceCreateRenderPipeline(_context.Device, &descriptor);

        SilkMarshal.Free((nint)vsEntryPoint);
        SilkMarshal.Free((nint)fsEntryPoint);
    }

    public void Bind(RenderPassEncoder* pass) {
        _context.WebGPU.RenderPassEncoderSetPipeline(pass, RenderPipeline);
    }

    public void Dispose() {
        if (RenderPipeline != null) {
            _context.WebGPU.RenderPipelineRelease(RenderPipeline);
            RenderPipeline = null;
        }
        GC.SuppressFinalize(this);
    }
}
