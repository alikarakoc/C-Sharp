using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace rhApi.Controllers
{
    public class ImageController : ApiController
    {
        public class sonuc
        {
            public string path { get; set; }
        }
        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                string fileName = Path.ChangeExtension(
                    Path.GetRandomFileName(),
                    ".jpg"
                );
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                string root = HttpContext.Current.Server.MapPath("~/Uploads");
                var provider = new MultipartFormDataStreamProvider(root);
                try
                {
                    await Request.Content.ReadAsMultipartAsync(provider);
                    var tempImageName = provider.FileData[0].LocalFileName;
                    //var formData = provider.FormData["form-data"].ToString();
                    File.Copy(tempImageName, HttpContext.Current.Server.MapPath("~/Uploads/" + fileName + ""));
                    File.Delete(tempImageName);
                    #region İHTİYAÇ DURUMUNDA KULLANILABİLİR...
                    //foreach (MultipartFileData file in provider.FileData)
                    //{
                    //Trace.WriteLine(file.Headers.ContentDisposition.FileName); gibi...
                    //}
                    #endregion İHTİYAÇ DURUMUNDA KULLANILABİLİR...
                    sonuc sonuc = new sonuc()
                    {
                        path = "~/Uploads/" + fileName + ""
                    };

                    return Request.CreateResponse(sonuc);
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
