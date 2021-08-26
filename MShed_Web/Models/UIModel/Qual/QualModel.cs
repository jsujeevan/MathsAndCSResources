using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualModel
    {
        public Guid PK_Qual_ID_s { get; set; }
        public int FK_Qual_Type_i { get; set; }
        public string Qual_AgeGroup_s { get; set; }
        public string Qual_Description_s { get; set; }
        public string Qual_Duration_s { get; set; }
        public string QualType_s { get; set; }
        public string Qual_s { get; set; }

    }
}