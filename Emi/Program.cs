using Emi.Graphics.BgfxCS;
using SDL;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Emi.Graphics;
using System.Threading.Tasks.Dataflow;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Emi;

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
            reset = (uint)ResetFlags.MsaaX2,
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

        Bgfx.Reset((uint)width, (uint)height, (uint)(ResetFlags.MsaaX2 | ResetFlags.Vsync), TextureFormat.Count);
        Bgfx.SetViewRect(0, 0, 0, (ushort)width, (ushort)height);
        Bgfx.SetViewClear(0, (ushort)(ClearFlags.Color | ClearFlags.Depth), 0x303030ff, 1.0f, 0);

        SDL_Event* @event = stackalloc SDL_Event[1];

        var renderer = Bgfx.GetRendererType();

        Mesh cube = MeshFactory.CreateCube(1.0f);
        ProgramHandle program = DiskShaderLoader.LoadShaderProgram("fs_shader.bin", "vs_shader.bin");
        if (!program.Valid) {
            Console.WriteLine("Shader program invalid. Exiting.");
            Bgfx.Shutdown();
            return;
        }

        Camera camera = new() {
            Position = new Vector3(0, 0, -10),
            Rotation = Vector3.Zero,
            Fov = 60f,
            Near = 0.1f,
            Far = 100f,
            Aspect = (float)width / height
        };

        Vector3 cubePosition = Vector3.Zero;
        Quaternion cubeRotation = Quaternion.Identity;

        Stopwatch stopwatch = Stopwatch.StartNew();

        Vector3 targetPos = new(0, 0, 0);

        while (true) {
            float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();
            float fps = 1.0f / deltaTime;

            if (!SDL3.SDL_PollEvent(@event))
                goto render;

            var type = (SDL_EventType)@event->type;
            switch (type) {
                case SDL_EventType.SDL_EVENT_QUIT:
                    goto end;
                case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                    var resizedEvent = @event->window;
                    Bgfx.Reset((uint)resizedEvent.data1, (uint)resizedEvent.data2, (uint)(ResetFlags.MsaaX2 | ResetFlags.Vsync), TextureFormat.Count);
                    Bgfx.SetViewRect(0, 0, 0, (ushort)resizedEvent.data1, (ushort)resizedEvent.data2);
                    camera.Aspect = (float)resizedEvent.data1 / resizedEvent.data2;
                    break;
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                    goto end;

                case SDL_EventType.SDL_EVENT_KEY_DOWN:
                    var key = @event->key.key;
                    switch (key) {
                        case SDL_Keycode.SDLK_ESCAPE:
                            goto end;
                        case SDL_Keycode.SDLK_LEFT:
                            targetPos.X -= 10.0f * deltaTime;
                            break;
                        case SDL_Keycode.SDLK_RIGHT:
                            targetPos.X += 10.0f * deltaTime;
                            break;
                        case SDL_Keycode.SDLK_UP:
                            targetPos.Y += 10.0f * deltaTime;
                            break;
                        case SDL_Keycode.SDLK_DOWN:
                            targetPos.Y -= 10.0f * deltaTime;
                            break;
                        case SDL_Keycode.SDLK_W:
                            targetPos.Z += 10.0f * deltaTime;
                            break;
                        case SDL_Keycode.SDLK_S:
                            targetPos.Z -= 10.0f * deltaTime;
                            break;

                        // e/qfor rotation on x
                        case SDL_Keycode.SDLK_E:
                            cubeRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, -MathF.PI * deltaTime / 1f);
                            break;
                        case SDL_Keycode.SDLK_Q:
                            cubeRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI * deltaTime / 1f);
                            break;
                    }
                    break;
            }

        render:
            Bgfx.Touch(0);

            cubePosition = Vector3.Lerp(cubePosition, targetPos, 5.0f * deltaTime);

            Bgfx.DbgTextClear(0, false);
            Bgfx.DbgTextPrintf(0, 0, 0x0f, $"FPS: {fps:F2}", "");
            // Bgfx.SetDebug((uint)BgfxDebugFlags.Text);
            // Bgfx.SetDebug((uint)(BgfxDebugFlags.Text | BgfxDebugFlags.Stats | BgfxDebugFlags.Profiler | BgfxDebugFlags.Wireframe));
// Even BgfxDebugFlags.Wireframe can be useful here


            Bgfx.SetState((ulong)(StateFlags.Default | StateFlags.CullCcw | StateFlags.Msaa), 0);
            // Console.WriteLine($"Camera view: {String.Join(", ", camera._view)}");
            // Console.WriteLine($"Camera proj: {String.Join(", ", camera._proj)}");
            camera.Apply(0);

            Matrix4x4 modelMatrix = Matrix4x4.CreateFromQuaternion(cubeRotation) * Matrix4x4.CreateTranslation(cubePosition);
            Matrix4x4 transposed = Matrix4x4.Transpose(modelMatrix);

            unsafe {
                float[] modelM = new float[16];
                // MathHelpers.MtxRotateXY(modelM, cubeRotation.X, cubeRotation.Y);

                // // Movement
                // modelM[12] = cubePosition.X;
                // modelM[13] = cubePosition.Y;
                // modelM[14] = 0f;
                // fixed (float* mPtr = &transposed.M11) {
                    for (int i = 0; i < 16; i++) {
                        modelM[i] = transposed[i / 4, i % 4];
                    }
                // }

                // Console.WriteLine($"Model matrix: {String.Join(", ", modelM)}");

                fixed (float* modelPtr = modelM) {
                    Bgfx.SetTransform(modelPtr, 1);
                }
            }
            cube.Render(program);

            Bgfx.Frame(false);
        }

    end:
        Bgfx.Shutdown();
    }
}

