using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Encrypting
    {
        public static string EncryptMessage(string message, string secretKey)
        {
            while (message.Length % 8 != 0) // dopunjavamo praznim mestaima, koje uklanjamo pri dekripciji, da bismo dosli do duzine deljive sa 8, jer bi se ostatak karaktera odbacio (blokoci su kod 3DS duzine 64bit-a)
            {
                message = message + " ";
            }
            byte[] encryptedMessage;

            byte[] messageByte = Encoding.UTF8.GetBytes(message);
            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };

            tripleDesCryptoProvider.GenerateIV();
            ICryptoTransform tripleDesEncryptTransform = tripleDesCryptoProvider.CreateEncryptor();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesEncryptTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(messageByte, 0, messageByte.Length);
                    encryptedMessage = tripleDesCryptoProvider.IV.Concat(memoryStream.ToArray()).ToArray();    //encrypted image body with IV			
                }
            }
            //return encryptedMessage.ToString();
            return Convert.ToBase64String(encryptedMessage); //https://stackoverflow.com/questions/1134671/how-can-i-safely-convert-a-byte-array-into-a-string-and-back
            /*
            ICryptoTransform tripleDesEncryptTransform = tripleDesCryptoProvider.CreateEncryptor();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesEncryptTransform, CryptoStreamMode.Write))
                {
                    
                    cryptoStream.Write(messageByte, 0, messageByte.Length);
                    cryptoStream.FlushFinalBlock();
                    encryptedMessage = ASCIIEncoding.ASCII.GetString(memoryStream.ToArray());                         //encrypted image body
                }
            }
            return encryptedMessage;
            */
        }

        public static string DecryptMessage(string encryptedMessage, string secretKey)
        {
            string decryptedMessage = null;
            byte[] encryptedMessageByte = Convert.FromBase64String(encryptedMessage);
            byte[] decryptedMessageByte = null;

            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None
            };


            tripleDesCryptoProvider.IV = encryptedMessageByte.Take(tripleDesCryptoProvider.BlockSize / 8).ToArray();                // take the iv off the beginning of the ciphertext message			
            ICryptoTransform tripleDesDecryptTransform = tripleDesCryptoProvider.CreateDecryptor();

            using (MemoryStream memoryStream = new MemoryStream(encryptedMessageByte.Skip(tripleDesCryptoProvider.BlockSize / 8).ToArray()))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesDecryptTransform, CryptoStreamMode.Read))
                {
                    decryptedMessageByte = new byte[encryptedMessageByte.Length - tripleDesCryptoProvider.BlockSize / 8];     //decrypted image body - the same lenght as encrypted part
                    cryptoStream.Read(decryptedMessageByte, 0, decryptedMessageByte.Length);
      
                }
            }
            decryptedMessage = Encoding.UTF8.GetString(decryptedMessageByte).Trim();
            /*
            ICryptoTransform tripleDesDecryptTransform = tripleDesCryptoProvider.CreateDecryptor();
            using (MemoryStream memoryStream = new MemoryStream(encryptedMessageByte))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesDecryptTransform, CryptoStreamMode.Write))
                {
                    decryptedMessageByte = new byte[encryptedMessageByte.Length]; //decrypted image body - the same lenght as encrypted part
                    
                    cryptoStream.Write(encryptedMessageByte, 0, encryptedMessageByte.Length);
                    cryptoStream.FlushFinalBlock();
                    decryptedMessage = ASCIIEncoding.ASCII.GetString(memoryStream.ToArray());
                }
            }
            */


            return decryptedMessage;
        }
    }
}
