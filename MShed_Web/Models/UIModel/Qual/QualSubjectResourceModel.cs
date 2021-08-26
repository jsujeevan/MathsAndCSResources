using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectResourceModel
    {

        public Guid FK_QualSubject_ID_s { get; set; }

        public Guid FK_Resource_ID_s { get; set; }

        public Guid PK_QualSubjectResource_ID_s { get; set; }

        public bool QualSubjectResource_Archive_b { get; set; }

        public Guid FK_Session_Person_ID_s { get; set; }
    }
}