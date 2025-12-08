using Silk.NET.Core.Native;
using Silk.NET.WebGPU;

namespace Emi.Graphics;

public unsafe class Shader : IDisposable {
    private readonly GPUContext _context;
    public ShaderModule* Module { get; private set; }

    public Shader(GPUContext context, string source) {
        _context = context;
        CreateShaderModule(source);
    }

    private void CreateShaderModule(string shader) {
        ShaderModuleWGSLDescriptor wgslShaderDesc = new() {
            Code = (byte*)SilkMarshal.StringToPtr(shader)
        };
        wgslShaderDesc.Chain.SType = SType.ShaderModuleWgslDescriptor;

        ShaderModuleDescriptor shaderDesc = new() {
            NextInChain = (ChainedStruct*)&wgslShaderDesc
        };

        Module = _context.WebGPU.DeviceCreateShaderModule(_context.Device, &shaderDesc);
        
        SilkMarshal.Free((nint)wgslShaderDesc.Code);
    }

    public void Dispose() {
        if (Module != null) {
            _context.WebGPU.ShaderModuleRelease(Module);
            Module = null;
        }
        GC.SuppressFinalize(this);
    }
}
