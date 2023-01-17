using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptString
{
    class Program
    {
        static string key = "OS/Ez7b2Lf/2V9ZW3f3BqoszshGptkn0CNTAhD1zx2I=";
        static string password = "FC89TERbsFp3tqM5KmBxiQ==";

        static void Main(string[] args)
        {
            //int[] Data = new int[15];
            //for (int i = 0; i < Data.Length; i++)
            //{
            //    Data[i] = i;
            //}

            //for (int j = 0; j < 2; j++)
            //{
            //    var t = ((j + 1) * 10);
            //    var length = t > Data.Length ? Data.Length : t;
            //    for (int i = j * 10; i < length; i++)
            //    {
            //        if (i > (j * 10) - 1 && i  < ((j + 1) * 10))
            //        {
            //            Console.Write(Data[i] + "  ");
            //        }
            //    }
            //    Console.WriteLine();
            //}

            var encrypted = EncryptString(key, "#Welcome123");
            var plainText = DecryptString(key, password);

            DecryptString decryptString = new DecryptString();
            var _password = decryptString.Decrypt();

        }

        private static void CallEncryptString()
        {
            Console.WriteLine("Welcome to the Aes Encryption Test tool");
            Console.WriteLine("Please enter the text that you want to encrypt:");
            string plainText = Console.ReadLine();
            string cipherText = EncryptDataWithAes(plainText, out string keyBase64, out string vectorBase64);

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Here is the cipher text:");
            Console.WriteLine(cipherText);

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Here is the Aes key in Base64:");
            Console.WriteLine(keyBase64);

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Here is the Aes IV in Base64:");
            Console.WriteLine(vectorBase64);
        }

        private static void CallDescryptString()
        {
            Console.WriteLine("Welcome to the Aes Encryption Test tool");
            Console.WriteLine("Please enter the text that you want to decrypt:");
            string cipherText = "YkrZuO3t13q8hDObtWgipg==";
            Console.WriteLine("--------------------------------------------------------------");

            Console.WriteLine("Provide the Aes Key:");
            string keyBase64 = "OS/Ez7b2Lf/2V9ZW3f3BqoszshGptkn0CNTAhD1zx2I=";
            Console.WriteLine("--------------------------------------------------------------");

            Console.WriteLine("Provide the initialization vector:");
            string vectorBase64 = "eMixOAlqMe9uN+ngLfhVaw==";
            Console.WriteLine("--------------------------------------------------------------");


            string plainText = DecryptDataWithAes(cipherText, keyBase64, vectorBase64);

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Here is the decrypted data:");
            Console.WriteLine(plainText);
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                //aes.Key = Convert.FromBase64String(key);
                //aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                //aes.Key = Convert.FromBase64String(key);
                //aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static string EncryptDataWithAes(string plainText, out string keyBase64, out string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                Console.WriteLine($"Aes Cipher Mode : {aesAlgorithm.Mode}");
                Console.WriteLine($"Aes Padding Mode: {aesAlgorithm.Padding}");
                Console.WriteLine($"Aes Key Size : {aesAlgorithm.KeySize}");
                Console.WriteLine($"Aes Block Size : {aesAlgorithm.BlockSize}");

                //set the parameters with out keyword
                keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
                vectorBase64 = Convert.ToBase64String(aesAlgorithm.IV);

                // Create encryptor object
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();

                byte[] encryptedData;

                //Encryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(encryptedData);
            }
        }

        private static string DecryptDataWithAes(string cipherText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);

                Console.WriteLine($"Aes Cipher Mode : {aesAlgorithm.Mode}");
                Console.WriteLine($"Aes Padding Mode: {aesAlgorithm.Padding}");
                Console.WriteLine($"Aes Key Size : {aesAlgorithm.KeySize}");
                Console.WriteLine($"Aes Block Size : {aesAlgorithm.BlockSize}");

                // Create decryptor object
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

                byte[] cipher = Convert.FromBase64String(cipherText);

                //Decryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
