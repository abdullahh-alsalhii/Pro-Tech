using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace ProـTech_Web.Global
{
    class EncreptionRes<T>
    {
        public string Msg { get; set; }
        public bool IsOk { get; set; }
        public T Value { get; set; }
    }
    class Encreption
    {
        public static readonly string IV = "qwd";
        public static readonly string Passowrd = "55404";
        public static readonly string Salt = "651كهيغعهلس";
        private static byte[] Get_16_Byte_From(string key)
        {
            byte[] salt = Encoding.Unicode.GetBytes(Salt);
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.Unicode.GetBytes(key),
                    salt,
                    iterations,
                    hashMethod,
                    desiredKeyLength);
        }
        public static EncreptionRes<string> En_Code(string words)
        {
            try
            {
                async Task<byte[]> EncryptAsync()
                {
                    using Aes aes = Aes.Create();
                    aes.Key = Get_16_Byte_From(Passowrd);
                    aes.IV = Get_16_Byte_From(IV);
                    using MemoryStream output = new();
                    using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(words));
                    await cryptoStream.FlushFinalBlockAsync();
                    return output.ToArray();
                }
                byte[] encodedBytes = EncryptAsync().GetAwaiter().GetResult();
                string final = BitConverter.ToString(encodedBytes);
                byte[] finalByte = Encoding.UTF8.GetBytes(final);
                return new EncreptionRes<string>()
                {
                    Value = Convert.ToBase64String(finalByte),
                    IsOk = true,
                    Msg = "Ok"
                };
            }
            catch (Exception err)
            {
                return new EncreptionRes<string>()
                {
                    Value = null,
                    IsOk = false,
                    Msg = err.Message
                };
            }

        }
        public static EncreptionRes<string> De_Code(string words)
        {
            try
            {
                async Task<string> DecryptAsync()
                {

                    var getBytesFromWords = Convert.FromBase64String(words);
                    words = Encoding.UTF8.GetString(getBytesFromWords);
                    List<byte> wordsBytes = new List<byte>();
                    foreach (string str_byte in words.Split("-"))
                    {
                        wordsBytes.Add(Convert.ToByte(str_byte, 16));
                    }
                    using Aes aes = Aes.Create();
                    aes.Key = Get_16_Byte_From(Passowrd);
                    aes.IV = Get_16_Byte_From(IV);
                    using MemoryStream input = new(wordsBytes.ToArray());
                    using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
                    using MemoryStream output = new();
                    await cryptoStream.CopyToAsync(output);
                    return Encoding.Unicode.GetString(output.ToArray());
                }
                string final = DecryptAsync().GetAwaiter().GetResult();
                return new EncreptionRes<string>()
                {
                    Value = final,
                    IsOk = true,
                    Msg = "Ok"
                };
            }
            catch (Exception err)
            {
                return new EncreptionRes<string>()
                {
                    Value = null,
                    IsOk = false,
                    Msg = err.Message
                };
            }
        }
    }
    public class Hasher
    {
        private static string password_1 = "Look Its And He Is Aaying !#25#%$^";
        private static string password_2 = "Hah How That Is Impossible Fu_wqu@#5$#487";
        private static string password_3 = "No Its Him D: no waAaay";
        private static byte[] salt_password_1 = Encoding.ASCII.GetBytes(password_1);
        private static byte[] salt_password_2 = Encoding.ASCII.GetBytes(password_2);
        private static byte[] salt_password_3 = Encoding.ASCII.GetBytes(password_3);
        private static string Hash_Prf(string word, KeyDerivationPrf prf, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: word!,
                salt: salt,
                prf: prf,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
            );
            return hashed;
        }
        public static string Hash(string word)
        {
            string final_1 = Hash_Prf(word, KeyDerivationPrf.HMACSHA512, salt_password_1);
            string final_2 = Hash_Prf(final_1, KeyDerivationPrf.HMACSHA256, salt_password_2);
            string final_3 = Hash_Prf(final_2, KeyDerivationPrf.HMACSHA512, salt_password_3);
            return final_3;
        }
        public static bool Compare_Hash(string? word, string hash)
        {
            if(word is null || hash is null)
                return false;
            string final_1 = Hash_Prf(word, KeyDerivationPrf.HMACSHA512, salt_password_1);
            string final_2 = Hash_Prf(final_1, KeyDerivationPrf.HMACSHA256, salt_password_2);
            string final_3 = Hash_Prf(final_2, KeyDerivationPrf.HMACSHA512, salt_password_3);
            return final_3 == hash;
        }
    }
}
