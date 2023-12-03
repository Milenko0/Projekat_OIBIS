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
            string encryptedMessage = string.Empty;

            byte[] messageByte =   ASCIIEncoding.ASCII.GetBytes(message);
            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            };
            
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
        }

        public static string DecryptMessage(string encryptedMessage, string secretKey)
        {
            string decryptedMessage = null;
            byte[] encryptedMessageByte = ASCIIEncoding.ASCII.GetBytes(encryptedMessage);
            byte[] decryptedMessageByte = null;

            TripleDESCryptoServiceProvider tripleDesCryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros
            };

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

            

            return decryptedMessage;
        }
    }
}
