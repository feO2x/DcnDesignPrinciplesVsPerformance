using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AspNetCoreService.Tests.TestHelpers
{
    public abstract class WebApiControllerTests<T>
    {
        protected WebApiControllerTests(string expectedRoute) => ExpectedRoute = expectedRoute;

        protected static Type ControllerType { get; } = typeof(T);

        protected string ExpectedRoute { get; }

        [Fact]
        public void MustDeriveFromControllerBase() =>
            ControllerType.Should().BeDerivedFrom<ControllerBase>();

        [Fact]
        public void MustBeDecoratedWithApiControllerAttribute() =>
            ControllerType.Should().BeDecoratedWith<ApiControllerAttribute>();

        [Fact]
        public void MustHaveTheProperRouteApplied() =>
            ControllerType.MustBeDecoratedWithRouteAttribute(ExpectedRoute);

        [Fact]
        public void MustBeSealed() => ControllerType.Should().BeSealed();
    }
}