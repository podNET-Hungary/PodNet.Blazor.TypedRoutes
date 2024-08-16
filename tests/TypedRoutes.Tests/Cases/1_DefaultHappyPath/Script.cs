Assert.AreEqual("/fetchdata", FetchData.PageRouteTemplate);
Assert.AreEqual("/fetchdata", FetchData.PageUri);
CollectionAssert.AreEqual(new List<string> { "/fetchdata" }, FetchData.AllPageRouteTemplates.ToList());

var component = new FetchData();
Assert.IsTrue(component is INavigableComponent && component is IRoutableComponent);