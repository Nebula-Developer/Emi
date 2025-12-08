using Silk.NET.Core.Native;
using Silk.NET.WebGPU;
using Silk.NET.Maths;
using Emi.Graphics;

namespace Emi.Sandbox;

public unsafe class MyApp : Application {
    private Pipeline? _pipeline;
    private Mesh? _triangleMesh;
    private Mesh? _squareMesh;
    private Shader? _shader;
    private UniformBuffer? _uniformBuffer;
    private BindGroup* _bindGroup;
    private readonly Camera _camera = new();

    protected override void OnLoad() {
        base.OnLoad();

        const string shaderSource = @"
        struct Uniforms {
            view_proj: mat4x4<f32>,
        };
        @group(0) @binding(0) var<uniform> uniforms : Uniforms;

        struct VertexInput {
            @location(0) position : vec3<f32>,
            @location(1) color : vec3<f32>,
        };

        struct VertexOutput {
            @builtin(position) position : vec4<f32>,
            @location(0) color : vec3<f32>,
        };

        @vertex
        fn main_vs(in : VertexInput) -> VertexOutput {
            var out : VertexOutput;
            out.position = uniforms.view_proj * vec4<f32>(in.position, 1.0);
            out.color = in.color;
            return out;
        }
        
        @fragment
        fn main_fs(in : VertexOutput) -> @location(0) vec4<f32> {
            return vec4<f32>(in.color, 1.0);
        }";

        _shader = new Shader(Context, shaderSource);
        
        var layout = new VertexLayout();
        layout.AddAttribute(VertexFormat.Float32x3, 0, 0); // Position
        layout.AddAttribute(VertexFormat.Float32x3, 1, 12); // Color

        var pipelineDesc = GraphicsPipelineDescription.Default(_shader);
        pipelineDesc.VertexLayout = layout;
        
        _pipeline = new Pipeline(Context, pipelineDesc);

        _uniformBuffer = new UniformBuffer(Context, (ulong)sizeof(Matrix4X4<float>));

        var bindGroupLayout = Context.WebGPU.RenderPipelineGetBindGroupLayout(_pipeline.RenderPipeline, 0);

        var entry = new BindGroupEntry {
            Binding = 0,
            Buffer = _uniformBuffer.Buffer,
            Offset = 0,
            Size = _uniformBuffer.Size
        };

        var bgDesc = new BindGroupDescriptor {
            Layout = bindGroupLayout,
            EntryCount = 1,
            Entries = &entry
        };

        _bindGroup = Context.WebGPU.DeviceCreateBindGroup(Context.Device, &bgDesc);

        float[] triangleVertices = new float[] {
             0.0f,  0.5f, 0.0f, 1.0f, 0.0f, 0.0f, // Top Red
            -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // Bottom Left Green
             0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f  // Bottom Right Blue
        };

        _triangleMesh = new Mesh(Context, triangleVertices, 3);

        float[] squareVertices = new float[] {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 0.0f, // Bottom Left (of the square) Yellow
             0.9f,  0.5f, 0.0f, 1.0f, 0.0f, 1.0f, // Bottom Right Magenta
             0.5f,  0.9f, 0.0f, 0.0f, 1.0f, 1.0f, // Top Left Cyan

             0.9f,  0.5f, 0.0f, 1.0f, 0.0f, 1.0f, // Bottom Right Magenta
             0.9f,  0.9f, 0.0f, 1.0f, 1.0f, 1.0f, // Top Right White
             0.5f,  0.9f, 0.0f, 0.0f, 1.0f, 1.0f  // Top Left Cyan
        };

        _squareMesh = new Mesh(Context, squareVertices, 6);
    }

    protected override void OnRender(double delta) {
        base.OnRender(delta);
        
        if (_pipeline == null || _triangleMesh == null || _squareMesh == null || _uniformBuffer == null) return;

        _camera.UpdateProjection(Window.Size.X, Window.Size.Y);
        var projection = _camera.GetProjectionMatrixFlat();

        fixed (float* p = projection) {
            Context.WebGPU.QueueWriteBuffer(Context.Queue, _uniformBuffer.Buffer, 0, p, (nuint)(16 * sizeof(float)));
        }

        var pass = Renderer.CurrentPass;
        _pipeline.Bind(pass);
        Context.WebGPU.RenderPassEncoderSetBindGroup(pass, 0, _bindGroup, 0, null);
        _triangleMesh.Draw(pass);
        _squareMesh.Draw(pass);
    }
}
