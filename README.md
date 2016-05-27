
<img  width="60%" src="http://markmeeus.github.io/MarcelloDB/images/logo/logo_blue.svg"/>

[![Build Status](https://travis-ci.org/markmeeus/MarcelloDB.svg?branch=master)](https://travis-ci.org/markmeeus/MarcelloDB)

MarcelloDB is a mobile NoSql database.
It is very light-weight with minimal memory footprint.

MarcelloDB saves plain C# objects, including child objects, lists and collections.
Not having to map your objects to the relational model can save you hundreds of lines of code.

MarcelloDB is a pure C# implementation, so there is no need to package other binaries.

MarcelloDB is portable code: Xamarin (iOS and Android), Windows 8.1 and Windows Phone 8.1 and Windows 10



#Current Status

Current version: 0.4

Although still under heavy development, both the api and the file format are already quite stable.

See roadmap at the bottom of this page.

Be carefull. Backwards compatibility with existing data will not be guaranteed untill v1.x

#Upgrading to 0.4
Version 0.4 contains a refactor which causes data files created with older version to be unreadable.
So to upgrade, you'll have to delete your data files.
If you really need your data, you should get it out of of MarcelloDB 0.3, put it in a json file, and recreate your data again with 0.4

There is also a code-level incompatibility.
From 0.4 on, it is required to specify the type of your ID property, along with a function on how to get the ID when creating the collection.

Before 0.4:
```cs
session['products'].Collection<Product>("My-Products-Collection");
````

From 0.4:
```cs
session['products'].Collection<Product, Guid>("My-Products-Collection", p => p.ID);
````

You are now free to name your ID property as you wish.
The ID attribute has also been removed as there was no use for it anymore.

#Installation
````
PM > Install-Package MarcelloDB
```
#Sessions

Using MarcelloDB starts with the creation of a session.

The session manages access to the filesystem as well as transactions and concurrency.
Interaction with MarcelloDB always requires a valid session.

MarcelloDB is a portable class library, but requires injection of implementations of platform specific features.
(Currently, MarcelloDB only relies on a platform specific implementation for reading and writing files.)
These platform specifics are implemented in ```Platform```

The Platform implementation for mono/.net (for Xamarin iOS and Android, and regular mono/.net) can be found in the Marcello.netfx assembly.

The implementation for Windows Phone 8.1 and Windows 8.1/10 Apps can be found in the Marcello.uwp assembly.

#Getting started
Create the platform object
```cs
var platform =  new Platform();
```

Then create the session.
(You can do this in a portable class library)
```cs
var session = new MarcelloDB.Session(platform, "/path/to/data/folder/");
```

#CollectionFiles

MarcelloDB organizes it's data in collection-files and collections.
A session can manage multiple collection-files, and a collection-file can contain multiple collections.

You access a collection-file by it's actual filename.

```cs
// products.data is the actual file name within the sessions folder
var productsFile = session["products.data"];
```
With this collection-file, you have access to collections

#Collections

A collection can only handle objects of a specific type (including subclasses).

Deep down inside, MarcelloDB is a key-value store. It saves objects and allows you to retreive them back (or update/delete) by their ID.
Since this ID is so important, it has been made part of the construction of a collection.

A collection is a generic type with at least 2 generic type parameters. The first one should be the type of the objects you want to store, while the second one is the type of your object's ID value.

When constructing the collection, you need to give it a name - like you give tables a name - and you have to provide a function to retrieve the ID from an object.

like this:
```cs
//             Store instances of  [Book]  [with string ID]    [get ID from book]
//                                   ||      ||                       ||
var books = productsFile.Collection<Book, string>("books", book => book.BookId);

//             Store instances of  [Dvd] [with Guid ID] [get ID from dvd]
//                                  ||    ||                  ||
var dvds = productsFile.Collection<Dvd, Guid>("dvd", dvd => dvd.id);
```

If you use different collection-names you can have multiple collections for the same type within one collection-file.
```cs
var dvdCollection = productsFile.Collection<Dvd, Guid>("dvds", dvd => dvd.id);
var newDvdCollection = productsFile.Collection<Dvd, Guid>("new-dvds", dvd => dvd.id);
```

#Persisting objects

Once you have this collection, the method Persist will add or update your object in the collection.
```cs
var book = new Book(){ bookId = "123",  Title = "The Girl With The Dragon Tattoo" };
bookCollection.Persist(book);
```

#Enumerating a collection

A collection exposes an All property which implements IEnumerable.
You can use it to iterate all objects in the collection, and of course use Linq on it.
(Note that when using Linq, it is still enumerating the collection)
```cs
foreach(var book in bookCollection.All){}
```

#Finding objects
You can find a specific object by it's ID by calling 'Find' on the collection.

Find uses a btree index to search for objects. Depending on the size of your collection, a Find will be way faster than iterating the collection to find it.
Sub-millisecond lookups should be expected, even for large collections.

```cs
var book = bookCollection.Find(123)
```

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
//    ------------------                     ||
//                       ||------->Book----->||
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
* has to return null when instantiated
* Have a working setter (so that get returns what has been set)

If any of these requirements is not met, the Collection method on the collection-file will throw as soon as you try to get a reference to it. So, you either get a collection with valid and working indexes, or you get an error.

Once you have this collection, all index functionality is typed correctly. The compiler will prevent you from errors.

An example:
```cs
///
/// This IndexDefinition can be used to index
//                            instances of Book
///                                        ||
class DvdIndexDefinition : IndexDefinition<Book>
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

#Deleting objects

Delete your objects like this:
```cs
bookCollection.Destroy(book.ID);
```

#Transactions

To avoid data corruption, all changes are written to a write ahead journal and applied as a single atomic and durable action.
MarcelloDB does this for calls to Persist and Destroy, but you can extend the transaction to make it span multiple data mutations.

A transaction runs on a session and can include changes in multiple collections from multiple collection files.

(Warning: only collections obtained from that session will be included in the transaction. If you start to mix multiple sessions, you're on your own.)
```cs
session.Transaction(() => {
    session["articles.dat"].Collection<Article>("articles").Persist(article);
    session["project_management.dat"].Collection<Client>("clients").Persist(client);
    session["project_management.dat"].Collection<Project>("projects").Destroy(project);
});
```

Transactions roll back when an exception occurs within the block.
```cs
session.Transaction(() => {
    session["articles.dat"].Collection<Article>("articles").Persist(article);
    session["project_management.dat"].Collection<Client>("clients").Persist(client);
    session["project_management.dat"].Collection<Project>("projects").Destroy(project);
    throw new Exception("Nothing happened");
});
```

#Encryption

Data is stored in an unencrypted format. An encryption engine is available as a pro extension. Please contact mark.meeus at gmail.com for more info.


#Roadmap
0.1.0
-
- ~~Persisting objects~~
- ~~Deleting objects~~
- ~~enumerating objects~~
- ~~reusing storage of deleted objects~~
- ~~cross-collection transaction support~~
- ~~Indexing~~
- ~~iOS/Android (Xamarin) support~~
- ~~Polymorphic collections with Polymorphic children~~
- ~~Windows 8.1 Support~~
- ~~Windows Phone 8.1 Support~~

0.2.0
-
- ~~Indexing properties~~
- ~~Indexing user-defined values~~
- ~~Iterating indexes~~
- ~~Iterating a range from an index~~

0.3.0
-
- ~~Iterating indexes in descending order~~
- ~~Iterating index keys only~~

0.4.0 (Performance optimizations)
-
- ~~Use custom btree node serialization~~
- ~~Use Generic IDs~~
- ~~Remove unnecessary serializations~~

0.5.0
-
- Indexing list of values
- Contains/ContainsAll query

0.6.0
-
- Unique indexes
- IgnoreNull index attribute

1.0.0
-
- Stable file format, to be supported in all future 1.x versions

1.1.0
-
- Synch and PubSub.





