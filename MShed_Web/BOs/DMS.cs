using MShed_Web.Models.DataModel.Document;
using MShed_Web.Models.UIModel.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MShed_Web.BOs
{
    public class DMS
    {
        #region Fields[Private]

        private const string ConfigKey_s = "document.root";

        #endregion

        #region Methods[Public]

        public DocumentModel SaveDocument(String base64Doc_s, String fileName_s)
        {
            Guid _ID_s;
            _ID_s = Guid.NewGuid();
            return SaveDocumentWithID(base64Doc_s, fileName_s, _ID_s);
        }

        public DocumentModel SaveDocumentWithID(String base64Doc_s, String fileName_s, Guid _ID_s)
        {
            String rootDir_s = "";
            long fileSize_l = 0;
            String file_s = "";
            String extension_s = "";

            Document_Model newDocument_o = new Document_Model();
            SupportBO support_o = new SupportBO();
            Helper helper_o = new Helper();

            DateTime currentDate_d = DateTime.Now;
            String year_i = currentDate_d.Year.ToString();
            String month_i = currentDate_d.Month.ToString();
            String day_i = currentDate_d.Day.ToString();
            String hour_i = currentDate_d.Hour.ToString();
            String minute_i = currentDate_d.Minute.ToString();

            ApplicationBO application_o = new ApplicationBO();
            DocumentBO document_o = new DocumentBO();
            DocumentModel uiDocument_o = new DocumentModel();

            // Get the content store
            try
            {
                Models.DataModel.Application.Config_Model configModel_o = application_o.GetConfig(ConfigKey_s);

                rootDir_s = configModel_o.Config_Value_s;

                // Create the directory path if is doesn't exist
                System.IO.Directory.CreateDirectory(rootDir_s + "/" + year_i);
                System.IO.Directory.CreateDirectory(rootDir_s + "/" + year_i + "/" + month_i);
                System.IO.Directory.CreateDirectory(rootDir_s + "/" + year_i + "/" + month_i + "/" + day_i);
                System.IO.Directory.CreateDirectory(rootDir_s + "/" + year_i + "/" + month_i + "/" + day_i + "/" + hour_i);
                System.IO.Directory.CreateDirectory(rootDir_s + "/" + year_i + "/" + month_i + "/" + day_i + "/" + hour_i + "/" + minute_i);

                String filePath_s = rootDir_s + "/" + year_i + "/" + month_i + "/" + day_i + "/" + hour_i + "/" + minute_i + "/" + _ID_s.ToString() + ".bin";

                // Save File to directory
                var bytes = Convert.FromBase64String(base64Doc_s);
                using (var docFile = new FileStream(filePath_s, FileMode.Create))
                {
                    docFile.Write(bytes, 0, bytes.Length);
                    docFile.Flush();
                }

                // Get the length of the file
                FileInfo fileInfo = new FileInfo(filePath_s);
                fileSize_l = fileInfo.Length;

                int vl_Point_i = fileName_s.LastIndexOf(".");

                if (vl_Point_i >= 0)
                {
                    file_s = fileName_s.Substring(0, fileName_s.LastIndexOf("."));
                    extension_s = fileName_s.Substring(fileName_s.LastIndexOf(".") + 1);
                }
                else
                {
                    file_s = fileName_s;
                    extension_s = "";
                }

                // Create the Model
                newDocument_o.Document_Filename_s = file_s;
                newDocument_o.Document_Ext_s = extension_s;
                newDocument_o.PK_Document_ID_s = _ID_s;
                newDocument_o.Document_Location_s = "/" + year_i + "/" + month_i + "/" + day_i + "/" + hour_i + "/" + minute_i + "/";
                newDocument_o.Document_Size_s = (int)fileSize_l;
                newDocument_o.Created_d = currentDate_d;
                newDocument_o.Created_Person_ID_s = support_o.GetCurrentUserID();
                //Get content type
                newDocument_o.Document_ContentType_s = MimeMapping.GetMimeMapping(string.Concat(file_s, ".", extension_s));
                //Insert new document
                document_o.InsertDocument(newDocument_o);
                uiDocument_o = (Models.UIModel.Document.DocumentModel)helper_o.ConvertToType(newDocument_o, typeof(Models.DataModel.Document.Document_Model), typeof(Models.UIModel.Document.DocumentModel));
            }
            catch (Exception exception_o)
            {
                throw exception_o;
            }
            return uiDocument_o;
        }

        public DocumentModel getDocument(Guid _ID_s, Boolean includeFile_b)
        {
            String rootDir_s = "";
            Document_Model returnDocument_o;
            Models.UIModel.Document.DocumentModel uiDocument_o = new DocumentModel();
            DocumentBO document_o = new DocumentBO();
            ApplicationBO application_o = new ApplicationBO();
            Helper helper_o = new Helper();
            returnDocument_o = document_o.GetDocument(_ID_s);
            if (returnDocument_o.PK_Document_ID_s == _ID_s)
            {
                uiDocument_o = (Models.UIModel.Document.DocumentModel)helper_o.ConvertToType(returnDocument_o, typeof(Models.DataModel.Document.Document_Model), typeof(Models.UIModel.Document.DocumentModel));
                if (includeFile_b)
                {
                    Models.DataModel.Application.Config_Model configModel_o = application_o.GetConfig(ConfigKey_s);

                    rootDir_s = configModel_o.Config_Value_s;

                    Byte[] fileBytes_ba = File.ReadAllBytes(rootDir_s + uiDocument_o.Document_Location_s + _ID_s.ToString() + ".bin");

                    uiDocument_o.Document_s = Convert.ToBase64String(fileBytes_ba);
                }
            }
            return uiDocument_o;
        }

        #endregion
    }
}