<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewJobApplications.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.ViewJobApplications" %>
<%@ Register TagPrefix="he" TagName="applications" Src="~/Employer/controls/applications.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:applications id="heApplications" runat="server"></he:applications>
        </div>
    </div>
</asp:Content>

