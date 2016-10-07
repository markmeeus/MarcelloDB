#Upgrading to 0.4
Version 0.4 contains a refactor which causes data files created with older version to be unreadable.

There is also a code-level incompatibility.

  From 0.4 on, it is required to specify the type of your ID property.
  Also an Object-To-ID mapping function should be provided.

Before 0.4:
```cs
session['products'].Collection<Product>("My-Products-Collection");
````

From 0.4:
```cs
session['products'].Collection<Product, Guid>("My-Products-Collection", p => p.ID);
````

When using an IndexDefinition:
Before 0.4:
```cs
session['products'].Collection<Product, BookIndexes>("My-Products-Collection");
````

From 0.4:
```cs
session['products'].Collection<Product, Guid, BookIndexes>("My-Products-Collection", p => p.ID);
````

You are now free to name your ID property as you wish.

The ID attribute has also been removed as there was no use for it anymore.

