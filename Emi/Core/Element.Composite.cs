
namespace Emi.Core;

public partial class Element {
    /// <summary>
    /// The current children of this element.
    /// </summary>
    public IReadOnlyCollection<Element> Children {
        get => _children.AsReadOnly();
        set => SetChildren(value);
    }

    /// <summary>
    /// Raised when a new child is added to this element.
    /// </summary>
    public event Action<Element, int>? DoChildAdded;

    /// <summary>
    /// Raised when a child is removed from this element.
    /// </summary>
    public event Action<Element, int>? DoChildRemoved;

    /// <summary>
    /// Raised when this element changes parent.
    /// </summary>
    public event Action<Element?, Element?>? DoParentChanged;

    private readonly List<Element> _children = new();

    /// <summary>
    /// Adds a child to this element at the specified index.
    /// </summary>
    /// <param name="child">Child to add</param>
    /// <param name="index">Optional index override</param>
    public void Add(Element child, int? index = null) {
        child._parentIndex = index;
        child.SetParent(this);
    }

    /// <summary>
    /// Removes all children from this element.
    /// </summary>
    public void Clear() {
        foreach (var child in _children.ToArray())
            child.SetParent(null);
    }

    /// <summary>
    /// Replaces the current children with the provided collection.
    /// </summary>
    /// <param name="children">Children to assign</param>
    public void SetChildren(IReadOnlyCollection<Element> children) {
        Clear();

        int index = 0;
        foreach (var child in children) {
            child._parentIndex = index++;
            child.SetParent(this);
        }
    }

    /// <summary>
    /// The current parent element, if any.
    /// </summary>
    public Element? Parent {
        get => _parent;
        set => SetParent(value);
    }
    private Element? _parent;

    /// <summary>
    /// The index of this element within its parent.
    /// </summary>
    public int? ParentIndex {
        get => _parentIndex;
        set => _parentIndex = _parent == null ? value : _parent.HandleChildPlacement(this, value);
    }
    private int? _parentIndex = null;

    /// <summary>
    /// Sets the parent for this element.
    /// </summary>
    /// <param name="parent">Parent to assign</param>
    public void SetParent(Element? parent) {
        if (!HandleParentChange(parent, out var oldParent))
            return;

        if (parent != null)
            _parentIndex = parent.HandleChildPlacement(this, _parentIndex);

        OnParentChanged(oldParent, parent);
    }

    /// <summary>
    /// Invokes the parent change event.
    /// </summary>
    /// <param name="oldParent">Original parent</param>
    /// <param name="newParent">New parent</param>
    protected virtual void OnParentChanged(Element? oldParent, Element? newParent) => DoParentChanged?.Invoke(oldParent, newParent);

    /// <summary>
    /// Invokes the child-added event.
    /// </summary>
    /// <param name="child">Child that was added</param>
    /// <param name="index">Index of the child</param>
    protected virtual void OnChildAdded(Element child, int index) => DoChildAdded?.Invoke(child, index);

    /// <summary>
    /// Invokes the child-removed event.
    /// </summary>
    /// <param name="child">Child that was removed</param>
    /// <param name="index">Index from which the child was removed</param>
    protected virtual void OnChildRemoved(Element child, int index) => DoChildRemoved?.Invoke(child, index);

    /// <summary>
    /// Determines if this element is a descendant of the specified potential ancestor.
    /// </summary>
    /// <param name="potentialAncestor">Element to check against</param>
    /// <returns>Whether this element is a descendant of the specified element</returns>
    public bool IsDescendantOf(Element? potentialAncestor) {
        var current = this._parent;
        while (current != null) {
            if (current == potentialAncestor)
                return true;
            current = current._parent;
        }
        return false;
    }

    /// <summary>
    /// Recurses over the children of this element, preventing modification clashes.
    /// </summary>
    /// <param name="action">Action to perform on each child</param>
    /// <param name="reverse">Whether to iterate in reverse order</param>
    public void ForEachChild(Action<Element, int> action, bool reverse = false) {
        var childrenSnapshot = _children.ToArray();
        
        if (reverse) {
            for (int i = childrenSnapshot.Length - 1; i >= 0; i--)
                action(childrenSnapshot[i], i);
        } else {
            for (int i = 0; i < childrenSnapshot.Length; i++)
                action(childrenSnapshot[i], i);
        }
    }

    /// <inheritdoc cref="ForEachChild(Action{Element, int})"/>
    public void ForEachChild(Action<Element> action, bool reverse = false) => ForEachChild((child, _) => action(child), reverse);

    private int HandleChildPlacement(Element child, int? newIndex = null) {
        bool modifying = _children.Remove(child);
        int index = newIndex.HasValue ? int.Clamp(newIndex.Value, 0, _children.Count) : _children.Count;

        if (newIndex.HasValue) _children.Insert(index, child);
        else _children.Add(child);

        ReindexChildren();

        if (!modifying) OnChildAdded(child, child._parentIndex ?? index);

        return child._parentIndex ?? index;
    }

    private bool HandleParentChange(Element? parent, out Element? oldParent) {
        if (parent == this) throw new InvalidOperationException("An element cannot be its own parent.");

        if (parent != null && parent.IsDescendantOf(this))
            throw new InvalidOperationException("An element cannot be assigned a descendant as its parent.");
        
        oldParent = _parent;
        if (oldParent == parent) return false;

        if (oldParent != null && oldParent.TryRemoveChild(this, out var removedIndex))
            oldParent.OnChildRemoved(this, removedIndex);

        _parent = parent;

        if (parent == null)
            _parentIndex = null;

        return true;
    }

    private bool TryRemoveChild(Element child, out int removedIndex) {
        removedIndex = _children.IndexOf(child);
        if (removedIndex < 0) return false;

        _children.RemoveAt(removedIndex);
        ReindexChildren();
        return true;
    }

    private void ReindexChildren() {
        for (int i = 0; i < _children.Count; i++)
            _children[i]._parentIndex = i;
    }

}