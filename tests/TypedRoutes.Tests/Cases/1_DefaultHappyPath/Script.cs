Assert.AreEqual("/fetchdata", FetchData.PageRouteTemplate);
Assert.AreEqual("/fetchdata", FetchData.PageUri);
CollectionAssert.AreEqual(new List<string> { "/fetchdata" }, FetchData.AllPageRouteTemplates.ToList());
