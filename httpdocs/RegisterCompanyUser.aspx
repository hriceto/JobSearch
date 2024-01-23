<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RegisterCompanyUser.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.RegisterCompanyUser" %>
<%@ Register TagPrefix="he" TagName="registercompanyuser" Src="~/controls/registercompanyuser.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strRegisterCompanyUser").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:registercompanyuser id="heRegisterCompanyUser" runat="server"/>
        </div>
    </div>
</asp:Content>

