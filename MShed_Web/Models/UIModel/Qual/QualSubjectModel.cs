using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectModel
    {
        public DateTime Created_d { get; set; }

        public Guid Created_Person_ID_s { get; set; }

        public Guid FK_Qual_ID_s { get; set; }

        public int FK_Subject_i { get; set; }

        public DateTime Modified_d { get; set; }

        public Guid Modified_Person_ID_s { get; set; }

        public Guid PK_QualSubject_ID_s { get; set; }

        public bool QualSubject_Archive_b { get; set; }

        public string QualSubject_Description_s { get; set; }

        public string QualSubject_s { get; set; }

        public Guid FK_Session_Person_ID_s { get; set; }
    }
}