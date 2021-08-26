using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Resource
{
    public class ResourceSupporting_Model
    {
        public string Content_s { get; set; }

        public DateTime Created_d { get; set; }

        public Guid Created_Person_ID_s { get; set; }

        public int FK_ContentType_i { get; set; }

        public Guid FK_Document_ID_s { get; set; }

        public Guid FK_Resource_ID_s { get; set; }

        public int FK_ResourceSupportingType_i { get; set; }

        public DateTime Modified_d { get; set; }

        public Guid Modified_Person_ID_s { get; set; }

        public string Name_s { get; set; }

        public Guid PK_ResourceSupporting_ID_s { get; set; }

        public bool ResourceSupporting_Archive_b { get; set; }

        public Guid FK_Session_Person_ID_s { get; set; }
    }
}