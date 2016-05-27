##MarcelloDB - Docs
Welcome to MarcelloDB documentation pages.


###New to MarcelloDB?
Awesome!

Read the [Quickstart](quickstart.md) to get up to speed quickly.

MarcelloDB is designed to be simple to implement, I hope you'll love it.

###Current status:
Current version is 0.4

The API and data format are getting more stable with every release. However MarcelloDB is not guaranteed to be compatible with data from previous versions untill version 1.0

In other words: not ready for production apps yet.


###Upgrading to 0.4
Version 0.4 contains a refactor which causes data files created with older version to be unreadable.

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

