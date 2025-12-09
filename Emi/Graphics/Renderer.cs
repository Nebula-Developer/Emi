using Silk.NET.WebGPU;

namespace Emi.Graphics;

public unsafe class Renderer(GPUContext context) {
    private readonly GPUContext _context = context;
    protected WebGPU WebGPU => _context.WebGPU;
    private SurfaceConfiguration _config;
    private TextureView* _currentView;

    public Color ClearColor = new(0f, 0f, 0f, 1f);
    public CommandEncoder* CurrentEncoder;
    public RenderPassEncoder* CurrentPass;    

    public void ConfigureSurface(uint width, uint height) {
        _config = new SurfaceConfiguration {
            Device = _context.Device,
            Usage = TextureUsage.RenderAttachment,
            Format = _context.PreferredSurfaceFormat,
            PresentMode = PresentMode.Fifo,
            Width = width,
            Height = height
        };

        fixed (SurfaceConfiguration* configPtr = &_config)
            WebGPU.SurfaceConfigure(_context.Surface, configPtr);
    }

    public bool BeginFrame() {
        SurfaceTexture tex = new();
        WebGPU.SurfaceGetCurrentTexture(_context.Surface, ref tex);

        if (tex.Status != SurfaceGetCurrentTextureStatus.Success || tex.Texture is null) {
            switch (tex.Status) {
                case SurfaceGetCurrentTextureStatus.Outdated:
                case SurfaceGetCurrentTextureStatus.Lost:
                    ConfigureSurface(_config.Width, _config.Height);
                    break;
                case SurfaceGetCurrentTextureStatus.OutOfMemory:
                case SurfaceGetCurrentTextureStatus.DeviceLost:
                    throw new Exception($"Surface acquisition failed: {tex.Status}");
                default:
                    // Timeout or undefined; just skip this frame
                    break;
            }

            CurrentPass = null;
            CurrentEncoder = null;
            _currentView = null;
            return false;
        }

        _currentView = WebGPU.TextureCreateView(tex.Texture, null);

        // Create encoder / begin render pass
        CommandEncoder* encoder = WebGPU.DeviceCreateCommandEncoder(_context.Device, null);
        RenderPassColorAttachment colorAttachment = new() {
            View = _currentView,
            LoadOp = LoadOp.Clear,
            StoreOp = StoreOp.Store,
            ClearValue = ClearColor
        };
        RenderPassDescriptor passDesc = new() { ColorAttachments = &colorAttachment, ColorAttachmentCount = 1 };
        CurrentPass = WebGPU.CommandEncoderBeginRenderPass(encoder, &passDesc);

        CurrentEncoder = encoder;
        return true;
    }

    public void EndFrame() {
        WebGPU.RenderPassEncoderEnd(CurrentPass);
        CommandBuffer* cmdBuf = WebGPU.CommandEncoderFinish(CurrentEncoder, null);
        WebGPU.QueueSubmit(_context.Queue, 1, &cmdBuf);
        WebGPU.TextureViewRelease(_currentView);
        WebGPU.CommandEncoderRelease(CurrentEncoder);
        WebGPU.SurfacePresent(_context.Surface);
    }
}
