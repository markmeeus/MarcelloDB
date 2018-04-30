
<img  width="60%" src="http://markmeeus.github.io/MarcelloDB/images/logo/logo_blue.svg"/>

[![Build Status](https://travis-ci.org/markmeeus/MarcelloDB.svg?branch=master)](https://travis-ci.org/markmeeus/MarcelloDB)

MarcelloDB is an embedded NoSql object-database for Xamarin and  UWP (Windows Universal) apps.

MarcelloDB saves plain C# objects, including child objects, lists and dictionaries.
Not having to map your objects to the relational model can save you hundreds of lines of code.

It's a pure C# implementation, no need to package other binaries.

#Documentation
Read the docs here: [http://www.marcellodb.org](http://www.marcellodb.org)

#Current Status
Current version is 1.0.6.

Although it's the first version, the community has been testing beta versions since october 2015. It is allready really stable. 


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
