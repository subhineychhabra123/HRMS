﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Text.RegularExpressions;

namespace EMPMGMT.Utility
{
    public static class ExtensionMethods
    {
        public static string Encrypt(this Int32 toEncrypt)
        {
            if (toEncrypt == 0)
                return "0";

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(SessionManagement.LoggedInUser.UserId + "-" + toEncrypt.ToString());

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            string key = "App";
            // (string)settingsReader.GetValue("SecurityKey",
            //                                 typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            ////If hashing use get hashcode regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            //hashmd5.Clear();
            //else
            //keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format

            return Convert.ToBase64String(resultArray, 0, resultArray.Length).Replace("/", "^").Replace("+", "~");

        }

        public static int Decrypt(this string cipherString)
        {
            int checkInt = 0;
            if (string.IsNullOrEmpty(cipherString) || cipherString=="0" || (int.TryParse(cipherString, out checkInt) && checkInt == 0))
                return 0;

            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString.Replace("^", "/").Replace("~", "+"));

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = "App";
            //(string)settingsReader.GetValue("SecurityKey",
            //                                       typeof(String));

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            ////release any resource held by the MD5CryptoServiceProvider

            //hashmd5.Clear();

            //else
            //{
            //if hashing was not implemented get the byte code of the key
            // keyArray = UTF8Encoding.UTF8.GetBytes(key);
            //}

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            string result = UTF8Encoding.UTF8.GetString(resultArray);
            try
            {
                int iresult = 0;
                if (Convert.ToInt32(result.Split('-')[0]) == SessionManagement.LoggedInUser.UserId && int.TryParse(result.Split('-')[1], out iresult))
                    return iresult;
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }
        public static int Decrypt_uploaduser(this string cipherString)
        {

            int checkInt = 0;
            if (string.IsNullOrEmpty(cipherString) || (int.TryParse(cipherString, out checkInt) && checkInt == 0))
                return 0;

            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString.Replace("^", "/").Replace("~", "+"));

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = "App";
            //(string)settingsReader.GetValue("SecurityKey",
            //                                       typeof(String));

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            ////release any resource held by the MD5CryptoServiceProvider

            //hashmd5.Clear();

            //else
            //{
            //if hashing was not implemented get the byte code of the key
            // keyArray = UTF8Encoding.UTF8.GetBytes(key);
            //}

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            string result = UTF8Encoding.UTF8.GetString(resultArray);
            try
            {
                int iresult = 0;
                if (int.TryParse(result.Split('-')[1], out iresult))
                    return iresult;
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }
        //public static DateTime ToDateTimeNow(this DateTime utcDate)
        //{
        //    string offset = SessionManagement.LoggedInUser.TimeZoneOffSet;
        //    DateTime dateObj = new DateTime();
        //    dateObj = utcDate.AddHours(Convert.ToDouble(offset));
        //    return dateObj;
        //}
        public static bool IsValidEmailAddress(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }
    }
}
