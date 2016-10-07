#Indexes
Enumerating all objects in a collection to find a specific instance is not always be the best approach.
If the collection contains thousands of objects, they will all be read from disk and deserialized,  performance will degrade fast.

For such scenarios it's probably a good idea to add indexes to our collections.
MarcelloDB Indexes are implemented with a BTree algorithm, which makes them really efficient.

Besides searching for objects, indexes can also be used to iterate over the collection (or a subset) ordered by the indexed value.

There is no hard limit on the amount of indexes on a collection. However, too much indexes will slow down calls to Persist, and will increase the total filesize of the collection-file.

##IndexDefinition
Indexes are enabled on collections created with an IndexDefinition as third generic argument.

Like this:
```cs
productsFile.Collection<Book, string, BookIndexDefiniton>("books", book => book.Id);
```

The index definition should derive from `MarcelloDB.Index.IndexDefinition<T>`

```T``` should equal the type of the objects in the collection.

For instance:

```cs
//Define indexes
class BookIndexDefinition : IndexDefinition<Book>
{                                            //
  /*...*/                                    //
}                                            //
//                                           //
//Create collection      //------->Book----->//
productsFile.Collection<Book, string, BookIndexDefiniton>("books", book => book.Id);
//                            ------------------
```

### Index Properties
Indexes are defined by declaring properties on the index definition.

These properties can be of type `IndexedValue<T,TAttribute>` or `IndexedList<T,TAttribute>


```cs
///
/// This IndexDefinition can be used to index
//                             instances of Book
///                                          ||
class BookIndexDefinition : IndexDefinition<Book>
{
   //           Create an index named Title
   //                                  ||
   public  IndexedValue<Book, string> Title { /*...*/ }
   //                    ||     ||
   //  (for instances of Book)
   //                     as a string
   //

   //                Create an index named CategoryId_Code
   //                                             ||
   public  IndexedValue<Book, int, string> CategoryId_Code { /*...*/ }
   //                    ||   ||      ||
   // (for instances of Book) ||      ||
   //                         ||      ||
   // As a combination of an integer  ||
   //                          and a string

   //          Create an index named Tags
   //  indexing a list                ||
   //              ||                 ||
   public  IndexedList<Book, string> Tags { /*...*/ }
   //                   ||     ||
   //(for instances of Book)   ||
   //             values are string
   //
}
```


##Indexing a Single Value or a List of Values
An index is a bit like a sorted list, it keeps references to the objects sorted by the indexed value.
MarcelloDB is able to keep 1 or more references to an object in the same index.

Take for example this class:
```
class Article
{
  public string       Title       { get; set; }
  public int          CategoryID  { get; set; }
  public List<string> Tags        { get; set; }
}
```

An index on `CategoryID` will result in a single index entry per object.
An index on `Tags` on the other hand can contain more than 1 entry per object.

More in detail:
* [Indexing one or more fields with ```IndexedValue``` ](indexing/indexed_value.md)
* [Indexing a list of fields with ```IndexedList```](indexing/indexed_list.md)


##Using indexes
A collection created with an index definition has a property named `Indexes`.
This property returns an instance of the IndexDefinition used to construct the collection.
Every index on that definition can now be used to iterate, and search the data sorted by the indexed value.

All enumerations are implemented in a lazy fashion. The next object is only loaded when actually requested by the iteration.
So we should not worry about large amounts of data, MarcelloDB only loads the current object.


###All (IndexedValue)
All returns an IEnumerable<T> of all objects sorted by the indexed value.
```cs
bookCollection.Indexes.Title.All; //sorted by Title
```

###Equals (IndexedValue)
Equals returns an IEnumerable<T> of all objects that have the specific value for the indexed value.
```cs
//All books with the title "MarcelloDB For Dummies"
bookCollection.Indexes.Title.Equals("MarcelloDB For Dummies")
```

###Between And (IndexedValue)
You can iterate objects with an indexed value wich is contained in a range.

```cs
//all books suitable for 8 and 13, edges not included
bookCollection.Indexes.AgeRecommendation.Between(8).And(13)
```

```cs
//all books suitable for 8 and 13, edges included
bookCollection.Indexes.AgeRecommendation.BetweenIncluding(8).AndIncluding(13)
```

###GreaterThan (OrEqual) (IndexedValue)
Starts iteration at a specific value till the end of the index.
```cs
//all books suitable +12
bookCollection.Indexes.AgeRecommendation.GreaterThan(12)
```
```cs
//all books suitable 12 and up
bookCollection.Indexes.AgeRecommendation.GreaterThanOrEqual(12)
```

###SmallerThan (OrEqual) (IndexedValue)
Starts iteration at the beginning of the indexe untill a specific value
```cs
//all books suitable for -12
bookCollection.Indexes.AgeRecommendation.SmallerThan(12)
```
```cs
//all books suitable up until 12
bookCollection.Indexes.AgeRecommendation.SmallerThanOrEqual(12)
```

###Descending (IndexedValue)
Every index scope can be reversed to iterate your objects in a descending order
```cs
//starting from 12 down to 0
bookCollection.Indexes.AgeRecommendation.SmallerThan(12).Descending
//starting from the max value down to 12
bookCollection.Indexes.AgeRecommendation.GreaterThan(12).Descending
//from 16 down to 12
bookCollection.Indexes.AgeRecommendation.Between(12).And(16).Descending
```
###Iterating the keys of an index (IndexedValue)
You can iterate all keys of an index:
```cs
bookCollection.Indexes.AgeRecommendation.SmallerThan(12).Keys
//works also on the Descending scope
bookCollection.Indexes.AgeRecommendation.SmallerThan(12).Descending.Keys
```

###Contains (IndexedList)
Enumerates objects that have a specific item in the list.
```cs
bookCollections.Indexes.Tags.Contains("Thriller");
```

###ContainsAny (IndexedList)
Enumerates objects that have at least one of the items in the list
```cs
bookCollections.Indexes.Tags.ContainsAny(new string[] {"Thriller", "Comedy"});
```

###ContainsAll (IndexedList)
Enumerates objects that have all of the items in the list
```cs
bookCollections.Indexes.Tags.ContainsAny(new string[] {"Thriller", "Comedy"});
```

