// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.CryptoHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#nullable disable
namespace Gbs.Helpers
{
  public static class CryptoHelper
  {
    public static string SignDoc(string str, string thumbprint)
    {
      try
      {
        LogHelper.Debug("Начинаем подписывать присоедененную ЭЦП строку - " + str);
        X509Certificate2 certificate = (X509Certificate2) null;
        X509Store x509Store = new X509Store();
        x509Store.Open(OpenFlags.ReadOnly);
        X509Certificate2Enumerator enumerator = x509Store.Certificates.GetEnumerator();
        while (enumerator.MoveNext())
        {
          X509Certificate2 current = enumerator.Current;
          if (current.Thumbprint == thumbprint)
          {
            certificate = current;
            break;
          }
        }
        if (certificate == null)
        {
          LogHelper.Debug("Не найден сертификат со следом: " + thumbprint + ", список сертификатов:\n" + x509Store.Certificates.ToJsonString(true));
          return (string) null;
        }
        x509Store.Close();
        LogHelper.Debug("Нашли в хранилиже сертификат с отпечатком " + thumbprint);
        string path = Path.Combine(FileSystemHelper.TempFolderPath(), Guid.NewGuid().ToString());
        File.WriteAllText(path, str);
        SignedCms signedCms = new SignedCms(new ContentInfo(File.ReadAllBytes(path)), false);
        signedCms.ComputeSignature(new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, certificate));
        LogHelper.Debug("Смогли подписать");
        return Convert.ToBase64String(signedCms.Encode());
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось подписать присоединненой подписьюю.");
        return (string) null;
      }
    }

    public static void SignDisconnectedDoc(string thumbprint, string fileSignPath, string Base64 = "")
    {
      string path = FileSystemHelper.IsSoftwareInstalled("КриптоПро CSP").ToString();
      if (string.IsNullOrWhiteSpace(path))
        path = FileSystemHelper.IsSoftwareInstalled("CryptoPro CSP").ToString();
      if (string.IsNullOrWhiteSpace(path))
      {
        LogHelper.Debug("Похоже, Крипто Про не установлен или не найден");
      }
      else
      {
        try
        {
          string str = string.Format("cmd /k cd \"{0}\" & csptest.exe -sfsign -sign -add -detached -in  \"{1}\" -out \"{1}\" -my {2} {3} && Exit", (object) path, (object) fileSignPath, (object) thumbprint, (object) Base64);
          string Exit = string.Empty;
          Process proc = new Process()
          {
            StartInfo = {
              UseShellExecute = true,
              FileName = "cmd.exe",
              WorkingDirectory = Path.GetDirectoryName(path) ?? string.Empty,
              Arguments = str,
              CreateNoWindow = true
            }
          };
          proc.Exited += (EventHandler) ((sender, e) =>
          {
            Exit = "uygfliwerypweiu11";
            proc.EnableRaisingEvents = true;
          });
          proc.Start();
          proc.WaitForExit();
        }
        catch (Exception ex)
        {
          string message = string.Format("Ошибка подписания файла");
          LogHelper.Error(ex, message);
        }
      }
    }

    public static string GetSHA256Hash(string filePath, bool withSalt)
    {
      string s = "gbsmarket26092023";
      using (SHA256 shA256 = SHA256.Create())
      {
        List<byte> list = ((IEnumerable<byte>) File.ReadAllBytes(filePath)).ToList<byte>();
        if (withSalt)
          list.AddRange((IEnumerable<byte>) Encoding.UTF8.GetBytes(s));
        return Convert.ToBase64String(shA256.ComputeHash(list.ToArray()));
      }
    }

    public static string GetSHA1Hash(string input)
    {
      using (SHA1Managed shA1Managed = new SHA1Managed())
      {
        byte[] hash = shA1Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder stringBuilder = new StringBuilder(hash.Length * 2);
        foreach (byte num in hash)
          stringBuilder.Append(num.ToString("X2").ToLower());
        return stringBuilder.ToString().ToLower();
      }
    }

