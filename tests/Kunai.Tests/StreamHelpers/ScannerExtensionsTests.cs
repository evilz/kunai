using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Kunai.Stream;
using NUnit.Framework;

namespace KunaiTests.StreamHelpers
{
	[TestFixture]
	public class ScannerExtensionsTests
	{
		[Test]
		public void NextString_Should_Return_string_WhenExists()
		{
			string input = "    the string  032423";
			var reader = new StringReader(input);
			
			var s1 = reader.NextString();
			var s2 = reader.NextString();
			var s3 = reader.NextString();
			
			Assert.AreEqual("the",s1);
			Assert.AreEqual("string",s2);
			Assert.AreEqual("032423",s3);
		}

		[Test]
		public void NextNumber_Should_Return_int_When_TypeIsNotGiven()
		{
			string input = "    032423  ";
			var reader = new StringReader(input);

			var i = reader.NextNumber();
			
			Assert.AreEqual(32423, i);
		}


		// TODO : MAKE CASES
		[Test]
		public void NextNumber_Should_Return_SpecifiedType()
		{
			string input = "    032423  ";
			var reader = new StringReader(input);
			var l = reader.NextNumber<long>();
			Assert.AreEqual(32423L, l);
		}


		// TODO : MAKE CASES
		[Test]
		public void NextArray_Should_Return_ArrayOfSpecifiedType()
		{
			string input = "    032423 45 -678";
			var reader = new StringReader(input);
			var a = reader.NextNumberArray<long>(3);
			CollectionAssert.AreEqual(new long[] { 32423L , 45L, -678L },a);
		}


	}
}
