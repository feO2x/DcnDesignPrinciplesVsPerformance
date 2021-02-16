using System;

namespace AspNetCoreService.CoreModel
{
    public static class DateOfBirth
    {
        private static Func<int, int, int> FakerNextRandomNumber { get; } = Faker.RandomNumber.Next;

        public static DateTime CreateRandom(DateTime? today = null, Func<int, int, int>? getNextRandomNumber = null)
        {
            var todayValue = today ?? DateTime.Today;
            getNextRandomNumber ??= FakerNextRandomNumber;

            var age = getNextRandomNumber(18, 63);
            var year = todayValue.Year - age;

            var month = getNextRandomNumber(1, 12);
            var isYearDecremented = false;
            if (month > todayValue.Month)
            {
                year--;
                isYearDecremented = true;
            }

            var numberOfDaysInTargetMonth = DateTime.DaysInMonth(year, month);
            var day = getNextRandomNumber(1, numberOfDaysInTargetMonth);
            if (!isYearDecremented && todayValue.Month == month && day > todayValue.Day)
            {
                year--;
                if (month == 2 && day == 29)
                {
                    numberOfDaysInTargetMonth = DateTime.DaysInMonth(year, 2);
                    day = getNextRandomNumber(1, numberOfDaysInTargetMonth);
                }
            }

            return new DateTime(year, month, day);
        }
    }
}