using BTree.StudyCase.Models;

int t = 3;
var btree = new BPlusTree<int>(t);

btree.InsertNode(1);
btree.InsertNode(5);
btree.InsertNode(9);
btree.InsertNode(50);
btree.InsertNode(23);
btree.InsertNode(25);
btree.InsertNode(31);
btree.InsertNode(13);
btree.InsertNode(11);
btree.InsertNode(19);
btree.InsertNode(20);
btree.InsertNode(25);
btree.InsertNode(100);
btree.InsertNode(70);
btree.InsertNode(60);
btree.InsertNode(80);
btree.InsertNode(95);
btree.InsertNode(18);
btree.InsertNode(4);
btree.InsertNode(6);
btree.InsertNode(10);
btree.InsertNode(15);
btree.InsertNode(7);
btree.InsertNode(2);
btree.InsertNode(18);

Console.WriteLine(btree.SearchKey(6));
Console.WriteLine(btree.SearchKey(1000));
Console.WriteLine(btree.SearchKey(95));


btree.PrintTree();