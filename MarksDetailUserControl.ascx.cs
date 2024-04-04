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
    public partial class MarksDetailUserControl : System.Web.UI.UserControl
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
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], e.ExamId, e.GroupID, 
                                    c.GroupName, e.SubjectID, s.SubjectName, e.RollNo, e.TotalMarks, e.OutOfMarks from Exam e 
                                    inner join Groups c on c.GroupID = e.GroupID inner join Subject s on s.SubjectID = e.SubjectID");
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string groupId = ddlGroup.SelectedValue;
                string rollNo = txtRoll.Text.Trim();
                DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], e.ExamId, e.GroupID, 
                                    c.GroupName, e.SubjectID, s.SubjectName, e.RollNo, e.TotalMarks, e.OutOfMarks from Exam e 
                                    inner join Groups c on c.GroupID = e.GroupID inner join Subject s on s.SubjectID = e.SubjectID
                                    where e.GroupID = '" + groupId + "' and e.RollNo = '" + rollNo + "' ");
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
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
                foreach (object item in row.ItemArray)
                {
                    sb.Append(item.ToString()).Append("\t");
                }
                sb.AppendLine();
            }
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=MarksDetails.txt");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
    }
}