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

    public bool SearchKey(T key)
    {
        return SearchKeyInternal(Root, key);
    }

    private static bool SearchKeyInternal(Node<T> node, T key)
    {
        var hasKey = node.Keys.Contains(key);

        if (hasKey)
        {
            return true;
        }

        var position = node.Keys.TakeWhile(nodeKey => key.CompareTo(nodeKey) > 0).Count();

        if (!node.IsLeaf)
        {
            var child = node.Children[position];
            return SearchKeyInternal(child, key);
        }

        return false;
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

    private void SplitNode(Node<T> parentNode, int position, Node<T> oldNode)
    {
        var newNode = new Node<T>(Degree);

        var splitIndex = Degree - 1;
        var range = Degree - 1;

        parentNode.Keys.Insert(position, oldNode.Keys[splitIndex]);
        parentNode.AddChildren(position + 1, newNode);

        newNode.Keys.AddRange(oldNode.Keys.GetRange(index: Degree, count: range));

        oldNode.Keys.RemoveRange(splitIndex, count: Degree);

        if (!oldNode.IsLeaf)
        {
            newNode.Children.AddRange(oldNode.Children.GetRange(Degree, Degree));
            oldNode.Children.RemoveRange(Degree, Degree);
        }
    }

    public void DeleteKey(T key)
    {
        DeleteKeyInternal(Root, key);

        if (Root.Keys.Count == 0 && !Root.IsLeaf)
        {
            Root = Root.Children.Single();
            Height--;
        }
    }

    private void DeleteKeyInternal(Node<T> node, T key)
    {
        int position = node.Keys.TakeWhile(nodeKey => key.CompareTo(nodeKey) >= 0).Count();

        if (node.Keys.Count > position && node.Keys[position].CompareTo(key) == 0)
        {
            DeleteKeyFromNode(node, key, position);
        }

        if (!node.IsLeaf)
        {
            DeleteKeyFromSubtree(node, key, position);
        }
    }

    private void DeleteKeyFromSubtree(Node<T> node, T key, int position)
    {
        Node<T> childNode = node.Children[position];
        int nodeLength = node.Keys.Count - 1;

        if (childNode.HasMinKeys)
        {
            int leftIndex = position - 1;
            Node<T> leftSibling = position > 0 ? node.Children[leftIndex] : null;

            int rightIndex = position + 1;
            Node<T> rightSibling = position < nodeLength ? node.Children[rightIndex] : null;

            if (leftSibling != null && leftSibling.Keys.Count >= Degree)
            {
                int leftSiblingKeysLenght = leftSibling.Keys.Count - 1;
                int leftSiblingChildrenLenght = leftSibling.Children.Count - 1;

                childNode.Keys.Insert(0, node.Keys[position]);
                node.Keys[position] = leftSibling.Keys[leftSiblingKeysLenght];
                leftSibling.Keys.RemoveAt(leftSiblingKeysLenght);

                if (!leftSibling.IsLeaf)
                {
                    childNode.Children.Insert(0, leftSibling.Children.Last());
                    leftSibling.Children.RemoveAt(leftSiblingChildrenLenght);
                }
            }

        }
    }

    private void DeleteKeyFromNode(Node<T> node, T key, int position)
    {
        if (node.IsLeaf)
        {
            node.Keys.Remove(key);
            return;
        }

        Node<T> predecessorNode = node.Children[position];
        if (predecessorNode.Keys.Count >= Degree)
        {
            T predecessor = DeletePredecessor(predecessorNode);
            node.Keys[position] = predecessor;
            return;
        }

        Node<T> successorNode = node.Children[position + 1];
        if (successorNode.Keys.Count >= Degree)
        {
            T successor = DeleteSuccessor(successorNode);
            node.Keys[position] = successor;
            return;
        }

        MergeSuccessorAndPredecessor(node, predecessorNode, successorNode, position);
    }

    private static void MergeSuccessorAndPredecessor(Node<T> node, Node<T> predecessorNode, Node<T> successorNode, int position)
    {
        // Merge successor and predecessor into one node
        predecessorNode.Keys.Add(node.Keys[position]);
        predecessorNode.Keys.AddRange(successorNode.Keys);
        predecessorNode.Children.AddRange(successorNode.Children);

        // Remove merged Key and Child from parent node
        node.Keys.RemoveAt(position);
        node.Children.RemoveAt(position + 1);
    }

    private static T DeleteSuccessor(Node<T> successorNode)
    {
        if (successorNode.IsLeaf)
        {
            T result = successorNode.Keys[0];
            successorNode.Keys.RemoveAt(0);
            return result;
        }

        return DeleteSuccessor(successorNode);
    }

    private static T DeletePredecessor(Node<T> predecessorNode)
    {
        if (predecessorNode.IsLeaf)
        {
            int nodeLength = predecessorNode.Keys.Count - 1;
            T result = predecessorNode.Keys[nodeLength];
            predecessorNode.Keys.RemoveAt(nodeLength);
            return result;
        }

        return DeletePredecessor(predecessorNode);
    }

    private static void DeleteKeyFromInternalNodeAndMergeLeaf(Node<T> node, int position, T key)
    {
        node.Keys.Remove(key);
        node.Children[position].Keys.AddRange(node.Children[position + 1].Keys);
    }

    private static void DeleteKeyFromInternalNode(Node<T> node, int position, T key)
    {
        node.Keys.Remove(key);
        node.Keys.Insert(position - 1, node.Children[position].Keys.First());
        node.Children[position].Keys.Remove(node.Children[position].Keys.First());
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
