﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics;

namespace ConsumerMaster
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
        }

        //void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exc = Server.GetLastError();

        //    if (exc is HttpUnhandledException)
        //    {
        //        // Pass the error on to the error page.
        //        Server.Transfer("ErrorPage.aspx?handler=Application_Error%20-%20Global.asax", true);
        //    }
        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception objErr = Server.GetLastError().GetBaseException();
        //    string err = "Error Caught in Application_Error event\n" +
        //                 "Error in: " + Request.Url.ToString() +
        //                 "\nError Message:" + objErr.Message.ToString() +
        //                 "\nStack Trace:" + objErr.StackTrace.ToString();
        //    EventLog.WriteEntry("Sample_WebApp", err, EventLogEntryType.Error);
        //    Server.ClearError();
        //    //additional actions...
        //}
    }
}