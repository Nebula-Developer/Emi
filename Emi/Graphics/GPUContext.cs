using Silk.NET.Core.Native;
using Silk.NET.WebGPU;

namespace Emi.Graphics;

public unsafe class GPUContext {
#nullable disable
    public WebGPU WebGPU;
    public Instance* Instance;
    public Surface* Surface;
    public Adapter* Adapter;
    public Device* Device;
    public Queue* Queue;
#nullable restore

    public GPUContext() {
        WebGPU = WebGPU.GetApi();
        InstanceDescriptor instanceDescriptor = new();
        Instance = WebGPU.CreateInstance(ref instanceDescriptor);
    }

    public void Load(Surface* surface) {
        Surface = surface;

        RequestAdapter(new() {
            CompatibleSurface = Surface,
            PowerPreference = PowerPreference.HighPerformance,
            ForceFallbackAdapter = false
        });

        RequestDevice();
        SetDeviceErrorCallback(Device);

        Queue = WebGPU.DeviceGetQueue(Device);
        PreferredSurfaceFormat = GetSurfaceFormat();
    }

    private void RequestAdapter(RequestAdapterOptions requestAdapterOptions = default) {
        string? errorMessage = null;
        using var mre = new ManualResetEvent(false);
        WebGPU.InstanceRequestAdapter(Instance, ref requestAdapterOptions, new PfnRequestAdapterCallback((status, a, message, userData) => {
            if (status != RequestAdapterStatus.Success)
                errorMessage = SilkMarshal.PtrToString((nint)message);
            else Adapter = a;
            mre.Set();
        }), null);
        mre.WaitOne();

        if (errorMessage is not null)
            throw new Exception($"Unable to create adapter: {errorMessage}");
    }

    private void RequestDevice(DeviceDescriptor deviceDescriptor = default) {
        string? errorMessage = null;
        using var mre = new ManualResetEvent(false);
        WebGPU.AdapterRequestDevice(Adapter, ref deviceDescriptor, new PfnRequestDeviceCallback((status, d, message, userData) => {
            if (status != RequestDeviceStatus.Success)
                errorMessage = SilkMarshal.PtrToString((nint)message);
            else Device = d;
            mre.Set();
        }), null);
        mre.WaitOne();

        if (errorMessage is not null)
            throw new Exception($"Unable to create device: {errorMessage}");
    }

    private void SetDeviceErrorCallback(Device* device) => WebGPU.DeviceSetUncapturedErrorCallback(device, new PfnErrorCallback(ErrorFallback), null);

    public TextureFormat PreferredSurfaceFormat;
    protected TextureFormat GetSurfaceFormat() {
        SurfaceCapabilities caps;
        WebGPU.SurfaceGetCapabilities(Surface, Adapter, &caps);
        return caps.Formats[0];
    }

    private static void ErrorFallback(ErrorType type, byte* message, void* _) => Console.WriteLine($"WebGPU Error ({type}): {SilkMarshal.PtrToString((nint)message)}");
}
