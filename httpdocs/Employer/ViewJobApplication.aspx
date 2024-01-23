<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewJobApplication.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.ViewJobApplication" %>
<%@ Register TagPrefix="he" TagName="application" Src="~/Employer/controls/application.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:application id="heApplication" runat="server"/>
        </div>
    </div>
</asp:Content>

