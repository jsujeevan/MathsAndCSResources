using MShed_Web.BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MShed_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SingleQualIndex(Guid id)
        {
            Helper helper_o = new Helper();
            QualBO qual = new QualBO();

            Dictionary<int, Models.DataModel.Qual.QualType_Model> qualTypeLookupList_k = new Dictionary<int, Models.DataModel.Qual.QualType_Model>();
            List<Models.DataModel.Qual.QualType_Model> qualTypeList_k = qual.SearchQualType(new Dictionary<string, object>());
            foreach (Models.DataModel.Qual.QualType_Model qualType_o in qualTypeList_k)
            {
                qualTypeLookupList_k.Add(qualType_o.PK_QualType_i, qualType_o);
            }

            Models.DataModel.Qual.Qual_Model qualModel_o = qual.GetQual(id);
            Models.UIModel.Qual.QualModel qualUIModel_o = (Models.UIModel.Qual.QualModel)helper_o.ConvertToType(qualModel_o, typeof(Models.DataModel.Qual.Qual_Model), typeof(Models.UIModel.Qual.QualModel));
            qualUIModel_o.QualType_s = qualTypeLookupList_k[qualModel_o.FK_Qual_Type_i].QualType_s;

            Dictionary<string, object> qualSubjectParamList_k = new Dictionary<string, object>();
            qualSubjectParamList_k.Add("FK_Qual_ID_s", id);
            List<Models.DataModel.Qual.QualSubject_Model> qualSubjectList_k = qual.SearchQualSubject(qualSubjectParamList_k);
            List<Models.UIModel.Qual.QualSubjectModel> qualSubjectUIList_k = new List<Models.UIModel.Qual.QualSubjectModel>();
            foreach (Models.DataModel.Qual.QualSubject_Model qualSubjectModel_o in qualSubjectList_k)
            {
                qualSubjectUIList_k.Add((Models.UIModel.Qual.QualSubjectModel)helper_o.ConvertToType(qualSubjectModel_o, typeof(Models.DataModel.Qual.QualSubject_Model), typeof(Models.UIModel.Qual.QualSubjectModel)));
            }
            ViewData["QualSubjectList"] = qualSubjectUIList_k;
            return View(qualUIModel_o);
        }

        public ActionResult SingleQualSubjectIndex(Guid id)
        {

            Helper helper_o = new Helper();
            QualBO qual = new QualBO();
            ResourceImpl resource = new ResourceImpl();
            Models.DataModel.Qual.QualSubject_Model qualSubject_Model_o = qual.GetQualSubject(id);
            Models.UIModel.Qual.QualSubjectModel qualSubject_o = (Models.UIModel.Qual.QualSubjectModel)helper_o.ConvertToType(qualSubject_Model_o,typeof(Models.DataModel.Qual.QualSubject_Model),typeof(Models.UIModel.Qual.QualSubjectModel));

            //Resource Type
            Dictionary<int, Models.DataModel.Resource.ResourceType_Model> resourceTypeLookupList_k = new Dictionary<int, Models.DataModel.Resource.ResourceType_Model>();
            List<Models.DataModel.Resource.ResourceType_Model> resourceTypeList_k = resource.SearchResourceType(new Dictionary<string, object>());
            foreach (Models.DataModel.Resource.ResourceType_Model tempResourceType_o in resourceTypeList_k)
            {
                resourceTypeLookupList_k.Add(tempResourceType_o.PK_ResourceType_i, tempResourceType_o);
            }

            //Supporting Resource Type
            Dictionary<int, Models.DataModel.Resource.ResourceSupportingType_Model> resourceSupportingTypeLookupList_k = new Dictionary<int, Models.DataModel.Resource.ResourceSupportingType_Model>();
            List<Models.DataModel.Resource.ResourceSupportingType_Model> resourceSupportingTypeList_k = resource.SearchResourceSupportingType(new Dictionary<string, object>());
            foreach (Models.DataModel.Resource.ResourceSupportingType_Model tempsupportingResourceType_o in resourceSupportingTypeList_k)
            {
                resourceSupportingTypeLookupList_k.Add(tempsupportingResourceType_o.PK_ResourceSupportingType_i, tempsupportingResourceType_o);
            }
            //Qual Subject Resource
            List<Guid> resourceIDList_k = new List<Guid>();
            Dictionary<string, object> qualSubjectResourceParamList_k = new Dictionary<string, object>();
            qualSubjectResourceParamList_k.Add("FK_QualSubject_ID_s", id);
            List<Models.DataModel.Qual.QualSubjectResource_Model> qualSubjectResourceList_o = qual.SearchQualSubjectResource(qualSubjectResourceParamList_k);
            foreach (Models.DataModel.Qual.QualSubjectResource_Model qualSubjectResourceModel_o in qualSubjectResourceList_o)
            {
                Models.UIModel.Qual.QualSubjectResourceModel qualSubjectResource_o = (Models.UIModel.Qual.QualSubjectResourceModel)helper_o.ConvertToType(qualSubjectResourceModel_o, typeof(Models.DataModel.Qual.QualSubjectResource_Model),typeof(Models.UIModel.Qual.QualSubjectResourceModel));
                resourceIDList_k.Add(qualSubjectResource_o.FK_Resource_ID_s);
            }

            //Resource
            List<Models.UIModel.Resource.Resource_Model> resourceUIList_k = new List<Models.UIModel.Resource.Resource_Model>();
            List<Models.DataModel.Resource.Resource_Model> resourceList_k = resource.GetMultiResource(resourceIDList_k);
            foreach (Models.DataModel.Resource.Resource_Model tempResource in resourceList_k)
            {
                Models.UIModel.Resource.Resource_Model qualSubjectResource_o = (Models.UIModel.Resource.Resource_Model)helper_o.ConvertToType(tempResource, typeof(Models.DataModel.Resource.Resource_Model), typeof(Models.UIModel.Resource.Resource_Model));
                resourceUIList_k.Add(qualSubjectResource_o);
            }

            //Supporing Resource
            List<Models.UIModel.Resource.ResourceSupporting_Model> supportingResourceUIList_k = new List<Models.UIModel.Resource.ResourceSupporting_Model>();
            List<Models.DataModel.Resource.ResourceSupporting_Model> multiSupportingResourceList_o = resource.GetMultiResourceSupportingByResource(resourceIDList_k);
            foreach (Models.DataModel.Resource.ResourceSupporting_Model supportingResourceModel_o in multiSupportingResourceList_o)
            {
                Models.UIModel.Resource.ResourceSupporting_Model qualSubjectSupporingResource_o = (Models.UIModel.Resource.ResourceSupporting_Model)helper_o.ConvertToType(supportingResourceModel_o, typeof(Models.DataModel.Resource.ResourceSupporting_Model), typeof(Models.UIModel.Resource.ResourceSupporting_Model));
                supportingResourceUIList_k.Add(qualSubjectSupporingResource_o);
            }

            ViewData["ResourceTypeList"] = resourceTypeLookupList_k;
            ViewData["SupportingResourceTypeList"] = resourceSupportingTypeLookupList_k;
            ViewData["ResourceList"] = resourceUIList_k;
            ViewData["SupportingResourceList"] = supportingResourceUIList_k;

            return View(qualSubject_o);
        }
    }
}