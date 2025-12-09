
namespace Emi.Core;

/// <summary>
/// Represents a basic element within the Emi graph.
/// </summary>
public partial class Element {
    /// <summary>
    /// Initializes a new instance of <see cref="Element"/>.
    /// </summary>
    public Element() {
        Transform = CreateTransform();
    }

    /// <summary>
    /// A clear name primarily for debugging purposes.
    /// </summary>
    public string Name {
        get => _name ??= GetType().Name;
        set => _name = value ?? GetType().Name;
    }
    private string? _name;
}
