﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="StudentAttendanceDetails.aspx.cs" Inherits="UniversityManegementSystem.Teacher.StudentAttendanceDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%@ Register Src="~/StudentAttendanceUC.ascx" TagPrefix="uc" TagName="StudentAttendance" %>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc:StudentAttendance runat="server" ID="StudentAttendance1"/>

</asp:Content>
