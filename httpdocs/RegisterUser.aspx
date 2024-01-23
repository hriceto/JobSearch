<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RegisterUser.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.RegisterUser" ValidateRequest="false" %>
<%@ Register TagPrefix="he" TagName="registeruser" Src="~/controls/registeruser.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strRegisterUser").ToString()%>" />

    <script src="/controls/ckeditor/ckeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:registeruser id="heRegisterUser" runat="server" IsJobApplication="false" IsRegistration="true"/>
        </div>
    </div>
</asp:Content>

