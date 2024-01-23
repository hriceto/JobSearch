<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CheckoutConfirmation.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.CheckoutConfirmation" %>
<%@ Register TagPrefix="he" TagName="checkoutconfirmation" Src="~/Employer/controls/checkoutconfirmation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <div class="text-center">
                <he:checkoutconfirmation id="heCheckoutConfirmation" runat="server"/>
            </div>
        </div>
    </div>
</asp:Content>

