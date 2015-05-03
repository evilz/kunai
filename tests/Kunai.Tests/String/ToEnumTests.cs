using Kunai.TextExt;
using NUnit.Framework;

namespace KunaiTests.String
{
	public class ToEnumTests
	{
		[Test]
		public void ToEnumShouldParseEqualString()
		{
			const string test = "Abc";
			TestEnum? result = test.ToEnum<TestEnum>();
			Assert.AreEqual(TestEnum.Abc, result);
		}

		[Test]
		public void ToEnumShouldParseWrongCaseString()
		{
			const string test = "xyz";
			TestEnum? result = test.ToEnum<TestEnum>();
			Assert.AreEqual(TestEnum.Xyz, result);
		}

		[Test]
		public void ToEnumShouldBeDefaultForInvalidString()
		{
			const string test = "lmn";
			var result = test.ToEnum<TestEnum>();
			Assert.AreEqual(TestEnum.Abc, result);
		}

		private enum TestEnum
		{
			Abc,
			Xyz
		}
	}
}