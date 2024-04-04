<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarksDetailUserControl.ascx.cs" Inherits="UniversityManegementSystem.MarksDetailUserControl" %>

<div style="background-image:url('../Image/bg3.jpg'); 
        width:100%; 
        height:720px; 
        background-repeat:no-repeat;
        background-attachment:fixed ">
    <div class="container p-md-4 p-sm-4">
        <div>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
        <h2 class="text-center">Детали оценивания</h2>

        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-6">
                <label for="ddlGroup">Группа</label>
                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Group is required" 
                    ControlToValidate="ddlGroup" Display="Dynamic" ForeColor="Red" InitialValue="Select Groups" SetFocusOnError="True">
                </asp:RequiredFieldValidator>
            </div>
            <div class="col-md-6">
                <label for="txtRoll">Номер зачетной книжки студента</label>
                <asp:TextBox ID="txtRoll" runat="server" CssClass="form-control" placeholder="Введите номер зачетной книжки" required>
                </asp:TextBox>
            </div>
        </div>

        <div class="row mb-3 mr-lg-5 ml-lg-5">
            <div class="col-md-6 col-md-offset-2 mb-3">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" BackColor="#32DFA5" Text="Просмотр оценок" OnClick="btnAdd_Click" />
            </div>
        </div>

        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-12">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="Нет записей для отображения" 
                    AutoGenerateColumns="False" AllowPaging="true" PageSize="8" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Sr.No" HeaderText="Sr.No">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="ExamId" HeaderText="ExamId">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="GroupName" HeaderText="Groups">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SubjectName" HeaderText="Subject">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="RollNo" HeaderText="Roll Number">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="TotalMarks" HeaderText="Total Marks">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="OutOfMarks" HeaderText="Out Of Marks">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                    </Columns>
                    <HeaderStyle BackColor="#32DFA5" ForeColor="White"/>
                </asp:GridView>

                <asp:Button ID="btnExportToTxt" runat="server" Text="Выгрузить в TXT" OnClick="btnExportToTxt_Click" CssClass="btn btn-primary" BackColor="#32DFA5" ForeColor="White" />
            </div>
        </div>

    </div>
</div>
