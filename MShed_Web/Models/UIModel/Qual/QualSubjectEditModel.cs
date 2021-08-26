using MShed_Web.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectEditModel
    {
        [DisplayName("ID")]
        [UIField(Hidden = true)]
        public Guid PK_QualSubject_ID_s { get; set; }

        [DisplayName("Qualification Subject")]
        [Required]
        [UIField(UIControl = UIControl.Text)]
        public string QualSubject_s { get; set; }

        [DisplayName("Description")]
        [Required]
        [UIField(UIControl = UIControl.TextArea)]
        public string QualSubject_Description_s { get; set; }

        [DisplayName("Qualification")]
        [Required]
        [UIField(Lookup = true, LookupObject = "MShed_Web.BOs.QualBO, MShed_Web", LookupMethod = "SearchQual", LookupFieldName = "PK_Qual_ID_s", LookupFieldValue = "Qual_s")]
        public Guid FK_Qual_ID_s { get; set; }

        [DisplayName("Subject")]
        [Required]
        [UIField(Lookup = true, LookupObject = "MShed_Web.BOs.QualBO, MShed_Web", LookupMethod = "SearchSubject", LookupFieldName = "PK_Subject_i", LookupFieldValue = "Subject_s")]
        public int FK_Subject_i { get; set; }
    }
}