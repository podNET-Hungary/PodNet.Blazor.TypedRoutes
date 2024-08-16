CollectionAssert.AreEqual(new List<string> { "/my-component", "/my-component/{parameter:int}", "/alternative", "/alternative/{param:guid}" }, MyComponent.AllPageRouteTemplates.ToList());

// As defined: .razor file is considered first, then corresponding .cs partial class.
Assert.AreEqual("/my-component", MyComponent.PageUri);
Assert.AreEqual("/my-component", MyComponent.PageUri1());
Assert.AreEqual("/my-component/10", MyComponent.PageUri2(10));
Assert.AreEqual("/alternative", MyComponent.PageUri3());
var guid = Guid.NewGuid();
Assert.AreEqual($"/alternative/{guid}", MyComponent.PageUri4(guid));
