using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encrypting_data
{
    public static class Encrypting
    {
        public const string AES = "AES";
        public const string RSA = "RSA";

        // Symmetric:
        public static byte[] SymmetricEncryption(byte[] data, byte[] key)
        {
            try
            {
                // Создать алгоритм шифрования
                Aes aes = Aes.Create(AES);
                aes.Key = key;

                // Зашифровать информацию
                MemoryStream target = new MemoryStream();

                // Сгенерировать случайный вектор инициализации (IV)
                // для использования с алгоритмом
                aes.GenerateIV();
                target.Write(aes.IV, 0, aes.IV.Length);

                // Зашифровать реальные данные
                CryptoStream cryptoStream = new CryptoStream(target, aes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                // Вернуть зашифрованный поток данных в виде байтового массива
                return target.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
                return null;
            }
        }

        public static byte[] SymmetricDecryption(byte[] data, byte[] key)
        {
            try
            {
                // Создать алгоритм
                Aes aes = Aes.Create(AES);
                aes.Key = key;

                // Расшифровать информацию
                MemoryStream target = new MemoryStream();

                // Прочитать вектор инициализации (IV)
                // и инициализировать им алгоритм
                byte[] iv = new byte[aes.IV.Length];
                int readPos = 0;
                Array.Copy(data, iv, iv.Length);
                aes.IV = iv;
                readPos += aes.IV.Length;

                // Расшифровать реальные данные
                CryptoStream cryptoStream = new CryptoStream(target, aes.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                cryptoStream.Write(data, readPos, data.Length - readPos);
                cryptoStream.FlushFinalBlock();

                // Получить байты из потока в памяти и преобразовать их в текст
                return target.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The decryption failed. {ex}");
                return null;
            }
        }

        /// <summary>
        /// Generate AES Symmetric Key.
        /// </summary>
        /// <returns>Byte array is key.</returns>
        public static byte[] GenerateKeyAES()
        {
            Aes aes = Aes.Create();
            aes.GenerateKey();
            byte[] key = aes.Key;
            return key;
        }

        /// <summary>
        /// Read Key from file.
        /// </summary>
        /// <param name="keyFile">Path to file.</param>
        /// <returns>Byte array is key.</returns>
        public static byte[] ReadKey(string keyFile)
        {
            byte[] Key;
            using (FileStream fs = new FileStream(keyFile, FileMode.Open))
            {
                Key = new byte[fs.Length];
                fs.Read(Key, 0, (int)fs.Length);
            }
            return Key;
        }

        /// <summary>
        /// Save key to file
        /// </summary>
        /// <param name="targetFile">Path to file.</param>
        /// <param name="key">Key is byte array</param>
        public static void SaveKey(string targetFile, byte[] key)
        {
            using (FileStream fs = new FileStream(targetFile, FileMode.Create))
            {
                fs.Write(key, 0, key.Length);
            }
        }

        // Asymmetric:
        public static byte[] AsymmetricEncryption(byte[] data, byte[] keyPublic)
        {
            // Создать алгоритм на основе открытого ключа
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(Encoding.UTF8.GetString(keyPublic));

            // Зашифровать данные
            return rsa.Encrypt(data, false);
        }

        public static byte[] AsymmetricDecryption(byte[] data, byte[] keyPrivate)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(Encoding.UTF8.GetString(keyPrivate));
            return rsa.Decrypt(data, false);
        }

        /// <summary>
        /// Genarate key RSA
        /// </summary>
        /// <returns>Key - public key, value - private key</returns>
        public static KeyValuePair<byte[], byte[]> GenerateKeyRSA()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            // Сохранить секретный ключ
            byte[] keyPrivate = Encoding.UTF8.GetBytes(rsa.ToXmlString(true));
            // Сохранить открытый ключ
            byte[] keyPublic = Encoding.UTF8.GetBytes(rsa.ToXmlString(false));

            return new KeyValuePair<byte[], byte[]>(keyPublic, keyPrivate);
        }
    }
}
