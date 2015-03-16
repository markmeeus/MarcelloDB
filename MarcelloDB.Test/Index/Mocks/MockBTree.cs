using System;
using System.Collections.Generic;
using MarcelloDB.Index;

namespace MarcelloDB.Test
{
    public class MockBTree<TK,TP> : IBTree<TK, TP>
    {
        public string LastAction { get; set; }
        public List<TK> Searched { get; set; }
        public Dictionary<TK, TP> Inserted { get; set; }
        public List<TK> Deleted { get; set; }

        Node<TK, TP> _root;

        public MockBTree()
        {
            Searched = new List<TK>();
            Inserted = new Dictionary<TK, TP>();
            Deleted = new List<TK>();
            _root = new Node<TK, TP>(2);
        }

        public void SetRoot(Node<TK, TP> rootNode)
        {
            _root = rootNode;
        }

        #region IBTree implementation
        public Entry<TK, TP> Search(TK key)
        {
            Searched.Add(key);
            if(Inserted.ContainsKey(key) && !Deleted.Contains(key)){
                return new Entry<TK, TP>{Key=key, Pointer=Inserted[key]};
            }
            return null;
        }

        public void Insert(TK newKey, TP newPointer)
        {
            Inserted[newKey] = newPointer;
            LastAction = "Insert";
        }

        public void Delete(TK keyToDelete)
        {
            Inserted.Remove(keyToDelete);
            Deleted.Add(keyToDelete);
            LastAction = "Delete";
        }
            
        public Node<TK, TP> Root
        {
            get
            {
                return _root;
            }
        }
        #endregion
    }
}

