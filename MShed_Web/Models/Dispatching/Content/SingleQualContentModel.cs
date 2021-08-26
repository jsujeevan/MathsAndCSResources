using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.Dispatching.Content
{
    public class SingleQualContentModel : IPageContentModel
    {
         public Models.UIModel.Qual.QualEditModel Qualification{ get; set; }
         public string QualSubjectGridData { get; set; }
         public string QualSubjectGridColumns { get; set; }
    }
}