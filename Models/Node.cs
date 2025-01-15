namespace BTree.StudyCase.Models;

public class Node<T> where T : IComparable<T>
{
    private readonly int _t;

    public List<Node<T>> Children { get; set; }

    public List<T> Keys { get; set; }

    public bool IsLeaf
    {
        get
        {
            return Children.Count == 0;
        }
    }

    public bool HasMinKeys => Keys.Count == _t - 1;

    public bool HasMaxKeys => Keys.Count == 2 * _t - 1;

    public void AddKeyToNode(T keyValue, int position)
    {
        Keys.Insert(position, keyValue);
    }

    public void AddChildren(int position, Node<T> rightNode)
    {
        Children.Insert(position, rightNode);
    }

    public Node(int degree)
    {
        _t = degree;
        Children = [];
        Keys = [];
    }
}
