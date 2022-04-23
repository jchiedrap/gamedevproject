using System;
public class BinarySearchTree<T>
{
    public Node<T> root;

    public BinarySearchTree()
    {
        root = null;
    }

    public Node<T>[] GetLeftmostPair() {
        return new Node<T>[] {root.getFarLeft().Parent.Left, root.getFarLeft().Parent.Right};
    }
}