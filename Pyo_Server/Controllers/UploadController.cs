using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

namespace Pyo_Server.Controllers
{
    public class UploadController : ApiController
    {
        //public HttpResponseMessage PostFormData(List<HttpPostedFile> profileImages)
        public HttpResponseMessage PostFormData(HttpPostedFile profileImage)
        {
            //if (profileImages.Count > 0)
            if (profileImage != null)
            {
                //foreach (HttpPostedFile postedFile in profileImages)
                //{
                    //var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    //postedFile.SaveAs(filePath);
                    var filePath = HttpContext.Current.Server.MapPath("~/" + profileImage.FileName);
                    profileImage.SaveAs(filePath);
                // NOTE: To store in memory use postedFile.InputStream
                //}

                return Request.CreateResponse(HttpStatusCode.Created);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);

            /*
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    // NOTE: To store in memory use postedFile.InputStream
                }

                return Request.CreateResponse(HttpStatusCode.Created);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
            */
        }
        /*
        public Task<HttpResponseMessage> PostFormData()
        {
            
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/UploadedImages");
            var provider = new MultipartFormDataStreamProvider(root);

            // Read the form data and return an async task.
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
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
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                });

            return task;
        }
        */
    }
}
