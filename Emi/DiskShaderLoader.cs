

using System.Diagnostics;

using Emi.Graphics.BgfxCS;

namespace Emi.Graphics;

public static class DiskShaderLoader {
    public unsafe static ProgramHandle LoadShaderProgram(string fragment, string vertex) {
        string shaderPath = "resources/shaders/";
        string fragPath = Path.Combine(shaderPath, fragment);
        string vertPath = Path.Combine(shaderPath, vertex);

        if (!File.Exists(fragPath) || !File.Exists(vertPath))
        {
            Console.WriteLine($"Shader binary missing. Expected files:\n  {fragPath}\n  {vertPath}\nPlease run the shader build script (e.g. scripts/bgfx-compile.sh) to generate .bin files.");
            Console.WriteLine($"CWD: {Directory.GetCurrentDirectory()}");
            // Return invalid handle
            return new ProgramHandle { idx = UInt16.MaxValue };
        }

        byte[] fragBytes = File.ReadAllBytes(fragPath);
        byte[] vertBytes = File.ReadAllBytes(vertPath);

        fixed (byte* fragPtr = fragBytes)
        fixed (byte* vertPtr = vertBytes)
        {
            var fragMem = Bgfx.MakeRef(fragPtr, (uint)fragBytes.Length);
            var vertMem = Bgfx.MakeRef(vertPtr, (uint)vertBytes.Length);

            var fragShader = Bgfx.CreateShader(fragMem);
            var vertShader = Bgfx.CreateShader(vertMem);

            var program = Bgfx.CreateProgram(vertShader, fragShader, false);
            if (!program.Valid)
            {
                Console.WriteLine("Failed to create shader program from binaries.");
                return new ProgramHandle { idx = UInt16.MaxValue };
            }

            return program;
        }
    }
}