using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;  
using System.Text;
using System.Web;

namespace NBCZ.DBUtility
{
	/// <summary>
	/// DES加密/解密类。
	/// </summary>
	public class DesEncrypt
	{
        public static string jmkeys = ConfigurationManager.AppSettings["jmkey"];
        public static string encryption = "NBCZ948";
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return DESEnCode(text, jmkeys);
        }
      

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            return DESDeCode(text, jmkeys);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Ecode(string text)
        {
            return DESEnCode(text, encryption);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Dcode(string text)
        {
            return DESDeCode(text, encryption);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string DESEnCode(string pToEncrypt, string sKey)
        {
            try
            {
                pToEncrypt = System.Web.HttpUtility.UrlEncode(pToEncrypt);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
            }
            catch
            {
                return null;
            }
        }

        #region DESDeCode DES解密
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="pToDecrypt">解密字符</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string DESDeCode(string pToDecrypt, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (System.Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                return HttpContext.Current.Server.UrlDecode(System.Text.Encoding.Default.GetString(ms.ToArray()));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()  
        {  
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();  
        }
        #endregion
	}
}
