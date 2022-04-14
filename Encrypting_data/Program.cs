using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encrypting_data
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1 - зашифровать данные AES");
                Console.WriteLine("2 - зашифровать данные RSA");
                Console.WriteLine("3 - расшифровать данные AES");
                Console.WriteLine("4 - расшифровать данные RSA");

                string? tmp = Console.ReadLine();
                Console.Clear();
                switch (tmp)
                {
                    case "1":
                        {
                            Console.WriteLine("Введите директорию для шифрования AES:");
                            string? pathDir = Console.ReadLine();
                            if (pathDir == null || !Directory.Exists(pathDir))
                                break;

                            Console.Clear();
                            string[] allFiles = Directory.GetFiles(pathDir);
                            foreach (string filename in allFiles)
                            {
                                Console.WriteLine(filename);
                            }

                            Console.Write("Зашифровать данные? [y / n]: ");
                            string? yes = Console.ReadLine();
                            if (!(yes == "y" || yes == "Y"))
                                break;

                            Console.Clear();
                            string? exportDir = Directory.GetParent(pathDir).FullName + "\\data_AES";
                            if (Directory.Exists(exportDir))
                                Directory.Delete(exportDir, true);
                            Directory.CreateDirectory(exportDir);
                            byte[] key = Encrypting.GenerateKeyAES();
                            foreach (string file in allFiles)
                            {
                                FileStream fsRead = File.OpenRead(file);
                                byte[] data = new byte[fsRead.Length];
                                fsRead.Read(data);
                                fsRead.Close();

                                byte[] encrData = Encrypting.SymmetricEncryption(data, key);

                                string fileName = Path.GetFileName(file);

                                FileStream fsWrite = new FileStream(exportDir + "\\" + fileName + ".encr", FileMode.OpenOrCreate);
                                fsWrite.Write(encrData);
                                fsWrite.Close();
                            }

                            Encrypting.SaveKey(exportDir + "\\key.encr", key);
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("Введите директорию для шифрования RSA:");
                            string? pathDir = Console.ReadLine();
                            if (pathDir == null || !Directory.Exists(pathDir))
                                break;

                            Console.Clear();
                            string[] allFiles = Directory.GetFiles(pathDir);
                            foreach (string filename in allFiles)
                            {
                                Console.WriteLine(filename);
                            }

                            Console.Write("Зашифровать данные? [y / n]: ");
                            string? yes = Console.ReadLine();
                            if (!(yes == "y" || yes == "Y"))
                                break;

                            Console.Clear();

                            string? exportDir = Directory.GetParent(pathDir).FullName + "\\data_RSA";
                            if (Directory.Exists(exportDir))
                                Directory.Delete(exportDir, true);
                            Directory.CreateDirectory(exportDir);
                            var keys = Encrypting.GenerateKeyRSA();
                            byte[] keyPublic = keys.Key;
                            byte[] keyPrivate = keys.Value;

                            foreach (string file in allFiles)
                            {
                                FileStream fsRead = File.OpenRead(file);
                                byte[] data = new byte[fsRead.Length];
                                fsRead.Read(data);
                                fsRead.Close();

                                byte[] encrData = Encrypting.AsymmetricEncryption(data, keyPublic);

                                string fileName = Path.GetFileName(file);

                                FileStream fsWrite = new FileStream(exportDir + "\\" + fileName + ".encr", FileMode.OpenOrCreate);
                                fsWrite.Write(encrData);
                                fsWrite.Close();
                            }

                            Encrypting.SaveKey(exportDir + "\\key_public.encr", keyPublic);
                            Encrypting.SaveKey(exportDir + "\\key_private.encr", keyPrivate);
                            break;
                        }

                    case "3":
                        { 
                            Console.WriteLine("Введите директорию для дешифрования AES:");
                            string? pathDir = Console.ReadLine();
                            if (pathDir == null || !Directory.Exists(pathDir))
                                break;

                            Console.Clear();
                            string[] allFilesTmp = Directory.GetFiles(pathDir);
                            List<string> allFiles = new List<string>();
                            string? keyPath = null;

                            foreach (string filename in allFilesTmp)
                            {
                                if (Path.GetFileName(filename) == "key.encr")
                                    keyPath = filename;
                                else
                                    allFiles.Add(filename);
                            }

                            if (keyPath == null)
                                break;

                            foreach (string filename in allFiles)
                                Console.WriteLine(filename);

                            Console.Write("Расшифровать данные? [y / n]: ");
                            string? yes = Console.ReadLine();
                            if (!(yes == "y" || yes == "Y"))
                                break;


                            string? exportDir = Directory.GetParent(pathDir).FullName + "\\data_NOT_AES";
                            if (Directory.Exists(exportDir))
                                Directory.Delete(exportDir, true);
                            Directory.CreateDirectory(exportDir);

                            byte[] key = Encrypting.ReadKey(keyPath);
                            foreach (string file in allFiles)
                            {
                                FileStream fsRead = File.OpenRead(file);
                                byte[] data = new byte[fsRead.Length];
                                fsRead.Read(data);
                                fsRead.Close();
                                File.Delete(file);

                                byte[] decrData = Encrypting.SymmetricDecryption(data, key);

                                string fileName = Path.GetFileNameWithoutExtension(file);

                                FileStream fsWrite = new FileStream(exportDir + "\\" + fileName, FileMode.OpenOrCreate);
                                fsWrite.Write(decrData);
                                fsWrite.Close();
                            }

                            File.Delete(keyPath);
                            Directory.Delete(pathDir);
                        }
                        break;

                    case "4":
                        {
                            Console.WriteLine("Введите директорию для дешифрования RSA:");
                            string? pathDir = Console.ReadLine();
                            if (pathDir == null || !Directory.Exists(pathDir))
                                break;

                            Console.Clear();
                            string[] allFilesTmp = Directory.GetFiles(pathDir);
                            List<string> allFiles = new List<string>();
                            string? keyPublic = null;
                            string? keyPrivate = null;

                            foreach (string filename in allFilesTmp)
                            {
                                if (Path.GetFileName(filename) == "key_private.encr")
                                    keyPrivate = filename;
                                else if (Path.GetFileName(filename) == "key_public.encr")
                                    keyPublic = filename;
                                else
                                    allFiles.Add(filename);
                            }

                            if (keyPrivate == null)
                                break;

                            foreach (string filename in allFiles)
                                Console.WriteLine(filename);

                            Console.Write("Расшифровать данные? [y / n]: ");
                            string? yes = Console.ReadLine();
                            if (!(yes == "y" || yes == "Y"))
                                break;

                            string? exportDir = Directory.GetParent(pathDir).FullName + "\\data_NOT_RSA";
                            if (Directory.Exists(exportDir))
                                Directory.Delete(exportDir, true);
                            Directory.CreateDirectory(exportDir);

                            byte[] key = Encrypting.ReadKey(keyPrivate);
                            foreach (string file in allFiles)
                            {
                                FileStream fsRead = File.OpenRead(file);
                                byte[] data = new byte[fsRead.Length];
                                fsRead.Read(data);
                                fsRead.Close();
                                File.Delete(file);

                                byte[] decrData = Encrypting.AsymmetricDecryption(data, key);

                                string fileName = Path.GetFileNameWithoutExtension(file);

                                FileStream fsWrite = new FileStream(exportDir + "\\" + fileName, FileMode.OpenOrCreate);
                                fsWrite.Write(decrData);
                                fsWrite.Close();
                            }

                            File.Delete(keyPrivate);
                            File.Delete(keyPublic);
                            Directory.Delete(pathDir);

                            break;
                        }
                    default:
                        break;
                }

            } while (true);

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
            q = (decryptData == data);

            // Создали ключи Key - public, value - private
            KeyValuePair<byte[], byte[]> keys = Encrypting.GenerateKeyRSA();

            // Зашифровали
            byte[] encryptData = Encrypting.AsymmetricEncryption(data, keys.Key);

            // Преобразовать строку data в байтовый массив
            byte[] clearData = Encoding.UTF8.GetBytes(data);

            // Попробуем дешифровать верным ключом (если false - где-то косяк в шифровании / дешифровании)
            string decryptData = Encrypting.AsymmetricDecryption(encryptData, keys.Value);*/
        }
    }
}
