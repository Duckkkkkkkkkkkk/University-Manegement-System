using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static UniversityManegementSystem.Models.CommonFn;

namespace UniversityManegementSystem
{
    public partial class StudentAttendanceUC : System.Web.UI.UserControl
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt;
            DateTime date = Convert.ToDateTime(txtMonth.Text);

            if(ddlSubject.SelectedValue == "Select subject")
            {
                dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], s.Name, sa.Status, sa.Date from StudentAttendance sa 
                                inner join Student s on s.RollNo = sa.RollNo where sa.GroupID = '" + ddlGroup.SelectedValue + "' and sa.RollNo = '" + txtRollNo.Text.Trim() 
                                + "' and DATEPART(yy, Date) = '" + date.Year + "' and DATEPART(M, Date) = '" + date.Month + "' ");
            }
            else
            {
                dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], s.Name, sa.Status, sa.Date from StudentAttendance sa 
                                inner join Student s on s.RollNo = sa.RollNo where sa.GroupID = '" + ddlGroup.SelectedValue + "' and sa.RollNo = '" + 
                                txtRollNo.Text.Trim() + "'and sa.SubjectID = '" + ddlSubject.SelectedValue + "' and DATEPART(yy, Date) = '" + date.Year + 
                                "' and DATEPART(M, Date) = '" + date.Month + "' ");
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label labelStatus = e.Row.FindControl("Label1") as Label;
                if (labelStatus != null)
                {
                    string statusText = Server.HtmlDecode(labelStatus.Text);
                    bool status;

                    // Попытка преобразования значения в логическое
                    if (bool.TryParse(statusText, out status))
                    {
                        (GridView1.DataSource as DataTable).Rows[e.Row.RowIndex]["Status"] = status;
                    }
                    else
                    {
                        // В случае неудачного преобразования, можно установить значение по умолчанию или обработать ошибку
                        (GridView1.DataSource as DataTable).Rows[e.Row.RowIndex]["Status"] = DBNull.Value;
                    }
                }
            }
        }


        protected void btnExportToTxt_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            foreach (DataControlFieldHeaderCell headerCell in GridView1.HeaderRow.Cells)
            {
                dt.Columns.Add(headerCell.Text);
            }

            foreach (GridViewRow gridViewRow in GridView1.Rows)
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < gridViewRow.Cells.Count; i++)
                {
                    dataRow[i] = gridViewRow.Cells[i].Text;
                }
                dt.Rows.Add(dataRow);
            }

            ExportToTxt(dt);
        }

        private void ExportToTxt(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn col in dt.Columns)
            {
                sb.Append(col.ColumnName).Append("\t");
            }
            sb.AppendLine();

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append(row[col].ToString()).Append("\t");
                }
                sb.AppendLine();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=AttendanceDetails.txt");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

    }
}