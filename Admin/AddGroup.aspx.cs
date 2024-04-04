using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniversityManegementSystem.Models;
using static UniversityManegementSystem.Models.CommonFn;

namespace UniversityManegementSystem.Admin
{
    public partial class AddGroup : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetGroup();
            }
        }

        private void GetGroup()
        {
            DataTable dt = fn.Fetch("Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No], GroupID, GroupName from Groups");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = fn.Fetch("Select * from Groups where GroupName = '"+txtGroup.Text.Trim()+"'");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into Groups values('" + txtGroup.Text.Trim() + "')";
                    fn.Query(query);
                    lblMsg.Text = "Группа добавлена успешно";
                    lblMsg.CssClass = "alert alert-success";
                    txtGroup.Text = string.Empty;
                    GetGroup();
                }
                else
                {
                    lblMsg.Text = "Такая группа уже существует";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('"+ ex.Message+"');</script>");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetGroup();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetGroup();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int cId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string GroupName = (row.FindControl("txtGroupEdit") as TextBox).Text;
                fn.Query("Update Groups set Groupname = '" + GroupName + "' where GroupID = '" + cId + "' ");
                lblMsg.Text = "Группа изменена успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetGroup();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}