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
    public partial class Subject : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetGroup();
                GetSubject();
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
                string groupVal = ddlGroup.SelectedItem.Text;
                DataTable dt = fn.Fetch("Select * from Subject where GroupID = '" + 
                                            ddlGroup.SelectedItem.Value + "' and SubjectName = '" + txtSubject.Text.Trim() + "' ");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into Subject values( '" + ddlGroup.SelectedItem.Value + "' , '" + txtSubject.Text.Trim() + "')";
                    fn.Query(query);
                    lblMsg.Text = "Взнос успешно добавлен";
                    lblMsg.CssClass = "alert alert-success";
                    ddlGroup.SelectedIndex = -1;
                    txtSubject.Text = string.Empty;
                    GetSubject();
                }
                else
                {
                    lblMsg.Text = "Такой предмет уже существует для группы <b> '" + groupVal + "' </b> ! ";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void GetSubject()
        {
            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No], s.SubjectID, s.GroupID, c.GroupName, 
                                    s.SubjectName from Subject s inner join Groups c on c.GroupID = s.GroupID");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetSubject();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetSubject();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetSubject();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int subjId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string groupId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("DropDownList1")).SelectedValue;
                string subjName = (row.FindControl("TextBox1") as TextBox).Text;
                fn.Query("Update Subject set GroupID = '" + groupId + "', subjectName = '"+subjName+"' where SubjectID = '" + subjId + "' ");
                lblMsg.Text = "Предмет изменен успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetSubject();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}