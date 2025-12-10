
using Emi.Core;

namespace Emi.Sandbox;

public static class Program {
    public static void Main(string[] args) {
        // MyApp app = new();
        // app.Run();

        Element parent = new() {
            Position = new(20f),
            EulerDegrees = new(0, 0, 30f)
        };

        Element child = new() {
            Position = new(10f),
            EulerDegrees = new(0, 0, 15f),
            Parent = parent
        };

        Console.WriteLine(child.Transform.WorldPosition);

        // Debug: check matrix multiplication order for translations
        var parentTrans = Emi.Mathematics.Matrix4x4.CreateTranslation(parent.Position);
        var childTrans = Emi.Mathematics.Matrix4x4.CreateTranslation(child.Position);

        var a = childTrans * parentTrans; // current implementation
        var b = parentTrans * childTrans; // alternative implementation

        Console.WriteLine($"local * parent world origin = {a.TransformPoint(new(0f))}");
        Console.WriteLine($"parent * local world origin = {b.TransformPoint(new(0f))}");
    }
}
