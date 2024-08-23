namespace _4_ClassWithMultipleAndConstraints;

[Microsoft.AspNetCore.Components.RouteAttribute("/my-component")]
public partial class MyComponent<T1, T2> where T1: struct where T2: IEnumerable<object>;
