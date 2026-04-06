using System;
using Kunai.DateTimeExt;
using NUnit.Framework;

namespace KunaiTests.DateTime;

    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void ToUnixTimestamp_EpochReturnsZero()
        {
            var epoch = new System.DateTime(1970, 1, 1, 0, 0, 0);
            Assert.AreEqual(0L, epoch.ToUnixTimestamp());
        }

        [Test]
        public void ToUnixTimestamp_KnownDate_ReturnsCorrectValue()
        {
            var date = new System.DateTime(2000, 1, 1, 0, 0, 0);
            Assert.AreEqual(946684800L, date.ToUnixTimestamp());
        }

        [Test]
        public void ToHumanTimeString_Milliseconds_ReturnsCorrectUnit()
        {
            var ts = TimeSpan.FromMilliseconds(500);
            Assert.IsTrue(ts.ToHumanTimeString().Contains("milliseconds"));
        }

        [Test]
        public void ToHumanTimeString_Seconds_ReturnsCorrectUnit()
        {
            var ts = TimeSpan.FromSeconds(30);
            Assert.IsTrue(ts.ToHumanTimeString().Contains("seconds"));
        }

        [Test]
        public void ToHumanTimeString_Minutes_ReturnsCorrectUnit()
        {
            var ts = TimeSpan.FromMinutes(5);
            Assert.IsTrue(ts.ToHumanTimeString().Contains("minutes"));
        }

        [Test]
        public void ToHumanTimeString_Hours_ReturnsCorrectUnit()
        {
            var ts = TimeSpan.FromHours(2);
            Assert.IsTrue(ts.ToHumanTimeString().Contains("hours"));
        }

        [Test]
        public void ToHumanTimeString_Days_ReturnsCorrectUnit()
        {
            var ts = TimeSpan.FromDays(3);
            Assert.IsTrue(ts.ToHumanTimeString().Contains("days"));
        }

        [Test]
        public void January_ReturnsCorrectDate()
        {
            var date = 15.January(2024);
            Assert.AreEqual(new System.DateTime(2024, 1, 15), date);
        }

        [Test]
        public void WeeksSelector_FromNow_IsApproxCorrect()
        {
            var twoWeeksFromNow = 2.Weeks().FromNow;
            var expected = System.DateTime.Now.AddDays(14);
            Assert.AreEqual(expected.Date, twoWeeksFromNow.Date);
        }

        [Test]
        public void DaysSelector_Ago_IsApproxCorrect()
        {
            var threeDaysAgo = 3.Days().Ago;
            var expected = System.DateTime.Now.AddDays(-3);
            Assert.AreEqual(expected.Date, threeDaysAgo.Date);
        }

        [Test]
        public void FirstDayOfMonth_ReturnsFirstDay()
        {
            var date = new System.DateTime(2024, 6, 15);
            Assert.AreEqual(new System.DateTime(2024, 6, 1), date.FirstDayOfMonth());
        }

        [Test]
        public void LastDayOfMonth_ReturnsLastDay()
        {
            var date = new System.DateTime(2024, 6, 15);
            Assert.AreEqual(new System.DateTime(2024, 6, 30), date.LastDayOfMonth());
        }

        [Test]
        public void IsWeekend_Saturday_ReturnsTrue()
        {
            var saturday = new System.DateTime(2024, 6, 1); // a Saturday
            Assert.IsTrue(saturday.IsWeekend());
        }

        [Test]
        public void IsWeekend_Monday_ReturnsFalse()
        {
            var monday = new System.DateTime(2024, 6, 3); // a Monday
            Assert.IsFalse(monday.IsWeekend());
        }

        [Test]
        public void GetDateRangeTo_ReturnsCorrectDays()
        {
            var start = new System.DateTime(2024, 1, 1);
            var end = new System.DateTime(2024, 1, 4);
            var range = start.GetDateRangeTo(end);
            Assert.AreEqual(3, System.Linq.Enumerable.Count(range));
        }

        [Test]
        public void ToRFC822DateString_ReturnsFormattedString()
        {
            var date = new System.DateTime(2024, 1, 15, 10, 30, 0);
            var result = date.ToRFC822DateString();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
