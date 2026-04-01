using Kunai.TextExt;
using NUnit.Framework;

namespace KunaiTests.Text
{
    [TestFixture]
    public class TextExtensionsTests
    {
        [Test]
        public void FormatWithMask_AppliesMask()
        {
            var result = "12345".FormatWithMask("##-##-#");
            Assert.AreEqual("12-34-5", result);
        }

        [Test]
        public void FormatWithMask_EmptyInput_ReturnsEmpty()
        {
            var result = "".FormatWithMask("##-##");
            Assert.AreEqual("", result);
        }

        [Test]
        public void AsBoolean_TrueValues_ReturnsTrue()
        {
            Assert.IsTrue("yes".AsBoolean());
            Assert.IsTrue("true".AsBoolean());
            Assert.IsTrue("1".AsBoolean());
            Assert.IsTrue("y".AsBoolean());
            Assert.IsTrue("t".AsBoolean());
        }

        [Test]
        public void AsBoolean_FalseValue_ReturnsFalse()
        {
            Assert.IsFalse("no".AsBoolean());
            Assert.IsFalse("false".AsBoolean());
        }

        [Test]
        public void IsBoolean_ValidBooleanStrings_ReturnsTrue()
        {
            Assert.IsTrue("true".IsBoolean());
            Assert.IsTrue("false".IsBoolean());
            Assert.IsTrue("yes".IsBoolean());
            Assert.IsTrue("no".IsBoolean());
        }

        [Test]
        public void IsBoolean_InvalidString_ReturnsFalse()
        {
            Assert.IsFalse("maybe".IsBoolean());
        }

        [Test]
        public void TakeFrom_ReturnsSubstringFromSearch()
        {
            var result = "Hello World".TakeFrom("World");
            Assert.AreEqual("World", result);
        }

        [Test]
        public void TakeFrom_SearchNotFound_ReturnsOriginal()
        {
            var result = "Hello World".TakeFrom("Missing");
            Assert.AreEqual("Hello World", result);
        }

        [Test]
        public void Truncate_LongString_TruncatesWithEllipsis()
        {
            var result = "Hello World".Truncate(8);
            Assert.AreEqual("Hello...", result);
        }

        [Test]
        public void Truncate_ShortString_ReturnsOriginal()
        {
            var result = "Hi".Truncate(10);
            Assert.AreEqual("Hi", result);
        }

        [Test]
        public void RightOf_CharFound_ReturnsRightPart()
        {
            var result = "key=value".RightOf('=');
            Assert.AreEqual("value", result);
        }

        [Test]
        public void LeftOf_CharFound_ReturnsLeftPart()
        {
            var result = "key=value".LeftOf('=');
            Assert.AreEqual("key", result);
        }

        [Test]
        public void Right_ReturnsLastNChars()
        {
            var result = "Hello".Right(3);
            Assert.AreEqual("llo", result);
        }

        [Test]
        public void Left_ReturnsFirstNChars()
        {
            var result = "Hello".Left(3);
            Assert.AreEqual("Hel", result);
        }

        [Test]
        public void Reverse_ReversesString()
        {
            var result = "abc".Reverse();
            Assert.AreEqual("cba", result);
        }

        [Test]
        public void WordCount_CountsWords()
        {
            var result = "the quick brown fox".WordCount();
            Assert.AreEqual(4, result);
        }

        [Test]
        public void IsValidEmailAddress_ValidEmail_ReturnsTrue()
        {
            Assert.IsTrue("user@example.com".IsValidEmailAddress());
        }

        [Test]
        public void IsValidEmailAddress_InvalidEmail_ReturnsFalse()
        {
            Assert.IsFalse("not-an-email".IsValidEmailAddress());
        }

        [Test]
        public void IsValidIPAddress_ValidIp_ReturnsTrue()
        {
            Assert.IsTrue("192.168.1.1".IsValidIPAddress());
        }

        [Test]
        public void IsValidIPAddress_InvalidIp_ReturnsFalse()
        {
            Assert.IsFalse("999.999.999.999".IsValidIPAddress());
        }

