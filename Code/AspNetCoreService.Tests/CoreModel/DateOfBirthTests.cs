using System;
using AspNetCoreService.CoreModel;
using FluentAssertions;
using Xunit;

namespace AspNetCoreService.Tests.CoreModel
{
    public static class DateOfBirthTests
    {
        [Fact]
        public static void DateOfBirthInSameYear()
        {
            var today = new DateTime(2021, 2, 6);
            var numberGenerator = new NumberGeneratorStub(new[] { 21, 1, 25 });
            var expectedDateOfBirth = new DateTime(2000, 1, 25);
            CheckDateOfBirth(today, numberGenerator, expectedDateOfBirth);
        }

        [Fact]
        public static void DateOfBirthInLaterMonthThanToday()
        {
            var today = new DateTime(2021, 2, 7);
            var numberGenerator = new NumberGeneratorStub(new[] { 40, 5, 31 });
            var expectedDateOfBirth = new DateTime(1980, 5, 31);
            CheckDateOfBirth(today, numberGenerator, expectedDateOfBirth);
        }

        [Fact]
        public static void DateOfBirthOnLaterDayInSameMonth()
        {
            var today = new DateTime(2021, 3, 15);
            var numberGenerator = new NumberGeneratorStub(new[] { 33, 3, 28 });
            var expectedDateOfBirth = new DateTime(1987, 3, 28);
            CheckDateOfBirth(today, numberGenerator, expectedDateOfBirth);
        }

        [Fact]
        public static void DateOfBirthIsOn29thOfFebruary()
        {
            var today = new DateTime(2021, 2, 10);
            var numberGenerator = new NumberGeneratorStub(new[] { 29, 2, 29, 23 });
            var expectedDateOfBirth = new DateTime(1991, 2, 23);
            CheckDateOfBirth(today, numberGenerator, expectedDateOfBirth);
        }

        [Fact]
        public static void TodayIsBirthday()
        {
            var today = new DateTime(2021, 2, 12);
            var numberGenerator = new NumberGeneratorStub(new[] { 34, 2, 12 });
            var expectedDateOfBirth = new DateTime(1987, 2, 12);
            CheckDateOfBirth(today, numberGenerator, expectedDateOfBirth);
        }

        [Fact]
        public static void TodayIsBirthdayFebruary29Th()
        {
            var today = new DateTime(2021, 3, 1);
            var numberGenerator = new NumberGeneratorStub(new[] { 33, 2, 29 });
            var expectedDateOfBirth = new DateTime(1988, 2, 29);
            CheckDateOfBirth(today, numberGenerator, expectedDateOfBirth);
        }

        private static void CheckDateOfBirth(DateTime today, NumberGeneratorStub numberGenerator, DateTime expectedDateOfBirth)
        {
            var actualDateOfBirth = DateOfBirth.CreateRandom(today, numberGenerator.GetNextNumberDelegate);
            actualDateOfBirth.Should().Be(expectedDateOfBirth);
        }

        private sealed class NumberGeneratorStub
        {
            private readonly int[] _numbers;
            private int _currentIndex;

            public NumberGeneratorStub(int[] numbers)
            {
                _numbers = numbers;
                GetNextNumberDelegate = GetNextNumber;
            }

            public Func<int, int, int> GetNextNumberDelegate { get; }

            private int GetNextNumber(int min, int max) => _numbers[_currentIndex++];
        }
    }
}