using MShed_Web.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualEditModel
    {
        [DisplayName("ID")]
        [UIField(Hidden = true)]
        public Guid PK_Qual_ID_s { get; set; }

        [DisplayName("Qualification")]
        [Required]
        [UIField(UIControl = UIControl.Text)]
        public string Qual_s { get; set; }

        [DisplayName("Description")]
        [Required]
        [UIField(UIControl = UIControl.TextArea)]
        public string Qual_Description_s { get; set; }

        [DisplayName("Qualification Type")]
        [Required]
        [UIField(Lookup = true, LookupObject = "MShed_Web.BOs.QualBO, MShed_Web", LookupMethod = "SearchQualType", LookupFieldName = "PK_QualType_i", LookupFieldValue = "QualType_s")]
        public int FK_Qual_Type_i { get; set; }

        [DisplayName("Age Group")]
        [Required]
        [UIField(UIControl = UIControl.Text)]
        public string Qual_AgeGroup_s { get; set; }

        [DisplayName("Duration")]
        [Required]
        [UIField(UIControl = UIControl.Text)]
        public string Qual_Duration_s { get; set; }
    }
}