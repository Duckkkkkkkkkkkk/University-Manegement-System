<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="UniversityManegementSystem.Admin.AdminHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .card-counter{
    box-shadow: 2px 2px 10px #DADADA;
    margin: 5px;
    padding: 20px 10px;
    background-color: #fff;
    height: 100px;
    border-radius: 5px;
    transition: .3s linear all;
  }

  .card-counter:hover{
    box-shadow: 4px 4px 20px #DADADA;
    transition: .3s linear all;
  }

  .card-counter.primary{
    background-color: #FF9E7D;
    color: #FFF;
  }

  .card-counter.danger{
    background-color: #32DFA5;
    color: #FFF;
  }  

  .card-counter.success{
    background-color: #FF9E7D;
    color: #FFF;
  }  

  .card-counter.info{
    background-color: #32DFA5;
    color: #FFF;
  }  

  .card-counter i{
    font-size: 5em;
    opacity: 0.2;
  }

  .card-counter .count-numbers{
    position: absolute;
    right: 35px;
    top: 20px;
    font-size: 32px;
    display: block;
  }

  .card-counter .count-name{
    position: absolute;
    right: 35px;
    top: 65px;
    font-style: italic;
    text-transform: capitalize;
    opacity: 0.5;
    display: block;
    font-size: 18px;
  }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image:url('../Image/bg3.jpg'); 
            width:100%; 
            height:720px; 
            background-repeat:no-repeat;
            background-attachment:fixed ">
<%--        <div ckass="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <h2 class="text-center">Домашняя страница</h2>
        </div>--%>

        <div class="container">

            <div class="row pt-5">
                <div class="col-md-3">
                    <div class="card-counter primary">
                        <i class="fa fa-users"></i>
                        <span class="count-numbers"><%Response.Write(Session["Student"]); %></span>
                        <span class="count-name">Студенты</span>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="card-counter danger">
                        <i class="fa fa-flask"></i>
                        <span class="count-numbers"><%Response.Write(Session["Teacher"]); %></span>
                        <span class="count-name">Преподаватели</span>
                    </div>
                </div>
                
                <div class="col-md-3">
                    <div class="card-counter success">
                        <i class="fa fa-graduation-cap"></i>
                        <span class="count-numbers"><%Response.Write(Session["Groups"]); %></span>
                        <span class="count-name">Группы</span>
                    </div>
                </div>
                                
                <div class="col-md-3">
                    <div class="card-counter info">
                        <i class="fa fa-book"></i>
                        <span class="count-numbers"><%Response.Write(Session["Subject"]); %></span>
                        <span class="count-name">Предметы</span>
                    </div>
                </div>

            </div>
        </div>

    </div>

</asp:Content>
