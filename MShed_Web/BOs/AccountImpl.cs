using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MShed_Web.BOs
{
    public class AccountImpl : AccountBO
    {
        public string GetClientBrowser(HttpRequestBase vl_Request_o)
        {
            string vl_browser_s = "";
            try
            {
                vl_browser_s = vl_Request_o.Browser.Browser;
            }
            catch (Exception e)
            {
                vl_browser_s = "";
            }
            return vl_browser_s;
        }

        public string GetClientBrowserInfo(HttpRequestBase vl_Request_o)
        {
            string vl_browserInfo_s = "";
            try
            {

                vl_browserInfo_s = "RemoteUser=" + vl_Request_o.ServerVariables["REMOTE_USER"] + ";\n"
                + "RemoteHost=" + vl_Request_o.ServerVariables["REMOTE_HOST"] + ";\n"
                + "Type=" + vl_Request_o.Browser.Type + ";\n"
                + "Name=" + vl_Request_o.Browser.Browser + ";\n"
                + "Version=" + vl_Request_o.Browser.Version + ";\n"
                + "MajorVersion=" + vl_Request_o.Browser.MajorVersion + ";\n"
                + "MinorVersion=" + vl_Request_o.Browser.MinorVersion + ";\n"
                + "Platform=" + vl_Request_o.Browser.Platform + ";\n"
                + "SupportsCookies=" + vl_Request_o.Browser.Cookies + ";\n"
                + "SupportsJavaScript=" + vl_Request_o.Browser.EcmaScriptVersion.ToString() + ";\n"
                + "SupportsActiveXControls=" + vl_Request_o.Browser.ActiveXControls + ";\n"
                + "SupportsJavaScriptVersion=" + vl_Request_o.Browser["JavaScriptVersion"] + "\n";
            }
            catch (Exception e)
            {
                vl_browserInfo_s = "";
            }
            return vl_browserInfo_s;
        }

        public string GetClientIP(HttpRequestBase vl_Request_o)
        {
            string vl_clientIPInfo_s = "";
            try
            {
                vl_clientIPInfo_s = (vl_Request_o.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? vl_Request_o.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            }
            catch (Exception e)
            {
                vl_clientIPInfo_s = "";
            }
            return vl_clientIPInfo_s;
        }
    }
}