class T1 { }
class T2 : T1 { }

Assert.AreEqual("/my-component", MyComponent<T1, T2>.PageUri);

// The next one should not even compile given the constraints in the .razor file.
// However, constraints seem to be quite unstable in the framework itself when
// using a .razor with a partial .cs in itself. So unless this gives problems for
// users, we'll leave it as is, and if it breaks by a newer framework version, it
// needs to be investigated anyways.
Assert.AreEqual("/my-component", MyComponent<int, T2>.PageUri); 
