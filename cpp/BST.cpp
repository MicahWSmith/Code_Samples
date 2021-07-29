#include "BST.h"

// node constructor
Node::Node() : data(""), left(nullptr), right(nullptr) {}

// BST constructor
BST::BST() : root(nullptr) {}

// insert handler calls private member function
void BST::insert_handler(std::string word) {
	insert(word, root);
}

// private insert member takes node and adds child if appropriate
void BST::insert(std::string word, NodePtr& node) {
	// if end of tree add node with word value
	if (node == nullptr) {
		node = new Node();
		node->data = word;
	}
	// if word comes before current nodes word go left
	else if (toLower(word) < toLower(node->data)) {
		insert(word, node->left);
	}
	// if word comes after go right
	else if (toLower(word) > toLower(node->data)) {
		insert(word, node->right);
	}
	// else node value must exist already
	else {
		std::cout << "Node value Exists" << std::endl;
	}
}

// convert string to lowercase for consistent comparison
std::string BST::toLower(std::string s) {
	std::transform(s.begin(), s.end(), s.begin(),
		[](unsigned char c) { return std::tolower(c); }
	);
	return s;
}

// print tree to stream obj
void BST::printTree(std::ostream& output, NodePtr& node, int indent) {
	if (node != nullptr) {
		printTree(output, node->right, indent + 8);
		output << std::setw(indent) << node->data << std::endl;
		printTree(output, node->left, indent + 8);
	}
}

// builds a balanced tree from an alphabetically sorted list of strings
void BST::balanceTree(std::vector<std::string> words, int start, int end) {
	// once all indexes are exhausted, break recursion
	if (start > end) {
		return;
	}
	// middle of word index range formula
	int mid = (start + end) / 2;
	insert_handler(words[mid]);
	// recursive calling for left and right sides of node
	balanceTree(words, start, mid - 1);
	balanceTree(words, mid + 1, end);
	return;
}

// read dictionary file specified into words vector
void BST::ReadDictionary(std::string dictionary) {
	 std::vector<std::string> words;
	 // stream read for dict file
	 std::ifstream f(dictionary);

	 // get each word from dictionary file
	 std::string line;

	 // until the end of the file is reached
	 while (getline(f, line)) {
		 // push to vector as lowercase for ease of comparing
		 words.push_back(toLower(line));
	 }
	 // close file stream
	 f.close();

	 // sort vector like dictionary
	 sort(words.begin(), words.end());

	 // begin balance by insering from sorted vector
	 balanceTree(words, 0, words.size()-1);
}

// read and spell check text document words
void BST::ReadAndSpellCheck(std::string txt_file) {
	std::vector<std::string> words;
	// input file stream for spell check
	std::ifstream f(txt_file);

	// get each word from dictionary file
	std::string line;

	// until the end of the file is reached
	while (f >> line)
	{
		// displaying content 
		line = toLower(line);

		// empty word to append chars
		std::string word = "";
		bool is_word = false; // only add word to list if it has alphabetical chars
		// for each char of the word check if alphabetical
		for (int i = 0; i < line.length(); i++) {
			if (line.at(i) >='a' && line.at(i) <= 'z') {
				word += line.at(i);
				is_word = true;// assume is a word if contains alphabetical char
			}
		}
		// if is alphabetical add to vector of words
		if (is_word) {
			words.push_back(word);
		}
	}
	// close file stream
	f.close();
	// for each word, check if it is spelled correctly
	for (int i = 0; i < words.size(); i++) {
		SearchForWord(words[i]);
	}
}

// public member calls private member with word string arg
void BST::SearchForWord(std::string word) {
	search_for_word(root, word);
	return;
}

// private member for searching uses temp nodes to traverse BST
void BST::search_for_word(NodePtr& node, std::string word) {
	// temp node creation and assignment
	NodePtr temp = new Node();
	temp = node;
	// while not at end of tree
	while (temp != NULL) {
		if (temp->data == word) {
			return; // do nothing if found word
		}
		else if (temp->data > word)
			temp = temp->left; // go left if word is less
		else
			temp = temp->right; // go right if word is greater
	}
	std::cout << "\nmis-spelled: " << word;// write to console each mis-spelled word
	return;
}

std::stringstream& operator<<(std::stringstream& output, BST& bst) {
	//print tree string to file

	bst.printTree(output, bst.root, 0);

	// open filestream for writing
	std::ofstream file;
	file.open("BST.txt");

	// append text from stream to file
	file << output.str();

	// close file
	file.close();

	return output;
}