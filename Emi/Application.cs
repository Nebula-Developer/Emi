using Emi.Graphics;

using Silk.NET.Maths;
using Silk.NET.WebGPU;
using Silk.NET.Windowing;

namespace Emi;

// TODO: Externalize / abstract windowing system
public partial class Application {
    public IWindow Window;
    public GPUContext Context;
    public Renderer Renderer;

    protected virtual void OnLoad() { }
    protected virtual void OnRender(double delta) { }
    protected virtual void OnUpdate(double delta) { }
    protected virtual void OnResize(uint width, uint height) { }

    public Application() {
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with {
            API = GraphicsAPI.None,
            IsContextControlDisabled = true,
            ShouldSwapAutomatically = false,
            VSync = true
        });
        Context = new();
        Renderer = new(Context);

        Window.Load += Load;
        Window.Render += Render;
        Window.Update += Update;
        Window.Resize += Resize;
    }

    public void Run() => Window.Run();

    protected unsafe void Load() {
        Context.Surface = Window.CreateWebGPUSurface(Context.WebGPU, Context.Instance);
        Context.Load(Context.Surface);

        Renderer.ConfigureSurface((uint)Window.Size.X, (uint)Window.Size.Y);

        OnLoad();
    }

    protected void Render(double delta) {
        if (!Renderer.BeginFrame())
            return;

        OnRender(delta);
        Renderer.EndFrame();
        Window.SwapBuffers();
    }

    protected void Update(double delta) => OnUpdate(delta);

    public void Resize(Vector2D<int> size) {
        if (size.X == 0 || size.Y == 0)
            return;

        Renderer.ConfigureSurface((uint)size.X, (uint)size.Y);
        OnResize((uint)size.X, (uint)size.Y);
    }
}
