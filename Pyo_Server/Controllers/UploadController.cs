using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Pyo_Server.Extensions;
using System.Web;
using System.Net.Http.Headers;
using System.IO;

namespace Pyo_Server.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        public Task<HttpResponseMessage> PostFormData(String pk)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\UploadedImages\" + pk;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string root = HttpContext.Current.Server.MapPath("~/App_Data/UploadedImages/" + pk);
            var provider = new CustomMultipartFormDataStreamProvider(root);

            // Read the form data and return an async task.
            var task = Request.Content.ReadAsMultipartAsync(provider).ContinueWith<HttpResponseMessage>(t =>
            {
                 if (t.IsFaulted || t.IsCanceled)
                {
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                }

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    AnalyzeImage(file.LocalFileName);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            });

            return task;
        }

        private static async Task AnalyzeImage(String localFileName)
        {

        }
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }
}
