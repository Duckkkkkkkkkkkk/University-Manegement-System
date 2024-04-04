using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static UniversityManegementSystem.Models.CommonFn;

namespace UniversityManegementSystem.Admin
{
    public partial class AdminHome : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            else
            {
                StudentCount();
                TeacherCount();
                SubjectCount();
                GroupCount();
            }
        }

        void StudentCount()
        {
            DataTable dt = fn.Fetch("Select Count(*) from Student");
            Session["Student"] = dt.Rows[0][0];
        }

        void TeacherCount()
        {
            DataTable dt = fn.Fetch("Select Count(*) from Teacher");
            Session["Teacher"] = dt.Rows[0][0];
        }

        void SubjectCount()
        {
            DataTable dt = fn.Fetch("Select Count(*) from Subject");
            Session["Subject"] = dt.Rows[0][0];
        }

        void GroupCount()
        {
            DataTable dt = fn.Fetch("Select Count(*) from Groups");
            Session["Groups"] = dt.Rows[0][0];
        }
    }
}