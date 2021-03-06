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
            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Создали ключ
            byte[] key = Encrypting.GenerateKeyAES();

            // Зашифровали
            byte[] encryptData = Encrypting.SymmetricEncryption(clearData, key);

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
            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Создали ключ
            byte[] key = Encrypting.GenerateKeyAES();

            // Зашифровали
            byte[] encryptData = Encrypting.SymmetricEncryption(clearData, key);

            // Попробуем дешифровать другим ключом (если true - ошибка в шифровании, подходит любой ключ)
            byte[] decyptBadData = Encrypting.SymmetricDecryption(encryptData, Encrypting.GenerateKeyAES());
            if (decyptBadData != null)
                Assert.IsFalse(decyptBadData.SequenceEqual(clearData), "Wrong key - correct");

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            byte[] decryptData = Encrypting.SymmetricDecryption(encryptData, key);

            Assert.IsTrue(decryptData.SequenceEqual(clearData), "Source is not equal to decrypted");
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

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Зашифровали
            byte[] encryptData = Encrypting.AsymmetricEncryption(clearData, keys.Key);

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
            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Создали ключи Key - public, value - private
            KeyValuePair<byte[], byte[]> keys = Encrypting.GenerateKeyRSA();

            // Зашифровали
            byte[] encryptData = Encrypting.AsymmetricEncryption(clearData, keys.Key);

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            byte[] decryptData = Encrypting.AsymmetricDecryption(encryptData, keys.Value);
            Assert.IsTrue(decryptData.SequenceEqual(clearData), "Source is not equal to decrypted");
        }
    }
}