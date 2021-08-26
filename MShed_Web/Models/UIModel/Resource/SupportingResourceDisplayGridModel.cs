using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Resource
{
    public class SupportingResourceDisplayGridModel
    {
        [DisplayName("ID")]
        public Guid PK_ResourceSupporting_ID_s { get; set; }

        [DisplayName("Document ID")]
        public Guid FK_Document_ID_s { get; set; }

        [DisplayName("Name")]
        public string Name_s { get; set; }

        [DisplayName("Content")]
        public string Content_s { get; set; }

        [DisplayName("Type")]
        public string ResourceSupportingType_s { get; set; }

        [DisplayName("Content Type")]
        public string ContentType_s { get; set; }
    }
}