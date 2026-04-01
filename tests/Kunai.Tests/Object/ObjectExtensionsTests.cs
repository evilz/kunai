using System.Collections.Generic;
using Kunai.ObjectExt;
using NUnit.Framework;

namespace KunaiTests.Object
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void IsBetween_ValueWithin_ReturnsTrue()
        {
            Assert.IsTrue(5.IsBetween(1, 10));
        }

        [Test]
        public void IsBetween_ValueAtLowerBound_ReturnsTrue()
        {
            Assert.IsTrue(1.IsBetween(1, 10));
        }

        [Test]
        public void IsBetween_ValueAtUpperBound_ReturnsFalse()
        {
            // upper bound is exclusive
            Assert.IsFalse(10.IsBetween(1, 10));
        }

        [Test]
        public void IsBetween_ValueOutside_ReturnsFalse()
        {
            Assert.IsFalse(11.IsBetween(1, 10));
        }

        [Test]
        public void IsDefault_DefaultInt_ReturnsTrue()
        {
            Assert.IsTrue(0.IsDefault());
        }

        [Test]
        public void IsDefault_NonDefaultInt_ReturnsFalse()
        {
            Assert.IsFalse(5.IsDefault());
        }

        [Test]
        public void IsIn_ValueInList_ReturnsTrue()
        {
            Assert.IsTrue(3.IsIn(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void IsIn_ValueNotInList_ReturnsFalse()
        {
            Assert.IsFalse(5.IsIn(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void HasValueAndEquals_NullableWithMatchingValue_ReturnsTrue()
        {
            int? x = 42;
            Assert.IsTrue(x.HasValueAndEquals(42));
        }

        [Test]
        public void HasValueAndEquals_NullableWithDifferentValue_ReturnsFalse()
        {
            int? x = 42;
            Assert.IsFalse(x.HasValueAndEquals(99));
        }

        [Test]
        public void HasValueAndEquals_NullNullable_ReturnsFalse()
        {
            int? x = null;
            Assert.IsFalse(x.HasValueAndEquals(42));
        }

        [Test]
        public void GetPropertyDictionary_ReturnsAllProperties()
        {
            var obj = new TestClass { Name = "Alice", Value = 42 };
            var dict = obj.GetPropertyDictionary();
            Assert.AreEqual("Alice", dict["Name"]);
            Assert.AreEqual(42, dict["Value"]);
        }

        [Test]
        public void ChangeType_ConvertsIntToDouble()
        {
            object val = 42;
            var result = val.ChangeType<double>();
            Assert.AreEqual(42.0, result);
        }

        [Test]
        public void ChangeType_ConvertsStringToInt()
        {
            object val = "123";
            var result = val.ChangeType<int>();
            Assert.AreEqual(123, result);
        }

        [Test]
        public void Format_UsesFormatString()
        {
            var result = 42.5.Format("{0:F1}");
            Assert.AreEqual("42.5", result);
        }

        [Test]
        public void IfType_MatchingType_ExecutesAction()
        {
            object obj = "hello";
            var executed = false;
            obj.IfType<string>(_ => executed = true);
            Assert.IsTrue(executed);
        }

        [Test]
        public void IfType_NonMatchingType_DoesNotExecute()
        {
            object obj = 42;
            var executed = false;
            obj.IfType<string>(_ => executed = true);
            Assert.IsFalse(executed);
        }

        [Test]
        public void Or_ReturnsFirstNonNull()
        {
            string? a = null;
            string? b = null;
            string c = "value";
            var result = a.Or(b!, c);
            Assert.AreEqual("value", result);
        }

        private class TestClass
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }
    }
}
