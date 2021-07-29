#pragma once
#include <string>
#include <ostream>
#include <iostream>
#include <sstream>
#include <iomanip>
#include <algorithm>
#include <vector>
#include <fstream>
#include <math.h>

// forward declaration for typedef
class Node;

// typedef for ease of reading
typedef Node* NodePtr;

// node class for bst data
class Node {
public:
	std::string data;
	NodePtr left;
	NodePtr right;

	Node();
};

// Spell checker/dictionary re-purposed BST class
class BST {
private:
	// root node of leaf
	NodePtr root;
	
	void BST::balanceTree(std::vector<std::string> words, int start, int end);
	void insert(std::string word, NodePtr& node);
	void insert_handler(std::string word);
	std::string toLower(std::string s);
	void printTree(std::ostream& output, NodePtr& node, int indent);
	void search_for_word(NodePtr& node, std::string word);

public:
	BST();
	void ReadDictionary(std::string dictionary);
	void ReadAndSpellCheck(std::string txt_file);
	void SearchForWord(std::string word);
	void SaveTree();

	friend std::stringstream& operator<<(std::stringstream& output, BST& bst);
};

// ref for chars append to str https://www.techiedelight.com/append-char-end-string-cpp/#:~:text=How%20to%20append%20char%20to%20end%20of%20string,call%20to%20the...%203.%20append%20().
// ref for file io https://iq.opengenus.org/write-file-in-cpp/
// ref for vector methods https://www.geeksforgeeks.org/vector-in-cpp-stl/
// ref for checking if char is alphabetical https://codescracker.com/cpp/program/cpp-program-check-alphabet.htm
// thought about ways to balance. if all data is read first, then figured it could be inserted in order before tree was made
// to avoid balancing issues afterwards. Made looping solution then found out it was an actual method. reference: https://www.geeksforgeeks.org/sorted-array-to-balanced-bst/