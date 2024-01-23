<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyCoupons.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.MyCoupons" %>
<%@ Register TagPrefix="he" TagName="mycoupons" Src="~/Employer/controls/mycoupons.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:mycoupons id="heMyCoupons" runat="server"/>
        </div>
    </div>
</asp:Content>

