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
using System.Data.Entity;

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
            ParsedTable parTable = new ParsedTable();
            parTable.fk_User = pk;
            parTable.filename = file.LocalFileName;
            parTable.time = DateTime.Now;
            parTable.isProccessed = false;
            parTable.result = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.ParsedTables.Add(parTable);
                db.SaveChanges();
            }
            Task.Run(() =>
            {
                Trace.WriteLine("Task of AnalyzeImage starts!!");
                try
                {
                    String result = PyoCore.PyoCore.ProcessPngImage(file.LocalFileName);
                    parTable.result = result;
                }
                catch (PyoCoreException)
                {
                }
                catch (Exception)
                {
                }
                finally
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        Trace.WriteLine("** let's db!!");
                        parTable.isProccessed = true;
                        db.Entry(parTable).State = EntityState.Modified;
                        db.SaveChanges();
                    }
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
