using Kunai.NumberExt;
using NUnit.Framework;

namespace KunaiTests.Number;

    [TestFixture]
    public class NumberExtensionsTests
    {
        [Test]
        public void ExcelColumnName_1_ReturnsA()
        {
            Assert.AreEqual("A", 1.ExcelColumnName());
        }

        [Test]
        public void ExcelColumnName_26_ReturnsZ()
        {
            Assert.AreEqual("Z", 26.ExcelColumnName());
        }

        [Test]
        public void ExcelColumnName_27_ReturnsAA()
        {
            Assert.AreEqual("AA", 27.ExcelColumnName());
        }

        [Test]
        public void IsInRange_ValueWithinRange_ReturnsTrue()
        {
            Assert.IsTrue(5.IsInRange(1, 10));
        }

        [Test]
        public void IsInRange_ValueAtLowerBound_ReturnsTrue()
        {
            Assert.IsTrue(1.IsInRange(1, 10));
        }

        [Test]
        public void IsInRange_ValueAtUpperBound_ReturnsTrue()
        {
            Assert.IsTrue(10.IsInRange(1, 10));
        }

        [Test]
        public void IsInRange_ValueOutsideRange_ReturnsFalse()
        {
            Assert.IsFalse(11.IsInRange(1, 10));
        }

        [Test]
        public void KB_ConvertsCorrectly()
        {
            Assert.AreEqual(1024, 1.KB());
        }

        [Test]
        public void MB_ConvertsCorrectly()
        {
            Assert.AreEqual(1048576, 1.MB());
        }

        [Test]
        public void GB_ConvertsCorrectly()
        {
            Assert.AreEqual(1073741824, 1.GB());
        }

        [Test]
        public void TB_ConvertsCorrectly()
        {
            Assert.AreEqual(1099511627776L, 1.TB());
        }

        [Test]
        public void ToReadableSize_Bytes()
        {
            Assert.AreEqual("512 bytes", 512L.ToReadableSize());
        }

        [Test]
        public void ToReadableSize_Kilobytes()
        {
            Assert.AreEqual("2 KB", 2048L.ToReadableSize());
        }

        [Test]
        public void IsPrime_2_ReturnsTrue()
        {
            Assert.IsTrue(2.IsPrime());
        }

        [Test]
        public void IsPrime_7_ReturnsTrue()
        {
            Assert.IsTrue(7.IsPrime());
        }

        [Test]
        public void IsPrime_4_ReturnsFalse()
        {
            Assert.IsFalse(4.IsPrime());
        }

        [Test]
        public void IsPrime_1_ReturnsFalse()
        {
            Assert.IsFalse(1.IsPrime());
        }

        [Test]
        public void ToLocalCurrencyString_ReturnsNonNullString()
        {
            var result = 42.5.ToLocalCurrencyString();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void StdDevP_PopulationStdDev_ReturnsCorrectValue()
        {
            // Test with double[] so the buffer parameter is correctly applied
            var data = new double[] { 2, 4, 4, 4, 5, 5, 7, 9 };
            var result = data.StdDevP();
            Assert.AreEqual(2.0, result, 0.0001);
        }

        [Test]
        public void StdDev_SampleStdDev_ReturnsCorrectValue()
        {
            var data = new double[] { 2, 4, 4, 4, 5, 5, 7, 9 };
            var result = data.StdDev();
            Assert.AreEqual(2.138, result, 0.001);
        }
    }
