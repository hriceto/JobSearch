<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="CouponEdit.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.CouponEdit" %>
<%@ Register TagPrefix="he" TagName="couponedit" Src="~/Admin/controls/couponedit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/js/bootstrap-datepicker.js" type="text/javascript" language="javascript"></script>
    <link rel="Stylesheet" type="text/css" href="/css/datepicker3.css"/>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:couponedit id="heCouponEdit" runat="server"/>
        </div>
    </div>
</asp:Content>
