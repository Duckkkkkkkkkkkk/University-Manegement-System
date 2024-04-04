using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static UniversityManegementSystem.Models.CommonFn;

namespace UniversityManegementSystem.Teacher
{
    public partial class StudentAttendance : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetGroup();
                btnMarkAttendance.Visible = false;
            }
        }
        private void GetGroup()
        {
            DataTable dt = fn.Fetch("Select * from Groups");
            ddlGroup.DataSource = dt;
            ddlGroup.DataTextField = "GroupName";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, "Select Groups");
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string groupId = ddlGroup.SelectedValue;
            DataTable dt = fn.Fetch("Select * from Subject where GroupId = '" + groupId + "' ");
            ddlSubject.DataSource = dt;
            ddlSubject.DataTextField = "SubjectName";
            ddlSubject.DataValueField = "SubjectID";
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, "Select Groups");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt = fn.Fetch(@"SELECT StudentID, RollNo, Name, Mobile FROM Student where GroupID = '" + ddlGroup.SelectedValue + "' ");
            GridView1.DataSource = dt;
            GridView1.DataBind();
            if(dt.Rows.Count > 0)
            {
                btnMarkAttendance.Visible = true;
            }
            else
            {
                btnMarkAttendance.Visible = false;
            }
        }

        protected void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            bool isTrue = false;
            foreach (GridViewRow row in GridView1.Rows)
            {
                string rollNo = row.Cells[2].Text.Trim();

                RadioButton rb1 = (row.Cells[0].FindControl("RadioButton1") as RadioButton);
                RadioButton rb2 = (row.Cells[0].FindControl("RadioButton2") as RadioButton);
                int status = 0;
                if (rb1.Checked)
                {
                    status = 1;
                }
                else if (rb2.Checked)
                {
                    status = 0;
                }

                fn.Query(@"Insert into StudentAttendance values ('" + ddlGroup.SelectedValue + "', '" + ddlSubject.SelectedValue + "','" +
                    rollNo + "', '" + status + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + "')");
                isTrue = true;
            }
            if (isTrue)
            {
                lblMsg.Text = "Изменено успешно";
                lblMsg.CssClass = "alert alert-success";
            }
            else
            {
                lblMsg.Text = "Что-то пошло не так...";
                lblMsg.CssClass = "alert alert-danger";
            }
        }
    }
}