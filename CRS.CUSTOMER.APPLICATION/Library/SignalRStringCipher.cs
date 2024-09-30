using CRS.CUSTOMER.APPLICATION.Models;
using System;
using System.IO;
using System.Security.Cryptography;

namespace CRS.CUSTOMER.APPLICATION.Library
{
    public class SignalRStringCipher
    {
        private readonly byte[] _keyBytes;
        private readonly byte[] _ivBytes;
        private readonly byte[] _secretKeyBytes;
        private static SignalRConfigruationModel _signalRConfigruation = ApplicationUtilities.GetAppDataJsonConfigValue<SignalRConfigruationModel>("SignalRConfigruation");
        public SignalRStringCipher()
        {
            _keyBytes = Convert.FromBase64String(_signalRConfigruation.securityKey);
            _ivBytes = Convert.FromBase64String(_signalRConfigruation.securityIV);
        }

        private Aes CreateAesAlgorithm()
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _keyBytes;
            aes.IV = _ivBytes;
            return aes;
        }

        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            var aesAlg = CreateAesAlgorithm();
            var encryptor = aesAlg.CreateEncryptor();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            var streamWriter = new StreamWriter(cryptoStream);

            streamWriter.Write(plainText);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Decryption
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public string Decrypt(string encryptedText)
        {
            var encryptedTextBytes = Convert.FromBase64String(encryptedText);

            var aesAlg = CreateAesAlgorithm();
            var decryptor = aesAlg.CreateDecryptor();
            var memoryStream = new MemoryStream(encryptedTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}