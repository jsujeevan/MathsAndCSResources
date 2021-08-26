using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.Dispatching.Content
{
    public class SingleQualSubjectContentModel : IPageContentModel
    {
        public Models.UIModel.Qual.QualSubjectEditModel QualificationSubject { get; set; }
        public string ResourceGridData { get; set; }
        public string ResourceGridColumns { get; set; }

        public string SupportingResourceGridData { get; set; }
        public string SupportingResourceGridColumns { get; set; }
    }
}