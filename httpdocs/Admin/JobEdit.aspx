<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JobEdit.aspx.cs" Inherits="Admin_JobEdit" %>
<%@ Register TagPrefix="he" TagName="jobedit" Src="~/Admin/controls/jobedit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:jobedit id="heJobEdit" runat="server"></he:jobedit>
        </div>
    </div>
</asp:Content>


