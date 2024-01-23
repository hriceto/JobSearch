<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Login" %>
<%@ Register TagName="login" TagPrefix="he" Src="~/controls/login.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strLogin").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">

    <div class="row">
        <div class="col-sm-12">
            <br /><br />
            <div class="Login">
                <asp:ScriptManager ID="smLogin" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="upLogin" runat="server">
                    <ContentTemplate>
                        <he:login id="heLogin" runat="server"></he:login>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdateProgress ID="uprogLogin" runat="server" AssociatedUpdatePanelID="upLogin" DisplayAfter="1">
                    <ProgressTemplate>
                        <div class="AjaxProgress"></div>
                        <div class="AjaxProgressText">Logging in...</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </div>
    </div>

</asp:Content>


