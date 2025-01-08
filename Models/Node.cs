namespace BTree.StudyCase.Models;

public class Node<T> where T : IComparable<T>
{
    private readonly int _t;
    private readonly int _childrenCount;

    public IList<Node<T>> Children { get; set; }

    public IList<T> Keys { get; set; }

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

    public Node(int degree)
    {
        _t = degree;
        Children = [];
        Keys = [];
    }
}
