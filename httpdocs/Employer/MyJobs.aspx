<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyJobs.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.MyJobs" %>
<%@ Register TagPrefix="he" TagName="myjobs" Src="~/Employer/controls/myjobs.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <he:myjobs id="heMyJobs" runat="server"/>
        </div>
    </div>
</asp:Content>

