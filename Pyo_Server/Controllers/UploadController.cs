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
using System.Runtime.InteropServices;
using Pyo_Server.Models;
using PyoCore;

namespace Pyo_Server.Controllers
{
    public class UploadController : ApiController
    {
        private static int DateTimeToInt(DateTime theDate)
        {
            return (int)(theDate.Date - new DateTime(1900, 1, 1)).TotalDays + 2;
        }

        private static DateTime IntToDateTime(int intDate)
        {
            return new DateTime(1900, 1, 1).AddDays(intDate - 2);
        }

        // pk : fk_User
        //[TODO] PyoCore.dll의 내제된 parser 함수를 이용해 분석을 돌린 후, db에 알맞게 저장.
        private static void AnalyzeImage(String pk, MultipartFileData file)
        {
            CapturedImage capImage = new CapturedImage();
            capImage.filename = file.LocalFileName;
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.CapturedImages.Add(capImage);
                db.SaveChanges();
            }

            Task.Run(() =>
            {
                try
                {
                    Trace.WriteLine("Task of AnalyzeImage starts!!");
                    String result = PyoCore.PyoCore.ProcessPngImage(file.LocalFileName);
                    ParsedTable parTable = new ParsedTable();
                    parTable.fk_User = pk;
                    parTable.filename = result;
                    parTable.time = DateTimeToInt(DateTime.Now);
                    parTable.fk_CapturedImages = capImage.pk;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        Trace.WriteLine("** let's db!!");
                        db.ParsedTables.Add(parTable);
                        db.SaveChanges();
                    }
                    Trace.WriteLine("Task of AnalyzeImage success!!");
                }
                catch (PyoCoreException pe)
                {
                    Trace.WriteLine("PyoCore process error : " + pe.getErrorCode());
                    ParsedTable parTable = new ParsedTable();
                    parTable.fk_User = pk;
                    parTable.filename = null;
                    parTable.time = DateTimeToInt(DateTime.Now);
                    parTable.fk_CapturedImages = capImage.pk;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.ParsedTables.Add(parTable);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    Trace.WriteLine("mistery error : ");
                    ParsedTable parTable = new ParsedTable();
                    parTable.fk_User = pk;
                    parTable.filename = null;
                    parTable.time = DateTimeToInt(DateTime.Now);
                    parTable.fk_CapturedImages = capImage.pk;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.ParsedTables.Add(parTable);
                        db.SaveChanges();
                    }
                }
                finally
                {
                    Trace.WriteLine("Taeguk's Task finish. pk = " + pk + ", file = " + file.LocalFileName);
                }
            });


        }

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
                    AnalyzeImage(pk, file);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            });

            return task;
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
