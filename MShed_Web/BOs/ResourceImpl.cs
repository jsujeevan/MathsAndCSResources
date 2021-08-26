using MShed_Web.Models;
using MShed_Web.Models.DataModel.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MShed_Web.BOs
{
    public class ResourceImpl : ResourceBO
    {
        public List<ResourceSupporting_Model> GetMultiResourceSupportingByResource(List<Guid> parameterList_k)
        {
            List<ResourceSupporting_Model> vl_ResourceSupportingList_o = null;
            DataBinder_BO vl_dataBinder_o = new DataBinder_BO();
            SQLCommandModel vl_sqlcmd_o = new SQLCommandModel();
            DataTable dataTable_o = new DataTable("_PK_");
            DataColumn tableColumn_o = new DataColumn();
            tableColumn_o.DataType = System.Type.GetType("System.Guid");
            tableColumn_o.ColumnName = "_PK_";
            dataTable_o.Columns.Add(tableColumn_o);
            foreach (Guid temp_o in parameterList_k ?? new List<Guid>())
            {
                DataRow row_o = dataTable_o.NewRow();
                row_o["_PK_"] = temp_o;
                dataTable_o.Rows.Add(row_o);
            }
            List<SQLParameterModel> vl_paramList = new List<SQLParameterModel>();
            SQLParameterModel tempParam_o = new SQLParameterModel { Parameter_Name = "_PK_", Parameter_Value = dataTable_o, SqlDbType = SqlDbType.Structured, TypeName = "dbo.UNIQUEIDENTIFIER_COLLECTION" };
            vl_paramList.Add(tempParam_o);
            vl_sqlcmd_o.SQL_Name = "ResourceSupporting_SELECT_COLLECTION_Resource";
            vl_sqlcmd_o.SQL_ParameterList = vl_paramList;
            vl_ResourceSupportingList_o = vl_dataBinder_o.GetGenericModel(typeof(ResourceSupporting_Model), vl_sqlcmd_o).Cast<ResourceSupporting_Model>().ToList();
            return vl_ResourceSupportingList_o;
        }
    }
}