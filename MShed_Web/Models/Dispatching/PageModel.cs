using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.Dispatching
{
    public class PageModel
    {
        public PageModel(IPaneItem _PaneItem, IPageContentModel _PageContentModel)
        {
            this.PaneItem = _PaneItem;
            this.PageContentModel = _PageContentModel;
        }
        public IPaneItem PaneItem { get; set; }

        public IPageContentModel PageContentModel { get; set; }
    }
}