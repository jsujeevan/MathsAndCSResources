using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace MShed_Web.BOs
{
    public class Helper
    {
        public object ConvertToType(object object_o, System.Type fromType_o, System.Type toType_o)
        {
            object convertedItem_o = Activator.CreateInstance(toType_o);

            foreach (PropertyInfo property_o in toType_o.GetProperties())
            {
                foreach (PropertyInfo vl_Fromproperty_o in fromType_o.GetProperties())
                {
                    if (property_o.Name == vl_Fromproperty_o.Name)
                    {
                        property_o.SetValue(convertedItem_o, vl_Fromproperty_o.GetValue(object_o), null);
                        break;
                    }
                }

            }
            return convertedItem_o;
        }

        public string ConvertListToGridData(List<object> listToConvert_k, System.Type modelType_o)
        {
            string finalValue_s = "";
            string propertyType_s = "";
            object propertyValue_o = null;
            StringBuilder return_s = new StringBuilder();
            return_s.Append("[\n");
            List<object> convertedList_k = listToConvert_k.ToList<object>();
            for (int objectCounter_i = 0; objectCounter_i < listToConvert_k.Count; objectCounter_i++)
            {
                object temp_o = convertedList_k[objectCounter_i];
                return_s.Append("\t[");
                List<PropertyInfo> modelProperties_k = modelType_o.GetProperties().ToList();
                for (int counter_i = 0; counter_i < modelProperties_k.Count; counter_i++)
                {
                    PropertyInfo property_o = modelType_o.GetProperties()[counter_i];
                    propertyType_s = property_o.PropertyType.Name;
                    propertyValue_o = property_o.GetValue(temp_o);
                    if (propertyValue_o != null)
                    {
                        //Do null checks (against c# types)
                        switch (propertyType_s)
                        {
                            case "DateTime":
                                DateTime temp_d = (DateTime)property_o.GetValue(temp_o);
                                finalValue_s = HttpUtility.HtmlEncode(temp_d.ToString("dd/MM/yyyy"));
                                break;
                            default:
                                finalValue_s = HttpUtility.HtmlEncode(property_o.GetValue(temp_o).ToString());
                                break;
                        }
                    }
                    else
                    {
                        finalValue_s = "";
                    }
                    return_s.Append("\"" + finalValue_s + "\"");

                    if (counter_i < (modelProperties_k.Count - 1))
                    {
                        return_s.Append(",");
                    }
                        
                }

                if (objectCounter_i < (listToConvert_k.Count - 1))
                {
                    return_s.Append("],\n");
                }
                else
                {
                    return_s.Append("]\n");
                }
            }
            return_s.Append("]");
            return return_s.ToString();
        }

        public string ConvertModelToGridColumns(System.Type modelType_o)
        {
            StringBuilder return_s = new StringBuilder();
            return_s.Append("[\n");
            List<PropertyInfo> modelPropertyList_k = modelType_o.GetProperties().ToList();
            for (int counter_i = 0; counter_i < modelPropertyList_k.Count; counter_i++)
            {
                PropertyInfo property_o = modelPropertyList_k[counter_i];
                object[] attrs = property_o.GetCustomAttributes(true);
                String displayName_s = property_o.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single().DisplayName;

                return_s.Append("{ title: \"" + displayName_s + "\" }");
                if (counter_i < (modelPropertyList_k.Count - 1))
                {
                    return_s.Append(",\n");
                }
                else
                {
                    return_s.Append("\n");
                }
            }
            return_s.Append("]");
            return return_s.ToString();
        }
    }
}