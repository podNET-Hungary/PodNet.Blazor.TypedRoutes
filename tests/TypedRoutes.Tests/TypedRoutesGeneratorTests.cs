using PodNet.Blazor.TypedRoutes.Generator;
using PodNet.Blazor.TypedRoutes.Tests.Cases;

namespace PodNet.Blazor.TypedRoutes.Tests;

[TestClass] public class _1_DefaultHappyPath : TypedRoutesGeneratorTestCase<Cases._1_DefaultHappyPath>;
[TestClass] public class _2_MultipleRoutes : TypedRoutesGeneratorTestCase<Cases._2_MultipleRoutes>;
[TestClass] public class _3_MultipleParameterless() : TypedRoutesGeneratorTestCase<Cases._3_MultipleParameterless>(true);
[TestClass] public class _4_CSharpComponent_1_NotPartialWarns() : TypedRoutesGeneratorTestCase<Cases._4_CSharpComponent._1_NotPartialWarns>([TypedRoutesGenerator.RoutedComponentShouldBePartialDescriptor]);
[TestClass] public class _4_CSharpComponent_2_PartialGeneratesAsExpected : TypedRoutesGeneratorTestCase<Cases._4_CSharpComponent._2_PartialGeneratesAsExpected>;
[TestClass] public class _5_RazorAndCSharpPartial() : TypedRoutesGeneratorTestCase<Cases._5_RazorAndCSharpPartial>(true);
