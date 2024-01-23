<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Checkout" %>
<%@ Register TagPrefix="he" TagName="checkout" Src="~/Employer/controls/checkout.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:checkout id="heCheckout" runat="server"/>
        </div>
    </div>
</asp:Content>

