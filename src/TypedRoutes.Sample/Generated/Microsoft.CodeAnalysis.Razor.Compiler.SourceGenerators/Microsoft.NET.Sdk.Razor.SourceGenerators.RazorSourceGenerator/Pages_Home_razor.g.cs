﻿#pragma checksum "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor" "{8829d00f-11b8-4213-878b-770e8597ac16}" "54346abddc3f4cbf32b7d382cfed8abf2d50f56cf6d4aa0429a12e9976f62f57"
// <auto-generated/>
#pragma warning disable 1591
namespace PodNet.Blazor.TypedRoutes.Sample.Pages
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using PodNet.Blazor.TypedRoutes.Sample;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using PodNet.Blazor.TypedRoutes.Sample.Layout;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Home : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1><code>PodNet.Blazor.TypedRoutes.Sample</code></h1>\r\n\r\n");
            __builder.AddMarkupContent(1, "<p>Yo! This is a simple sample app to demonstrate how the above project generates typed routing code for Blazor components.</p>\r\n\r\n");
            __builder.AddMarkupContent(2, "<p>In this project, there are a few components and various kinds/number of routes defined for them. Click on the name of the component below to see the code of the component and the relevant generated code.</p>");
#nullable restore
#line 9 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
 if (_components == null)
    return;

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(3, "<hr>\r\n\r\n");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "accordion");
            __builder.AddAttribute(6, "id", "examples-accordion");
#nullable restore
#line 15 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
     foreach (var item in _components)
    {
        var route = item;

#line default
#line hidden
#nullable disable
            __builder.OpenElement(7, "section");
            __builder.AddAttribute(8, "class", "accordion-item");
            __builder.OpenElement(9, "h2");
            __builder.AddAttribute(10, "class", "accordion-header");
            __builder.OpenElement(11, "button");
            __builder.AddAttribute(12, "class", "accordion-button" + " " + (
#nullable restore
#line 20 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
                                                  route.Show ? "" : "collapsed"

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(13, "onclick", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 20 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
                                                                                            () => { foreach(var c in _components.Where(c => c != route)) c.Show = false; route.Show = !route.Show; }

#line default
#line hidden
#nullable disable
            ));
#nullable restore
#line (21,22)-(21,41) 25 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
__builder.AddContent(14, route.ComponentName);

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(15, "\r\n            ");
            __builder.OpenElement(16, "div");
            __builder.AddAttribute(17, "class", "accordion-collapse" + " collapse" + " " + (
#nullable restore
#line 24 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
                                                      route.Show ? "show" : ""

#line default
#line hidden
#nullable disable
            ));
            __builder.OpenElement(18, "div");
            __builder.AddAttribute(19, "class", "row");
            __builder.OpenElement(20, "div");
            __builder.AddAttribute(21, "class", "col-md-6");
            __builder.AddMarkupContent(22, "<h3>Razor</h3>\r\n                        ");
            __builder.OpenElement(23, "pre");
            __builder.AddAttribute(24, "style", "white-space: pre-wrap");
            __builder.OpenElement(25, "code");
#nullable restore
#line (28,67)-(28,78) 25 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
__builder.AddContent(26, route.Razor);

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(27, "\r\n                    ");
            __builder.OpenElement(28, "div");
            __builder.AddAttribute(29, "class", "col-md-6");
            __builder.AddMarkupContent(30, "<h3>Generated</h3>\r\n                        ");
            __builder.OpenElement(31, "pre");
            __builder.AddAttribute(32, "style", "white-space: pre-wrap");
            __builder.OpenElement(33, "code");
#nullable restore
#line (32,67)-(32,82) 25 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
__builder.AddContent(34, route.Generated);

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 37 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 40 "D:\source\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Home.razor"
       
    IReadOnlyList<TypedRoute>? _components;

    protected override void OnInitialized()
    {
        _components = GetType().Assembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Any(i => i == typeof(INavigableComponent) || i == typeof(IRoutableComponent))).Select((component, index) =>
        {
            if (component.FullName == null)
                return null;
            var razorStream = GetType().Assembly.GetManifestResourceStream($"{component.FullName}.razor");
            var generatedStream = GetType().Assembly.GetManifestResourceStream($"PodNet.Blazor.TypedRoutes.Sample.Generated.PodNet.Blazor.TypedRoutes.PodNet.Blazor.TypedRoutes.TypedRoutesGenerator.{component.FullName.Replace('.', '_')}.g.cs");
            var relativeName = component.FullName.Split("PodNet.Blazor.TypedRoutes.Sample.").ElementAtOrDefault(1);
            if (razorStream == null || generatedStream == null || string.IsNullOrWhiteSpace(relativeName))
                return null;
            using var razorStreamReader = new StreamReader(razorStream);
            using var generatedStreamReader = new StreamReader(generatedStream);

            return new TypedRoute(relativeName, index, razorStreamReader.ReadToEnd(), generatedStreamReader.ReadToEnd())
                {
                    Show = index == 0
                };
        }).Where(c => c != null).Cast<TypedRoute>().ToList();
    }

    public record TypedRoute(string ComponentName, int Index, string Razor, string Generated)
    {
        public bool Show { get; set; }
    };

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591