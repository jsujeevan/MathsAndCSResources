using MShed_Web.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Resource
{
    public class ResourceEditModel
    {
        [DisplayName("Resource ID")]
        [UIField(Hidden = true)]
        public Guid PK_Resource_ID_s { get; set; }

        [DisplayName("Name")]
        [Required]
        [UIField(UIControl = UIControl.Text)]
        public string Name_s { get; set; }

        [DisplayName("Content")]
        [Required]
        [UIField(UIControl = UIControl.TextArea)]
        public string Content_s { get; set; }

        [DisplayName("Content Type")]
        [Required]
        [UIField(Lookup = true, LookupObject = "MShed_Web.BOs.ResourceBO, MShed_Web", LookupMethod = "SearchContentType", LookupFieldName = "PK_ContentType_i", LookupFieldValue = "ContentType_s")]
        public int FK_ContentType_i { get; set; }

        [DisplayName("Resource Type")]
        [Required]
        [UIField(Lookup = true, LookupObject = "MShed_Web.BOs.ResourceBO, MShed_Web", LookupMethod = "SearchResourceType", LookupFieldName = "PK_ResourceType_i", LookupFieldValue = "ResourceType_s")]
        public int FK_ResourceType_i { get; set; }
    }
}