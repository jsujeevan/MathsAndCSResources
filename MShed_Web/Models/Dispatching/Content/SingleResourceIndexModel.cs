using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.Dispatching.Content
{
    public class SingleResourceIndexModel : IPageContentModel
    {
        public Models.UIModel.Resource.ResourceEditModel Resource { get; set; }
        public string SupportingResourceGridData { get; set; }
        public string SupportingResourceGridColumns { get; set; }
    }
}