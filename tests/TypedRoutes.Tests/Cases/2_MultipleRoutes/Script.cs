Assert.AreEqual("/other-pages/{*catchAll}", MyComponent.PageRouteTemplate);
Assert.AreEqual("/other-pages", MyComponent.PageUri);
CollectionAssert.AreEqual(new List<string> { "/items/{id}", "/items/{category}/{id:int}", "/items/{from:datetime}/{to:datetime?}", "/other-pages/{*catchAll}" }, MyComponent.AllPageRouteTemplates.ToList());

Assert.AreEqual("/items/a%3Fb%3Dc%26d", MyComponent.PageUri1("a?b=c&d"));
Assert.AreEqual("/items/books/100", MyComponent.PageUri2("books", 100));
var date = new DateTime(2000, 1, 1);
Assert.AreEqual("/items/2000-01-01/", MyComponent.PageUri3(date));
Assert.AreEqual("/items/2000-01-01/2000-01-11", MyComponent.PageUri3(date, date.AddDays(10)));
Assert.AreEqual("/other-pages/a/b/c/d", MyComponent.PageUri4("a/b/c%2Fd"));
