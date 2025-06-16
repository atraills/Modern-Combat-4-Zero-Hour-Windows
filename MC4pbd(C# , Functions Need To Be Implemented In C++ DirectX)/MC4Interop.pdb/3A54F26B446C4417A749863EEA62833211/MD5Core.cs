using System;
using System.Text;




using atldef.h;
using atlcore.h;
using atlexcept.h;
using atlmem.h;




public sealed class MD5Core
{
	public static string hexaChars;

	static MD5Core()
	{
		MD5Core.hexaChars = "0123456789ABCDEF";
	}

	private MD5Core()
	{
	}

	private static uint[] Converter(byte[] input, int ibStart)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input", "Unable convert null array to array of uInts");
		}
		uint[] numArray = new uint[16];
		for (int i = 0; i < 16; i++)
		{
			numArray[i] = input[ibStart + i * 4];
			&nRefs uint numPointer = ref numArray[i];
			numPointer = numPointer + (input[ibStart + i * 4 + 1] << 8);
			&nRefs uint numPointer1 = ref numArray[i];
			numPointer1 = numPointer1 + (input[ibStart + i * 4 + 2] << 16);
			&nRefs uint numPointer2 = ref numArray[i];
			numPointer2 = numPointer2 + (input[ibStart + i * 4 + 3] << 24);
		}
		return numArray;
	}

	public static byte[] GetHash(string input, Encoding encoding)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input", "Unable to calculate hash over null input data");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding", "Unable to calculate hash over a string without a default encoding. Consider using the GetHash(string) overload to use UTF8 Encoding");
		}
		return MD5Core.GetHash(encoding.GetBytes(input));
	}

	public static byte[] GetHash(string input)
	{
		return MD5Core.GetHash(input, new UTF8Encoding());
	}

	public static byte[] GetHash(byte[] input)
	{
		int i;
		if (input == null)
		{
			throw new ArgumentNullException("input", "Unable to calculate hash over null input data");
		}
		ABCDStruct aBCDStruct = new ABCDStruct()
		{
			A = 1732584193,
			B = -271733879,
			C = -1732584194,
			D = 271733878
		};
		for (i = 0; i <= (int)input.Length - 64; i += 64)
		{
			MD5Core.GetHashBlock(input, ref aBCDStruct, i);
		}
		return MD5Core.GetHashFinalBlock(input, i, (int)input.Length - i, aBCDStruct, (long)((int)input.Length) * (long)8);
	}

	internal static void GetHashBlock(byte[] input, ref ABCDStruct ABCDValue, int ibStart)
	{
		uint[] numArray = MD5Core.Converter(input, ibStart);
		uint a = ABCDValue.A;
		uint b = ABCDValue.B;
		uint c = ABCDValue.C;
		uint d = ABCDValue.D;
		a = MD5Core.r1(a, b, c, d, numArray[0], 7, -680876936);
		d = MD5Core.r1(d, a, b, c, numArray[1], 12, -389564586);
		c = MD5Core.r1(c, d, a, b, numArray[2], 17, 606105819);
		b = MD5Core.r1(b, c, d, a, numArray[3], 22, -1044525330);
		a = MD5Core.r1(a, b, c, d, numArray[4], 7, -176418897);
		d = MD5Core.r1(d, a, b, c, numArray[5], 12, 1200080426);
		c = MD5Core.r1(c, d, a, b, numArray[6], 17, -1473231341);
		b = MD5Core.r1(b, c, d, a, numArray[7], 22, -45705983);
		a = MD5Core.r1(a, b, c, d, numArray[8], 7, 1770035416);
		d = MD5Core.r1(d, a, b, c, numArray[9], 12, -1958414417);
		c = MD5Core.r1(c, d, a, b, numArray[10], 17, -42063);
		b = MD5Core.r1(b, c, d, a, numArray[11], 22, -1990404162);
		a = MD5Core.r1(a, b, c, d, numArray[12], 7, 1804603682);
		d = MD5Core.r1(d, a, b, c, numArray[13], 12, -40341101);
		c = MD5Core.r1(c, d, a, b, numArray[14], 17, -1502002290);
		b = MD5Core.r1(b, c, d, a, numArray[15], 22, 1236535329);
		a = MD5Core.r2(a, b, c, d, numArray[1], 5, -165796510);
		d = MD5Core.r2(d, a, b, c, numArray[6], 9, -1069501632);
		c = MD5Core.r2(c, d, a, b, numArray[11], 14, 643717713);
		b = MD5Core.r2(b, c, d, a, numArray[0], 20, -373897302);
		a = MD5Core.r2(a, b, c, d, numArray[5], 5, -701558691);
		d = MD5Core.r2(d, a, b, c, numArray[10], 9, 38016083);
		c = MD5Core.r2(c, d, a, b, numArray[15], 14, -660478335);
		b = MD5Core.r2(b, c, d, a, numArray[4], 20, -405537848);
		a = MD5Core.r2(a, b, c, d, numArray[9], 5, 568446438);
		d = MD5Core.r2(d, a, b, c, numArray[14], 9, -1019803690);
		c = MD5Core.r2(c, d, a, b, numArray[3], 14, -187363961);
		b = MD5Core.r2(b, c, d, a, numArray[8], 20, 1163531501);
		a = MD5Core.r2(a, b, c, d, numArray[13], 5, -1444681467);
		d = MD5Core.r2(d, a, b, c, numArray[2], 9, -51403784);
		c = MD5Core.r2(c, d, a, b, numArray[7], 14, 1735328473);
		b = MD5Core.r2(b, c, d, a, numArray[12], 20, -1926607734);
		a = MD5Core.r3(a, b, c, d, numArray[5], 4, -378558);
		d = MD5Core.r3(d, a, b, c, numArray[8], 11, -2022574463);
		c = MD5Core.r3(c, d, a, b, numArray[11], 16, 1839030562);
		b = MD5Core.r3(b, c, d, a, numArray[14], 23, -35309556);
		a = MD5Core.r3(a, b, c, d, numArray[1], 4, -1530992060);
		d = MD5Core.r3(d, a, b, c, numArray[4], 11, 1272893353);
		c = MD5Core.r3(c, d, a, b, numArray[7], 16, -155497632);
		b = MD5Core.r3(b, c, d, a, numArray[10], 23, -1094730640);
		a = MD5Core.r3(a, b, c, d, numArray[13], 4, 681279174);
		d = MD5Core.r3(d, a, b, c, numArray[0], 11, -358537222);
		c = MD5Core.r3(c, d, a, b, numArray[3], 16, -722521979);
		b = MD5Core.r3(b, c, d, a, numArray[6], 23, 76029189);
		a = MD5Core.r3(a, b, c, d, numArray[9], 4, -640364487);
		d = MD5Core.r3(d, a, b, c, numArray[12], 11, -421815835);
		c = MD5Core.r3(c, d, a, b, numArray[15], 16, 530742520);
		b = MD5Core.r3(b, c, d, a, numArray[2], 23, -995338651);
		a = MD5Core.r4(a, b, c, d, numArray[0], 6, -198630844);
		d = MD5Core.r4(d, a, b, c, numArray[7], 10, 1126891415);
		c = MD5Core.r4(c, d, a, b, numArray[14], 15, -1416354905);
		b = MD5Core.r4(b, c, d, a, numArray[5], 21, -57434055);
		a = MD5Core.r4(a, b, c, d, numArray[12], 6, 1700485571);
		d = MD5Core.r4(d, a, b, c, numArray[3], 10, -1894986606);
		c = MD5Core.r4(c, d, a, b, numArray[10], 15, -1051523);
		b = MD5Core.r4(b, c, d, a, numArray[1], 21, -2054922799);
		a = MD5Core.r4(a, b, c, d, numArray[8], 6, 1873313359);
		d = MD5Core.r4(d, a, b, c, numArray[15], 10, -30611744);
		c = MD5Core.r4(c, d, a, b, numArray[6], 15, -1560198380);
		b = MD5Core.r4(b, c, d, a, numArray[13], 21, 1309151649);
		a = MD5Core.r4(a, b, c, d, numArray[4], 6, -145523070);
		d = MD5Core.r4(d, a, b, c, numArray[11], 10, -1120210379);
		c = MD5Core.r4(c, d, a, b, numArray[2], 15, 718787259);
		b = MD5Core.r4(b, c, d, a, numArray[9], 21, -343485551);
		ABCDValue.A = a + ABCDValue.A;
		ABCDValue.B = b + ABCDValue.B;
		ABCDValue.C = c + ABCDValue.C;
		ABCDValue.D = d + ABCDValue.D;
	}

	internal static byte[] GetHashFinalBlock(byte[] input, int ibStart, int cbSize, ABCDStruct ABCD, long len)
	{
		byte[] numArray = new byte[64];
		byte[] bytes = BitConverter.GetBytes(len);
		Array.Copy(input, ibStart, numArray, 0, cbSize);
		numArray[cbSize] = 128;
		if (cbSize >= 56)
		{
			MD5Core.GetHashBlock(numArray, ref ABCD, 0);
			numArray = new byte[64];
			Array.Copy(bytes, 0, numArray, 56, 8);
			MD5Core.GetHashBlock(numArray, ref ABCD, 0);
		}
		else
		{
			Array.Copy(bytes, 0, numArray, 56, 8);
			MD5Core.GetHashBlock(numArray, ref ABCD, 0);
		}
		byte[] numArray1 = new byte[16];
		Array.Copy(BitConverter.GetBytes(ABCD.A), 0, numArray1, 0, 4);
		Array.Copy(BitConverter.GetBytes(ABCD.B), 0, numArray1, 4, 4);
		Array.Copy(BitConverter.GetBytes(ABCD.C), 0, numArray1, 8, 4);
		Array.Copy(BitConverter.GetBytes(ABCD.D), 0, numArray1, 12, 4);
		return numArray1;
	}

	public static string GetHashString(byte[] input)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input", "Unable to calculate hash over null input data");
		}
		string str = BitConverter.ToString(MD5Core.GetHash(input));
		return str.Replace("-", "");
	}

	public static string GetHashString(string input, Encoding encoding)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input", "Unable to calculate hash over null input data");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding", "Unable to calculate hash over a string without a default encoding. Consider using the GetHashString(string) overload to use UTF8 Encoding");
		}
		return MD5Core.GetHashString(encoding.GetBytes(input));
	}

	public static string GetHashString(string input)
	{
		return MD5Core.GetHashString(input, new UTF8Encoding());
	}

	public static string GLMD5(byte[] input)
	{
		byte[] hash = MD5Core.GetHash(input);
		string str = "";
		for (int i = 0; i < 16; i++)
		{
			str = string.Concat(str, MD5Core.hexaChars[15 & hash[i] >> 4]);
			str = string.Concat(str, MD5Core.hexaChars[15 & hash[i]]);
		}
		return str;
	}

	private static uint LSR(uint i, int s)
	{
		return i << (s & 31) | i >> (32 - s & 31);
	}

	private static uint r1(uint a, uint b, uint c, uint d, uint x, int s, uint t)
	{
		return b + MD5Core.LSR(a + (b & c | (b ^ -1) & d) + x + t, s);
	}

	private static uint r2(uint a, uint b, uint c, uint d, uint x, int s, uint t)
	{
		return b + MD5Core.LSR(a + (b & d | c & (d ^ -1)) + x + t, s);
	}

	private static uint r3(uint a, uint b, uint c, uint d, uint x, int s, uint t)
	{
		return b + MD5Core.LSR(a + (b ^ c ^ d) + x + t, s);
	}

	private static uint r4(uint a, uint b, uint c, uint d, uint x, int s, uint t)
	{
		return b + MD5Core.LSR(a + (c ^ (b | d ^ -1)) + x + t, s);
	}
}