        [Test]
        public void IsStrongPassword_StrongPassword_ReturnsTrue()
        {
            Assert.IsTrue("P@ssw0rd!".IsStrongPassword());
        }

        [Test]
        public void IsStrongPassword_WeakPassword_ReturnsFalse()
        {
            Assert.IsFalse("password".IsStrongPassword());
        }

        [Test]
        public void NullToEmpty_NullInput_ReturnsEmpty()
        {
            string? s = null;
            Assert.AreEqual(string.Empty, s!.NullToEmpty());
        }

        [Test]
        public void NullToEmpty_NonNullInput_ReturnsOriginal()
        {
            Assert.AreEqual("hello", "hello".NullToEmpty());
        }

        [Test]
        public void ToPlural_RegularWord_AddsS()
        {
            Assert.AreEqual("cats", "cat".ToPlural());
        }

        [Test]
        public void ToPlural_WordEndingInY_ChangesToIes()
        {
            Assert.AreEqual("countries", "country".ToPlural());
        }

        [Test]
        public void StripHtml_RemovesTags()
        {
            var result = "<b>Hello</b> World".StripHtml();
            Assert.IsTrue(result.Contains("Hello"));
            Assert.IsFalse(result.Contains("<b>"));
        }

        [Test]
        public void CompressAndDecompress_RoundTrip()
        {
            var original = "This is a test string for compression.";
            var compressed = original.CompressString();
            var decompressed = TextExtensions.DecompressString(compressed);
            Assert.AreEqual(original, decompressed);
        }

        [Test]
        public void AsStream_ReturnsReadableStream()
        {
            var stream = "hello".AsStream();
            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.Length > 0);
        }

        [Test]
        public void AsTextReader_ReturnsReader()
        {
            var reader = "hello world".AsTextReader();
            var text = reader.ReadToEnd();
            Assert.AreEqual("hello world", text);
        }

        [Test]
        public void ExcelColumnIndex_A_Returns1()
        {
            Assert.AreEqual(1, "A".ExcelColumnIndex());
        }

        [Test]
        public void ExcelColumnIndex_Z_Returns26()
        {
            Assert.AreEqual(26, "Z".ExcelColumnIndex());
        }

        [Test]
        public void IsGuid_ValidGuid_ReturnsTrue()
        {
            Assert.IsTrue("550e8400-e29b-41d4-a716-446655440000".IsGuid());
        }

        [Test]
        public void IsGuid_InvalidGuid_ReturnsFalse()
        {
            Assert.IsFalse("not-a-guid".IsGuid());
        }

        [Test]
        public void ContainsNoSpaces_AlphanumericString_ReturnsTrue()
        {
            Assert.IsTrue("abc123".ContainsNoSpaces());
        }

        [Test]
        public void ContainsNoSpaces_StringWithSpace_ReturnsFalse()
        {
            Assert.IsFalse("abc 123".ContainsNoSpaces());
        }

        [Test]
        public void ToNameValueCollection_ParsesKeyValuePairs()
        {
            var nvc = "a=1;b=2".ToNameValueCollection(';', '=');
            Assert.AreEqual("1", nvc!["a"]);
            Assert.AreEqual("2", nvc!["b"]);
        }

        [Test]
        public void CSVSplit_SplitsCorrectly()
        {
            var result = new System.Collections.Generic.List<string>("a,b,c".CSVSplit());
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, result);
        }

        [Test]
        public void CSVSplit_HandlesQuotedValues()
        {
            var result = new System.Collections.Generic.List<string>("\"a,b\",c".CSVSplit());
            Assert.AreEqual("a,b", result[0]);
            Assert.AreEqual("c", result[1]);
        }

        [Test]
        public void AppendIf_ConditionTrue_Appends()
        {
            var sb = new System.Text.StringBuilder("Hello");
            sb.AppendIf(true, " World");
            Assert.AreEqual("Hello World", sb.ToString());
        }

        [Test]
        public void AppendIf_ConditionFalse_DoesNotAppend()
        {
            var sb = new System.Text.StringBuilder("Hello");
            sb.AppendIf(false, " World");
            Assert.AreEqual("Hello", sb.ToString());
        }
    }
}
