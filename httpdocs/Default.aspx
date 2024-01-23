<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Default" %>
<%@ Register Src="~/controls/jobsearchinputhomepage.ascx" TagPrefix="he" TagName="jobsearchinputhomepage" %>
<%@ Register Src="~/controls/latestjobs.ascx" TagPrefix="he" TagName="latestjobs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strDefault").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <div class="jumbotron">
                <h1><asp:Label ID="lblHeader1" runat="server"></asp:Label></h1>
                <p><asp:Label ID="lblHeader2" runat="server"></asp:Label></p>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <h2 class="text-center"><asp:Label ID="lblHeaderEmployer" runat="server" meta:resourcekey="lblHeaderEmployer"></asp:Label></h2>
            <hr />
            <p>
                <asp:Label ID="lblEmployers" runat="server"></asp:Label>
            </p>
            <div class="text-center">
                <asp:HyperLink ID="hypEmployerRegistration" runat="server" meta:resourcekey="hypEmployerRegistration" 
                    CssClass="btn btn-primary"></asp:HyperLink>
            </div>
            <br />
            <p>
                <asp:Label ID="lblEmployersPricing" runat="server"></asp:Label>
            </p>
        </div>
        <div class="col-sm-1">
        </div>
        <div class="col-sm-3">
            <h2 class="text-center"><asp:Label ID="lblHeaderJobSeeker" runat="server" meta:resourcekey="lblHeaderJobSeeker"></asp:Label></h2>
            <hr />
            <p>
                <asp:Label ID="lblJobSeekers" runat="server" meta:resourcekey="lblJobSeekers"></asp:Label>
            </p>
            <div>
                <he:jobsearchinputhomepage id="heJobSearchInputHomepage" runat="server"/>
            </div>

            <div class="text-center">
                <asp:HyperLink ID="hypJobSeekerRegistration" runat="server" meta:resourcekey="hypJobSeekerRegistration" 
                    CssClass="btn btn-link"></asp:HyperLink>
            </div>
        </div>
        <div class="col-sm-1">
        </div>
        <div class="col-sm-4">
            <h2 class="text-center"><asp:Label ID="lblLatestJobs" runat="server" meta:resourcekey="lblLatestJobs"></asp:Label></h2>
            <hr />
            <he:latestjobs id="heLatestJobs" runat="server" NumberOfItems="5"></he:latestjobs>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-1"></div>
        <div class="col-sm-6">
            <br />
            <asp:Label ID="lblLogin" runat="server"></asp:Label>
        </div>
        <div class="col-sm-5"></div>
    </div>
</asp:Content>

