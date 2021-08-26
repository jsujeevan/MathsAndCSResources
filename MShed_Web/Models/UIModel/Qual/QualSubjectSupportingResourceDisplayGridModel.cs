using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectSupportingResourceDisplayGridModel
    {
        [DisplayName("ID")]
        public Guid PK_QualSubjectSupportingResource_ID_s { get; set; }

        [DisplayName("Document ID")]
        public Guid FK_Document_ID_s { get; set; }

        [DisplayName("Name")]
        public string QualSubjectSupportingResource_s { get; set; }

        [DisplayName("Content")]
        public string QualSubjectSupportingResourceContent_s { get; set; }

        [DisplayName("Resource Name")]
        public string QualSubjectResource_s { get; set; }

        [DisplayName("Type")]
        public string QualSubjectSupportingResourceType_s { get; set; }

        [DisplayName("Content Type")]
        public string ContentType_s { get; set; }

    }
}