using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Document
{
    public class DocumentModel
    {
        public DateTime Created_d { get; set; }

        public Guid Created_Person_ID_s { get; set; }

        public string Document_ContentType_s { get; set; }

        public string Document_Description_s { get; set; }

        public string Document_Ext_s { get; set; }

        public string Document_Filename_s { get; set; }

        public string Document_Location_s { get; set; }

        public int Document_Size_s { get; set; }

        public DateTime Modified_d { get; set; }

        public Guid Modified_Person_ID_s { get; set; }

        public Guid PK_Document_ID_s { get; set; }

        public Guid FK_Session_Person_ID_s { get; set; }

        public string Document_s { get; set; }
    }
}