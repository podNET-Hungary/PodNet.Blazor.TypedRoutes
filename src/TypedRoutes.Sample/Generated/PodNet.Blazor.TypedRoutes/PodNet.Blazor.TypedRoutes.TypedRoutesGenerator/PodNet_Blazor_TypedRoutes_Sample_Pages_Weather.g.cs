﻿// <auto-generated />
#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PodNet.Blazor.TypedRoutes;

namespace PodNet.Blazor.TypedRoutes.Sample.Pages
{
    partial class Weather : IRoutableComponent, INavigableComponent
    {
        /// <summary>
        /// The primary route template for the component, the constant string <c>"/weather"</c>.
        /// </summary>
        public static string PageRouteTemplate => "/weather";
    
        /// <summary>
        /// All available route templates for the component, containing the strings: <c>"/weather"</c>.
        /// </summary>
        public static IReadOnlyList<string> AllPageRouteTemplates { get; } = ImmutableArray.Create("/weather");

        /// <summary>
        /// Returns the absolute page URI: <c>"/weather"</c>.
        /// </summary>
        public static string PageUri => "/weather";

    }
}
#nullable restore