using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Encrypting_data
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            string data = "Test 123";

            /*
            // Создали ключ
            byte[] key = Encrypting.GenerateKeyAES();

            // Зашифровали
            byte[] encryptData = Encrypting.SymmetricEncryption(data, key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Попробовали сравнить с исходным: (если true - незашифрован)
            bool q = encryptData.SequenceEqual(clearData);

            // Попробуем дешифровать другим ключом (если true - ошибка в шифровании, подходит любой ключ)
            string decyptBadData = Encrypting.SymmetricDecryption(encryptData, Encrypting.GenerateKeyAES());
            q = (decyptBadData == data);

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            string decryptData = Encrypting.SymmetricDecryption(encryptData, key);
            q = (decryptData == data);*/

            // Создали ключи Key - public, value - private
            KeyValuePair<byte[], byte[]> keys = Encrypting.GenerateKeyRSA();

            // Зашифровали
            byte[] encryptData = Encrypting.AsymmetricEncryption(data, keys.Key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            string decryptData = Encrypting.AsymmetricDecryption(encryptData, keys.Value);
        }
    }
}
