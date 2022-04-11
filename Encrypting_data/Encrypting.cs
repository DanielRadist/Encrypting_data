﻿using System;
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
        public const string DES = "DES";
        public const string RSA = "RSA";

        // Symmetric:
        public static byte[] SymmetricEncryption(string data, byte[] key)
        {
            // Преобразовать строку data в байтовый массив
            byte[] сlearData = Encoding.UTF8.GetBytes(data);

            // Создать алгоритм шифрования
            SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create(DES);
            algorithm.Key = key;

            // Зашифровать информацию
            MemoryStream Target = new MemoryStream();

            // Сгенерировать случайный вектор инициализации (IV)
            // для использования с алгоритмом
            algorithm.GenerateIV();
            Target.Write(algorithm.IV, 0, algorithm.IV.Length);

            // Зашифровать реальные данные
            CryptoStream cs = new CryptoStream(Target, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(сlearData, 0, сlearData.Length);
            cs.FlushFinalBlock();

            // Вернуть зашифрованный поток данных в виде байтового массива
            return Target.ToArray();
        }

        public static string SymmetricDecryption(byte[] data, byte[] key)
        {
            return null;
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
