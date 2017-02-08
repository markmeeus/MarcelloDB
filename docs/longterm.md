## A few long-term ideas.

Some ideas for MarcelloDB after 1.0:

###Pub-Sub

After deploying a pubsub server (to be developed).
One would be able to do this:

```cs
//on the publishing side
session['products.dat'].Publish("marcellodb://user:pass@someserver.tld/products.dat");
```

```cs
//on the receiving side
session['products.dat'].Subscribe("marcellodb://user:pass@someserver.tld/products.dat");
```
Supports 1 Publisher, and many Subscribers per collection-file.


###Full Text indexes

```cs
class BookIndexDefinition : IndexDefinition<Book>
{
  public  FullTextIndexedValue<Book> AllFullText { get;set; }
}
```

The FullTextIndexedValue will create a full-text index for every String.
When using the auto-implemented get-set, it will use the instance of the persisted object.

It will also be possible to implement a mapper if only a specific set of properties needs to be indexed.

Find will return a FullTextResult:
```cs
  bookCollection.AllFullText.Find("for dummies");
```