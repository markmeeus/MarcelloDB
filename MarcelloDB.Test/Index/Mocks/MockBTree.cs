using System;
using System.Collections.Generic;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Test
{
    internal class MockBTree<TK,TP> : IBTree<TK, TP>
    {
        public string LastAction { get; set; }
        public List<TK> Searched { get; set; }
        public Dictionary<TK, TP> Inserted { get; set; }
        public List<TK> Deleted { get; set; }

        public MockBTree()
        {
            Searched = new List<TK>();
            Inserted = new Dictionary<TK, TP>();
            Deleted = new List<TK>();
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
        #endregion
    }
}

