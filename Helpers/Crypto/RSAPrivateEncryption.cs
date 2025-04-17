// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.RSAPrivateEncryption
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Numerics;
using System.Security.Cryptography;

#nullable disable
namespace Gbs.Helpers
{
  public static class RSAPrivateEncryption
  {
    public static byte[] PrivateEncryption(this RSACryptoServiceProvider rsa, byte[] data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (rsa.PublicOnly)
        throw new InvalidOperationException("Private key is not loaded");
      int num = rsa.KeySize / 8 - 6;
      if (data.Length > num)
        throw new ArgumentOutOfRangeException(nameof (data), string.Format("Maximum data length for the current key size ({0} bits) is {1} bytes (current length: {2} bytes)", (object) rsa.KeySize, (object) num, (object) data.Length));
      BigInteger big1 = RSAPrivateEncryption.GetBig(RSAPrivateEncryption.AddPadding(data));
      RSAParameters rsaParameters = rsa.ExportParameters(true);
      BigInteger big2 = RSAPrivateEncryption.GetBig(rsaParameters.D);
      BigInteger big3 = RSAPrivateEncryption.GetBig(rsaParameters.Modulus);
      BigInteger exponent = big2;
      BigInteger modulus = big3;
      return BigInteger.ModPow(big1, exponent, modulus).ToByteArray();
    }

    public static byte[] PublicDecryption(this RSACryptoServiceProvider rsa, byte[] cipherData)
    {
      BigInteger bigInteger = cipherData != null ? new BigInteger(cipherData) : throw new ArgumentNullException(nameof (cipherData));
      RSAParameters rsaParameters = rsa.ExportParameters(false);
      BigInteger big1 = RSAPrivateEncryption.GetBig(rsaParameters.Exponent);
      BigInteger big2 = RSAPrivateEncryption.GetBig(rsaParameters.Modulus);
      BigInteger exponent = big1;
      BigInteger modulus = big2;
      byte[] byteArray = BigInteger.ModPow(bigInteger, exponent, modulus).ToByteArray();
      byte[] numArray1 = new byte[byteArray.Length - 1];
      Array.Copy((Array) byteArray, (Array) numArray1, numArray1.Length);
      byte[] numArray2 = RSAPrivateEncryption.RemovePadding(numArray1);
      Array.Reverse((Array) numArray2);
      return numArray2;
    }

    private static BigInteger GetBig(byte[] data)
    {
      byte[] sourceArray = (byte[]) data.Clone();
      Array.Reverse((Array) sourceArray);
      byte[] destinationArray = new byte[sourceArray.Length + 1];
      Array.Copy((Array) sourceArray, (Array) destinationArray, sourceArray.Length);
      return new BigInteger(destinationArray);
    }

    private static byte[] AddPadding(byte[] data)
    {
      Random random = new Random();
      byte[] sourceArray = new byte[4];
      byte[] buffer = sourceArray;
      random.NextBytes(buffer);
      sourceArray[0] = (byte) ((uint) sourceArray[0] | 128U);
      byte[] destinationArray = new byte[data.Length + 4];
      Array.Copy((Array) sourceArray, (Array) destinationArray, 4);
      Array.Copy((Array) data, 0, (Array) destinationArray, 4, data.Length);
      return destinationArray;
    }

    private static byte[] RemovePadding(byte[] data)
    {
      byte[] destinationArray = new byte[data.Length - 4];
      Array.Copy((Array) data, (Array) destinationArray, destinationArray.Length);
      return destinationArray;
    }
  }
}
