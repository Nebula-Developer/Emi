using Emi.Graphics.BgfxCS;
using System.Security.AccessControl;
using SDL;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Data;

public struct NativeWindowHandle {
    public nint Nwh;
    public nint Ndt;

    public NativeWindowHandle(nint nwh, nint ndt = default) {
        Nwh = nwh;
        Ndt = ndt;
    }
}

public static class Program {
    public static unsafe NativeWindowHandle GetNativeWindowHandle(SDL_Window* window) {
        var props = SDL3.SDL_GetWindowProperties(window);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            nint hwnd = SDL3.SDL_GetPointerProperty(props, SDL3.SDL_PROP_WINDOW_WIN32_HWND_POINTER, 0);
            return new(hwnd);
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            nint nsWindow = SDL3.SDL_GetPointerProperty(props, SDL3.SDL_PROP_WINDOW_COCOA_WINDOW_POINTER, 0);
            return new(nsWindow);
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            string? driver = SDL3.SDL_GetCurrentVideoDriver();
            if (driver == "x11") {
                nint xdisplay = SDL3.SDL_GetPointerProperty(SDL3.SDL_GetWindowProperties(window), SDL3.SDL_PROP_WINDOW_X11_DISPLAY_POINTER, 0);
                long xwindow = SDL3.SDL_GetNumberProperty(SDL3.SDL_GetWindowProperties(window), SDL3.SDL_PROP_WINDOW_X11_WINDOW_NUMBER, 0);

                if (xdisplay != nint.Zero && xwindow != 0)
                    return new((nint)xwindow, xdisplay);
            } else if (driver == "wayland") {
                nint wlDisplay = SDL3.SDL_GetPointerProperty(SDL3.SDL_GetWindowProperties(window), SDL3.SDL_PROP_WINDOW_WAYLAND_DISPLAY_POINTER, 0);
                nint wlSurface = SDL3.SDL_GetPointerProperty(SDL3.SDL_GetWindowProperties(window), SDL3.SDL_PROP_WINDOW_WAYLAND_SURFACE_POINTER, 0);

                if (wlDisplay != nint.Zero && wlSurface != nint.Zero)
                    return new(wlSurface, wlDisplay);
            }
        }

        Console.WriteLine("Warning: Unsupported platform for native window handle retrieval");

        return new(nint.Zero);
    }

    public static unsafe BgfxInitConfig CreateBgfxInit(SDL_Window* window) {
        NativeWindowHandle handle = GetNativeWindowHandle(window);

        var init = new BgfxInitConfig();
        Bgfx.InitCtor(&init);

        init.platformData = new PlatformData {
            nwh = (void*)handle.Nwh,
            ndt = (void*)handle.Ndt,
            backBuffer = null,
            backBufferDS = null,
            context = null,
            type = NativeWindowHandleType.Count
        };

        init.vendorId = 0;

        init.resolution = new RendererResolution {
            width = 1080,
            height = 720,
            reset = (uint)ResetFlags.Vsync,
            formatColor = TextureFormat.RGBA8
        };

        init.limits.maxEncoders = 4;
        init.limits.maxTransientVbSize = 6 * 1024 * 1024;
        init.limits.maxTransientIbSize = 2 * 1024 * 1024;
        init.type = RendererType.Count;

        return init;
    }

    public static unsafe SDL_Window* Init() {
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO))
            throw new Exception("Failed to initialize SDL");

        var window = SDL3.SDL_CreateWindow("BGFX with SDL3",
            1080,
            720,
            SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

        if (window == null)
            throw new Exception("Failed to create SDL window");

        var init = CreateBgfxInit(window);
        Bgfx.RenderFrame(0);

        if (!Bgfx.Init(&init))
            throw new Exception("Initialization failed");

        return window;
    }

    public static unsafe void Main() {
        var window = Init();

        int width, height;
        SDL3.SDL_GetWindowSize(window, &width, &height);

        Bgfx.Reset((uint)width, (uint)height, (uint)ResetFlags.Vsync, TextureFormat.Count);
        Bgfx.SetViewRect(0, 0, 0, (ushort)width, (ushort)height);
        Bgfx.SetViewClear(0, (ushort)(ClearFlags.Color | ClearFlags.Depth), 0x303030ff, 1.0f, 0);

        SDL_Event* @event = stackalloc SDL_Event[1];

        var renderer = Bgfx.GetRendererType();

        // Bgfx.
        Console.WriteLine($"Using bgfx renderer: {renderer}");

        while (true) {
            if (!SDL3.SDL_PollEvent(@event))
                goto render;

            var type = (SDL_EventType)@event->type;
            switch (type) {
                case SDL_EventType.SDL_EVENT_QUIT:
                    goto end;
                case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                    var resizedEvent = @event->window;
                    Bgfx.Reset((uint)resizedEvent.data1, (uint)resizedEvent.data2, (uint)ResetFlags.Vsync, TextureFormat.Count);
                    Bgfx.SetViewRect(0, 0, 0, (ushort)resizedEvent.data1, (ushort)resizedEvent.data2);
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    goto end;
            }

        render:
            Bgfx.Touch(0);

            Bgfx.DbgTextClear(0, false);
            Bgfx.DbgTextPrintf(0, 0, 0x0f, "Hello bgfx + SDL3!", "");
            Bgfx.SetDebug((uint)(BgfxDebugFlags.Text | BgfxDebugFlags.Stats));
            Bgfx.Frame(false);
        }

    end:
        Bgfx.Shutdown();
    }
}

