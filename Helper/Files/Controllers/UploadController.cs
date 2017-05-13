using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Orp.Files.Controllers
{
    public class UploadController : ApiController
    {
        [Route("UploadImage")]
        public IHttpActionResult PostImage(string directory)
        {
            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count == 0) return BadRequest();
                HttpPostedFile file = files[0];
                string directoryPath = HttpContext.Current.Server.MapPath(directory);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                string path = directoryPath + "\\" + file.FileName; 
                file.SaveAs(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
