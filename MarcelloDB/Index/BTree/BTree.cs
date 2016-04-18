using System;
using System.Diagnostics;
using System.Linq;

namespace MarcelloDB.Index.BTree
{
    internal interface IBTree<TK, TP>
    {
        Entry<TK, TP> Search(TK key);

        void Insert(TK newKey, TP newPointer);

        void Delete(TK keyToDelete);
    }

    /// <summary>
    /// B tree implementation based on https://github.com/rdcastro/btree-dotnet
    /// </summary>
    internal class BTree<TK, TP> : IBTree<TK, TP>
    {
        IBTreeDataProvider<TK, TP> DataProvider { get; set;}

        ObjectComparer Comparer { get; set; }

        internal Node<TK, TP> Root
        {
            get
            {
                return this.DataProvider.GetRootNode(this.Degree);
            }
            private set
            {
                this.DataProvider.SetRootNode(value);
            }
        }

        internal BTree(IBTreeDataProvider<TK, TP> dataProvider, int degree)
        {
            DataProvider = dataProvider;
            Comparer = new ObjectComparer();

            if (degree < 2)
            {
                throw new ArgumentException("BTree degree must be at least 2", "degree");
            }

            this.Degree = degree;
            this.Height = 1;
        }

        public int Degree { get; private set; }

        public int Height { get; private set; }

        /// <summary>
        /// Searches a key in the BTree, returning the entry with it and with the pointer.
        /// </summary>
        /// <param name="key">Key being searched.</param>
        /// <returns>Entry for that key, null otherwise.</returns>
        public Entry<TK, TP> Search(TK key)
        {
            return this.SearchInternal(this.Root, key);
        }

        /// <summary>
        /// Inserts a new key associated with a pointer in the BTree. This
        /// operation splits nodes as required to keep the BTree properties.
        /// </summary>
        /// <param name="newKey">Key to be inserted.</param>
        /// <param name="newPointer">Pointer to be associated with inserted key.</param>
        public void Insert(TK newKey, TP newPointer)
        {
            if (this.Search(newKey) != null)
            {
                throw new ArgumentException("Cannot insert duplicate keys in the tree.");
            }

            // there is space in the root node
            if (!this.Root.HasReachedMaxEntries)
            {
                this.InsertNonFull(this.Root, newKey, newPointer);
                return;
            }

            // need to create new node and have it split
            Node<TK, TP> oldRoot = this.Root;

            this.Root = this.DataProvider.CreateNode(this.Degree);
            this.Root.ChildrenAddresses.Add(oldRoot.Address);
            this.SplitChild(this.Root, 0, oldRoot);
            this.InsertNonFull(this.Root, newKey, newPointer);

            this.Height++;
        }

        /// <summary>
        /// Deletes a key from the BTree. This operations moves keys and nodes
        /// as required to keep the BTree properties.
        /// </summary>
        /// <param name="keyToDelete">Key to be deleted.</param>
        public void Delete(TK keyToDelete)
        {
            this.DeleteInternal(this.Root, keyToDelete);

            // if root's last entry was moved to a child node, remove it
            if (this.Root.EntryList.Count == 0 && !this.Root.IsLeaf)
            {
                this.Root = this.DataProvider.GetNode(this.Root.ChildrenAddresses.Single());
                this.Height--;
            }
        }

        /// <summary>
        /// Internal method to delete keys from the BTree
        /// </summary>
        /// <param name="node">Node to use to start search for the key.</param>
        /// <param name="keyToDelete">Key to be deleted.</param>
        private void DeleteInternal(Node<TK, TP> node, TK keyToDelete)
        {
            int i = node.EntryList.TakeWhile(entry => Comparer.Compare(keyToDelete, entry.Key) > 0).Count();

            // found key in node, so delete if from it
            if (i < node.EntryList.Count && Comparer.Compare(node.EntryList[i].Key, keyToDelete) == 0)
            {
                this.DeleteKeyFromNode(node, keyToDelete, i);
                return;
            }

            // delete key from subtree
            if (!node.IsLeaf)
            {
                this.DeleteKeyFromSubtree(node, keyToDelete, i);
            }
        }

