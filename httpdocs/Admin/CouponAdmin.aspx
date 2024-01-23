<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="CouponAdmin.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.CouponAdmin" %>
<%@ Register TagPrefix="he" TagName="couponadmin" Src="~/Admin/controls/couponadmin.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:couponadmin id="heCouponAdmin" runat="server"/>
        </div>
    </div>
</asp:Content>
