using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReadPhoto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strImageID = Convert.ToString(Request.QueryString["id"]);
        if(!string.IsNullOrEmpty(strImageID))        
        {
            foto.Src = "readimageDynamics.aspx?id=" + strImageID;
        }
        
    }
}