        /// <summary>
        /// Helper method that deletes a key from a subtree.
        /// </summary>
        /// <param name="parentNode">Parent node used to start search for the key.</param>
        /// <param name="keyToDelete">Key to be deleted.</param>
        /// <param name="subtreeIndexInNode">Index of subtree node in the parent node.</param>
        private void DeleteKeyFromSubtree(Node<TK, TP> parentNode, TK keyToDelete, int subtreeIndexInNode)
        {
            Node<TK, TP> childNode = this.DataProvider.GetNode(parentNode.ChildrenAddresses[subtreeIndexInNode]);

            // node has reached min # of entries, and removing any from it will break the btree property,
            // so this block makes sure that the "child" has at least "degree" # of nodes by moving an
            // entry from a sibling node or merging nodes
            if (childNode.HasReachedMinEntries)
            {
                int leftIndex = subtreeIndexInNode - 1;
                var leftSibling = subtreeIndexInNode > 0 ?
                      this.DataProvider.GetNode(parentNode.ChildrenAddresses[leftIndex]) : null;

                int rightIndex = subtreeIndexInNode + 1;
                var rightSibling = subtreeIndexInNode < parentNode.ChildrenAddresses.Count - 1
                    ? this.DataProvider.GetNode(parentNode.ChildrenAddresses[rightIndex])
                    : null;

                if (leftSibling != null && leftSibling.EntryList.Count > this.Degree - 1)
                {
                    // left sibling has a node to spare, so this moves one node from left sibling
                    // into parent's node and one node from parent into this current node ("child")
                    childNode.EntryList.Insert(0, parentNode.EntryList[subtreeIndexInNode - 1]);
                    parentNode.EntryList[subtreeIndexInNode - 1] = leftSibling.EntryList.Last();
                    leftSibling.EntryList.RemoveAt(leftSibling.EntryList.Count - 1);

                    if (!leftSibling.IsLeaf)
                    {
                        childNode.ChildrenAddresses.Insert(0, leftSibling.ChildrenAddresses.Last());
                        leftSibling.ChildrenAddresses.RemoveAt(leftSibling.ChildrenAddresses.Count - 1);
                    }
                }
                else if (rightSibling != null && rightSibling.EntryList.Count > this.Degree - 1)
                {
                    // right sibling has a node to spare, so this moves one node from right sibling
                    // into parent's node and one node from parent into this current node ("child")
                    childNode.EntryList.Add(parentNode.EntryList[subtreeIndexInNode]);
                    parentNode.EntryList[subtreeIndexInNode] = rightSibling.EntryList.First();
                    rightSibling.EntryList.RemoveAt(0);

                    if (!rightSibling.IsLeaf)
                    {
                        childNode.ChildrenAddresses.Add(rightSibling.ChildrenAddresses.First());
                        rightSibling.ChildrenAddresses.RemoveAt(0);
                    }
                }
                else
                {
                    // this block merges either left or right sibling into the current node "child"
                    if (leftSibling != null)
                    {
                        childNode.EntryList.Insert(0, parentNode.EntryList[subtreeIndexInNode - 1]);
                        var oldEntries = childNode.EntryList;
                        childNode.EntryList = leftSibling.EntryList;
                        childNode.EntryList.AddRange(oldEntries);
                        if (!leftSibling.IsLeaf)
                        {
                            var oldChildrenAddresses = childNode.ChildrenAddresses;
                            childNode.ChildrenAddresses = leftSibling.ChildrenAddresses;
                            childNode.ChildrenAddresses.AddRange(oldChildrenAddresses);
                        }

                        parentNode.ChildrenAddresses.RemoveAt(leftIndex);
                        parentNode.EntryList.RemoveAt(subtreeIndexInNode - 1);
                    }
                    else
                    {
                        Debug.Assert(rightSibling != null, "Node should have at least one sibling");
                        childNode.EntryList.Add(parentNode.EntryList[subtreeIndexInNode]);
                        childNode.EntryList.AddRange(rightSibling.EntryList);
                        if (!rightSibling.IsLeaf)
                        {
                            childNode.ChildrenAddresses.AddRange(rightSibling.ChildrenAddresses);
                        }

                        parentNode.ChildrenAddresses.RemoveAt(rightIndex);
                        parentNode.EntryList.RemoveAt(subtreeIndexInNode);
                    }
                }
            }

            // at this point, we know that "child" has at least "degree" nodes, so we can
            // move on - this guarantees that if any node needs to be removed from it to
            // guarantee BTree's property, we will be fine with that
            this.DeleteInternal(childNode, keyToDelete);
        }

        /// <summary>
        /// Helper method that deletes key from a node that contains it, be this
        /// node a leaf node or an internal node.
        /// </summary>
        /// <param name="node">Node that contains the key.</param>
        /// <param name="keyToDelete">Key to be deleted.</param>
        /// <param name="keyIndexInNode">Index of key within the node.</param>
        private void DeleteKeyFromNode(Node<TK, TP> node, TK keyToDelete, int keyIndexInNode)
        {
            // if leaf, just remove it from the list of entries (we're guaranteed to have
            // at least "degree" # of entries, to BTree property is maintained
            if (node.IsLeaf)
            {
                node.EntryList.RemoveAt(keyIndexInNode);
                return;
            }

            Node<TK, TP> predecessorChild = this.DataProvider.GetNode(node.ChildrenAddresses[keyIndexInNode]);
            if (predecessorChild.EntryList.Count >= this.Degree)
            {
                Entry<TK, TP> predecessorEntry = this.GetLastEntry(predecessorChild);
                this.DeleteInternal(predecessorChild, predecessorEntry.Key);
                node.EntryList[keyIndexInNode] = predecessorEntry;
            }
            else
            {
                Node<TK, TP> successorChild = this.DataProvider.GetNode(node.ChildrenAddresses[keyIndexInNode + 1]);
                if (successorChild.EntryList.Count >= this.Degree)
                {
                    Entry<TK, TP> successorEntry = this.GetFirstEntry(successorChild);
                    this.DeleteInternal(successorChild, successorEntry.Key);
                    node.EntryList[keyIndexInNode] = successorEntry;
                }
                else
                {
                    predecessorChild.EntryList.Add(node.EntryList[keyIndexInNode]);
                    predecessorChild.EntryList.AddRange(successorChild.EntryList);
                    predecessorChild.ChildrenAddresses.AddRange(successorChild.ChildrenAddresses);

                    node.EntryList.RemoveAt(keyIndexInNode);
                    node.ChildrenAddresses.RemoveAt(keyIndexInNode + 1);

                    this.DeleteInternal(predecessorChild, keyToDelete);
                }
            }
        }

