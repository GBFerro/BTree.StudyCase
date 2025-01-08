using BTree.StudyCase.Models;

int t = 3;
var btree = new BPlusTree<int>(t);

btree.InsertNode(1);
btree.InsertNode(5);
btree.InsertNode(9);
btree.InsertNode(4);

btree.PrintTree();