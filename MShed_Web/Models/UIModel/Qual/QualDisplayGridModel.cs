using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualDisplayGridModel
    {
        [DisplayName("ID")]
        public Guid PK_Qual_ID_s { get; set; }

        [DisplayName("Qualification")]
        public string Qual_s { get; set; }

        [DisplayName("Duration")]
        public string Qual_Duration_s { get; set; }

        [DisplayName("Age Group")]
        public string Qual_AgeGroup_s { get; set; }

    }
}