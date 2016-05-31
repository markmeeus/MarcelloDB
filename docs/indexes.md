#Indexes
MarcelloDB allows you to define indexes on your data. These indexes are implemented with a btree algorithm, which results in O(log n) complexity for search and input/update/delete.
Indexed values are stored in sorted order toghether with the location of the corresponding object.
This allows searching for objects by an indexed value, but also iterating the collection in the order of the indexed value.

##IndexDefinition
To enable indexes on a collection, you need to create it with an index definition.
```cs
productsFile.Collection<Book, string, BookIndexDefiniton>("books", book => book.Id);
```

An index definition has to derive from `MarcelloDB.Index.IndexDefinition<T>`, where T is the same type as used for your collection.
In case of the bookCollection:

```cs
class BookIndexDefinition : IndexDefinition<Book>{}
//    =================== >------------------|||--||
//                       ||------->Book----->|||  ||
productsFile.Collection<Book, string, BookIndexDefiniton>("books", book => book.Id);
//                            ------------------
```

##Indexing properties

Indexed values are defined by declaring properties on the index definition.

You can add as many of these properties as needed, but be aware that there is a perfomance and storage-size pricetag with every index you add.

The simplest type of index is one based on a property of the objects you are storing.
In this scenario, a corresponding property on the index definition is all that is needed.

This property has to
* have the same name as the property you want to index
* be of type `IndexedValue<T, TAttribute>`
* have the same type for TAttribute as the type of the property you want to index
* have the same type for  T as the type of the objects in the collection
* has to defined as ```{ get; set; }``` (or at least behave similar)

If any of these requirements is not met, the Collection method on the collection-file will throw as soon as you try to get a reference to it. So, you either get a collection with valid and working indexes, or you get an error.

Once you have this collection, all index functionality is typed correctly. The compiler will prevent you from errors.

An example:
```cs
///
/// This IndexDefinition can be used to index
//                            instances of Book
///                                        ||
class BookIndexDefinition : IndexDefinition<Book>
{
   //          Create an index on property Title
   //                                 ||
   public  IndexedValue<Book, string> Title { get; set; }
   //                   ||     ||
   //             of Book which is a string
}
```

If indexing a property is not enough, you can define custom indexes.
In this case you also define a property like above, except you return an instance of IndexedValue.
And a setter is not needed (nor usefull).

```cs
///
/// This IndexDefinition can be used to index the main author name property of Books
///
class BookIndexDefinition : IndexDefinition<Book>
{
   public  IndexedValue<Book, string> MainAuthorName
   {
      get{
        //index the name of the first autor
        return base.IndexedValue<Book, string>((book)=>{
          return book.Authors.First().Name;
        });
      }
   }
}
```

##If it is comparable, it can be indexed

If a type implements IComparable, it can be indexed. Even your custom objects. Just make sure they compare ok.
Custom objects can be usefull for instance when you want to order your objects by more than 1 property.

```cs
class AuthorNameAndTitle : IComparable
{
  public string AuthorName { get; set; }
  public string Title { get; set; }
  public bool IgnoreTitle { get; set;}

  //Sort by AuthorName, Then by Title
  public int CompareTo(object obj)
  {
    var other = (AuthorTitle)obj;
    var authorNameCompared = AuthorName.CompareTo(other.AuthorName);
    if(authorNameCompared == 0)
    {
      if(IgnoreTitle) //Ignore the title when searching for Author only.
        return 0;

      return Title.CompareTo(other.Title);
    }
    return authorNameCompared;
   }
}
```

```cs
class BookIndexDefinition : IndexDefinition<Book>
{
  public  IndexedValue<Book, AuthorNameAndTitle> AuthorNameAndTitle
  {
    get{
      return base.IndexedValue<Book, AuthorNameAndTitle>((book)=>{
        return new AuthorNameAndTitle{AuthorName = book.Author.Name, Title = Book.Title};
      });
    }
  }
}
```


```cs
//Get all books, ordered by author and title
bookCollection.Indexes.AuthorNameAndTitle.All

//Get all books, for a given author
var books = bookCollection
  .Indexes
  .AuthorNameAndTitle
  .Find(
    new AuthorNameAndTitle{AuthorName = 'Ernest Hemingway', IgnoreTitle = true})
  );

```


##Using indexes
A collection created with an index definition has a property named 'Indexes'. This property has a property for every index available. In fact, it is just an instance of the index definition used to create the collection.

Every index on that definition is now able to iterate (and search) the data sorted by the indexed value.

All enumerations are implemented in a lazy fashion. The next object is only loaded when actually requested by the iteration.
So don't worry if you have a really large amount of data, Marcello never loads all that data in memory.

###All
Find returns an IEnumerable<T> of all objects sorted by the indexed value.
(In later versions you'll also be able to iterate the index backwards)
```cs
bookCollection.Indexes.Title.All; //sorted by Title
```

###Find
Find returns an IEnumerable<T> of all objects that have the specific value for the indexed value.
```cs
//All books with the title "MarcelloDB For Dummies"
boolCollection.Indexes.Title.Find("MarcelloDB For Dummies")
```
(In later versions, you'll be able to define a unique index, in which case a find will return just a single object, no IEnumerable.)

###Between And
You can iterate objects with an indexed value wich is contained in a range.

```cs
//all books suitable for 8 and 13, edges not included
bookCollection.Indexes.AgeRecommendation.Between(8).And(13)
```

```cs
//all books suitable for 8 and 13, edges included
bookCollection.Indexes.AgeRecommendation.BetweenIncluding(8).AndIncluding(13)
```

###GreaterThan (OrEqual)
Starts iteration at a specific value till the end of the index.
```cs
//all books suitable +12
bookCollection.Indexes.AgeRecommendation.GreaterThan(12)
```
```cs
//all books suitable 12 and up
bookCollection.Indexes.AgeRecommendation.GreaterThanOrEqual(12)
```

###SmallerThan (OrEqual)
Starts iteration at the beginning of the indexe untill a specific value
```cs
//all books suitable for -12
bookCollection.Indexes.AgeRecommendation.SmallerThan(12)
```
```cs
//all books suitable up until 12
bookCollection.Indexes.AgeRecommendation.SmallerThanOrEqual(12)
```

###Descending
Every index scope can be reversed to iterate your objects in a descending order
```cs
//starting from 12 down to 0
bookCollection.Indexes.AgeRecommendation.SmallerThan(12).Descending
//starting from the max value down to 12
bookCollection.Indexes.AgeRecommendation.GreaterThan(12).Descending
//from 16 down to 12
bookCollection.Indexes.AgeRecommendation.Between(12).And(16).Descending
```
##Iterating the keys of an index
You can iterate all keys of an index:
```cs
bookCollection.Indexes.AgeRecommendation.SmallerThan(12).Keys
//works also on the Descending scope
bookCollection.Indexes.AgeRecommendation.SmallerThan(12).Descending.Keys
```