    public static string GetMd5Hash(string strToHash)
    {
      return ((IEnumerable<byte>) new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(strToHash))).Aggregate<byte, string>("", (Func<string, byte, string>) ((current, b) => current + b.ToString("x2")));
    }

    public static string GetMd5HashForFile(string filename)
    {
      using (MD5 md5 = MD5.Create())
      {
        using (FileStream inputStream = File.OpenRead(filename))
          return BitConverter.ToString(md5.ComputeHash((Stream) inputStream)).Replace("-", "").ToLowerInvariant();
      }
    }

    public static class StringCrypter
    {
      private static readonly Dictionary<string, string> CryptoCache = new Dictionary<string, string>();
      private const string PassPhrase = "PkHZkK0vm0yuywVlH67z4A";
      private const int KeySize = 256;
      private const int DerivationIterations = 1000;

      public static string Encrypt(string plainText)
      {
        try
        {
          if (plainText.IsNullOrEmpty())
            return string.Empty;
          byte[] numArray1 = CryptoHelper.StringCrypter.Generate256BitsOfRandomEntropy();
          byte[] numArray2 = CryptoHelper.StringCrypter.Generate256BitsOfRandomEntropy();
          byte[] bytes1 = Encoding.UTF8.GetBytes(plainText);
          using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("PkHZkK0vm0yuywVlH67z4A", numArray1, 1000))
          {
            byte[] bytes2 = rfc2898DeriveBytes.GetBytes(32);
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
              rijndaelManaged.BlockSize = 256;
              rijndaelManaged.Mode = CipherMode.CBC;
              rijndaelManaged.Padding = PaddingMode.PKCS7;
              using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, numArray2))
              {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                  using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
                  {
                    cryptoStream.Write(bytes1, 0, bytes1.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] array = ((IEnumerable<byte>) ((IEnumerable<byte>) numArray1).Concat<byte>((IEnumerable<byte>) numArray2).ToArray<byte>()).Concat<byte>((IEnumerable<byte>) memoryStream.ToArray()).ToArray<byte>();
                    memoryStream.Close();
                    cryptoStream.Close();
                    return Convert.ToBase64String(array);
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка шифрования данных", false);
          return (string) null;
        }
      }

      public static string Decrypt(string cipherText)
      {
        try
        {
          if (cipherText.IsNullOrEmpty())
            return string.Empty;
          if (CryptoHelper.StringCrypter.CryptoCache.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Key == cipherText)))
            return CryptoHelper.StringCrypter.CryptoCache[cipherText];
          byte[] source = Convert.FromBase64String(cipherText);
          byte[] array1 = ((IEnumerable<byte>) source).Take<byte>(32).ToArray<byte>();
          byte[] array2 = ((IEnumerable<byte>) source).Skip<byte>(32).Take<byte>(32).ToArray<byte>();
          byte[] array3 = ((IEnumerable<byte>) source).Skip<byte>(64).Take<byte>(source.Length - 64).ToArray<byte>();
          using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("PkHZkK0vm0yuywVlH67z4A", array1, 1000))
          {
            byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
              rijndaelManaged.BlockSize = 256;
              rijndaelManaged.Mode = CipherMode.CBC;
              rijndaelManaged.Padding = PaddingMode.PKCS7;
              using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, array2))
              {
                using (MemoryStream memoryStream = new MemoryStream(array3))
                {
                  using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
                  {
                    byte[] numArray = new byte[array3.Length];
                    int count = cryptoStream.Read(numArray, 0, numArray.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                    string str = Encoding.UTF8.GetString(numArray, 0, count);
                    if (!CryptoHelper.StringCrypter.CryptoCache.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Key == cipherText)))
                      CryptoHelper.StringCrypter.CryptoCache.Add(cipherText, str);
                    return str;
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "ошибка расшифровки данных", false);
          return (string) null;
        }
      }

      private static byte[] Generate256BitsOfRandomEntropy()
      {
        byte[] data = new byte[32];
        using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
        {
          cryptoServiceProvider.GetBytes(data);
          return data;
        }
      }
    }
  }
}
