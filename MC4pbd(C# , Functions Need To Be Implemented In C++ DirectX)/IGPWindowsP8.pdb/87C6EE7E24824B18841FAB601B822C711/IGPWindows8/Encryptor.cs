using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IGPWindows8
{
	public class Encryptor
	{
		private static Encryptor.eEncryptionType m_CurrentEncryption;

		public static Encryptor.eEncryptionType Encryption
		{
			get
			{
				return Encryptor.eEncryptionType.EET_B64BASE;
			}
			set
			{
				Encryptor.m_CurrentEncryption = value;
			}
		}

		static Encryptor()
		{
			Encryptor.m_CurrentEncryption = Encryptor.eEncryptionType.EET_TOTAL;
		}

		public Encryptor()
		{
		}

		public static string Decrypt(string input)
		{
			if (Encryptor.m_CurrentEncryption == Encryptor.eEncryptionType.EET_B64BASE)
			{
				return Encryptor.EncryptB64(input, false);
			}
			if (Encryptor.m_CurrentEncryption != Encryptor.eEncryptionType.EET_AES)
			{
				return "Unsupported Encryption Selected";
			}
			return Encryptor.EncryptAES(input, false);
		}

		public static string Encrpyt_MD5_SHA1(string input)
		{
			return input;
		}

		public static string Encrypt(string input)
		{
			if (Encryptor.m_CurrentEncryption == Encryptor.eEncryptionType.EET_B64BASE)
			{
				return Encryptor.EncryptB64(input, true);
			}
			if (Encryptor.m_CurrentEncryption != Encryptor.eEncryptionType.EET_AES)
			{
				return "Unsupported Encryption Selected";
			}
			return Encryptor.EncryptAES(input, true);
		}

		public static string Encrypt_MD5(string input)
		{
			return MD5Core.GLMD5(Encoding.UTF8.GetBytes(input));
		}

		public static string Encrypt_SHA1_MD5(string input)
		{
			return input;
		}

		public static string EncryptAES(string input, bool bEncrypt)
		{
			byte[] array;
			string end = input;
			try
			{
				AesManaged aesManaged = new AesManaged()
				{
					Key = Encryptor.GetBytesB64(Encryptor.GetKey()),
					IV = Encryptor.GetBytesB64(Encryptor.GetIV())
				};
				if (!bEncrypt)
				{
					byte[] bytesB64 = Encryptor.GetBytesB64(input);
					ICryptoTransform cryptoTransform = aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV);
					using (MemoryStream memoryStream = new MemoryStream(bytesB64))
					{
						try
						{
							using (StreamReader streamReader = new StreamReader(new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read)))
							{
								end = streamReader.ReadToEnd();
							}
						}
						catch (Exception exception)
						{
							end = exception.ToString();
						}
					}
				}
				else
				{
					ICryptoTransform cryptoTransform1 = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);
					using (MemoryStream memoryStream1 = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream1, cryptoTransform1, CryptoStreamMode.Write))
						{
							using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
							{
								streamWriter.Write(input);
							}
							array = memoryStream1.ToArray();
						}
					}
					end = Encryptor.EncryptB64(array, true);
				}
			}
			catch (Exception exception1)
			{
				end = string.Concat("Encryptor error : ", exception1.ToString());
			}
			return end;
		}

		private static string EncryptB64(string input, bool bEncrypt)
		{
			string str = "";
			if (!bEncrypt)
			{
				try
				{
					Decoder decoder = (new UTF8Encoding()).GetDecoder();
					byte[] numArray = Convert.FromBase64String(input);
					int charCount = decoder.GetCharCount(numArray, 0, (int)numArray.Length);
					char[] chrArray = new char[charCount];
					decoder.GetChars(numArray, 0, (int)numArray.Length, chrArray, 0);
					str = new string(chrArray);
				}
				catch (Exception exception)
				{
					str = string.Concat("Exception in b64Base Decoding ", exception.ToString());
				}
			}
			else
			{
				try
				{
					byte[] bytes = new byte[input.Length];
					bytes = Encoding.UTF8.GetBytes(input);
					str = Convert.ToBase64String(bytes);
				}
				catch (Exception exception1)
				{
					str = string.Concat("Exception in b64Base Encoding ", exception1.ToString());
				}
			}
			int length = str.Length;
			int num = input.Length;
			return str;
		}

		private static string EncryptB64(byte[] input, bool bEncrypt)
		{
			string str = "";
			if (!bEncrypt)
			{
				try
				{
					Decoder decoder = (new UTF8Encoding()).GetDecoder();
					byte[] numArray = input;
					int charCount = decoder.GetCharCount(numArray, 0, (int)numArray.Length);
					char[] chrArray = new char[charCount];
					decoder.GetChars(numArray, 0, (int)numArray.Length, chrArray, 0);
					str = new string(chrArray);
				}
				catch (Exception exception)
				{
					str = string.Concat("Exception in b64Base Decoding ", exception.ToString());
				}
			}
			else
			{
				try
				{
					str = Convert.ToBase64String(input);
				}
				catch (Exception exception1)
				{
					str = string.Concat("Exception in b64Base Encoding ", exception1.ToString());
				}
			}
			int length = str.Length;
			return str;
		}

		public static byte[] GetBytesB64(string input)
		{
			return Convert.FromBase64String(input);
		}

		public static byte[] GetBytesB64Decode(string input)
		{
			byte[] numArray = Convert.FromBase64String(input);
			Decoder decoder = (new UTF8Encoding()).GetDecoder();
			int charCount = decoder.GetCharCount(numArray, 0, (int)numArray.Length);
			char[] chrArray = new char[charCount];
			decoder.GetChars(numArray, 0, (int)numArray.Length, chrArray, 0);
			numArray = Convert.FromBase64CharArray(chrArray, 0, charCount);
			return numArray;
		}

		public static string GetIV()
		{
			byte[] numArray = new byte[25];
			byte[] numArray1 = new byte[] { 0, 35, 242, 229, 39, 241, 245, 227, 49, 2, 6, 6, 216, 40, 236, 237, 1, 247, 231, 69, 239, 208, 9, 0, 0 };
			numArray[0] = 79;
			for (int i = 1; i < 24; i++)
			{
				numArray[i] = (byte)(numArray[i - 1] + numArray1[i]);
			}
			return Encoding.UTF8.GetString(numArray, 0, 24);
		}

		public static string GetKey()
		{
			byte[] numArray = new byte[25];
			byte[] numArray1 = new byte[] { 0, 38, 212, 239, 22, 0, 243, 43, 204, 0, 4, 55, 206, 9, 14, 36, 189, 5, 1, 66, 213, 40, 199, 0, 0 };
			numArray[0] = 71;
			for (int i = 1; i < 24; i++)
			{
				numArray[i] = (byte)(numArray[i - 1] + numArray1[i]);
			}
			return Encoding.UTF8.GetString(numArray, 0, 24);
		}

		public static string PadString(string input)
		{
			int length = input.Length % 4;
			string str = input;
			if (length != 0)
			{
				str = str.PadRight(input.Length + 4 - length, '=');
			}
			return str;
		}

		public enum eEncryptionType
		{
			EET_B64BASE,
			EET_AES,
			EET_TOTAL
		}
	}
}