        /// <summary>
        /// Helper method that gets the last entry (i.e. rightmost key) for a given node.
        /// </summary>
        private Entry<TK, TP> GetLastEntry(Node<TK, TP> node)
        {
            if (node.IsLeaf)
            {
                return node.EntryList[node.EntryList.Count - 1];
            }

            return this.GetLastEntry(
                this.DataProvider.GetNode(node.ChildrenAddresses.Last())
            );
        }

        /// <summary>
        /// Helper method that gets the first entry (i.e. leftmost key) for a given node.
        /// </summary>
        private Entry<TK, TP> GetFirstEntry(Node<TK, TP> node)
        {
            if (node.IsLeaf)
            {
                return node.EntryList[0];
            }

            return this.GetFirstEntry(
                this.DataProvider.GetNode(node.ChildrenAddresses.First())
            );
        }

        /// <summary>
        /// Helper method that search for a key in a given BTree.
        /// </summary>
        /// <param name="node">Node used to start the search.</param>
        /// <param name="key">Key to be searched.</param>
        /// <returns>Entry object with key information if found, null otherwise.</returns>
        private Entry<TK, TP> SearchInternal(Node<TK, TP> node, TK key)
        {
            int i = node.EntryList.TakeWhile(entry => Comparer.Compare(key, entry.Key) > 0).Count();

            if (i < node.EntryList.Count && Comparer.Compare(node.EntryList[i].Key, key) == 0)
            {
                return node.EntryList[i];
            }

            return node.IsLeaf ? null : this.SearchInternal(
                this.DataProvider.GetNode(node.ChildrenAddresses[i]),
                key);

        }

        /// <summary>
        /// Helper method that splits a full node into two nodes.
        /// </summary>
        /// <param name="parentNode">Parent node that contains node to be split.</param>
        /// <param name="nodeToBeSplitIndex">Index of the node to be split within parent.</param>
        /// <param name="nodeToBeSplit">Node to be split.</param>
        private void SplitChild(Node<TK, TP> parentNode, int nodeToBeSplitIndex, Node<TK, TP> nodeToBeSplit)
        {
            var newNode = this.DataProvider.CreateNode(this.Degree);

            parentNode.EntryList.Insert(nodeToBeSplitIndex, nodeToBeSplit.EntryList[this.Degree - 1]);
            parentNode.ChildrenAddresses.Insert(nodeToBeSplitIndex + 1, newNode.Address);

            newNode.EntryList.AddRange(nodeToBeSplit.EntryList.GetRange(this.Degree, this.Degree - 1));

            // remove also Entries[this.Degree - 1], which is the one to move up to the parent
            nodeToBeSplit.EntryList.RemoveRange(this.Degree - 1, this.Degree);

            if (!nodeToBeSplit.IsLeaf)
            {
                newNode.ChildrenAddresses.AddRange(nodeToBeSplit.ChildrenAddresses.GetRange(this.Degree, this.Degree));
                nodeToBeSplit.ChildrenAddresses.RemoveRange(this.Degree, this.Degree);
            }
        }

        private void InsertNonFull(Node<TK, TP> node, TK newKey, TP newPointer)
        {
            int positionToInsert = node.EntryList.TakeWhile(entry => Comparer.Compare(newKey, entry.Key) >= 0).Count();

            // leaf node
            if (node.IsLeaf)
            {
                node.EntryList.Insert(positionToInsert, new Entry<TK, TP>() { Key = newKey, Pointer = newPointer });
                return;
            }

            // non-leaf
            Node<TK, TP> child = this.DataProvider.GetNode(node.ChildrenAddresses[positionToInsert]);
            if (child.HasReachedMaxEntries)
            {
                this.SplitChild(node, positionToInsert, child);
                if (Comparer.Compare(newKey, node.EntryList[positionToInsert].Key) > 0)
                {
                    positionToInsert++;
                }
            }

            this.InsertNonFull(
                this.DataProvider.GetNode(node.ChildrenAddresses[positionToInsert]),
                newKey, newPointer);
        }
    }
}