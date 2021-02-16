using AspNetCoreService.CoreModel;
using FluentAssertions;
using Xunit;

namespace AspNetCoreService.Tests.CoreModel
{
    public static class EntityTests
    {
        [Theory]
        [MemberData(nameof(Ids))]
        public static void EqualsMustReturnTrueForEntitiesWithSameId(int id)
        {
            var firstEntity = new DummyEntity(id);
            var secondEntity = new DummyEntity(id);

            var result1 = firstEntity.Equals(secondEntity);
            var result2 = secondEntity.Equals(firstEntity);

            result1.Should().BeTrue();
            result2.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(42, 87)]
        public static void EqualsMustReturnFalseForEntitiesWithDifferentIds(int firstId, int secondId)
        {
            var firstEntity = new DummyEntity(firstId);
            var secondEntity = new DummyEntity(secondId);

            var result1 = firstEntity.Equals(secondEntity);
            var result2 = secondEntity.Equals(firstEntity);

            result1.Should().BeFalse();
            result2.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(Ids))]
        public static void GetHashCodeMustReturnId(int id) =>
            new DummyEntity(id).GetHashCode().Should().Be(id);

        [Fact]
        public static void ObjectEqualsWithDifferentObjectsMustReturnFalse()
        {
            object entity = new DummyEntity(1);
            const string otherObject = "Foo";

            entity.Equals(otherObject).Should().BeFalse();
        }

        [Fact]
        public static void ObjectEqualsWithSimilarEntityShouldReturnTrue()
        {
            object firstEntity = new DummyEntity(42);
            object secondEntity = new DummyEntity(42);

            firstEntity.Equals(secondEntity).Should().BeTrue();
        }

        [Fact]
        public static void ObjectEqualsWithNullMustReturnFalse()
        {
            object entity = new DummyEntity(55);

            entity.Equals(null).Should().BeFalse();
        }

        public static readonly TheoryData<int> Ids =
            new()
            {
                1,
                13,
                -59,
                1337
            };

        private sealed class DummyEntity : Entity<DummyEntity>
        {
            public DummyEntity(int id) => Id = id;
        }
    }
}