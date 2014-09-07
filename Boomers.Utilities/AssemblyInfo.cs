using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Reflection;

//''Version information for an assembly consists of the following four values:
//''
//''      Major Version
//''      Minor Version 
//''      Build Number
//''      Revision
/// <summary>
/// Summary description for AssemblyInfo
/// </summary>
public class AssemblyID
{

    public AssemblyID()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string GetVersion()
    { return "2.1.0.1"; }
}
