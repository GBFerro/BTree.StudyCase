using BTree.StudyCase.Models;

int t = 3;
var btree = new BPlusTree<int>(t);

btree.InsertNode(40);

btree.InsertNode(14);
btree.InsertNode(22);
btree.InsertNode(30);

btree.InsertNode(55);
btree.InsertNode(63);

btree.InsertNode(1);
btree.InsertNode(9);

btree.InsertNode(17);
btree.InsertNode(19);
btree.InsertNode(21);

btree.InsertNode(23);
btree.InsertNode(25);
btree.InsertNode(27);

btree.InsertNode(31);
btree.InsertNode(32);
btree.InsertNode(39);

btree.InsertNode(41);
btree.InsertNode(47);
btree.InsertNode(50);

btree.InsertNode(56);
btree.InsertNode(60);

btree.InsertNode(72);
btree.InsertNode(90);
btree.InsertNode(20);
btree.InsertNode(16);
btree.InsertNode(18);
btree.InsertNode(15);

btree.PrintTree();

Console.WriteLine("============================================");

btree.DeleteKey(25);
btree.PrintTree();
btree.DeleteKey(18);
btree.PrintTree();
btree.DeleteKey(21);
btree.PrintTree();