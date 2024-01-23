<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JobApplication.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.JobApplication" ValidateRequest="false" %>
<%@ Register TagPrefix="he" TagName="registeruser" Src="~/controls/registeruser.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strJobApplication").ToString()%>" />
    
    <script src="/controls/ckeditor/ckeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <he:registeruser id="heRegisterUser" runat="server" IsJobApplication="true" IsRegistration="true"/>
</asp:Content>

