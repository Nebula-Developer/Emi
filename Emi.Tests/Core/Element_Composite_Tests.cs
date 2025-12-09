using System.Collections.Generic;
using System.Linq;

using Emi.Core;

using Xunit;

namespace Emi.Tests.Core;

public class Element_Composite_Tests {
    [Fact]
    public void Add_SetsParentAndRaisesEvents() {
        var parent = new Element();
        var child = new Element();

        Element? addedChild = null;
        int addedIndex = -1;
        parent.DoChildAdded += (c, i) => {
            addedChild = c;
            addedIndex = i;
        };

        (Element? oldParent, Element? newParent)? parentChange = null;
        child.DoParentChanged += (oldParent, newParent) => parentChange = (oldParent, newParent);

        parent.Add(child);

        Assert.Same(parent, child.Parent);
        Assert.Equal(0, child.ParentIndex);
        Assert.Collection(parent.Children, c => Assert.Same(child, c));
        Assert.Same(child, addedChild);
        Assert.Equal(0, addedIndex);
        Assert.Equal((null, parent), parentChange);
    }

    [Fact]
    public void Add_WithIndex_InsertsAtPosition() {
        var parent = new Element();
        var first = new Element();
        var second = new Element();
        parent.Add(first);
        parent.Add(second);

        var third = new Element();
        int addedIndex = -1;
        parent.DoChildAdded += (c, i) => {
            if (c == third) addedIndex = i;
        };

        parent.Add(third, 1);

        Assert.Equal(new[] { first, third, second }, parent.Children.ToArray());
        Assert.Equal(0, first.ParentIndex);
        Assert.Equal(1, third.ParentIndex);
        Assert.Equal(2, second.ParentIndex);
        Assert.Equal(1, addedIndex);
    }

    [Fact]
    public void Clear_RemovesChildrenAndRaisesEvents() {
        var parent = new Element();
        var childA = new Element();
        var childB = new Element();
        parent.Add(childA);
        parent.Add(childB);

        var removed = new List<(Element child, int index)>();
        parent.DoChildRemoved += (c, i) => removed.Add((c, i));

        var parentChanges = new List<(Element? oldParent, Element? newParent)>();
        childA.DoParentChanged += (o, n) => parentChanges.Add((o, n));
        childB.DoParentChanged += (o, n) => parentChanges.Add((o, n));

        parent.Clear();

        Assert.Empty(parent.Children);
        Assert.Null(childA.Parent);
        Assert.Null(childB.Parent);
        Assert.Equal(new[] { (childA, 0), (childB, 0) }, removed);
        Assert.Equal(new[] { (Element?)parent, null, parent, null }, parentChanges.SelectMany(tuple => new[] { tuple.oldParent, tuple.newParent }));
    }

    [Fact]
    public void ParentIndex_ReordersWithoutTriggeringAddOrRemoveEvents() {
        var parent = new Element();
        var first = new Element();
        var second = new Element();
        parent.Add(first);
        parent.Add(second);

        bool addRaised = false;
        bool removeRaised = false;
        parent.DoChildAdded += (_, _) => addRaised = true;
        parent.DoChildRemoved += (_, _) => removeRaised = true;

        second.ParentIndex = 0;

        Assert.Equal(new[] { second, first }, parent.Children.ToArray());
        Assert.Equal(1, first.ParentIndex);
        Assert.Equal(0, second.ParentIndex);
        Assert.False(addRaised);
        Assert.False(removeRaised);
    }

    [Fact]
    public void SetChildren_ReplacesCollectionAndRaisesEvents() {
        var parent = new Element();
        var original = new Element();
        parent.Add(original);

        var removed = new List<Element>();
        var added = new List<Element>();
        parent.DoChildRemoved += (c, _) => removed.Add(c);
        parent.DoChildAdded += (c, _) => added.Add(c);

        var newChildren = new[] { new Element(), new Element() };
        parent.SetChildren(newChildren);

        Assert.Equal(newChildren, parent.Children.ToArray());
        Assert.Equal(new[] { original }, removed);
        Assert.Equal(newChildren, added);
        Assert.All(newChildren.Select((child, index) => (child, index)), pair => Assert.Equal(pair.index, pair.child.ParentIndex));
        Assert.All(newChildren, child => Assert.Same(parent, child.Parent));
        Assert.Null(original.Parent);
    }

    [Fact]
    public void Add_ChildWithExistingParent_RemovesFromOldParent() {
        var oldParent = new Element();
        var newParent = new Element();
        var child = new Element();

        oldParent.Add(child);

        var removedFromOld = false;
        oldParent.DoChildRemoved += (_, _) => removedFromOld = true;

        newParent.Add(child);

        Assert.Same(newParent, child.Parent);
        Assert.Equal(0, child.ParentIndex);
        Assert.Empty(oldParent.Children);
        Assert.True(removedFromOld);
    }

    [Fact]
    public void SetParent_Null_RemovesFromParent() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);

        bool removedRaised = false;
        parent.DoChildRemoved += (_, _) => removedRaised = true;

        child.SetParent(null);

        Assert.Null(child.Parent);
        Assert.Empty(parent.Children);
        Assert.True(removedRaised);
    }

    [Fact]
    public void ParentIndex_SetToSameIndex_DoesNothing() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);

        bool addRaised = false;
        bool removeRaised = false;
        parent.DoChildAdded += (_, _) => addRaised = true;
        parent.DoChildRemoved += (_, _) => removeRaised = true;

        child.ParentIndex = 0;

        Assert.Equal(0, child.ParentIndex);
        Assert.False(addRaised);
        Assert.False(removeRaised);
    }

    [Fact]
    public void SetChildren_EmptyCollection_ClearsChildren() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);

        bool removedRaised = false;
        parent.DoChildRemoved += (_, _) => removedRaised = true;

        parent.SetChildren(Array.Empty<Element>());

        Assert.Empty(parent.Children);
        Assert.Null(child.Parent);
        Assert.True(removedRaised);
    }

    [Fact]
    public void SetParent_ToSelf_Throws() {
        var element = new Element();
        Assert.Throws<InvalidOperationException>(() => element.SetParent(element));
    }

    [Fact]
    public void SetParent_ParentToChild_Throws() {
        var parent = new Element();
        var child = new Element();
        parent.Add(child);
        Assert.Throws<InvalidOperationException>(() => parent.SetParent(child));
    }

    [Fact]
    public void ParentIndex_MultipleChildren_ReordersCorrectly() {
        var parent = new Element();
        var a = new Element();
        var b = new Element();
        var c = new Element();
        parent.Add(a);
        parent.Add(b);
        parent.Add(c);

        b.ParentIndex = 2;

        Assert.Equal(new[] { a, c, b }, parent.Children.ToArray());
        Assert.Equal(0, a.ParentIndex);
        Assert.Equal(1, c.ParentIndex);
        Assert.Equal(2, b.ParentIndex);
    }
}
