using System.Linq;
using Kunai.TextExt;
using NUnit.Framework;

namespace KunaiTests.String
{
	[TestFixture]
	class GetMatchValueTests
	{
		const string INPUT = "the quick big brown fox jumps over the lazy dog";
		const string PATTERN = @"\b[a-zA-z]{3}\b";		// find three-letter words


		[Test]
			public void GetRegexMatches()
			{
				const int EXPECTED_MATCHED = 5;	// the, big, fox, the, dog

				var result = INPUT.GetMatchValue(PATTERN);

				Assert.AreEqual(EXPECTED_MATCHED, result.Count());
			}

			[Test]
			public void GetRegexUniqueMatches()
			{
				const int EXPECTED_MATCHED = 4;					// the, big, fox, dog

				var result = INPUT.GetMatchValue(PATTERN, true);

				Assert.AreEqual(EXPECTED_MATCHED, result.Count());
			
		}
	}
}
