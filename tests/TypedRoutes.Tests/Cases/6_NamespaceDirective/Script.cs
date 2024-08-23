Assert.AreEqual("TestNamespace", typeof(__AssemblyAnchor).Assembly.DefinedTypes.Single(t => t.Name == "MyComponent").Namespace.ToString());
