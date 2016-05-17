using System;
using System.Collections.Generic;
using MarcelloDB.Index;
using MarcelloDB.Index.BTree;

namespace MarcelloDB.Test
{
    internal class MockBTree<TK> : IBTree<TK>
    {
        public string LastAction { get; set; }
        public List<TK> Searched { get; set; }
        public Dictionary<TK, Int64> Inserted { get; set; }
        public List<TK> Deleted { get; set; }

        public MockBTree()
        {
            Searched = new List<TK>();
            Inserted = new Dictionary<TK, Int64>();
            Deleted = new List<TK>();
        }


        #region IBTree implementation
        public Entry<TK> Search(TK key)
        {
            Searched.Add(key);
            if(Inserted.ContainsKey(key) && !Deleted.Contains(key)){
                return new Entry<TK>{Key=key, Pointer=Inserted[key]};
            }
            return null;
        }

        public void Insert(TK newKey, Int64 newPointer)
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

