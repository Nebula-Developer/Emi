using Silk.NET.WebGPU;

namespace Emi.Graphics;

public struct GraphicsPipelineDescription {
    public Shader Shader;
    public VertexLayout VertexLayout;
    public PrimitiveTopology Topology;
    public CullMode CullMode;
    public FrontFace FrontFace;
    public bool DepthWriteEnabled;
    public CompareFunction DepthCompare;
    public string VertexEntryPoint;
    public string FragmentEntryPoint;

    // TODO: add BlendState

    public static GraphicsPipelineDescription Default(Shader shader) {
        return new GraphicsPipelineDescription {
            Shader = shader,
            VertexLayout = VertexLayout.CreateDefault(),
            Topology = PrimitiveTopology.TriangleList,
            CullMode = CullMode.None,
            FrontFace = FrontFace.Ccw,
            DepthWriteEnabled = false,
            DepthCompare = CompareFunction.Always,
            VertexEntryPoint = "main_vs",
            FragmentEntryPoint = "main_fs"
        };
    }
}
