// Copyright (c) Vincent Bourdon. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using Kunai.TextExt;
using NUnit.Framework;

namespace KunaiTests
{
	[TestFixture]
	public class StringTest1
	{
		private const decimal d = -8.99999M;

		private static object[] SpecifiedTypeCase =
		{
			new object[] {"foo bar foo", "foo {String} foo", "bar"},
			new object[] {"foo 32767 foo", "foo {Int16} foo", Int16.MaxValue},
			new object[] {"foo 65535 foo", "foo {UInt16} foo", UInt16.MaxValue},
			new object[] {"foo 2147483647 foo", "foo {Int32} foo", Int32.MaxValue},
			new object[] {"foo 4294967295 foo", "foo {UInt32} foo", UInt32.MaxValue},
			new object[] {"foo 9223372036854775807 foo", "foo {Int64} foo", Int64.MaxValue},
			new object[] {"foo 18446744073709551615 foo", "foo {UInt64} foo", UInt64.MaxValue},
			new object[] {"foo -8.9 foo", "foo {Single} foo", -8.9F},
			new object[] {"foo -8.9 foo", "foo {Double} foo", -8.9D},
			new object[] {"foo true foo", "foo {Boolean} foo", true},
			new object[] {"foo 255 foo", "foo {Byte} foo", byte.MaxValue},
			new object[] {"foo 127 foo", "foo {SByte} foo", SByte.MaxValue},
			new object[] {"foo H foo", "foo {Char} foo", 'H'},
			new object[] {"foo -8.99999 foo", "foo {Decimal} foo", -8.99999M}
		};

		private static object[] SpecifiedIndexCase =
		{
			new object[] {"foo bar foo", "foo {0} foo",string.Empty, "bar"},
			new object[] {"foo 32767 foo", "foo {0} foo",(Int16)0, Int16.MaxValue},
			new object[] {"foo 65535 foo", "foo {0} foo",(UInt16)0, UInt16.MaxValue},
			new object[] {"foo 2147483647 foo", "foo {0} foo",0, Int32.MaxValue},
			new object[] {"foo 4294967295 foo", "foo {0} foo",(UInt32)0, UInt32.MaxValue},
			new object[] {"foo 9223372036854775807 foo", "foo {0} foo",(Int64)0, Int64.MaxValue},
			new object[] {"foo 18446744073709551615 foo", "foo {0} foo",(UInt64)0, UInt64.MaxValue},
			new object[] {"foo -8.9 foo", "foo {0} foo",0F, -8.9F},
			new object[] {"foo -8.9 foo", "foo {0} foo",0D, -8.9D},
			new object[] {"foo true foo", "foo {0} foo",false, true},
			new object[] {"foo 255 foo", "foo {0} foo",(byte)0, byte.MaxValue},
			new object[] {"foo 127 foo", "foo {0} foo",(SByte)0, SByte.MaxValue},
			new object[] {"foo H foo", "foo {0} foo",' ' ,'H'},
			new object[] {"foo -8.99999 foo", "foo {0} foo",0M, -8.99999M}
		};

		[TestCaseSource("SpecifiedTypeCase")]
		public void ScanFormat_Schould_ScanForSpecifiedType(string input, string format, object expected)
		{
			var result = input.ScanFormat(format);
			Assert.AreEqual(1,result.Length);
			Assert.AreEqual(expected, result[0]);
		}

		[TestCaseSource("SpecifiedIndexCase")]
		public void ScanFormat_Schould_ScanForSpecifiedIndex(string input, string format,object output, object expected)
		{
			var o = new object[] {output};
			input.ScanFormat(format,o);
			Assert.AreEqual(expected, o[0]);
		}

		//[Test]
		//public void ScanFormat_Schould_ScanNullableForSpecifiedIndex()
		//{
		//	var input = "The score is about 12/5";
		//	var format = "The score is about {0}/{1}";
		//	int? part1 = new int?();
		//	int? part2 = new int?();
		//	input.ScanFormat(format, part1, part2);
		//	Assert.AreEqual(part1.Value, 12);
		//	Assert.AreEqual(part2.Value, 5);
		//}

		[Test]
		public void ScanFormat_Schould_Scan7Tuple()
		{
			var text = "12 foo is not -12.5 | 888888";

			var output = text.ScanFormat<short, string, string, string, decimal, char, long>("{0} {1} {2} {3} {4} {5} {6}");
			Assert.AreEqual(12, output.Item1);
			Assert.AreEqual("foo", output.Item2);
			Assert.AreEqual("is", output.Item3);
			Assert.AreEqual("not", output.Item4);
			Assert.AreEqual(-12.5M, output.Item5);
			Assert.AreEqual('|', output.Item6);
			Assert.AreEqual(888888L, output.Item7);
		}

		[Test]
		public void ScanFormat_Schould_Scan6Tuple()
		{
			var text = "12 foo is not -12.5 | 888888";

			var output = text.ScanFormat<short, string, string, string, decimal, char>("{0} {1} {2} {3} {4} {5} 888888");
			Assert.AreEqual(12, output.Item1);
			Assert.AreEqual("foo", output.Item2);
			Assert.AreEqual("is", output.Item3);
			Assert.AreEqual("not", output.Item4);
			Assert.AreEqual(-12.5M, output.Item5);
			Assert.AreEqual('|', output.Item6);
		}

		[Test]
		public void ScanFormat_Schould_Scan5Tuple()
		{
			var text = "12 foo is not -12.5 | 888888";

			var output = text.ScanFormat<short, string, string, string, decimal>("{0} {1} {2} {3} {4} | 888888");
			Assert.AreEqual(12, output.Item1);
			Assert.AreEqual("foo", output.Item2);
			Assert.AreEqual("is", output.Item3);
			Assert.AreEqual("not", output.Item4);
			Assert.AreEqual(-12.5M, output.Item5);
		}

		[Test]
		public void ScanFormat_Schould_Scan4Tuple()
		{
			var text = "12 foo is not -12.5 | 888888";

			var output = text.ScanFormat<short, string, string, string>("{0} {1} {2} {3} -12.5 | 888888");
			Assert.AreEqual(12, output.Item1);
			Assert.AreEqual("foo", output.Item2);
			Assert.AreEqual("is", output.Item3);
			Assert.AreEqual("not", output.Item4);
		}

		[Test]
		public void ScanFormat_Schould_Scan3Tuple()
		{
			var text = "12 foo is not -12.5 | 888888";

			var output = text.ScanFormat<short, string, string>("{0} {1} {2} not -12.5 | 888888");
			Assert.AreEqual(12, output.Item1);
			Assert.AreEqual("foo", output.Item2);
			Assert.AreEqual("is", output.Item3);
		}

		[Test]
		public void ScanFormat_Schould_Scan2Tuple()
		{
			var text = "12 foo is not -12.5 | 888888";

			var output = text.ScanFormat<short, string, string>("{0} {1} is not -12.5 | 888888");
			Assert.AreEqual(12, output.Item1);
			Assert.AreEqual("foo", output.Item2);
		}

		[Test]
		public void ScanFormat_Schould_ScanSimpleValue()
		{
			var text = "12 foo is not -12.5 | 888888";

			var output = text.ScanFormat<short, string, string>("{0} foo is not -12.5 | 888888");
			Assert.AreEqual(12, output.Item1);
		}
	}
}
