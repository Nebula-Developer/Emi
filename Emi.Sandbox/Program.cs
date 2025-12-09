
using Emi.Core;

namespace Emi.Sandbox;

public unsafe static class Program {
    public static void Main(string[] args) {
        SandboxElementTest();

        // MyApp app = new();
        // app.Run();
    }

    public static void SandboxElementTest() {
        Element parent = new();

        for (int i = 0; i < 5; i++) {
            Element child = new() {
                Name = $"Child {i}"
            };
            parent.Add(child);
        }

        parent.ForEachChild((child) => {
            child.ParentIndex = 0;
        });

        parent.ForEachChild((child) => {
            Console.WriteLine($"Child at index {child.ParentIndex}: {child.Name}");
        });
    }
}