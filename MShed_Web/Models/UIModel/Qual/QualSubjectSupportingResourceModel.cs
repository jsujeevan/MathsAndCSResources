using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectSupportingResourceModel
    {
        public DateTime Created_d { get; set; }

        public Guid Created_Person_ID_s { get; set; }

        public int FK_ContentType_i { get; set; }

        public Guid FK_Document_ID_s { get; set; }

        public Guid FK_QualSubjectResource_ID_s { get; set; }

        public int FK_QualSubjectSupportingResourceType_i { get; set; }

        public DateTime Modified_d { get; set; }

        public Guid Modified_Person_ID_s { get; set; }

        public Guid PK_QualSubjectSupportingResource_ID_s { get; set; }

        public bool QualSubjectSupportingResource_Archive_b { get; set; }

        public string QualSubjectSupportingResource_s { get; set; }

        public string QualSubjectSupportingResourceContent_s { get; set; }

        public Guid FK_Session_Person_ID_s { get; set; }
    }
}