using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.Models.UIModel.Person
{
    public class PersonModel
    {
        public DateTime Created_d { get; set; }

        public Guid Created_Person_ID_s { get; set; }

        public int FK_Person_Role_i { get; set; }

        public int FK_Person_Status_i { get; set; }

        public DateTime Modified_d { get; set; }

        public Guid Modified_Person_ID_s { get; set; }

        public bool Person_Archive_b { get; set; }

        public string Person_Email_s { get; set; }

        public string Person_Firstname_s { get; set; }

        public string Person_Lastname_s { get; set; }

        public string Person_Telephone_s { get; set; }

        public Guid PK_Person_ID_s { get; set; }

        public Guid FK_Session_Person_ID_s { get; set; }
    }
}