<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.master" AutoEventWireup="true" CodeBehind="Marks.aspx.cs" Inherits="UniversityManegementSystem.Admin.StudAttendanceDetails" %>
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
        background-attachment:fixed ">
    <div class="container p-md-4 p-sm-4">
        <div>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
        <h2 class="text-center">Add Marks</h2>

        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-6">
                <label for="ddlGroup">Группа</label>
                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Group is required" 
                    ControlToValidate="ddlGroup" Display="Dynamic" ForeColor="Red" InitialValue="Select Groups" SetFocusOnError="True">
                </asp:RequiredFieldValidator>
            </div>

            <div class="col-md-6">
                <label for="ddlSubject">Предмет</label>
                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-control" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Subject is required" 
                    ControlToValidate="ddlSubject" Display="Dynamic" ForeColor="Red" InitialValue="Select Subject" SetFocusOnError="True">
                </asp:RequiredFieldValidator>
            </div>

            <div class="col-md-12 mt-2">
                <label for="txtRoll">Номер зачетной книжки студента</label>
                <asp:TextBox ID="txtRoll" runat="server" CssClass="form-control" placeholder="Введите номер зачетной книжки" required></asp:TextBox>
            </div>

            <div class="col-md-6 mt-2">
                <label for="txtStudMarks">Оценка студента</label>
                <asp:TextBox ID="txtStudMarks" runat="server" CssClass="form-control" placeholder="Введите оценку студента" 
                    TextMode="Number" required></asp:TextBox>
            </div>
            
            <div class="col-md-6 mt-2">
                <label for="txtOutOfMarks">Максимальная оценка</label>
                <asp:TextBox ID="txtOutOfMarks" runat="server" CssClass="form-control" placeholder="Введите максимальную оценку" 
                    TextMode="Number" required></asp:TextBox>
            </div>
        </div>

        <div class="row mb-3 mr-lg-5 ml-lg-5">
            <div class="col-md-6 col-md-offset-2 mb-3">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" BackColor="#32DFA5" Text="Добавить оценку" 
                    OnClick="btnAdd_Click"/>
            </div>
        </div>

        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-12">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="Нет записей для отображения" 
                    AutoGenerateColumns="False" AllowPaging="True" PageSize="4" OnPageIndexChanging="GridView1_PageIndexChanging" 
                    DataKeyNames="ExamId" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" 
                    OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound" >
                    <Columns>
                        <asp:BoundField DataField="Sr.No" HeaderText="Sr.No" ReadOnly="True">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="Groups">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlGroupGv" runat="server" DataSourceID="SqlDataSource1" DataTextField="GroupName" 
                                    DataValueField="GroupID" SelectedValue='<%# Eval("GroupID") %>' CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGroupGv_SelectedIndexChanged">
                                    <asp:ListItem>Выберете группу</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UniversitySysDBConnectionString %>" 
                                    SelectCommand="SELECT * FROM [Groups]"></asp:SqlDataSource>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Subject">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlSubjectGv" runat="server" CssClass="form-control"></asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("SubjectName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Roll Number">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRollNoGv" runat="server" CssClass="form-control"  Text='<%# Eval("RollNo") %>' 
                                    TextMode="Number" ></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("RollNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total Marks">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStudMarksGv" runat="server" CssClass="form-control"  Text='<%# Eval("TotalMarks") %>' 
                                    TextMode="Number" ></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("TotalMarks") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Out Of Marks">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOutOfMarksGv" runat="server" CssClass="form-control"  Text='<%# Eval("OutOfMarks") %>' 
                                    TextMode="Number" ></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("OutOfMarks") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:CommandField CausesValidation="false" HeaderText="Operation" ShowEditButton="True">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>

                    </Columns>
                    <HeaderStyle BackColor="#32DFA5" ForeColor="White"/>
                </asp:GridView>
            </div>
        </div>

    </div>
</div>

</asp:Content>
