﻿#pragma checksum "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Counter.razor" "{8829d00f-11b8-4213-878b-770e8597ac16}" "886d5e15f1c63f4cc1caec7ff58936690cc18dd69931479d26471f7575854ec2"
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
#line 1 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using PodNet.Blazor.TypedRoutes.Sample;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\_Imports.razor"
using PodNet.Blazor.TypedRoutes.Sample.Shared;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Components.RouteAttribute("/counter")]
    public partial class Counter : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<global::Microsoft.AspNetCore.Components.Web.PageTitle>(0);
            __builder.AddAttribute(1, "ChildContent", (global::Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddContent(2, "Counter");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(3, "\r\n\r\n");
            __builder.AddMarkupContent(4, "<h1>Counter</h1>\r\n\r\n");
            __builder.OpenElement(5, "p");
            __builder.AddAttribute(6, "role", "status");
            __builder.AddContent(7, "Current count: ");
#nullable restore
#line (7,34)-(7,46) 24 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Counter.razor"
__builder.AddContent(8, currentCount);

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.AddMarkupContent(9, "\r\n\r\n");
            __builder.OpenElement(10, "button");
            __builder.AddAttribute(11, "class", "btn btn-primary");
            __builder.AddAttribute(12, "onclick", global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<global::Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 9 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Counter.razor"
                                          IncrementCount

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(13, "Click me");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 11 "C:\Users\podnet\source\repos\podNET-Hungary\blazor-typed-routes\src\TypedRoutes.Sample\Pages\Counter.razor"
       
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
