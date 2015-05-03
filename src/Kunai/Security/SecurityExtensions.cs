using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kunai.Security
{
	public static class SecurityExtensions
	{
		/// <summary>
		/// Converts a string into a "SecureString"
		/// </summary>
		/// <param name="str">Input String</param>
		/// <returns></returns>
		public static System.Security.SecureString ToSecureString(this String str)
		{
			System.Security.SecureString secureString = new System.Security.SecureString();
			foreach (Char c in str)
				secureString.AppendChar(c);

			return secureString;
		}
		
		/// Supported hash algorithms
		/// </summary>
		public enum HashType
		{
			HMAC, HMACMD5, HMACSHA1, HMACSHA256, HMACSHA384, HMACSHA512,
			MACTripleDES, MD5, RIPEMD160, SHA1, SHA256, SHA384, SHA512
		}

		private static byte[] GetHash(string input, HashType hash)
		{
			byte[] inputBytes = Encoding.ASCII.GetBytes(input);

			switch (hash)
			{
				case HashType.HMAC:
					return HMAC.Create().ComputeHash(inputBytes);

				case HashType.HMACMD5:
					return HMACMD5.Create().ComputeHash(inputBytes);

				case HashType.HMACSHA1:
					return HMACSHA1.Create().ComputeHash(inputBytes);

				case HashType.HMACSHA256:
					return HMACSHA256.Create().ComputeHash(inputBytes);

				case HashType.HMACSHA384:
					return HMACSHA384.Create().ComputeHash(inputBytes);

				case HashType.HMACSHA512:
					return HMACSHA512.Create().ComputeHash(inputBytes);

				case HashType.MACTripleDES:
					return MACTripleDES.Create().ComputeHash(inputBytes);

				case HashType.MD5:
					return MD5.Create().ComputeHash(inputBytes);

				case HashType.RIPEMD160:
					return RIPEMD160.Create().ComputeHash(inputBytes);

				case HashType.SHA1:
					return SHA1.Create().ComputeHash(inputBytes);

				case HashType.SHA256:
					return SHA256.Create().ComputeHash(inputBytes);

				case HashType.SHA384:
					return SHA384.Create().ComputeHash(inputBytes);

				case HashType.SHA512:
					return SHA512.Create().ComputeHash(inputBytes);

				default:
					return inputBytes;
			}
		}

		/// <summary>
		/// Computes the hash of the string using a specified hash algorithm
		/// </summary>
		/// <param name="input">The string to hash</param>
		/// <param name="hashType">The hash algorithm to use</param>
		/// <returns>The resulting hash or an empty string on error</returns>
		public static string ComputeHash(this string input, HashType hashType)
		{
			try
			{
				byte[] hash = GetHash(input, hashType);
				StringBuilder ret = new StringBuilder();

				for (int i = 0; i < hash.Length; i++)
					ret.Append(hash[i].ToString("x2"));

				return ret.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
		/// </summary>
		/// <param name="stringToEncrypt">String that must be encrypted.</param>
		/// <param name="key">Encryptionkey.</param>
		/// <returns>A string representing a byte array separated by a minus sign.</returns>
		/// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
		public static string Encrypt(this string stringToEncrypt, string key)
		{
			if (string.IsNullOrEmpty(stringToEncrypt))
			{
				throw new ArgumentException("An empty string value cannot be encrypted.");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
			}

			CspParameters cspp = new CspParameters();
			cspp.KeyContainerName = key;

			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
			rsa.PersistKeyInCsp = true;

			byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);

			return BitConverter.ToString(bytes);
		}

		/// <summary>
		/// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
		/// </summary>
		/// <param name="stringToDecrypt">String that must be decrypted.</param>
		/// <param name="key">Decryptionkey.</param>
		/// <returns>The decrypted string or null if decryption failed.</returns>
		/// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
		public static string Decrypt(this string stringToDecrypt, string key)
		{
			string result = null;

			if (string.IsNullOrEmpty(stringToDecrypt))
			{
				throw new ArgumentException("An empty string value cannot be encrypted.");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
			}

			try
			{
				CspParameters cspp = new CspParameters();
				cspp.KeyContainerName = key;

				RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
				rsa.PersistKeyInCsp = true;

				string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
				byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


				byte[] bytes = rsa.Decrypt(decryptByteArray, true);

				result = System.Text.UTF8Encoding.UTF8.GetString(bytes);

			}
			finally
			{
				// no need for further processing
			}

			return result;
		}


	}

}

