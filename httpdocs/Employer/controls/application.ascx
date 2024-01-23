<%@ Control Language="C#" AutoEventWireup="true" CodeFile="application.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.application" %>

<div class="form-group row">
    <div class="col-sm-12">
        <h1><%= GetLocalResourceObject("strHeading").ToString() %></h1>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <strong><asp:Label ID="lblJobTitleLabel" runat="server" meta:resourcekey="lblJobTitleLabel" 
            CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblJobTitle" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <strong><asp:Label ID="lblJobApplicantNameLabel" runat="server" meta:resourcekey="lblJobApplicantNameLabel" 
            CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblJobApplicantName" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <strong><asp:Label ID="lblJobApplicationDateLabel" runat="server" meta:resourcekey="lblJobApplicationDateLabel" 
            CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblJobApplicationDate" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <strong><asp:Label ID="lblJobApplicantEmailLabel" runat="server" meta:resourcekey="lblJobApplicantEmailLabel" 
            CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblJobApplicantEmail" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-3">
        <strong><asp:Label ID="lblJobApplicantPhoneLabel" runat="server" meta:resourcekey="lblJobApplicantPhoneLabel" 
            CssClass="control-label"></asp:Label></strong>
    </div>
    <div class="col-sm-9">
        <asp:Label ID="lblJobApplicantPhone" runat="server" CssClass="control-label"></asp:Label>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-12">
        <strong><asp:Label ID="lblCoverLetterLabel" runat="server" meta:resourcekey="lblCoverLetterLabel" 
            CssClass="control-label"></asp:Label></strong>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:Label ID="lblCoverLetter" runat="server"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <strong><asp:Label ID="lblResumeLabel" runat="server" meta:resourcekey="lblResumeLabel" CssClass="control-label"></asp:Label></strong>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:Label ID="lblResume" runat="server"></asp:Label>
    </div>
</div>