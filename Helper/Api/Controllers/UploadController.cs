using Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    
    public class UploadController : ApiController
    {
        private DbEntity db = new DbEntity();

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

        [Route("Upload/{Type}")]
        public IHttpActionResult Post(string Type)
        {
            if (Type == null) return BadRequest("请指定照片种类");
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (files.Count == 0) return BadRequest("请上传文件");
            Models.Type type =db.Types.First(s => s.Name == Type);
            if (type == null) return BadRequest("不存在此照片种类");
            HttpPostedFile file = files[0];
            string directory = null;
            string extension = new FileInfo(file.FileName).Extension;
            string fileName = Type + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string result = null;
            directory = @"\Photos\"+ type.Name;
            result = UploadFileHelper.UploadImage(file, directory, fileName, extension);
            if (!string.IsNullOrEmpty(result)) return BadRequest(result);
            string url = (directory + "/" + fileName + extension).Replace("\\", "/");
            return Ok(url); 
        }
    }
}
