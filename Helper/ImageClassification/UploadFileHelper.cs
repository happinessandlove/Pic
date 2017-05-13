using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace System
{
    public static class UploadFileHelper
    {
        public static string UploadImage(HttpPostedFile file, string directory, string fileName,string extension)
        {
            FileStream fileStream = null;
            string tempPath = null;
            try
            {
                string checkResult = checkImage(file);
                if (!string.IsNullOrEmpty(checkResult)) return checkResult;
                string directoryPath = HttpContext.Current.Server.MapPath("\\Temp\\");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                tempPath = directoryPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;
                file.SaveAs(tempPath);
                fileStream = new FileStream(tempPath, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[0];
                BinaryReader reader = new BinaryReader(fileStream);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                bytes = reader.ReadBytes((int)reader.BaseStream.Length);
                string result = UploadFileHelper.Upload(bytes, directory.Replace("/", "\\"), fileName+extension, fileName, file.ContentType, null);
                return null;
            }
            catch
            {
                return "上传出现错误";
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
                if (tempPath != null) File.Delete(tempPath);
            }
        }
        public static string UploadImage(HttpPostedFileBase file, string directory, string fileName, string extension)
        {
            FileStream fileStream = null;
            string tempPath = null;
            try
            {
                string checkResult = checkImage(file);
                if (!string.IsNullOrEmpty(checkResult)) return checkResult;
                string directoryPath = HttpContext.Current.Server.MapPath("\\Temp\\");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                tempPath = directoryPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileName;
                file.SaveAs(tempPath);
                fileStream = new FileStream(tempPath, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[0];
                BinaryReader reader = new BinaryReader(fileStream);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                bytes = reader.ReadBytes((int)reader.BaseStream.Length);
                string result = UploadFileHelper.Upload(bytes, directory.Replace("/", "\\"), fileName + extension, fileName, file.ContentType, null);
                return null;
            }
            catch
            {
                return "上传出现错误";
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
                if (tempPath != null) File.Delete(tempPath);
            }
        }

        public static string Upload(byte[] fileBytes, string directory, string fileName, string paramName, string contentType, NameValueCollection nvc)
        {
            HttpWebResponse webResponse = null;
            HttpWebRequest webRequest = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(ConfigHelper.FileServerDomainName + "/UploadImage?directory=" + directory);
            }
            catch (Exception ex)
            {

            }
            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                webRequest.Method = "POST";
                webRequest.KeepAlive = true;
                webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream rs = webRequest.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                if (nvc != null && nvc.Count > 0)
                {
                    foreach (string key in nvc.Keys)
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, nvc[key]);
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        rs.Write(formitembytes, 0, formitembytes.Length);
                    }
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, fileName, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                rs.Write(fileBytes, 0, fileBytes.Length);

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                var result = "";
                webResponse = webRequest.GetResponse() as HttpWebResponse;
                if (webResponse.StatusCode != HttpStatusCode.OK)
                    return "上传发生错误";
                Stream stream2 = webResponse.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //成功返回的結果
                result = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "上传发生错误";
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                }
                webRequest = null;
            }
            return null;
        }

        // 检查是否为合法的上传图片
        private static string checkImage(HttpPostedFile imgfile)
        {
            string allowExt = ".gif.jpg.png";
            string fileName = imgfile.FileName;
            FileInfo file = new FileInfo(fileName);
            string imgExt = file.Extension;
            if (allowExt.IndexOf(imgExt.ToLower()) == -1)
            {
                return "图片格式不正确（gif、jpg、png）";
            }
            try
            {
                Image img = Image.FromStream(imgfile.InputStream);
            }
            catch
            {
                return "图片格式不正确（gif、jpg、png）";
            }
            //if (imgfile.ContentLength > 512 * 1024)
            //{
            //    errorMsg += "图片最大限制为0.5Mb；";
            //}
            //if (img.Width < 20 || img.Width > 480 || img.Height < 20 || img.Height > 854)
            //{
            //    errorMsg += "请上传正确尺寸的图片，图片最小为20x20，最大为480*854。";
            //}
            return null;
        }

        // 检查是否为合法的上传图片
        private static string checkImage(HttpPostedFileBase imgfile)
        {
            string allowExt = ".gif.jpg.png";
            string fileName = imgfile.FileName;
            FileInfo file = new FileInfo(fileName);
            string imgExt = file.Extension;
            if (allowExt.IndexOf(imgExt.ToLower()) == -1)
            {
                return "图片格式不正确（gif、jpg、png）";
            }
            try
            {
                Image img = Image.FromStream(imgfile.InputStream);
            }
            catch
            {
                return "图片格式不正确（gif、jpg、png）";
            }
            //if (imgfile.ContentLength > 512 * 1024)
            //{
            //    errorMsg += "图片最大限制为0.5Mb；";
            //}
            //if (img.Width < 20 || img.Width > 480 || img.Height < 20 || img.Height > 854)
            //{
            //    errorMsg += "请上传正确尺寸的图片，图片最小为20x20，最大为480*854。";
            //}
            return null;
        }
    }
}
