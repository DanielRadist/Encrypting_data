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
            byte[] key = Encrypting.GenerateKey(Encrypting.DES);

            // Зашифровали
            byte[] encryptData = Encrypting.SymmetricEncryption(data, key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Попробовали сравнить с исходным: (если true - незашифрован)
            Assert.IsFalse(encryptData.SequenceEqual(clearData), "byte[] encryptData and byte[] clearData are Equal");

            // Попробуем дешифровать другим ключом (если true - ошибка в шифровании, подходит любой ключ)
            string decyptBadData = Encrypting.SymmetricDecryption(encryptData, Encrypting.GenerateKey(Encrypting.DES));
            Assert.IsFalse(decyptBadData == data, "Wrong key - correct");

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            string decryptData = Encrypting.SymmetricDecryption(encryptData, key);
            Assert.IsTrue(decryptData == data, "Source is not equal to decrypted");
        }

        [TestMethod()]
        public void SymmetricDecryptionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AsymmetricEncryptionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AsymmetricDecryptionTest()
        {
            Assert.Fail();
        }
    }
}