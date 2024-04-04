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
    public partial class Expense : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetGroup();
                GetExpense();
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

        private void GetExpense()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], e.ExpenseId, e.GroupID, c.GroupName, e.SubjectID, 
                                    s.SubjectName, e.ChargeAmount FROM Expense e INNER JOIN Groups c ON e.GroupID = c.GroupID 
                                    INNER JOIN Subject s ON e.SubjectID = s.SubjectID");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string groupId = ddlGroup.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string chargeAmt = txtExpenseAmt.Text.Trim();
                DataTable dt = fn.Fetch("Select * from Expense where GroupID = '" +
                                            groupId + "' AND SubjectID = '" + subjectId + "' or ChargeAmount = '" + chargeAmt + "' ");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into Expense values( '" + groupId + "' , '" + subjectId + "', '" + chargeAmt + "')";
                    fn.Query(query);
                    lblMsg.Text = "Сохранено успешно!";
                    lblMsg.CssClass = "alert alert-success";
                    ddlGroup.SelectedIndex = 0;
                    ddlSubject.SelectedIndex = 0;
                    txtExpenseAmt.Text = string.Empty;
                    GetExpense();
                }
                else
                {
                    lblMsg.Text = "Такое <b>значение</b> уже существует! ";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetExpense();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetExpense();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int expenseId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                fn.Query("Delete from Expense where ExpenseId = '" + expenseId + "' ");
                lblMsg.Text = "Удалено успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetExpense();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetExpense();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int expenseId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string groupId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlGroupGv")).SelectedValue;
                string subjectId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string chargeAmt = (row.FindControl("txtExpenseAmt") as TextBox).Text.Trim();
                fn.Query(@"Update Expense set GroupID = '" + groupId + "', SubjectID = '" + subjectId + "', ChargeAmount = '" + chargeAmt + "' " +
                    "where ExpenseId = '" + expenseId + "' ");
                lblMsg.Text = "Изменено успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetExpense();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ddlGroupGv_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlGroupSelected = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlGroupSelected.NamingContainer;

            if (row != null)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlSubjectGV = (DropDownList)row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("Select * from Subject where GroupID = '" + ddlGroupSelected.SelectedValue + "' ");
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "SubjectName";
                    ddlSubject.DataValueField = "SubjectID";
                    ddlSubject.DataBind();
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlGroup = (DropDownList)e.Row.FindControl("ddlGroupGv");
                    DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("Select * from Subject where GroupId = '" + ddlGroup.SelectedValue + "' ");
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "SubjectName";
                    ddlSubject.DataValueField = "SubjectID";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, "Select Subject");
                    string selectedSubject = DataBinder.Eval(e.Row.DataItem, "SubjectName").ToString();
                    ddlSubject.Items.FindByText(selectedSubject).Selected = true;
                }
            }
        }
    }
}