using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace PhasmoDecrypt;



public class Crypter
{

  public string Decrypt(byte[] data)
  {
    if (data == null || data.Length == 0)
    {
      return "Error: Data is null or empty.";
    }

    var iv = new byte[16];
    Array.Copy(data, iv, 16);

    using var dbytes = new Rfc2898DeriveBytes(Globals.Save_Secret, iv, 100, HashAlgorithmName.SHA1);
    var key = dbytes.GetBytes(16);

    using var aes = Aes.Create();
    aes.Mode = CipherMode.CBC;
    aes.Padding = PaddingMode.PKCS7;
    aes.Key = key;
    aes.IV = iv;

    using var decryptor = aes.CreateDecryptor();
    using var ms = new MemoryStream(data, 16, data.Length - 16);
    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
    using var swd = new StreamReader(cs, Encoding.UTF8);

    var decrypted = swd.ReadToEnd();

    try
    {
      var obj = JsonConvert.DeserializeObject(decrypted);

      var output = JsonConvert.SerializeObject(obj, Formatting.Indented);

      return output;
    }
    catch (Exception e)
    {
      return "check the correctness of the file: " + e.Message;
    }
  }

  public byte[] EncryptData(string data)
  {
    byte[] iv = new byte[16];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(iv);
    }

    var key = new Rfc2898DeriveBytes(Globals.Save_Secret, iv, 100, HashAlgorithmName.SHA1).GetBytes(16);

    using var aes = Aes.Create();
    aes.Mode = CipherMode.CBC;
    aes.Padding = PaddingMode.PKCS7;
    aes.Key = key;
    aes.IV = iv;

    var ms = new MemoryStream();
    using (var enc = aes.CreateEncryptor())
    using (var cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
    using (var swe = new StreamWriter(cs, Encoding.UTF8))
    {
      swe.Write(data);
    }

    byte[] edata = ms.ToArray();

    byte[] res = new byte[iv.Length + edata.Length];
    Buffer.BlockCopy(iv, 0, res, 0, iv.Length);
    Buffer.BlockCopy(edata, 0, res, iv.Length, edata.Length);

    return res;

  }


}

