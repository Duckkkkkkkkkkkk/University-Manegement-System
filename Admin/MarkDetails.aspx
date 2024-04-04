<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.master" AutoEventWireup="true" CodeBehind="MarkDetails.aspx.cs" Inherits="UniversityManegementSystem.Admin.StudAttendanceDetails1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%@ Register Src="~/MarksDetailUserControl.ascx" TagPrefix="uc" TagName="MarksDetail" %>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc:MarksDetail runat="server" ID="MarksDetail1"/>

</asp:Content>
