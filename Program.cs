using Bgfx = BgfxCS.Bgfx;
using System.Security.AccessControl;
using SDL;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Data;

public struct NativeWindowHandle
{
    public nint Nwh;
    public nint Ndt;

    public NativeWindowHandle(nint nwh, nint ndt = default)
    {
        Nwh = nwh;
        Ndt = ndt;
    }
}

public static class Program
{
    public static unsafe NativeWindowHandle GetNativeWindowHandle(SDL_Window* window)
    {
        var props = SDL3.SDL_GetWindowProperties(window);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            nint hwnd = SDL3.SDL_GetPointerProperty(props, SDL3.SDL_PROP_WINDOW_WIN32_HWND_POINTER, 0);
            return new(hwnd);
        }

        return new(nint.Zero);
    }

    public static unsafe Bgfx.Init CreateBgfxInit(SDL_Window* window)
    {
        NativeWindowHandle handle = GetNativeWindowHandle(window);

        var init = new Bgfx.Init();
        Bgfx.init_ctor(&init);

        init.platformData = new Bgfx.PlatformData
        {
            nwh = (void*)handle.Nwh,
            ndt = (void*)handle.Ndt,
            type = Bgfx.NativeWindowHandleType.Count
        };

        init.resolution = new Bgfx.Resolution
        {
            width = 1080,
            height = 720,
            reset = (uint)Bgfx.ResetFlags.Vsync
        };

        init.limits.maxEncoders = 4;
        init.limits.transientVbSize = 6 * 1024 * 1024;
        init.limits.transientIbSize = 2 * 1024 * 1024;
        init.type = Bgfx.RendererType.Count;

        return init;
    }

    public static unsafe SDL_Window* Init()
    {
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO))
            throw new Exception("Failed to initialize SDL");

        var window = SDL3.SDL_CreateWindow("BGFX with SDL3",
            1080,
            720,
            SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL_WindowFlags.SDL_WINDOW_VULKAN);

        if (window == null)
            throw new Exception("Failed to create SDL window");

        var init = CreateBgfxInit(window);
        Bgfx.render_frame(0);

        if (!Bgfx.init(&init))
            throw new Exception("Initialization failed");

        return window;
    }

    public static unsafe void Main()
    {
        var window = Init();

        var renderer = Bgfx.get_renderer_type();
        Console.WriteLine($"Using bgfx renderer: {renderer}");

        int width, height;
        SDL3.SDL_GetWindowSize(window, &width, &height);

        Bgfx.reset((uint)width, (uint)height, (uint)Bgfx.ResetFlags.Vsync, Bgfx.TextureFormat.Count);
        Bgfx.set_view_rect(0, 0, 0, (ushort)width, (ushort)height);
        Bgfx.set_view_clear(0, (ushort)(Bgfx.ClearFlags.Color | Bgfx.ClearFlags.Depth), 0x303030ff, 1.0f, 0);

        SDL_Event* @event = stackalloc SDL_Event[1];

        while (true)
        {
            if (!SDL3.SDL_PollEvent(@event))
                goto render;

            var type = (SDL_EventType)@event->type;
            switch (type)
            {
                case SDL_EventType.SDL_EVENT_QUIT:
                    goto end;
                case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                    var resizedEvent = @event->window;
                    Bgfx.reset((uint)resizedEvent.data1, (uint)resizedEvent.data2, (uint)Bgfx.ResetFlags.Vsync, Bgfx.TextureFormat.Count);
                    Bgfx.set_view_rect(0, 0, 0, (ushort)resizedEvent.data1, (ushort)resizedEvent.data2);
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    goto end;
            }

        render:
            Bgfx.touch(0);

            Bgfx.dbg_text_clear(0, false);
            Bgfx.dbg_text_printf(0, 0, 0x0f, "Hello bgfx + SDL3!", "");
            Bgfx.set_debug((uint)(Bgfx.DebugFlags.Text));
            Bgfx.frame(false);
        }

    end:
        Bgfx.shutdown();
    }
}

