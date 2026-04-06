using System;
using Kunai.FunctionalExt;
using NUnit.Framework;

namespace KunaiTests.Functional;

    [TestFixture]
    public class FunctionalExtensionsTests
    {
        [Test]
        public void Times_ExecutesSpecifiedNumberOfTimes()
        {
            var count = 0;
            Action<int> increment = _ => count++;
            increment.Times(5);
            Assert.AreEqual(5, count);
        }

        [Test]
        public void Times_PassesIterationNumber()
        {
            var last = 0;
            Action<int> capture = i => last = i;
            capture.Times(3);
            Assert.AreEqual(3, last);
        }

        [Test]
        public void Pipe_WithFunc_ReturnsTransformedValue()
        {
            var result = 5.Pipe(x => x * 2);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void Pipe_WithAction_ReturnsOriginalValue()
        {
            var sideEffect = 0;
            var result = 42.Pipe(x => { sideEffect = x; });
            Assert.AreEqual(42, result);
            Assert.AreEqual(42, sideEffect);
        }

        [Test]
        public void Pipe_WithNullFunc_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => 5.Pipe((Func<int, int>)null!));
        }

        [Test]
        public void If_ConditionTrue_AppliesTransformation()
        {
            var result = 10.If(x => x > 5, x => x * 2);
            Assert.AreEqual(20, result);
        }

        [Test]
        public void If_ConditionFalse_ReturnsOriginalValue()
        {
            var result = 3.If(x => x > 5, x => x * 2);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Memoize_CachesResults()
        {
            var callCount = 0;
            Func<int, int> expensive = x =>
            {
                callCount++;
                return x * x;
            };

            var memoized = expensive.Memoize();
            Assert.AreEqual(25, memoized(5));
            Assert.AreEqual(25, memoized(5));
            Assert.AreEqual(1, callCount); // called only once
        }

        [Test]
        public void Memoize_ComputesDifferentArguments()
        {
            Func<int, int> square = x => x * x;
            var memoized = square.Memoize();
            Assert.AreEqual(4, memoized(2));
            Assert.AreEqual(9, memoized(3));
        }
    }
