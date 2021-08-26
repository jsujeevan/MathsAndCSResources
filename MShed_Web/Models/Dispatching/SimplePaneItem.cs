using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.Dispatching
{
    public class SimplePaneItem : IPaneItem
    {
        public SimplePaneItem(string _PageTitle, string _PageHeading)
        {
            this.PageTitle = _PageTitle;
            this.PageHeading = _PageHeading;
        }
        public string PageTitle { get; set; }
        public string PageHeading { get; set; }
    }
}