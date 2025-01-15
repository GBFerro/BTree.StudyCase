namespace BTree.StudyCase.Models;

public class BPlusTree<T> where T : IComparable<T>
{
    public Node<T> Root { get; set; }

    public int Degree { get; set; }

    public int Height { get; set; }

    public BPlusTree(int degree)
    {
        if (degree < 2)
            throw new ArgumentException();

        Root = new Node<T>(degree);
        Degree = degree;
        Height = 1;
    }

    public void InsertNode(T key)
    {

        if (!Root.HasMaxKeys)
        {
            // adiciona key no node não nulo
            InsertKeyIntoNode(Root, key);
            return;
        }

        Node<T> oldNode = Root;
        Root = new Node<T>(Degree);
        Root.AddChildren(0, oldNode);
        SplitNode(this.Root, 0, oldNode);
        InsertKeyIntoNode(Root, key);
        Height++;
    }

    private void SplitNode(Node<T> parentNode, int position, Node<T> oldNode)
    {
        var rightNode = new Node<T>(Degree);

        var splitIndex = Degree - 1;
        var range = Degree - 1;

        parentNode.Keys.Insert(position, oldNode.Keys[splitIndex]);
        parentNode.AddChildren(position + 1, rightNode);
        rightNode.Keys.AddRange(oldNode.Keys.GetRange(index: Degree, count: range));
        oldNode.Keys.RemoveRange(splitIndex, count: Degree);


        if (!oldNode.IsLeaf)
        {
            rightNode.Children.AddRange(oldNode.Children.GetRange(Degree, Degree));
            oldNode.Children.RemoveRange(Degree, Degree);
        }
    }

    private void InsertKeyIntoNode(Node<T> node, T key)
    {
        int position = node.Keys.TakeWhile(nodeKey => key.CompareTo(nodeKey) >= 0).Count();

        if (node.IsLeaf)
        {
            node.AddKeyToNode(key, position);
            return;
        }

        var child = node.Children[position];
        if (child.HasMaxKeys)
        {
            SplitNode(node, position, child);
            if (key.CompareTo(node.Keys[position]) > 0)
            {
                position++;
            }
        }

        InsertKeyIntoNode(node.Children[position], key);
    }

    public void PrintTree()
    {
        if (Root == null)
        {
            Console.WriteLine("A árvore está vazia.");
            return;
        }

        var queue = new Queue<(Node<T>, int)>(); // Fila de nós, com seus níveis na árvore
        queue.Enqueue((Root, 0));
        int currentLevel = -1;

        while (queue.Count > 0)
        {
            var (node, level) = queue.Dequeue();

            // Verifica se estamos em um novo nível da árvore
            if (level != currentLevel)
            {
                currentLevel = level;
                Console.WriteLine($"\nNível {currentLevel}:");
            }

            // Imprime as chaves do nó
            Console.Write("[");
            for (int i = 0; i < node.Keys.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(node.Keys[i], default(T))) // Ignora valores padrão (vazios)
                    Console.Write($"{node.Keys[i]}");
                if (i < node.Keys.Count - 1 && !EqualityComparer<T>.Default.Equals(node.Keys[i + 1], default))
                    Console.Write(", ");
            }
            Console.Write("] ");

            // Adiciona os filhos à fila se não for uma folha
            if (!node.IsLeaf)
            {
                foreach (var child in node.Children)
                {
                    if (child != null)
                    {
                        queue.Enqueue((child, level + 1));
                    }
                }
            }
        }

        Console.WriteLine(); // Linha final para organização
    }
}
