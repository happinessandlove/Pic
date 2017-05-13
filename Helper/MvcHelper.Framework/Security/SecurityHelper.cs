using System.Text;
using System.Security.Cryptography;

namespace System
{
    /// <summary>
    /// （自定义）安全相关
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <returns></returns>
        public static string MD5Hash(string plaintext)
        {
            MD5 md5 = MD5.Create();
            string p = BitConverter.ToString(md5.ComputeHash(Encoding.Unicode.GetBytes(plaintext.Trim()))).Replace("-", "");
            md5.Clear();
            md5.Dispose();
            return p;
        }
    }
}