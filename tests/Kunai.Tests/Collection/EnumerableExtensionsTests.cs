using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kunai.CollectionExt;
using NUnit.Framework;

namespace KunaiTests.Collection
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void To_AscendingRange_ReturnsCharacters()
        {
            var result = 'a'.To('e').ToList();
            CollectionAssert.AreEqual(new[] { 'a', 'b', 'c', 'd', 'e' }, result);
        }

        [Test]
        public void To_DescendingRange_ReturnsReversedCharacters()
        {
            var result = 'e'.To('a').ToList();
            CollectionAssert.AreEqual(new[] { 'e', 'd', 'c', 'b', 'a' }, result);
        }

        [Test]
        public void Each_AppliesActionToAllElements()
        {
            var list = new List<int> { 1, 2, 3 };
            var sum = 0;
            list.Each(x => sum += x);
            Assert.AreEqual(6, sum);
        }

        [Test]
        public void Each_NullSource_DoesNotThrow()
        {
            IEnumerable<int>? list = null;
            Assert.DoesNotThrow(() => list!.Each(x => { }));
        }

        [Test]
        public void IsNullOrEmpty_NullSource_ReturnsTrue()
        {
            IEnumerable<int>? source = null;
            Assert.IsTrue(source!.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_EmptySource_ReturnsTrue()
        {
            var source = Enumerable.Empty<int>();
            Assert.IsTrue(source.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_NonEmptySource_ReturnsFalse()
        {
            var source = new[] { 1, 2, 3 };
            Assert.IsFalse(source.IsNullOrEmpty());
        }

        [Test]
        public void IndicesOf_FindsSingleValue()
        {
            var source = new[] { 10, 20, 30, 20 };
            var result = source.IndicesOf(20).ToList();
            CollectionAssert.AreEqual(new[] { 1, 3 }, result);
        }

        [Test]
        public void IndicesOf_FindsMultipleValues()
        {
            var source = new[] { 10, 20, 30, 40, 50 };
            var result = source.IndicesOf(new[] { 20, 40 }).ToList();
            CollectionAssert.AreEqual(new[] { 1, 3 }, result);
        }

        [Test]
        public void Slice_ReturnsSubsequence()
        {
            var source = new[] { 0, 1, 2, 3, 4 };
            var result = source.Slice(1, 4).ToList();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [Test]
        public void AddElement_AddsItemAndReturnsList()
        {
            IList<int> list = new List<int> { 1, 2 };
            var result = list.AddElement(3);
            Assert.AreSame(list, result);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
        }

        [Test]
        public void AddElementIf_ConditionTrue_AddsItem()
        {
            IList<int> list = new List<int> { 1 };
            list.AddElementIf(true, 2);
            CollectionAssert.AreEqual(new[] { 1, 2 }, list);
        }

        [Test]
        public void AddElementIf_ConditionFalse_DoesNotAdd()
        {
            IList<int> list = new List<int> { 1 };
            list.AddElementIf(false, 2);
            CollectionAssert.AreEqual(new[] { 1 }, list);
        }

        [Test]
        public void AddElementRange_AddsAllItems()
        {
            IList<int> list = new List<int> { 1 };
            list.AddElementRange(new[] { 2, 3 });
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
        }

        [Test]
        public void AddElementRangeIf_ConditionTrue_AddsItems()
        {
            IList<int> list = new List<int> { 1 };
            list.AddElementRangeIf(true, new[] { 2, 3 });
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
        }

        [Test]
        public void AddElementRangeIf_ConditionFalse_DoesNotAdd()
        {
            IList<int> list = new List<int> { 1 };
            list.AddElementRangeIf(false, new[] { 2, 3 });
            CollectionAssert.AreEqual(new[] { 1 }, list);
        }

        [Test]
        public void TakeUntil_StopsAtCondition()
        {
            var source = new[] { 1, 2, 3, 4, 5 };
            var result = source.TakeUntil(x => x == 4).ToList();
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [Test]
        public void Combinations_ReturnsCorrectCount()
        {
            var source = new[] { 1, 2, 3 };
            var result = source.Combinations(2).ToList();
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void ToString_JoinsWithSeparator()
        {
            var source = new[] { 1, 2, 3 };
            var result = source.ToString(", ");
            Assert.AreEqual("1, 2, 3", result);
        }

        [Test]
        public void AddRange_ObservableCollection_AddsAllItems()
        {
            var oc = new ObservableCollection<int> { 1 };
            oc.AddRange(new[] { 2, 3 });
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, oc);
        }

        [Test]
        public void AddRange_ObservableCollection_NullThrows()
        {
            var oc = new ObservableCollection<int>();
            Assert.Throws<ArgumentNullException>(() => oc.AddRange(null!));
        }

        [Test]
        public void ToEnumerable_ConvertsEnumerator()
        {
            var list = new List<int> { 10, 20, 30 };
            var result = list.GetEnumerator().ToEnumerable().ToList();
            CollectionAssert.AreEqual(new[] { 10, 20, 30 }, result);
        }
    }
}
