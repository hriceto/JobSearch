<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Unsubscribe.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Unsubscribe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-2"></div>
        <div class="col-sm-8">
            <asp:Label ID="lblUnsubscribe" runat="server"></asp:Label><br />
            <asp:Button ID="btnUnsubscribe" runat="server" OnClick="btnUnsubscribe_Click" meta:resourcekey="btnUnsubscribe" CssClass="btn btn-primary"/>
        </div>
        <div class="col-sm-2"></div>
    </div>
    <div class="row">
        <div class="col-sm-2"></div>
        <div class="col-sm-8">
            <asp:Label ID="lblSuccess" runat="server" meta:resourcekey="lblSuccess" CssClass="text-success" Visible="false"></asp:Label>
            <asp:Label ID="lblFailure" runat="server" meta:resourcekey="lblFailure" CssClass="text-danger" Visible="false"></asp:Label>
        </div>
        <div class="col-sm-2"></div>
    </div>
</asp:Content>

