using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Support
{
    public class UIField : Attribute
    {
        public bool Hidden { get; set; }
        public bool ReadOnly { get; set; }
        public UIControl UIControl { get; set; }
        public bool Lookup { get; set; }
        public string LookupObject { get; set; }
        public string LookupMethod { get; set; }
        public string LookupFieldName { get; set; }
        public string LookupFieldValue { get; set; }
    }

    public enum UIControl
    {
        Text = 1,
        TextArea = 2,
        DateTime = 3,
        CheckBox = 4,
    }
}