using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static UniversityManegementSystem.Models.CommonFn;

namespace UniversityManegementSystem.Admin
{
    public partial class ExpenseDetails : System.Web.UI.Page
    {
        Commonfnx fn = new Commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetExpenseDetails();
            }
        }

        private void GetExpenseDetails()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], e.ExpenseId, e.GroupID, c.GroupName, e.SubjectID, 
                                    s.SubjectName, e.ChargeAmount FROM Expense e INNER JOIN Groups c ON e.GroupID = c.GroupID 
                                    INNER JOIN Subject s ON e.SubjectID = s.SubjectID");
            GridView1.DataSource = dt;
            GridView1.DataBind();
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
            Response.AddHeader("content-disposition", "attachment;filename=ExpenseDetails.txt");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
    }
}