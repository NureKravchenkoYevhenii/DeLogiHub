using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Helpers;
public static class HashHelper
{
	private const string DEFAULT_SALT = "98+_)+(_+a?}\">?\\\"kf98bvsocn01234-)(U^)QWEJOFkn9uwe0tj)ASDJF)(H0INHO$%uh09hj";
	private const string SYMBOLS = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm[]'/.,{}:\"<>?`1234567890-=~!@#$%^&*()_+\\|";
	private const int KEY_SIZE = 16;
	private const int ITERATION_COUNT = 1000;

	private static readonly Random _random = new((int)(DateTime.Now.Ticks % 181081));
	private static readonly byte[] _symbolsBytes;
	private static readonly byte[] _key;
	static HashHelper()
	{
		_symbolsBytes = Encoding.UTF8.GetBytes(SYMBOLS);
		_key = GenerateKeyFromSalt();
	}

	public static (string salt, string passwordHash) GenerateNewPasswordHash(string newPassword)
	{
		const int saltLength = 16;
		var salt = new string(Enumerable.Repeat(SYMBOLS, saltLength)
			.Select(s => s[_random.Next(s.Length)]).ToArray()
		);

		var passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword + salt + DEFAULT_SALT);

		return (salt, passwordHash);
	}

	public static bool VerifyPassword(string password, string salt, string passwordHash)
	{
		return BCrypt.Net.BCrypt.Verify(password + salt + DEFAULT_SALT, passwordHash);
	}

	public static string? Encrypt(string? plainText)
	{
		if (plainText == null)
			return null;

		if (plainText == string.Empty)
			return string.Empty;

		using (var aes = Aes.Create())
		{
			aes.Key = _key;
			aes.GenerateIV();
			var iv = aes.IV;

			ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv);

			byte[] encryptedData;

			using (var memoryStream = new MemoryStream())
			using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
			{
				var plainBytes = Encoding.UTF8.GetBytes(plainText);
				cryptoStream.Write(plainBytes, 0, plainBytes.Length);
				cryptoStream.FlushFinalBlock();

				encryptedData = memoryStream.ToArray();
			}

			var encryptedBytesWithIV = new byte[iv.Length + encryptedData.Length];
			Buffer.BlockCopy(iv, 0, encryptedBytesWithIV, 0, iv.Length);
			Buffer.BlockCopy(encryptedData, 0, encryptedBytesWithIV, iv.Length, encryptedData.Length);

			return Convert.ToBase64String(encryptedBytesWithIV);
		}
	}

	public static byte[] Encrypt(byte[] data)
	{
		using (var aes = Aes.Create())
		{
			aes.KeySize = 128;
			aes.BlockSize = 128;
			aes.Key = _key;
			aes.GenerateIV();
			byte[] encryptedData;

			using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
			{
				encryptedData = PerformCryptography(data, encryptor);
			}

			var encryptedBytesWithIV = new byte[aes.IV.Length + encryptedData.Length];
			Buffer.BlockCopy(aes.IV, 0, encryptedBytesWithIV, 0, aes.IV.Length);
			Buffer.BlockCopy(encryptedData, 0, encryptedBytesWithIV, aes.IV.Length, encryptedData.Length);

			return encryptedBytesWithIV;
		}
	}

	public static string? Decrypt(string? encryptedText)
	{
		if (encryptedText == null)
			return null;
		if (encryptedText == string.Empty)
			return string.Empty;

		var encryptedBytes = Convert.FromBase64String(encryptedText);
		var iv = new byte[KEY_SIZE];
		var encryptedData = new byte[encryptedBytes.Length - iv.Length];

		Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
		Buffer.BlockCopy(encryptedBytes, iv.Length, encryptedData, 0, encryptedData.Length);

		using (var aes = Aes.Create())
		{
			aes.Key = _key;
			aes.IV = iv;

			ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

			using (var memoryStream = new MemoryStream(encryptedData))
			using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
			using (var streamReader = new StreamReader(cryptoStream))
			{
				return streamReader.ReadToEnd();
			}
		}
	}

	public static byte[] Decrypt(byte[] encryptedBytes)
	{
		var iv = new byte[KEY_SIZE];
		var encryptedData = new byte[encryptedBytes.Length - iv.Length];

		Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
		Buffer.BlockCopy(encryptedBytes, iv.Length, encryptedData, 0, encryptedData.Length);

		using (var aes = Aes.Create())
		{
			aes.KeySize = 128;
			aes.BlockSize = 128;
			aes.Key = _key;
			aes.IV = iv;

			using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
			{
				return PerformCryptography(encryptedData, decryptor);
			}
		}
	}

	private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
	{
		using (var ms = new MemoryStream())
		using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
		{
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.FlushFinalBlock();

			return ms.ToArray();
		}
	}

	private static byte[] GenerateKeyFromSalt()
	{
		using var deriveBytes = new Rfc2898DeriveBytes(
			DEFAULT_SALT,
			_symbolsBytes,
			ITERATION_COUNT,
			HashAlgorithmName.SHA1);

		return deriveBytes.GetBytes(KEY_SIZE);
	}
}
