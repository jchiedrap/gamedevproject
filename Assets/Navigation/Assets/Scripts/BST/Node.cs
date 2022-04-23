using System;
public class Node<T>
{
    public T Data;
    public Node<T> Parent;
    public Node<T> Left;
    public Node<T> Right;
    
    public void DisplayNode()
    {
        Console.Write(Data + " ");
    }

    public Node (T t) {
        Data = t;
    }
    public Node (T t, Node<T> _parent) {
        Data = t;
        Parent = _parent;
    }

    public Node<T> getFarLeft() {
        if (Left != null) return Left.getFarLeft();
        else if (Right != null) return Right.getFarLeft();
        else return this;
    }

    public Node<T> getFarRight() {
        if (Left != null) return Left.getFarLeft();
        else if (Right != null) return Right.getFarLeft();
        else return this;
    }
}