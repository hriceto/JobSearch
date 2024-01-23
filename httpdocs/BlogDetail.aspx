<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BlogDetail.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.BlogDetail" %>
<%@ Register TagPrefix="he" TagName="blogdetail" Src="~/Controls/blogdetail.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:blogdetail id="heBlogDetail" runat="server"/>
        </div>
    </div>
</asp:Content>

