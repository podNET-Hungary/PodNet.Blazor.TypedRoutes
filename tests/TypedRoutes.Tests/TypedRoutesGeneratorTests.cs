using PodNet.Blazor.TypedRoutes.Generator;
using PodNet.Blazor.TypedRoutes.Tests.Cases;

namespace PodNet.Blazor.TypedRoutes.Tests;

[TestClass] public class _1_DefaultHappyPath : TypedRoutesGeneratorTestCase<Cases._1_DefaultHappyPath>;
[TestClass] public class _2_MultipleRoutes : TypedRoutesGeneratorTestCase<Cases._2_MultipleRoutes>;
[TestClass] public class _3_MultipleParameterless() : TypedRoutesGeneratorTestCase<Cases._3_MultipleParameterless>(true);
[TestClass] public class _4_CSharpComponent_1_NotPartialWarns() : TypedRoutesGeneratorTestCase<Cases._4_CSharpComponent._1_NotPartialWarns>([TypedRoutesGenerator.RoutedComponentShouldBePartialDescriptor]);
[TestClass] public class _4_CSharpComponent_2_PartialGeneratesAsExpected : TypedRoutesGeneratorTestCase<Cases._4_CSharpComponent._2_PartialGeneratesAsExpected>;
[TestClass] public class _5_RazorAndCSharpPartial() : TypedRoutesGeneratorTestCase<Cases._5_RazorAndCSharpPartial>(true);
[TestClass] public class _6_NamespaceDirective() : TypedRoutesGeneratorTestCase<Cases._6_NamespaceDirective>(ignoreSources: true, fakePublicRazorPartials: false);
[TestClass] public class _7_TypeParameters_1_Single() : TypedRoutesGeneratorTestCase<Cases._7_TypeParameters._1_Single>(true);
[TestClass] public class _7_TypeParameters_2_Class() : TypedRoutesGeneratorTestCase<Cases._7_TypeParameters._2_Class>(true);
[TestClass] public class _7_TypeParameters_3_Multiple() : TypedRoutesGeneratorTestCase<Cases._7_TypeParameters._3_Multiple>(true);
[TestClass] public class _7_TypeParameters_4_ClassWithMultipleAndConstraints() : TypedRoutesGeneratorTestCase<Cases._7_TypeParameters._4_ClassWithMultipleAndConstraints>(true);
[TestClass] public class _7_TypeParameters_5_MultipleWithConstraints() : TypedRoutesGeneratorTestCase<Cases._7_TypeParameters._5_MultipleWithConstraints>(true);
[TestClass] public class _8_RouteConstraints() : TypedRoutesGeneratorTestCase<Cases._8_RouteConstraints>(true);
