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
    public partial class StudAttendanceDetails : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetGroup();
                GetMarks();
            }
        }

        private void GetMarks()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], e.ExamId, e.GroupID, c.GroupName, e.SubjectID, 
                                    s.SubjectName, e.RollNo, e.TotalMarks, e.OutOfMarks FROM Exam e INNER JOIN Groups c ON e.GroupID = c.GroupID 
                                    INNER JOIN Subject s ON e.SubjectID = s.SubjectID");
            GridView1.DataSource = dt;
            GridView1.DataBind();
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string groupId = ddlGroup.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string rollNo = txtRoll.Text.Trim();
                string studMarks = txtStudMarks.Text.Trim();
                string outOfMarks = txtOutOfMarks.Text.Trim();
                DataTable dttbl = fn.Fetch("Select StudentID from Student where GroupID = '" + groupId + "' AND RollNo = '" + rollNo + "' ");

                if(dttbl.Rows.Count > 0)
                {
                    DataTable dt = fn.Fetch("Select * from Exam where GroupID = '" +
                        groupId + "' AND SubjectID = '" + subjectId + "' and RollNo = '" + rollNo + "' ");

                    if (dt.Rows.Count == 0)
                    {
                        string query = "Insert into Exam values( '" + groupId + "' , '" + subjectId + "', '" + rollNo + "', '" + studMarks +
                            "', '" + outOfMarks + "' )";
                        fn.Query(query);
                        lblMsg.Text = "Сохранено успешно!";
                        lblMsg.CssClass = "alert alert-success";
                        ddlGroup.SelectedIndex = 0;
                        ddlSubject.SelectedIndex = 0;
                        txtRoll.Text = string.Empty;
                        txtStudMarks.Text = string.Empty;
                        txtOutOfMarks.Text = string.Empty;
                        GetMarks();
                    }
                    else
                    {
                        lblMsg.Text = "Такое <b>значение</b> уже существует! ";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Text = "Такая зачетная книжка <b>" + rollNo+ "</b> не существует для выбранной группы! ";
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
            GetMarks();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetMarks();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetMarks();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int examId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string groupId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlGroupGv")).SelectedValue;
                string subjectId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string rollNo = (row.FindControl("txtRollNoGv") as TextBox).Text.Trim();
                string studMarks = (row.FindControl("txtTotalMarksGv") as TextBox).Text.Trim();
                string outOfMarks = (row.FindControl("txtOutOfMarksGv") as TextBox).Text.Trim();
                fn.Query(@"Update Exam set GroupID = '" + groupId + "', SubjectID = '" + subjectId + "', RollNo = '" + rollNo +
                    "', TotalMarks = '" + txtStudMarks + "', OutOfMarks = '" + outOfMarks + "' where ExamId = '" + examId + "' ");
                lblMsg.Text = "Изменено успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetMarks();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
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
    }
}