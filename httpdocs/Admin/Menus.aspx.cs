using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.Web.Admin
{
    public partial class Menus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            MenuManager menuManager = new MenuManager();
            menuManager.RegenerateMenuControls();
        }
    }
}