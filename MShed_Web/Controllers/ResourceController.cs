using MShed_Web.BOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MShed_Web.Controllers
{
    public class ResourceController : Controller
    {
        // GET: Resource
        public ActionResult QualIndex()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Application", new { redirectUrl = Request.RawUrl });
            }
            Helper helper_o = new Helper();
            QualBO qualBO_o = new QualBO();
            List<object> gridDataList_o = new List<object>();
            List<Models.DataModel.Qual.Qual_Model> qualList_k = qualBO_o.SearchQual(new Dictionary<string, object>());
            foreach (Models.DataModel.Qual.Qual_Model qual_o in qualList_k)
            {
                Models.UIModel.Qual.QualDisplayGridModel qualDisplayGrid_o = (Models.UIModel.Qual.QualDisplayGridModel)helper_o.ConvertToType(qual_o, typeof(Models.DataModel.Qual.Qual_Model),typeof(Models.UIModel.Qual.QualDisplayGridModel));
                gridDataList_o.Add(qualDisplayGrid_o);
            }
            string gridData_s = helper_o.ConvertListToGridData(gridDataList_o, typeof(Models.UIModel.Qual.QualDisplayGridModel));
            string gridColumns_s = helper_o.ConvertModelToGridColumns(typeof(Models.UIModel.Qual.QualDisplayGridModel));

            ViewData["GridData"] = gridData_s;
            ViewData["GridColumns"] = gridColumns_s;
            Models.Dispatching.PageModel pageModel = new Models.Dispatching.PageModel(new Models.Dispatching.GridClickThroughPaneItem("Qualifications","Qualifications", gridData_s, gridColumns_s),null);
            return View(pageModel);
        }

        public ActionResult SingleQualIndex(Guid id)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Application", new { redirectUrl = Request.RawUrl });
            }
            Helper helper_o = new Helper();
            QualBO qualBO_o = new QualBO();
            List<object> gridDataList_o = new List<object>();

            Dictionary<int, Models.DataModel.Qual.Subject_Model> subjectLookup_k = new Dictionary<int, Models.DataModel.Qual.Subject_Model>();
            List<Models.DataModel.Qual.Subject_Model> subjectList_k = qualBO_o.SearchSubject(new Dictionary<string, object>());
            foreach (Models.DataModel.Qual.Subject_Model subject_o in subjectList_k)
            {
                subjectLookup_k.Add(subject_o.PK_Subject_i, subject_o);
            }

            Dictionary<Guid, Models.DataModel.Qual.Qual_Model> qualLookup_k = new Dictionary<Guid, Models.DataModel.Qual.Qual_Model>();
            List<Models.DataModel.Qual.Qual_Model> qualList_k = qualBO_o.SearchQual(new Dictionary<string, object>());
            foreach (Models.DataModel.Qual.Qual_Model qual_o in qualList_k)
            {
                qualLookup_k.Add(qual_o.PK_Qual_ID_s, qual_o);
            }

            Models.DataModel.Qual.Qual_Model qualModel_o = qualBO_o.GetQual(id);
            Models.UIModel.Qual.QualEditModel qualUIModel_o = (Models.UIModel.Qual.QualEditModel)helper_o.ConvertToType(qualModel_o, typeof(Models.DataModel.Qual.Qual_Model), typeof(Models.UIModel.Qual.QualEditModel));

            Dictionary<string, object> qualSubjectParamList_k = new Dictionary<string, object>();
            qualSubjectParamList_k.Add("FK_Qual_ID_s", id);
            List<Models.DataModel.Qual.QualSubject_Model> qualSubjectList_k = qualBO_o.SearchQualSubject(qualSubjectParamList_k);

            foreach (Models.DataModel.Qual.QualSubject_Model qualSubject_o in qualSubjectList_k)
            {
                Models.UIModel.Qual.QualSubjectDisplayGridModel qualSubjectDisplayGrid_o = (Models.UIModel.Qual.QualSubjectDisplayGridModel)helper_o.ConvertToType(qualSubject_o, typeof(Models.DataModel.Qual.QualSubject_Model), typeof(Models.UIModel.Qual.QualSubjectDisplayGridModel));
                qualSubjectDisplayGrid_o.Qual_s = qualLookup_k[qualSubject_o.FK_Qual_ID_s].Qual_s;
                qualSubjectDisplayGrid_o.Subject_s = subjectLookup_k[qualSubject_o.FK_Subject_i].Subject_s;
                gridDataList_o.Add(qualSubjectDisplayGrid_o);
            }
            string gridData_s = helper_o.ConvertListToGridData(gridDataList_o, typeof(Models.UIModel.Qual.QualSubjectDisplayGridModel));
            string gridColumns_s = helper_o.ConvertModelToGridColumns(typeof(Models.UIModel.Qual.QualSubjectDisplayGridModel));

            Models.Dispatching.Content.SingleQualContentModel singleQualIndexModel_o = new Models.Dispatching.Content.SingleQualContentModel();
            singleQualIndexModel_o.Qualification = qualUIModel_o;
            singleQualIndexModel_o.QualSubjectGridColumns = gridColumns_s;
            singleQualIndexModel_o.QualSubjectGridData = gridData_s;
            Models.Dispatching.SimplePaneItem simplePaneItem_o = new Models.Dispatching.SimplePaneItem(qualUIModel_o.Qual_s, qualUIModel_o.Qual_s);
            Models.Dispatching.PageModel pageModel_o = new Models.Dispatching.PageModel(simplePaneItem_o, singleQualIndexModel_o);
            return View(pageModel_o);
        }

        [HttpPost]
        public ActionResult SingleQualEdit(Models.UIModel.Qual.QualEditModel qualEditModel_o)
        {
            try
            {
                string errorText_s = "";
                bool validationError_b = false;
                if (!ModelState.IsValid)
                {
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            validationError_b = true;
                            errorText_s += error.ErrorMessage + "<br />";
                        }
                    }
                }

                if (validationError_b)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.SeeOther;
                    ContentResult return_o = new ContentResult();
                    return_o.Content = errorText_s;
                    return return_o;
                }
                else
                {
                    QualBO qualBO_o = new QualBO();
                    SupportBO support_o = new SupportBO();
                    Helper helper_o = new Helper();
                    Models.DataModel.Qual.Qual_Model qualModel_o = (Models.DataModel.Qual.Qual_Model)helper_o.ConvertToType(qualEditModel_o, typeof(Models.UIModel.Qual.QualEditModel), typeof(Models.DataModel.Qual.Qual_Model));
                    qualModel_o.FK_Session_Person_ID_s = support_o.GetCurrentUserID();
                    qualBO_o.UpdateQual(qualModel_o);
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    ContentResult return_o = new ContentResult();
                    return return_o;
                }

            }
            catch (Exception e)
            {
                Response.TrySkipIisCustomErrors = true;
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ContentResult return_o = new ContentResult();
                return_o.Content = e.Message;
                return return_o;
            }

        }

        public ActionResult QualSubjectIndex()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Application", new { redirectUrl = Request.RawUrl });
            }
            Helper helper_o = new Helper();
            QualBO qualBO_o = new QualBO();
            List<object> gridDataList_o = new List<object>();

            Dictionary<int, Models.DataModel.Qual.Subject_Model> subjectLookup_k = new Dictionary<int, Models.DataModel.Qual.Subject_Model>();
            List<Models.DataModel.Qual.Subject_Model> subjectList_k = qualBO_o.SearchSubject(new Dictionary<string, object>());
            foreach (Models.DataModel.Qual.Subject_Model subject_o in subjectList_k)
            {
                subjectLookup_k.Add(subject_o.PK_Subject_i, subject_o);
            }

            Dictionary<Guid, Models.DataModel.Qual.Qual_Model> qualLookup_k = new Dictionary<Guid, Models.DataModel.Qual.Qual_Model>();
            List<Models.DataModel.Qual.Qual_Model> qualList_k = qualBO_o.SearchQual(new Dictionary<string, object>());
            foreach (Models.DataModel.Qual.Qual_Model qual_o in qualList_k)
            {
                qualLookup_k.Add(qual_o.PK_Qual_ID_s, qual_o);
            }

            List<Models.DataModel.Qual.QualSubject_Model> qualSubjectList_k = qualBO_o.SearchQualSubject(new Dictionary<string, object>());
            foreach (Models.DataModel.Qual.QualSubject_Model qualSubject_o in qualSubjectList_k)
            {
                Models.UIModel.Qual.QualSubjectDisplayGridModel qualSubjectDisplayGrid_o = (Models.UIModel.Qual.QualSubjectDisplayGridModel)helper_o.ConvertToType(qualSubject_o, typeof(Models.DataModel.Qual.QualSubject_Model), typeof(Models.UIModel.Qual.QualSubjectDisplayGridModel));
                qualSubjectDisplayGrid_o.Qual_s = qualLookup_k[qualSubject_o.FK_Qual_ID_s].Qual_s;
                qualSubjectDisplayGrid_o.Subject_s = subjectLookup_k[qualSubject_o.FK_Subject_i].Subject_s;
                gridDataList_o.Add(qualSubjectDisplayGrid_o);
            }
            string gridData_s = helper_o.ConvertListToGridData(gridDataList_o, typeof(Models.UIModel.Qual.QualSubjectDisplayGridModel));
            string gridColumns_s = helper_o.ConvertModelToGridColumns(typeof(Models.UIModel.Qual.QualSubjectDisplayGridModel));

            ViewData["GridData"] = gridData_s;
            ViewData["GridColumns"] = gridColumns_s;
            Models.Dispatching.PageModel pageModel = new Models.Dispatching.PageModel(new Models.Dispatching.GridClickThroughPaneItem("Qualification Subjects", "Qualification Subjects", gridData_s, gridColumns_s), null);
            return View(pageModel);
        }



        public ActionResult SingleQualSubjectIndex(Guid id)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Application", new { redirectUrl = Request.RawUrl });
            }
            Helper helper_o = new Helper();
            QualBO qualBO_o = new QualBO();
            ResourceImpl resource = new ResourceImpl();
            List<object> gridDataList_o = new List<object>();
            List<object> gridSupportingDataList_o = new List<object>();

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

            Dictionary<int, Models.DataModel.Resource.ContentType_Model> contentTypeLookup_k = new Dictionary<int, Models.DataModel.Resource.ContentType_Model>();
            List<Models.DataModel.Resource.ContentType_Model> contentTypeList_k = resource.SearchContentType(new Dictionary<string, object>());
            foreach (Models.DataModel.Resource.ContentType_Model contentType_o in contentTypeList_k)
            {
                contentTypeLookup_k.Add(contentType_o.PK_ContentType_i, contentType_o);
            }

            Models.DataModel.Qual.QualSubject_Model qualSubjectModel_o = qualBO_o.GetQualSubject(id);
            Models.UIModel.Qual.QualSubjectEditModel qualSubjectEditModel_o = (Models.UIModel.Qual.QualSubjectEditModel)helper_o.ConvertToType(qualSubjectModel_o,typeof(Models.DataModel.Qual.QualSubject_Model),typeof(Models.UIModel.Qual.QualSubjectEditModel));

            //Qual Subject Resource
            List<Guid> resourceIDList_k = new List<Guid>();
            Dictionary<string, object> qualSubjectResourceParamList_k = new Dictionary<string, object>();
            qualSubjectResourceParamList_k.Add("FK_QualSubject_ID_s", id);
            List<Models.DataModel.Qual.QualSubjectResource_Model> qualSubjectResourceList_o = qualBO_o.SearchQualSubjectResource(qualSubjectResourceParamList_k);
            foreach (Models.DataModel.Qual.QualSubjectResource_Model qualSubjectResourceModel_o in qualSubjectResourceList_o)
            {
                Models.UIModel.Qual.QualSubjectResourceModel qualSubjectResource_o = (Models.UIModel.Qual.QualSubjectResourceModel)helper_o.ConvertToType(qualSubjectResourceModel_o, typeof(Models.DataModel.Qual.QualSubjectResource_Model), typeof(Models.UIModel.Qual.QualSubjectResourceModel));
                resourceIDList_k.Add(qualSubjectResource_o.FK_Resource_ID_s);
            }

            List<Models.UIModel.Resource.Resource_Model> resourceUIList_k = new List<Models.UIModel.Resource.Resource_Model>();
            List<Models.DataModel.Resource.Resource_Model> resourceList_k = resource.GetMultiResource(resourceIDList_k);
            foreach (Models.DataModel.Resource.Resource_Model tempResource in resourceList_k)
            {
                Models.UIModel.Resource.ResourceDisplayGridModel resourceDisplayGrid_o = (Models.UIModel.Resource.ResourceDisplayGridModel)helper_o.ConvertToType(tempResource, typeof(Models.DataModel.Resource.Resource_Model), typeof(Models.UIModel.Resource.ResourceDisplayGridModel));
                resourceDisplayGrid_o.ResourceType_s = resourceTypeLookupList_k[tempResource.FK_ResourceType_i].ResourceType_s;
                resourceDisplayGrid_o.ContentType_s = contentTypeLookup_k[tempResource.FK_ContentType_i].ContentType_s;
                gridDataList_o.Add(resourceDisplayGrid_o);
            }

            string gridData_s = helper_o.ConvertListToGridData(gridDataList_o, typeof(Models.UIModel.Resource.ResourceDisplayGridModel));
            string gridColumns_s = helper_o.ConvertModelToGridColumns(typeof(Models.UIModel.Resource.ResourceDisplayGridModel));


            Models.Dispatching.Content.SingleQualSubjectContentModel singleQualSubjectContentModel_o = new Models.Dispatching.Content.SingleQualSubjectContentModel();
            singleQualSubjectContentModel_o.QualificationSubject = qualSubjectEditModel_o;
            singleQualSubjectContentModel_o.ResourceGridData = gridData_s;
            singleQualSubjectContentModel_o.ResourceGridColumns = gridColumns_s;
            Models.Dispatching.PageModel pageModel_o = new Models.Dispatching.PageModel(new Models.Dispatching.SimplePaneItem(qualSubjectEditModel_o.QualSubject_s, qualSubjectEditModel_o.QualSubject_s), singleQualSubjectContentModel_o);
            return View(pageModel_o);
        }

        [HttpPost]
        public ActionResult SingleQualSubjectEdit(Models.UIModel.Qual.QualSubjectEditModel qualSubjectEditModel_o)
        {
            try
            {
                string errorText_s = "";
                bool validationError_b = false;
                if (!ModelState.IsValid)
                {
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            validationError_b = true;
                            errorText_s += error.ErrorMessage + "<br />";
                        }
                    }
                }

                if (validationError_b)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.SeeOther;
                    ContentResult return_o = new ContentResult();
                    return_o.Content = errorText_s;
                    return return_o;
                }
                else
                {
                    QualBO qualBO_o = new QualBO();
                    SupportBO support_o = new SupportBO();
                    Helper helper_o = new Helper();
                    Models.DataModel.Qual.QualSubject_Model qualSubjectModel_o = (Models.DataModel.Qual.QualSubject_Model)helper_o.ConvertToType(qualSubjectEditModel_o, typeof(Models.UIModel.Qual.QualSubjectEditModel), typeof(Models.DataModel.Qual.QualSubject_Model));
                    qualSubjectModel_o.FK_Session_Person_ID_s = support_o.GetCurrentUserID();
                    qualBO_o.UpdateQualSubject(qualSubjectModel_o);
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    ContentResult return_o = new ContentResult();
                    return return_o;
                }

            }
            catch (Exception e)
            {
                Response.TrySkipIisCustomErrors = true;
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ContentResult return_o = new ContentResult();
                return_o.Content = e.Message;
                return return_o;
            }
        
        }

        public ActionResult SingleQualSubjectAdd(String modal, Guid id)
        {
            ViewData["mainHeading"] = "Edit Enquiry";
            ViewData["modalDialog"] = modal;
            return View();
        }

        
        public ActionResult SingleQualSubjectResourceAdd(String modal, Guid id)
        {
            Models.UIModel.Resource.ResourceAddModel resourceAddModel_o = new Models.UIModel.Resource.ResourceAddModel();
            ViewData["QualSubjectID"] = id;
            return View(resourceAddModel_o);
        }


        [HttpPost]
        public ActionResult SingleQualSubjectResourceAdd(Models.UIModel.Resource.ResourceAddModel resourceAddModel_o, string id)
        {
            try
            {
                bool vl_FileOK_z = false;
                bool vl_SizeOK_s = false;


                if (Request.Files[0].FileName != "")
                {
                    vl_FileOK_z = true;
                }

                string vl_maxFileSize_s = "4";
                int vl_filesize_i = Request.Files[0].ContentLength;
                double vl_filesizeinmb_d = (double)vl_filesize_i / 1000000;

                if (vl_filesizeinmb_d <= Convert.ToDouble(vl_maxFileSize_s))
                {
                    vl_SizeOK_s = true;
                }

                if (!vl_FileOK_z || !vl_SizeOK_s)
                {
                    string vl_ErrorText_s = "Validation Errors<br />";

                    if (!vl_FileOK_z) { vl_ErrorText_s += "File must be selected<br />"; };
                    if (!vl_SizeOK_s) { vl_ErrorText_s += "File size must be lesser than " + vl_maxFileSize_s + " Mb<br />"; };

                    HttpContext.Response.StatusCode = (int)HttpStatusCode.SeeOther;
                    ContentResult vl_ReturnError_o = new ContentResult();
                    vl_ReturnError_o.Content = vl_ErrorText_s;
                    return vl_ReturnError_o;
                }

                DMS vl_dms_o = new DMS();
                Helper helper_o = new Helper();
                QualBO qualBO_o = new QualBO();
                ResourceBO resourceBO = new ResourceBO();
                SupportBO supportBO_o = new SupportBO();
                DocumentBO vl_DocumentBO_o = new DocumentBO();

                HttpPostedFileBase vl_postedFile_o = HttpContext.Request.Files[0];
                string vl_Filename_s = Path.GetFileName(vl_postedFile_o.FileName);

                byte[] vl_fileData_b = null;
                using (System.IO.BinaryReader vl_InputReader_str = new System.IO.BinaryReader(Request.Files[0].InputStream))
                {
                    vl_fileData_b = vl_InputReader_str.ReadBytes(Request.Files[0].ContentLength);
                }

                string vl_Base64EncodedFile_s = Convert.ToBase64String(vl_fileData_b); ;

                Models.UIModel.Document.DocumentModel savedDocument_o = vl_dms_o.SaveDocument(vl_Base64EncodedFile_s, vl_Filename_s);

                Models.DataModel.Resource.Resource_Model resourceModel_o = (Models.DataModel.Resource.Resource_Model)helper_o.ConvertToType(resourceAddModel_o, typeof(Models.UIModel.Resource.ResourceAddModel), typeof(Models.DataModel.Resource.Resource_Model));
                resourceModel_o.PK_Resource_ID_s = Guid.NewGuid();
                resourceModel_o.FK_Document_ID_s = savedDocument_o.PK_Document_ID_s;
                resourceModel_o.FK_Session_Person_ID_s = supportBO_o.GetCurrentUserID();
                resourceBO.InsertResource(resourceModel_o);

                Models.DataModel.Qual.QualSubjectResource_Model qualSubjectResource_o = new Models.DataModel.Qual.QualSubjectResource_Model();
                qualSubjectResource_o.PK_QualSubjectResource_ID_s = Guid.NewGuid();
                qualSubjectResource_o.FK_QualSubject_ID_s = new Guid(id);
                qualSubjectResource_o.FK_Resource_ID_s = resourceModel_o.PK_Resource_ID_s;
                qualSubjectResource_o.FK_Session_Person_ID_s = supportBO_o.GetCurrentUserID();
                qualBO_o.InsertQualSubjectResource(qualSubjectResource_o);

                Dictionary<int, Models.DataModel.Resource.ResourceType_Model> qualSubjectResourceTypeLookup_k = new Dictionary<int, Models.DataModel.Resource.ResourceType_Model>();
                List<Models.DataModel.Resource.ResourceType_Model> qualSubjectResourceTypeList_k = resourceBO.SearchResourceType(new Dictionary<string, object>());
                foreach (Models.DataModel.Resource.ResourceType_Model qualSubjectResourceType_o in qualSubjectResourceTypeList_k)
                {
                    qualSubjectResourceTypeLookup_k.Add(qualSubjectResourceType_o.PK_ResourceType_i, qualSubjectResourceType_o);
                }

                Dictionary<int, Models.DataModel.Resource.ContentType_Model> contentTypeLookup_k = new Dictionary<int, Models.DataModel.Resource.ContentType_Model>();
                List<Models.DataModel.Resource.ContentType_Model> contentTypeList_k = resourceBO.SearchContentType(new Dictionary<string, object>());
                foreach (Models.DataModel.Resource.ContentType_Model contentType_o in contentTypeList_k)
                {
                    contentTypeLookup_k.Add(contentType_o.PK_ContentType_i, contentType_o);
                }

                Models.DataModel.Qual.QualSubject_Model qualSubjectModel_o = qualBO_o.GetQualSubject(new Guid(id));
                Models.UIModel.Qual.QualSubjectEditModel qualSubjectEditModel_o = (Models.UIModel.Qual.QualSubjectEditModel)helper_o.ConvertToType(qualSubjectModel_o, typeof(Models.DataModel.Qual.QualSubject_Model), typeof(Models.UIModel.Qual.QualSubjectEditModel));

                //Qual Subject Resource
                List<Guid> resourceIDList_k = new List<Guid>();
                Dictionary<string, object> qualSubjectResourceParamList_k = new Dictionary<string, object>();
                qualSubjectResourceParamList_k.Add("FK_QualSubject_ID_s", new Guid(id));
                List<Models.DataModel.Qual.QualSubjectResource_Model> qualSubjectResourceList_o = qualBO_o.SearchQualSubjectResource(qualSubjectResourceParamList_k);
                foreach (Models.DataModel.Qual.QualSubjectResource_Model qualSubjectResourceModel_o in qualSubjectResourceList_o)
                {
                    Models.UIModel.Qual.QualSubjectResourceModel tempQualSubjectResource_o = (Models.UIModel.Qual.QualSubjectResourceModel)helper_o.ConvertToType(qualSubjectResourceModel_o, typeof(Models.DataModel.Qual.QualSubjectResource_Model), typeof(Models.UIModel.Qual.QualSubjectResourceModel));
                    resourceIDList_k.Add(tempQualSubjectResource_o.FK_Resource_ID_s);
                }

                List<object> gridDataList_o = new List<object>();
                List<Models.UIModel.Resource.Resource_Model> resourceUIList_k = new List<Models.UIModel.Resource.Resource_Model>();
                List<Models.DataModel.Resource.Resource_Model> resourceList_k = resourceBO.GetMultiResource(resourceIDList_k);
                foreach (Models.DataModel.Resource.Resource_Model tempResource in resourceList_k)
                {
                    Models.UIModel.Resource.ResourceDisplayGridModel resourceDisplayGrid_o = (Models.UIModel.Resource.ResourceDisplayGridModel)helper_o.ConvertToType(tempResource, typeof(Models.DataModel.Resource.Resource_Model), typeof(Models.UIModel.Resource.ResourceDisplayGridModel));
                    resourceDisplayGrid_o.ResourceType_s = qualSubjectResourceTypeLookup_k[tempResource.FK_ResourceType_i].ResourceType_s;
                    resourceDisplayGrid_o.ContentType_s = contentTypeLookup_k[tempResource.FK_ContentType_i].ContentType_s;
                    gridDataList_o.Add(resourceDisplayGrid_o);
                }

                string gridData_s = helper_o.ConvertListToGridData(gridDataList_o, typeof(Models.UIModel.Resource.ResourceDisplayGridModel));

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                ContentResult vl_Return_o = new ContentResult();
                vl_Return_o.Content = gridData_s;
                return vl_Return_o;

            }
            catch (Exception e)
            {
                Response.TrySkipIisCustomErrors = true;
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ContentResult vl_Return_o = new ContentResult();
                vl_Return_o.Content = e.Message;
                return vl_Return_o;
            }
        }

        public ActionResult SingleResourceIndex(Guid id)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Application", new { redirectUrl = Request.RawUrl });
            }
            Helper helper_o = new Helper();
            QualBO qualBO_o = new QualBO();
            ResourceImpl resource = new ResourceImpl();
            List<object> gridSupportingDataList_o = new List<object>();

            Models.DataModel.Resource.Resource_Model resourceModel_o = resource.GetResource(id);
            Models.UIModel.Resource.ResourceEditModel resourceEditModel_o = (Models.UIModel.Resource.ResourceEditModel)helper_o.ConvertToType(resourceModel_o, typeof(Models.DataModel.Resource.Resource_Model), typeof(Models.UIModel.Resource.ResourceEditModel));

            //Supporting Resource Type
            Dictionary<int, Models.DataModel.Resource.ResourceSupportingType_Model> resourceSupportingTypeLookupList_k = new Dictionary<int, Models.DataModel.Resource.ResourceSupportingType_Model>();
            List<Models.DataModel.Resource.ResourceSupportingType_Model> resourceSupportingTypeList_k = resource.SearchResourceSupportingType(new Dictionary<string, object>());
            foreach (Models.DataModel.Resource.ResourceSupportingType_Model tempsupportingResourceType_o in resourceSupportingTypeList_k)
            {
                resourceSupportingTypeLookupList_k.Add(tempsupportingResourceType_o.PK_ResourceSupportingType_i, tempsupportingResourceType_o);
            }

            Dictionary<int, Models.DataModel.Resource.ContentType_Model> contentTypeLookup_k = new Dictionary<int, Models.DataModel.Resource.ContentType_Model>();
            List<Models.DataModel.Resource.ContentType_Model> contentTypeList_k = resource.SearchContentType(new Dictionary<string, object>());
            foreach (Models.DataModel.Resource.ContentType_Model contentType_o in contentTypeList_k)
            {
                contentTypeLookup_k.Add(contentType_o.PK_ContentType_i, contentType_o);
            }

            //Support Resources
            Dictionary<string, object> resourceParamList_k = new Dictionary<string, object>();
            resourceParamList_k.Add("FK_Resource_ID_s", id);
            List<Models.DataModel.Resource.ResourceSupporting_Model> resourceSupportingList_o = resource.SearchResourceSupporting(resourceParamList_k);
            foreach (Models.DataModel.Resource.ResourceSupporting_Model resourceSupportingModel_o in resourceSupportingList_o)
            {
                Models.UIModel.Resource.SupportingResourceDisplayGridModel supportingResource_o = (Models.UIModel.Resource.SupportingResourceDisplayGridModel)helper_o.ConvertToType(resourceSupportingModel_o, typeof(Models.DataModel.Resource.ResourceSupporting_Model), typeof(Models.UIModel.Resource.SupportingResourceDisplayGridModel));
                supportingResource_o.ContentType_s = contentTypeLookup_k[resourceSupportingModel_o.FK_ContentType_i].ContentType_s;
                supportingResource_o.ResourceSupportingType_s = resourceSupportingTypeLookupList_k[resourceSupportingModel_o.FK_ResourceSupportingType_i].ResourceSupportingType_s;
                gridSupportingDataList_o.Add(resourceSupportingModel_o);
            }

            string gridData_s = helper_o.ConvertListToGridData(gridSupportingDataList_o, typeof(Models.UIModel.Resource.SupportingResourceDisplayGridModel));
            string gridColumns_s = helper_o.ConvertModelToGridColumns(typeof(Models.UIModel.Resource.SupportingResourceDisplayGridModel));

            Models.Dispatching.Content.SingleResourceIndexModel ResourceContentModel_o = new Models.Dispatching.Content.SingleResourceIndexModel();
            ResourceContentModel_o.Resource = resourceEditModel_o;
            ResourceContentModel_o.SupportingResourceGridData = gridData_s;
            ResourceContentModel_o.SupportingResourceGridColumns = gridColumns_s;
            Models.Dispatching.PageModel pageModel_o = new Models.Dispatching.PageModel(new Models.Dispatching.SimplePaneItem("Resource", resourceEditModel_o.Name_s), ResourceContentModel_o);
            return View(pageModel_o);
        }

    }
}