<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.master" AutoEventWireup="true" CodeBehind="ExpenseDetails.aspx.cs" Inherits="UniversityManegementSystem.Admin.ExpenseDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=GridView1.ClientID%>').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({ "paging": true, "ordering": true, "searching": true });
        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image:url('../Image/bg3.jpg'); 
        width:100%; 
        height:720px; 
        background-repeat:no-repeat;
        background-size: cover;
        background-attachment:fixed ">
    <div class="container p-md-4 p-sm-4">

        <h2 class="text-center">Детали выдачи зарплаты</h2>



        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-12">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="Нет записей для отображения" 
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Sr.No" HeaderText="Sr.No">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="GroupName" HeaderText="Groups">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SubjectName" HeaderText="Subject">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="ChargeAmount" HeaderText="Charge Amount (per Lecture)">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                    </Columns>
                    <HeaderStyle BackColor="#32DFA5" ForeColor="White"/>
                </asp:GridView>
            </div>
        </div>
        <asp:Button ID="btnExportToTxt" runat="server" Text="Выгрузить в TXT" OnClick="btnExportToTxt_Click" CssClass="btn btn-primary" BackColor="#32DFA5" ForeColor="White" />

    </div>
</div>

</asp:Content>
