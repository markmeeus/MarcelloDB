
<img  width="60%" src="http://markmeeus.github.io/MarcelloDB/images/logo/logo_blue.svg"/>

[![Build Status](https://travis-ci.org/markmeeus/MarcelloDB.svg?branch=master)](https://travis-ci.org/markmeeus/MarcelloDB)

MarcelloDB is a mobile NoSql database.
It is very light-weight with minimal memory footprint.

MarcelloDB saves plain C# objects, including child objects, lists and collections.
Not having to map your objects to the relational model can save you hundreds of lines of code.

MarcelloDB is a pure C# implementation, so there is no need to package other binaries.

MarcelloDB is portable code: Xamarin (iOS and Android), Windows 8.1 and Windows Phone 8.1 and Windows 10



#Current Status

Current version: 0.3

Although still under heavy development, both the api and the file format are already quite stable.

See roadmap at the bottom of this page.

Be carefull. Backwards compatibility with existing data will not be guaranteed untill v1.x

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

#CollectionFiles and Collections

MarcelloDB organizes it's data in collection-files and collections. 
A session can manage multiple collection-files, and a collection-file can contain multiple collection.

You access a collection-file by it's actual filename.

```cs
// products.data is the actual file name within the sessions folder
var productsFile = session["products.data"];
```

With this collection-file, you can start accessing collections.
Collections are a bit like tables, the main difference is that they contain entire objects, not just rows with colums.
A collection can only handle objects of a specific type (including subclasses).

You can get the collection form the collection-file like this:
```cs
var bookCollection = productsFile.Collection<Book>("books");
var dvdCollection = productsFile.Collection<Dvd>("dvd");
```
If you use different collection-names you can have multiple collections for the same type within one collection-file.
```cs
var dvdCollection = productsFile.Collection<Dvd>("dvds");
var upcomingDvdCollection = productsFile.Collection<Dvd>("upcoming-dvds");
```

#Persisting objects

Once you have this collection, the method Persist will add or update your object in the collection.
```cs
var book = new Book(){ ID = "123",  Title = "The Girl With The Dragon Tattoo" };
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
Every objects needs to have an ID (more on ID's below)
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
productsFile.Collection<Book, BookIndexDefiniton>("books");
```

An index definition has to derive from `MarcelloDB.Index.IndexDefinition<T>`, where T is the same type as used for your collection.
In case of the bookCollection:

```cs
class BookIndexDefinition : IndexDefinition<Book>{}
//    ------------------
//                       ||------->Book----->||
productsFile.Collection<Book, BookIndexDefiniton>("books");
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

#How Objects are Identified

MarcelloDB needs a single property which uniquely identifies an object.
This value is needed to build the main index and make a distinction between an insert or an update.

You can use any of the following naming conventions, or add the MarcelloDB.Attributes.ID attribute.
```cs
class Article
{
  public string ID { get; set; }

  public string Id { get; set; }

  public string id { get; set; }

  public string ArticleID { get; set; }

  public string ArticleId { get; set; }

  public string Articleid { get; set; }

  [MarcelloDB.Attributes.ID]
  public string CustomIDProp { get; set; }
}
```
The ID, Id and id property can be defined anywhere in the class hierarchy, but the ClassID, ClassId and Classid attributes will only be used if they are defined on the class which is used as the collection class.

```cs
class Product
{
  public string ID { get; set; }
}
class Chair : Product{}

//This will work
session["data"].Collection<Product>("products").Persist(chair);

//This will also work because an ID (or Id or id) property is found in the class hiërachy.
session["data"].Collection<Chair>("chairs").Persist(chair);


class Article
{
  public string ArticleID { get; set; }
}
class Book : Article{}

//This will work
session["data"].Collection<Article>("articles").Persist(book);

//This will not work because Book doesn't define an ID and ArticleID on the parent isn't considered.
session["data"].Collection<Book>("books").Persist(book);
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

0.4.0
-
- Indexing list of values
- Contains/ContainsAll query

0.5.0
-
- Unique index attribute
- IncludeNull index attribute

1.0.0
-
- Stable file format, to be supported in all future 1.x versions






