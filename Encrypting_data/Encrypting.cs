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
        public static byte[] SymmetricEncryption(string data, byte[] key)
        {
            byte[] clearData = Encoding.UTF8.GetBytes(data);
            //data = Encoding.UTF8.GetString(tmp);

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
                cryptoStream.Write(clearData, 0, clearData.Length);
                cryptoStream.FlushFinalBlock();
                /*
                target.Write(iv, 0, iv.Length);
                CryptoStream cryptoStream = new CryptoStream(target, aes.CreateEncryptor(), CryptoStreamMode.Write);
                StreamWriter encryptWriter = new(cryptoStream);
                encryptWriter.Write(data);*/

                // Вернуть зашифрованный поток данных в виде байтового массива
                return target.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
                return null;
            }
        }

        public static string SymmetricDecryption(byte[] data, byte[] key)
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
                return Encoding.UTF8.GetString(target.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The decryption failed. {ex}");
                return null;
            }
        }

        // Asymmetric:
        public static byte[] AsymmetricEncryption(string data, byte[] key)
        {
            return null;
        }

        public static string AsymmetricDecryption(byte[] data, byte[] key)
        {
            return null;
        }

        /// <summary>
        /// Generate Key.
        /// </summary>
        /// <param name="algorithmName">DES or RSA algorithm.</param>
        /// <returns>Byte array is key.</returns>
        public static byte[] GenerateKey(string algorithmName)
        {
            SymmetricAlgorithm Algorithm = SymmetricAlgorithm.Create(algorithmName);
            Algorithm.GenerateKey();

            byte[] key = Algorithm.Key;
            return key;
        }

        /// <summary>
        /// Read Key.
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
        /// Save Key.
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
    }
}
