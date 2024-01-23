<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddEditJob.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.AddEditJob" ValidateRequest="false" %>
<%@ Register TagPrefix="he" TagName="addeditjob" Src="~/Employer/controls/addeditjob.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/controls/ckeditor/ckeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:addeditjob id="heAddEditJob" runat="server"/>
        </div>
    </div>
</asp:Content>

