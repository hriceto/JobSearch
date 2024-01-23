<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Menus.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.Menus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <asp:Button id="btnGenerate" runat="server" Text="Generate Menus From Database" OnClick="btnGenerate_Click"/>
</asp:Content>

