using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectDisplayGridModel
    {
        [DisplayName("Qualification Subject ID")]
        public Guid PK_QualSubject_ID_s { get; set; }

        [DisplayName("Qualification Subject")]
        public string QualSubject_s { get; set; }

        [DisplayName("Qualification")]
        public string Qual_s { get; set; }

        [DisplayName("Subject")]
        public string Subject_s { get; set; }

    }
}