using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShed_Web.Models.Dispatching
{
    public interface IPaneItem
    {
        string PageTitle { get; set; }

        string PageHeading { get; set; }
    }
}
