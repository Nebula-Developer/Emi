using Silk.NET.WebGPU;

namespace Emi.Graphics;

public class VertexLayout {
    public uint Stride { get; private set; }
    public List<VertexAttribute> Attributes { get; } = new();

    public VertexLayout(uint stride = 0) {
        Stride = stride;
    }

    public void AddAttribute(VertexFormat format, uint shaderLocation, uint offset) {
        Attributes.Add(new VertexAttribute {
            Format = format,
            ShaderLocation = shaderLocation,
            Offset = offset
        });
        
        uint end = offset + GetSize(format);
        if (end > Stride) Stride = end;
    }

    public static uint GetSize(VertexFormat format) {
        return format switch {
            VertexFormat.Uint8x2 or VertexFormat.Sint8x2 or VertexFormat.Unorm8x2 or VertexFormat.Snorm8x2 => 2,
            VertexFormat.Uint8x4 or VertexFormat.Sint8x4 or VertexFormat.Unorm8x4 or VertexFormat.Snorm8x4 => 4,
            VertexFormat.Uint16x2 or VertexFormat.Sint16x2 or VertexFormat.Unorm16x2 or VertexFormat.Snorm16x2 or VertexFormat.Float16x2 => 4,
            VertexFormat.Uint16x4 or VertexFormat.Sint16x4 or VertexFormat.Unorm16x4 or VertexFormat.Snorm16x4 or VertexFormat.Float16x4 => 8,
            VertexFormat.Float32 or VertexFormat.Uint32 or VertexFormat.Sint32 => 4,
            VertexFormat.Float32x2 or VertexFormat.Uint32x2 or VertexFormat.Sint32x2 => 8,
            VertexFormat.Float32x3 or VertexFormat.Uint32x3 or VertexFormat.Sint32x3 => 12,
            VertexFormat.Float32x4 or VertexFormat.Uint32x4 or VertexFormat.Sint32x4 => 16,
            _ => 0
        };
    }

    public static VertexLayout CreateDefault() {
        var layout = new VertexLayout();
        layout.AddAttribute(VertexFormat.Float32x3, 0, 0);
        return layout;
    }
}
