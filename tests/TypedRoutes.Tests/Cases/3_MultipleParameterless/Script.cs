Assert.AreEqual("/examples/parameterless-multiple", MyComponent.PageRouteTemplate);
Assert.AreEqual("/examples/parameterless-multiple", MyComponent.PageUri);
Assert.AreEqual("/examples/parameterless-multiple", MyComponent.PageUri1());
Assert.AreEqual("/samples/multiple", MyComponent.PageUri2());
CollectionAssert.AreEqual(new List<string> { "/examples/parameterless-multiple", "/samples/multiple" }, MyComponent.AllPageRouteTemplates.ToList());
