using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.BOs
{
    public class SupportBO
    {

        public void SetUserSession(Guid in_person_gu, Guid in_session_gu)
        {
            try
            {
                HttpContext.Current.Session["UserId_s"] = in_person_gu;
                HttpContext.Current.Session["SessionId_s"] = in_session_gu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid GetCurrentUserID()
        {
            try
            {
                return Guid.Parse(HttpContext.Current.Session["UserId_s"].ToString());
            }
            catch (Exception ex)
            {
                throw new SessionNotFoundException("Session user not found");
            }
        }

        public Models.UIModel.Person.PersonModel GetCurrentUser()
        {
            Models.DataModel.Person.Person_Model personModel_o = null;
            Models.UIModel.Person.PersonModel personUIModel_o = null;
            PersonBO person_o = new PersonBO();
            Helper helper_o = new Helper();
            try
            {
                Guid vl_person_gu = Guid.Parse(HttpContext.Current.Session["UserId_s"].ToString());
                personModel_o = person_o.GetPerson(vl_person_gu);
                personUIModel_o = (Models.UIModel.Person.PersonModel)helper_o.ConvertToType(personModel_o, typeof(Models.DataModel.Person.Person_Model),typeof(Models.UIModel.Person.PersonModel));

            }
            catch (Exception ex)
            {
                throw new SessionNotFoundException("Session user not found");
            }
            return personUIModel_o;
        }

        public Guid GetSystemUser()
        {
            String vl_systemuser_s = "";
            try
            {
                vl_systemuser_s = System.Configuration.ConfigurationManager.AppSettings["SystemUser"];
                return new Guid(vl_systemuser_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid GetCurrentSession()
        {
            try
            {
                return Guid.Parse(HttpContext.Current.Session["SessionId_s"].ToString());
            }
            catch (Exception ex)
            {
                throw new SessionNotFoundException("Session user not found");
            }
        }
    }
}