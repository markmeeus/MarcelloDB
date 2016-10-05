## Finding an object
MarcelloDB keeps track of all objects in a collection using it's ID. The ID's of a collection are stored in a BTree structure inside the same collection-file.

Finding an object by it's ID is very performant, even for very large collections.

```cs
bookCollection.Find(123);
```

The Find method uses the generic type for the ID as parameter. This way, the compiler will protect you from trivial type mistakes.


##Enumerating a collection

Using the collections All property you can enumerate all objects in the collection.

```cs
foreach(var book in bookCollection.All){}
```
MarcelloDB loads only the current object in memory, so even for large collections, enumerating would be quite safe to do memory-wise.

> To avoid data corruption, enumerating a collection locks the collection for other threads write operations untill the enumeration is done.

> MarcelloDB will throw an exception when the enumerating thread tries to write.

## A note about Linq
Since All is an IEnumerable&lt;T&gt; (T is the type of your objects) you can use Linq on it.

However, there is no such thing as Linq-To-MarcelloDB. Some Linq expressions will still enumerate all objects. Which may be fine for a small collection.

For instance, to get all thrillers from a book collection, this will work enumerate all objects.
Which is fine for a small collection.
```cs
var thrillers = bookCollection.All.Where(b => b.Category == Categories.Thriller);
```

However if we use an index, we'll enumerate only the objects we actually want.
[(More about indexes ...)](indexes.html)
```cs
var thrillers = bookCollection.Indexes.Category.Equals(Categories.Thriller);
```

###Why no Linq-To-MarcelloDB
Linq-To-MarcelloDB would certainly be possible. The expression could be evaluated and when an index exists, it could be used.
However, the philosofy of MarcelloDB is to be always explicit, allowing the compiler to protect you as much as possible.
Using this expressive api, it is obvious that an index is used. No need for debugging here.
