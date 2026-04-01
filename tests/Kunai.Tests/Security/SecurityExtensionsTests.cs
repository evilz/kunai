using Kunai.Security;
using NUnit.Framework;

namespace KunaiTests.Security
{
    [TestFixture]
    public class SecurityExtensionsTests
    {
        [Test]
        public void ComputeHash_MD5_ReturnsNonEmptyString()
        {
            var result = "hello".ComputeHash(SecurityExtensions.HashType.MD5);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void ComputeHash_SHA256_ReturnsCorrectLength()
        {
            var result = "hello".ComputeHash(SecurityExtensions.HashType.SHA256);
            Assert.AreEqual(64, result.Length);
        }

        [Test]
        public void ComputeHash_SHA1_ReturnsCorrectLength()
        {
            var result = "hello".ComputeHash(SecurityExtensions.HashType.SHA1);
            Assert.AreEqual(40, result.Length);
        }

        [Test]
        public void ComputeHash_SHA512_ReturnsCorrectLength()
        {
            var result = "hello".ComputeHash(SecurityExtensions.HashType.SHA512);
            Assert.AreEqual(128, result.Length);
        }

        [Test]
        public void ComputeHash_HMACMD5_ReturnsNonEmptyString()
        {
            var result = "hello".ComputeHash(SecurityExtensions.HashType.HMACMD5);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void ComputeHash_HMACSHA256_ReturnsCorrectLength()
        {
            var result = "hello".ComputeHash(SecurityExtensions.HashType.HMACSHA256);
            Assert.AreEqual(64, result.Length);
        }

        [Test]
        public void ComputeHash_SameInput_SameOutput()
        {
            var result1 = "test".ComputeHash(SecurityExtensions.HashType.SHA256);
            var result2 = "test".ComputeHash(SecurityExtensions.HashType.SHA256);
            Assert.AreEqual(result1, result2);
        }

        [Test]
        public void ComputeHash_DifferentInputs_DifferentOutputs()
        {
            var result1 = "hello".ComputeHash(SecurityExtensions.HashType.SHA256);
            var result2 = "world".ComputeHash(SecurityExtensions.HashType.SHA256);
            Assert.AreNotEqual(result1, result2);
        }

        [Test]
        public void ToSecureString_ReturnsSecureString()
        {
            var secure = "my-password".ToSecureString();
            Assert.IsNotNull(secure);
            Assert.AreEqual(11, secure.Length);
        }
    }
}
