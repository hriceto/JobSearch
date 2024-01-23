<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CompanyEdit.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.CompanyEdit" %>
<%@ Register TagPrefix="he" TagName="companyedit" Src="~/Admin/controls/companyedit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:companyedit id="heCompanyEdit" runat="server"/>
        </div>
    </div>
</asp:Content>
