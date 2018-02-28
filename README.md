
<img  width="60%" src="http://markmeeus.github.io/MarcelloDB/images/logo/logo_blue.svg"/>

[![Build Status](https://travis-ci.org/markmeeus/MarcelloDB.svg?branch=master)](https://travis-ci.org/markmeeus/MarcelloDB)

MarcelloDB is an embedded NoSql object-database for Xamarin and  UWP (Windows Universal) apps.

MarcelloDB saves plain C# objects, including child objects, lists and dictionaries.
Not having to map your objects to the relational model can save you hundreds of lines of code.

It's a pure C# implementation, no need to package other binaries.

#Documentation
Read the docs here: [http://www.marcellodb.org](http://www.marcellodb.org)

#Current Status
Current version is 1.0.4.

Although it's the first version, the community has been testing beta versions since october 2015. It is allready really stable. This version is backwards compatible with 0.6.

###Upgrading to 0.4
There are a few breaking changes from 0.3 to 0.4, read about them [here](http://www.marcellodb.org/upgrade04.html)

###Upgrading to 0.5
Code compatible with 0.4 is expected to compile and run without modifications with 0.5.

Data from 0.4 remains readable, but existing indexes are no longer readable so they will have to be rebuilt.

Copying all objects to a new collection will do the trick. It is even better to copy the data to a new collection-file and delete the previous one to save space.

###Upgrading to 0.6
Code compatible with 0.5 is expected to compile and run without modifications with 0.6.

###Installation
```cs
PM > Install-Package MarcelloDB
```

###A simple console app to get you started.

```cs

  public class Book{
      public string BookId { get; set; }
      public string Title { get; set; }
  }

  class MainClass
  {
      public static void Main (string[] args)
      {
          var platform =  new MarcelloDB.netfx.Platform();
          var session = new MarcelloDB.Session(platform, ".");

          var productsFile = session["products.data"];

          var bookCollection = productsFile.Collection<Book, string>("books", book => book.BookId);

          var newBook = new Book{ BookId = "123",  Title = "The Girl With The Dragon Tattoo" };

          bookCollection.Persist(newBook);

          foreach(var book in bookCollection.All)
          {
              Console.WriteLine(book.Title);
          }

          var theBook = bookCollection.Find("123");

          Console.WriteLine("Found book: " + theBook.Title);

          bookCollection.Destroy("123");
      }
  }

```
### Learn more: www.marcellodb.org
