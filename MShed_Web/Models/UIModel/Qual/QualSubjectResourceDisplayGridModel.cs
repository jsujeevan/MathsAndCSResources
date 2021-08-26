using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectResourceDisplayGridModel
    {
        [DisplayName("ID")]
        public Guid PK_QualSubjectResource_ID_s { get; set; }

        [DisplayName("Document ID")]
        public Guid FK_Document_ID_s { get; set; }

        [DisplayName("Name")]
        public string QualSubjectResource_s { get; set; }

        [DisplayName("Content")]
        public string QualSubjectResourceContent_s { get; set; }

        [DisplayName("Resource Type")]
        public string QualSubjectResourceType_s { get; set; }

        [DisplayName("Content Type")]
        public string ContentType_s { get; set; }

    }
}