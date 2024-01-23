<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PublishJob.aspx.cs" Inherits="Employer_PublishJob" %>
<%@ Register TagPrefix="he" TagName="publishjob" Src="~/Employer/controls/publishjob.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/js/bootstrap-datepicker.js" type="text/javascript" language="javascript"></script>
    <link rel="Stylesheet" type="text/css" href="/css/datepicker3.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:publishjob id="hePublishJob" runat="server"/>
        </div>
    </div>
</asp:Content>

