﻿// <auto-generated />
#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PodNet.Blazor.TypedRoutes;

namespace PodNet.Blazor.TypedRoutes.Samples.Pages.Examples
{
    partial class Parameters : IRoutableComponent
    {
        /// <summary>
        /// The primary route template for the component, the constant string <c>"/examples/parameters/{id:int}"</c>.
        /// </summary>
        public static string PageRouteTemplate => "/examples/parameters/{id:int}";
    
        /// <summary>
        /// All available route templates for the component, containing the strings: <c>"/examples/parameters/{id:int}"</c>, <c>"/examples/parameters/{category}/{id:int}"</c>, <c>"/examples/parameters/{category}"</c>, <c>"/examples/parameters/{from:datetime}/{to:datetime?}"</c>.
        /// </summary>
        public static IReadOnlyList<string> AllPageRouteTemplates { get; } = ImmutableArray.Create("/examples/parameters/{id:int}", "/examples/parameters/{category}/{id:int}", "/examples/parameters/{category}", "/examples/parameters/{from:datetime}/{to:datetime?}");

        /// <summary>
        /// Returns the URI for the page constructed from the template <c>"/examples/parameters/{id:int}"</c> with
        /// the provided parameters.
        /// </summary>
        public static string PageUri1(int id) => $"/examples/parameters/{id}";
            
        /// <summary>
        /// Returns the URI for the page constructed from the template <c>"/examples/parameters/{category}/{id:int}"</c> with
        /// the provided parameters.
        /// </summary>
        public static string PageUri2(string category, int id) => $"/examples/parameters/{Uri.EscapeDataString(category)}/{id}";
            
        /// <summary>
        /// Returns the URI for the page constructed from the template <c>"/examples/parameters/{category}"</c> with
        /// the provided parameters.
        /// </summary>
        public static string PageUri3(string category) => $"/examples/parameters/{Uri.EscapeDataString(category)}";
            
        /// <summary>
        /// Returns the URI for the page constructed from the template <c>"/examples/parameters/{from:datetime}/{to:datetime?}"</c> with
        /// the provided parameters.
        /// </summary>
        public static string PageUri4(DateTime from, DateTime? to) => $"/examples/parameters/{from.ToString(from.TimeOfDay == default ? "yyyy-MM-dd" : "s")}/{to?.ToString(to.Value.TimeOfDay == default ? "yyyy-MM-dd" : "s")}";
            
    }
}
#nullable restore
