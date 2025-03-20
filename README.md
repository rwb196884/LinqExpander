# LinqExpander

A way of expanding extension methods in your LINQ expressions so that it can be interperated by a third party who doesn't understand your extensions.

Originally by Luke McGregor https://github.com/lukemcgregor/LinqExpander

Ported to `netstandard` by Ben Cull https://github.com/bjcull/LinqExpander
See also: https://bencull.com/blog/expression-projection-magic-entity-framework-core

Updated EntityFramework pacakge and compile `` to net4 for legacy software to version 9 by Richard Barraclough https://github.com/rwb196884/LinqExpander

# Why two projects?

The `ReplaceWithExpressionAttribute` class has no dependencies and can therefore be compiled to old verisons of .NET. This is useful when model/POCO classes are used by legacy code.

The new stuff that contains the actual implementation depends on `Microsoft.EntityFramework` and therefore targets the latest LTS release of .NET which, at the time of writing, is `net8.0`.

# Usage

On your model class:
```

```