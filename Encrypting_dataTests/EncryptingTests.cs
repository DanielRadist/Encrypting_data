using Microsoft.VisualStudio.TestTools.UnitTesting;
using Encrypting_data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encrypting_data.Tests
{
    [TestClass()]
    public class EncryptingTests
    {
        [TestMethod()]
        [DataRow(@"Hello world!")]
        [DataRow(@"1234567890")]
        [DataRow(@"!@#$%^&*()_+-=':;,.<>|")]
        [DataRow(@"Привет мир! ")]
        public void SymmetricEncryptionTest(string data)
        {
            // Создали ключ
            byte[] key = Encrypting.GenerateKeyAES();

            // Зашифровали
            byte[] encryptData = Encrypting.SymmetricEncryption(data, key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Были проблемы с шифрованием
            Assert.IsFalse(encryptData == null || encryptData.Length < 1);

            // Попробовали сравнить с исходным: (если true - незашифрован)
            Assert.IsFalse(encryptData.SequenceEqual(clearData), "byte[] encryptData and byte[] clearData are Equal");
        }

        [TestMethod()]
        [DataRow(@"Hello world!")]
        [DataRow(@"1234567890")]
        [DataRow(@"!@#$%^&*()_+-=':;,.<>|")]
        [DataRow(@"Привет мир! ")]
        public void SymmetricDecryptionTest(string data)
        {
            // Создали ключ
            byte[] key = Encrypting.GenerateKeyAES();

            // Зашифровали
            byte[] encryptData = Encrypting.SymmetricEncryption(data, key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Попробуем дешифровать другим ключом (если true - ошибка в шифровании, подходит любой ключ)
            string decyptBadData = Encrypting.SymmetricDecryption(encryptData, Encrypting.GenerateKeyAES());
            Assert.IsFalse(decyptBadData == data, "Wrong key - correct");

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            string decryptData = Encrypting.SymmetricDecryption(encryptData, key);
            Assert.IsTrue(decryptData == data, "Source is not equal to decrypted");
        }

        [TestMethod()]
        [DataRow(@"Hello world!")]
        [DataRow(@"1234567890")]
        [DataRow(@"!@#$%^&*()_+-=':;,.<>|")]
        [DataRow(@"Привет мир! ")]
        public void AsymmetricDecryptionTest(string data)
        {
            // Создали ключи Key - public, value - private
            KeyValuePair<byte[], byte[]> keys = Encrypting.GenerateKeyRSA();

            // Зашифровали
            byte[] encryptData = Encrypting.AsymmetricEncryption(data, keys.Key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Были проблемы с шифрованием
            Assert.IsFalse(encryptData == null || encryptData.Length < 1);

            // Попробовали сравнить с исходным: (если true - незашифрован)
            Assert.IsFalse(encryptData.SequenceEqual(clearData), "byte[] encryptData and byte[] clearData are Equal");
        }

        [TestMethod()]
        [DataRow(@"Hello world!")]
        [DataRow(@"1234567890")]
        [DataRow(@"!@#$%^&*()_+-=':;,.<>|")]
        [DataRow(@"Привет мир! ")]
        public void AsymmetricEncryptionTest(string data)
        {
            // Создали ключи Key - public, value - private
            KeyValuePair<byte[], byte[]> keys = Encrypting.GenerateKeyRSA();

            // Зашифровали
            byte[] encryptData = Encrypting.AsymmetricEncryption(data, keys.Key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            string decryptData = Encrypting.AsymmetricDecryption(encryptData, keys.Value);
            Assert.IsTrue(decryptData == data, "Source is not equal to decrypted");
        }
    }
}