using MShed_Web.BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace MShed_Web.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Download(Guid id)
        {
            try
            {
                byte[] fileData_o = null;
                DMS vl_dms_o = new DMS();
                Models.UIModel.Document.DocumentModel documentModel_o = vl_dms_o.getDocument(id, true);
                if (!string.IsNullOrEmpty(documentModel_o.Document_s))
                {
                    fileData_o = Convert.FromBase64String(documentModel_o.Document_s);
                    ContentDisposition contentType_o = new ContentDisposition
                    {
                        FileName = documentModel_o.Document_Filename_s + "." + documentModel_o.Document_Ext_s,

                    };
                    Response.AppendHeader("Content-Disposition", contentType_o.ToString());
                    return File(fileData_o, contentType_o.ToString());
                }
                return File(fileData_o, "");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}