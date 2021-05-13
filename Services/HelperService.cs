using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace webui.Services
{

    public interface IHelpersService
    {
        string ConcatenateStrNum(string str, int num);
        string GetAppSetting(string name);
        string GetConnectionString(string name);
        bool GetBoolean(string inputValue);
        String GetStringWithMaxLength(object inputValue, int maxLength);
        bool ParseBoolean(string valuetoParse);
        float ParseFloat(string valuetoParse);
        int ParseInt(string valueToParse);
        long ParseLong(string valuetoParse);
        int ParseIntSetBaseReturn(string valueToParse, int optionalReturn);
        DateTime ParseDate(string valueToParse);
        string ParseString(object valueToParse);
        string EncryptRsa(string stringToEncrypt);
        string Decrypt(string password);
        string DecryptRsa(string encryptedString);
        string Encrypt(string password);
        string EncryptRijndael(string toEncrypt, string password);
        string DecryptRijndael(string toDecrypt, string password);
        string EncryptRijndael(string toEncrypt);
        string DecryptRijndael(string toDecrypt);
        string CleanUri(Uri uri);
        string GetMd5Hash(string password);
        bool ValidateVehicleVin(string inputString);
        string[] TokenizeString(string stringToTokenize);
        string GetTemporaryKey();
        int MaxForgottenEmailElapsedHours { get; }

    }

    //public interface IDbHelperService
    //{
    //    Database GetNamedDatabase(string namedDatabase);

    //    Database GetDefaultDatabase();
    //}


    /// <summary>
    ///     Class for helper methods utilized in different parts of the solution
    /// </summary>
    public class HelpersService : IHelpersService
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "tu89geji340t89u2";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        private const int _tempKeySize = 8;

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;


        public const int MAX_VIN_LENGTH = 17;
        public const int MIN_VEHICLE_PRICE = 500;
        public const int MAX_MPG = 1000;
        public const int MIN_DOORS = 2;
        public const int MAX_DOORS = 5;
        public const int MIN_CYLINDERS = 2;
        public const int MAX_CYLINDERS = 16;
        public const int MIN_MODEL_YEAR = 1990;
        public const int MIN_MILEAGE = 10000;
        public const int MAX_MILEAGE = 101000;
        public const int MAX_FORGOTTEN_PASSWORD_ELAPSED_HOURS = 24;
        public const string CRM_SUBJECT_SUFFIX = "Inquiry";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public HelpersService(ILogger logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public int MaxForgottenEmailElapsedHours
        {
            get
            {
                return HelpersService.MAX_FORGOTTEN_PASSWORD_ELAPSED_HOURS;
            }
        }

        /// <summary>
        ///     Returns value of app setting from config.file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAppSetting(string name)
        {
            var appString = string.Empty;
            //If a name was provided, try and find that environment string
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    appString = _configuration.GetSection("AppSettings").GetSection(name).Value;

                }
            }
            catch (ConfigurationErrorsException e)
            {
                _logger.LogError(e.ToString());
            }

            return appString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            var conString = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    conString = _configuration.GetConnectionString(name);

                }
            }
            catch (ConfigurationErrorsException e)
            {
                _logger.LogError(e.ToString());
            }

            return conString;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTemporaryKey()
        {
            return Guid.NewGuid().ToString().Substring(0, _tempKeySize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ConcatenateStrNum(string str, int num)
        {
            return string.Format("{0}{1}", str, num);
        }

        string IHelpersService.ConcatenateStrNum(string str, int num)
        {
            return ConcatenateStrNum(str, num);
        }

        #region helper methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes
                                             (typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetEnumDefaultValue(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DefaultValueAttribute[])fi.GetCustomAttributes
                                             (typeof(DefaultValueAttribute), false);
            return (attributes != null) ? (int)attributes[0].Value : 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public bool GetBoolean(string inputValue)
        {
            var returnValue = false;
            if (!string.IsNullOrEmpty(inputValue))
                returnValue = !(inputValue.Equals("false", StringComparison.OrdinalIgnoreCase) || inputValue.Equals("0", StringComparison.OrdinalIgnoreCase) || inputValue.Equals("n", StringComparison.OrdinalIgnoreCase));
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public String GetStringWithMaxLength(object inputValue, int maxLength)
        {
            var result = Convert.ToString(inputValue);
            var returnLength = result.Length > maxLength ? maxLength : result.Length;

            return inputValue != null ? result.Substring(0, returnLength) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuetoParse"></param>
        /// <returns></returns>
        public bool ParseBoolean(string valuetoParse)
        {
            var tempBool = false;
            if (!string.IsNullOrEmpty(valuetoParse))
                bool.TryParse(valuetoParse, out tempBool);

            return tempBool;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuetoParse"></param>
        /// <returns></returns>
        public float ParseFloat(string valuetoParse)
        {
            var tempFloat = 0f;
            if (!string.IsNullOrEmpty(valuetoParse))
                float.TryParse(valuetoParse, out tempFloat);

            return tempFloat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToParse"></param>
        /// <returns></returns>
        public int ParseInt(string valueToParse)
        {
            var tempInt = 0;
            if (!string.IsNullOrEmpty(valueToParse))
                int.TryParse(valueToParse, out tempInt);

            return tempInt;
        }

        public long ParseLong(string valuetoParse)
        {
            var tempLong = 0L;
            if (!string.IsNullOrEmpty(valuetoParse))
                long.TryParse(valuetoParse, out tempLong);

            return tempLong;
        }

        /// <summary>
        ///     Allows for returning a value other than 0 when the string to be parsed is null
        /// </summary>
        /// <param name="valueToParse"></param>
        /// <param name="optionalReturn"></param>
        /// <returns></returns>
        public int ParseIntSetBaseReturn(string valueToParse, int optionalReturn)
        {
            int tempInt;
            if (optionalReturn != 0 && valueToParse == null)
            {
                tempInt = optionalReturn;
            }
            else
            {
                int.TryParse(valueToParse, out tempInt);
            }

            return tempInt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToParse"></param>
        /// <returns></returns>
        public DateTime ParseDate(string valueToParse)
        {
            var tempDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(valueToParse))
                DateTime.TryParse(valueToParse, out tempDate);

            return tempDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToParse"></param>
        /// <returns></returns>
        public string ParseString(object valueToParse)
        {
            return valueToParse != null && valueToParse != DBNull.Value && !valueToParse.Equals(DBNull.Value) ? Convert.ToString(valueToParse) : null;
        }

        string IHelpersService.DecryptRijndael(string toDecrypt, string password)
        {
            return DecryptRijndael(toDecrypt, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string stringToHash)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();

            }// end using statement
        }

        /// <summary>
        /// Compares two hashed strings to determine if they are the same or different
        /// NOTE: this is good to use if the calling method has the stored hash and a plain text
        /// string
        /// </summary>
        /// <param name="hashedValue">hashed string from db or other data store</param>
        /// <param name="stringToCompare">plain text string</param>
        /// <returns>true if two hashed string match; false otherwise</returns>
        public static bool VerifyMd5Hash(string hashedValue, string stringToCompare)
        {
            using (MD5.Create())
            {
                // Hash the input. 
                string hashOfInput = GetMd5Hash(stringToCompare);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                return 0 == comparer.Compare(hashOfInput, hashedValue);

            }// end using statement
        }

        // end method: VerifyMd5Hash

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase"></param>
        /// <returns></returns>
        public static string EncryptRijndael(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);

        }

        string IHelpersService.EncryptRijndael(string toEncrypt, string password)
        {
            return EncryptRijndael(toEncrypt, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        string IHelpersService.EncryptRijndael(string toEncrypt)
        {
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.GenerateKey();
                myRijndael.GenerateIV();

                return EncryptStringRijndael(toEncrypt, myRijndael.Key, myRijndael.IV);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        string IHelpersService.DecryptRijndael(string toDecrypt)
        {

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                byte[] bytes = Convert.FromBase64String(toDecrypt);

                myRijndael.GenerateKey();
                myRijndael.GenerateIV();

                return DecryptStringRijndael(bytes, myRijndael.Key, myRijndael.IV);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="passPhrase"></param>
        /// <returns></returns>
        public static string DecryptRijndael(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <returns></returns>
        public string EncryptRsa(string stringToEncrypt)
        {
            RSACryptoServiceProvider myRsa = new RSACryptoServiceProvider();

            byte[] messageBytes = Encoding.Unicode.GetBytes(stringToEncrypt);
            byte[] encryptedMessage = myRsa.Encrypt(messageBytes, false);

            return Convert.ToBase64String(encryptedMessage);


        }// end method: EncryptRsa

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public string DecryptRsa(string encryptedString)
        {
            RSACryptoServiceProvider myRsa = new RSACryptoServiceProvider();


            byte[] encryptedByteArray = Encoding.Unicode.GetBytes(encryptedString); //Convert.FromBase64String(encryptedString);

            byte[] decryptedBytes = myRsa.Decrypt(encryptedByteArray, false);

            return Convert.ToBase64String(decryptedBytes);
        }

        string IHelpersService.GetMd5Hash(string password)
        {
            return GetMd5Hash(password);
        }

        /// <summary>
        /// Method used to encrypt in old application
        /// </summary>
        public string Encrypt(string password)
        {
            byte[] encData_byte = Encoding.UTF8.GetBytes(password);

            string encodedData = Convert.ToBase64String(encData_byte);

            return encodedData;

        }// end method

        /// <summary>
        /// Method used to decrypt in old application
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Decrypt(string password)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(password);

            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

            char[] decoded_char = new char[charCount];

            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

            string result = new String(decoded_char);

            return result;

        }// end method

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string CleanUri(Uri uri)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(uri.Scheme);
            //sb.Append(Uri.SchemeDelimiter);
            string authority = uri.Authority;
            sb.Append(Regex.Replace(authority.Replace(Convert.ToString(uri.Port), string.Empty), @"[\:]", string.Empty));

            return sb.ToString();

        }// end method: CleanUri

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string CleanInputString(string inputString)
        {
            return Regex.Replace(inputString, @"[^\w\.@-]", string.Empty);
        }

        public bool ValidateVehicleVin(string inputString)
        {

            return Regex.Match(inputString, @"^([A-Z]|[0-9]){17}").Success;
        }

        public string[] TokenizeString(string stringToTokenize)
        {

            char[] delim = { ' ', ',' };
            string[] tokens = stringToTokenize.Trim().Split(delim);

            return tokens;
        }

        /// <summary>
        /// Checks for the existence of a column in a IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool ReaderHasColumn(IDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
        #region Encryption/Hashing

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string EncryptStringRijndael(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Convert encrypted bytes from the memory stream to Base64 String
            return Convert.ToBase64String(encrypted);

        }// end method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string DecryptStringRijndael(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cypherBytes");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = string.Empty;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }



        #endregion

    }


    public static class EnumExtension
    {
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
            //or
            //throw new ArgumentException("Not found.", "description");

        }
    }
}
