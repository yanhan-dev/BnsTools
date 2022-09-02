using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

//http://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
namespace Enst
{
    public static class Stuff
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string intVec = "on1go10wasn8874@";
        // This constant is used to determine the keysize of the encryption algorithm.
        private const int kz = 256;
        //Decrypt
        public static string Dest(string Ct, string PPs)
        {
            byte[] iVecB = Encoding.ASCII.GetBytes(intVec);
            byte[] cTB = Convert.FromBase64String(Ct);
            PasswordDeriveBytes pw = new PasswordDeriveBytes(PPs, null);
            byte[] kBs = pw.GetBytes(kz / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(kBs, iVecB);
            MemoryStream memoryStream = new MemoryStream(cTB);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] pTB = new byte[cTB.Length];
            int dBc = cryptoStream.Read(pTB, 0, pTB.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(pTB, 0, dBc);
        }
    }
}