using MShed_Web.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Qual
{
    public class QualSubjectResourceAddModel
    {
        [DisplayName("Qual Subject Resource ID")]
        [UIField(Hidden = true)]
        public Guid PK_QualSubjectResource_ID_s { get; set; }

        [DisplayName("Qual Subject ID")]
        [UIField(Hidden = true)]
        public Guid FK_QualSubject_ID_s { get; set; }

        [DisplayName("Name")]
        [Required]
        [UIField(UIControl = UIControl.Text)]
        public string QualSubjectResource_s { get; set; }

        [DisplayName("Content")]
        [Required]
        [UIField(UIControl = UIControl.TextArea)]
        public string QualSubjectResourceContent_s { get; set; }

        [DisplayName("Content Type")]
        [Required]
        [UIField(Lookup = true, LookupObject = "MShed_Web.BOs.QualBO, MShed_Web", LookupMethod = "SearchContentType", LookupFieldName = "PK_ContentType_i", LookupFieldValue = "ContentType_s")]
        public int FK_ContentType_i { get; set; }

        [DisplayName("Resource Type")]
        [Required]
        [UIField(Lookup = true, LookupObject = "MShed_Web.BOs.QualBO, MShed_Web", LookupMethod = "SearchQualSubjectResourceType", LookupFieldName = "PK_QualSubjectResourceType_i", LookupFieldValue = "QualSubjectResourceType_s")]
        public int FK_QualSubjectResourceType_i { get; set; }
    }
}