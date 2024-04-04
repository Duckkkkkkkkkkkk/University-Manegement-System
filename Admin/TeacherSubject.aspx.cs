using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static UniversityManegementSystem.Models.CommonFn;


namespace UniversityManegementSystem.Admin
{
    public partial class TeacherSubject : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetGroup();
                GetTeacher();
                GetTeacherSubject();
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

        private void GetTeacher()
        {
            DataTable dt = fn.Fetch("Select * from Teacher");
            ddlTeacher.DataSource = dt;
            ddlTeacher.DataTextField = "Name";
            ddlTeacher.DataValueField = "TeacherID";
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, "Select Teacher");
        }

        private void GetTeacherSubject()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], ts.Id, ts.GroupID, c.GroupName, ts.SubjectID, 
                                    s.SubjectName, ts.TeacherID, t.Name 
                                    FROM TeacherSubject ts 
                                    INNER JOIN Groups c ON ts.GroupID = c.GroupID 
                                    INNER JOIN Subject s ON ts.SubjectID = s.SubjectID 
                                    INNER JOIN Teacher t ON ts.TeacherID = t.TeacherID");
            GridView1.DataSource = dt;
            GridView1.DataBind();
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
                string teacherId = ddlTeacher.SelectedValue;
                DataTable dt = fn.Fetch("Select * from TeacherSubject where GroupID = '" +
                                            groupId + "' AND SubjectID = '" + subjectId + "' or TeacherID = '" + teacherId + "' ");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into TeacherSubject values( '" + groupId + "' , '" + subjectId + "', '" + teacherId + "')";
                    fn.Query(query);
                    lblMsg.Text = "Сохранено успешно!";
                    lblMsg.CssClass = "alert alert-success";
                    ddlGroup.SelectedIndex = 0;
                    ddlSubject.SelectedIndex = 0;
                    ddlTeacher.SelectedIndex = 0;
                    GetTeacherSubject();
                }
                else
                {
                    lblMsg.Text = "Такая связь <b>Преподаватель-Предмет</b> уже существует! ";
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
            GetTeacherSubject();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetTeacherSubject();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int TeacherSubjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                fn.Query("Delete from TeacherSubject where Id = '" + TeacherSubjectId + "' ");
                lblMsg.Text = "Удалено успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeacherSubject();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetTeacherSubject();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int TeacherSubjectId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string groupId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlGroupGv")).SelectedValue;
                string subjectId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string teacherId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlTeacherGv")).SelectedValue;
                fn.Query(@"Update TeacherSubject set GroupID = '" + groupId + "', SubjectID = '" + subjectId + "', TeacherID = '" + teacherId + "' " +
                    "where Id = '" + TeacherSubjectId + "' ");
                lblMsg.Text = "Изменено успешно";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeacherSubject();
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
