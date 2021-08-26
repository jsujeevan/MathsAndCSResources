using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.Dispatching
{
    public class GridClickThroughPaneItem : IPaneItem
    {
        public GridClickThroughPaneItem(string _PageTitle, string _PageHeading, string _GridData, string _GridColumns)
        {
            this.PageTitle = _PageTitle;
            this.PageHeading = _PageHeading;
            this.GridData = _GridData;
            this.GridColumns = _GridColumns;
        }
        public string PageTitle { get; set; }
        public string PageHeading { get; set; }
        public string GridData { get; set; }
        public string GridColumns { get; set; }
    }
}