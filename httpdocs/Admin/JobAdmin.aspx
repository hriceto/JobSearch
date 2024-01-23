<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="JobAdmin.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.JobAdmin" %>
<%@ Register TagPrefix="he" TagName="jobadmin" Src="~/Admin/controls/jobadmin.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:jobadmin id="heJobAdmin" runat="server"></he:jobadmin>
        </div>
    </div>    
</asp:Content>
