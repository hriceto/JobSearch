<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resources.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-6"><h1><%= GetLocalResourceObject("strEmployerResources").ToString() %></h1><hr /></div>
        <div class="col-sm-6"><h1><%= GetLocalResourceObject("strJobSeekerResources").ToString() %></h1><hr /></div>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <ul>
                <li><a href="http://www.irs.gov/" target="_blank">Internal Revenue Service</a></li>
                <li><a href="http://www.revenue.state.il.us" target="_blank">Illinois Department of Revenue</a></li>
                <li><a href="http://www.illinois.gov/" target="_blank">Illinois Government</a></li>
                <li><a href="http://www.cookcountyil.gov/" target="_blank">Cook County Illinois</a></li>
            </ul>
        </div>
        <div class="col-sm-6">
            <ul>
                <li><a href="http://www.chipublib.org/" target="_blank">Chicago Public Library</a></li>
                <li><a href="http://dppl.org/" target="_blank">Des Plaines Public Library</a></li>
                <li><a href="http://work.illinois.gov/" target="_blank">State of Illinois Employment Opportunities</a></li>
                <li><a href="http://illinoisjoblink.com" target="_blank">Illinois Job Link</a></li>
                <li><a href="http://www.yourresumewiz.com/" target="_blank">Resume Critique and Services</a></li>
            </ul>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6"><h1><%= GetLocalResourceObject("strVendorResources").ToString() %></h1><hr /></div>
        <div class="col-sm-6"></div>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <ul>
                <li><a href="http://www.yourresumewiz.com/" target="_blank">YourResumeWiz</a> - Get a free professional critique from resume experts "YourResumeWiz" – a division of ClearPointHCO, LLC.</li>
            </ul>
        </div>
        <div class="col-sm-6"></div>
    </div>

    
</asp:Content>

