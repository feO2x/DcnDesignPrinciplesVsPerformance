using System;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AspNetCoreService.Tests.TestHelpers
{
    public static class Extensions
    {
        public static void MustBeDecoratedWithRouteAttribute(this Type controllerType, string expectedRoute)
        {
            var routeAttribute = controllerType.GetCustomAttribute<RouteAttribute>();
            if (routeAttribute == null)
                throw new XunitException($"The controller {controllerType} is not decorated with the route attribute.");
            routeAttribute.Template.Should().Be(expectedRoute);
        }

        public static void MustBeEquivalentToValidationProblem(this IActionResult actualResult, ActionResult expectedResult)
        {
            var resultDetails = actualResult.MustBeOfType<ObjectResult>().Value.MustBeOfType<ValidationProblemDetails>();
            resultDetails.Should().BeEquivalentTo(expectedResult.MustBeOfType<ObjectResult>().Value.MustBeOfType<ValidationProblemDetails>());
        }

        public static void ShouldBeWrittenTo(this Exception exception, ITestOutputHelper output) =>
            output.WriteLine(exception.ToString());
    }
}