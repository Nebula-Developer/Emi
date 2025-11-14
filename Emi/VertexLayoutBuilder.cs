

using System.Runtime.InteropServices;

using Emi.Graphics.BgfxCS;

namespace Emi.Graphics;

public unsafe class VertexLayoutBuilder {
    public VertexLayout* Layout;

    public unsafe VertexLayoutBuilder() {
        Layout = (VertexLayout*)Marshal.AllocHGlobal(sizeof(VertexLayout));
        Bgfx.CreateVertexLayout(Layout);
    }
    public unsafe VertexLayoutBuilder(VertexLayout* layout) => Layout = layout;

    public VertexLayoutBuilder Begin() {
        Bgfx.VertexLayoutBegin(Layout, RendererType.Count);
        return this;
    }

    public VertexLayoutBuilder Add(Attrib attrib, AttribType type, byte num = 1, bool normalized = false, bool asInt = false) {
        Bgfx.VertexLayoutAdd(Layout, attrib, num, type, normalized, asInt);
        return this;
    }

    public VertexLayoutBuilder End() {
        Bgfx.VertexLayoutEnd(Layout);
        return this;
    }

    public static implicit operator VertexLayout*(VertexLayoutBuilder builder) => builder.Layout;
    public static explicit operator VertexLayoutBuilder(VertexLayout* layout) => new VertexLayoutBuilder(layout);
}
