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
    public partial class Student : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetGroup();
                GetStudents();
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlGender.SelectedValue != "0")
                {
                    string rollNo = txtRoll.Text.Trim();
                    DataTable dt = fn.Fetch("Select * from Student where GroupID = '"+ ddlGroup.SelectedValue +"' and RollNo = '" + rollNo + "' ");
                    if (dt.Rows.Count == 0)
                    {
                        string query = "Insert into Student values('" + txtName.Text.Trim() + "', '" + txtDOB.Text.Trim() + "', '" +
                            ddlGender.SelectedValue + "', '" + txtMobile.Text.Trim() + "', '" + txtRoll.Text.Trim() + "', '" +
                            txtAddress.Text.Trim() + "', '" + ddlGroup.SelectedValue + "') ";
                        fn.Query(query);
                        lblMsg.Text = "Студент успешно добавлен";
                        lblMsg.CssClass = "alert alert-success";
                        ddlGender.SelectedIndex = 0;
                        txtName.Text = string.Empty;
                        txtDOB.Text = string.Empty;
                        txtMobile.Text = string.Empty;
                        txtRoll.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        ddlGroup.SelectedIndex = 0;
                        GetStudents();
                    }
                    else
                    {
                        lblMsg.Text = "Такая зачетная книжка <b> '" + rollNo + "' </b> уже существует для выбранной группы! ";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Text = "Недопустимое значение!";
                    lblMsg.CssClass = "alert alert-danger";
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void GetStudents()
        {
            DataTable dt = fn.Fetch(@"Select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) [Sr.No], s.StudentID, s.[Name], s.DOB, s.Gender, s.Mobile, 
                                    s.RollNo, s.[Address], c.GroupID, c.GroupName from Student s inner join Groups c on c.GroupID = s.GroupID");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetStudents();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetStudents();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetStudents();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int studentId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string name = (row.FindControl("txtName") as TextBox).Text;
                string mobile = (row.FindControl("txtMobile") as TextBox).Text;
                string rollNo = (row.FindControl("txtRollNo") as TextBox).Text;
                string address = (row.FindControl("txtAddress") as TextBox).Text;
                string groupId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[4].FindControl("ddlGroup")).SelectedValue;
                fn.Query("Update Student set Name = '" + name.Trim() + "', Mobile = '" + mobile.Trim() + "', Address = '" + address.Trim() +
                    "', RollNo = '" + rollNo.Trim() + "', GroupID = '"+ groupId +"' where StudentID = '" + studentId + "' ");
                lblMsg.Text = "Студент изменен успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetStudents();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlGroup = (DropDownList)e.Row.FindControl("ddlGroup");
                DataTable dt = fn.Fetch("Select * from Groups");
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ddlGroup.Items.Insert(0, "Select Groups");
                string selectedGroup = DataBinder.Eval(e.Row.DataItem, "GroupName").ToString();
                ddlGroup.Items.FindByText(selectedGroup).Selected = true;
            }
        }
    }
}