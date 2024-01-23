<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Employers.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta name="description" content="<%= GetGlobalResourceObject("PageDescriptions", "strEmployers").ToString()%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <div class="jumbotron">
                <h1><asp:Label ID="lblHeader1" runat="server" meta:resourcekey="lblHeader1"></asp:Label></h1>
                <br />
                <h2><asp:Label ID="lblHeader2" runat="server" meta:resourcekey="lblHeader2"></asp:Label></h2>
                <br />
                <asp:HyperLink ID="hypRegister" runat="server" meta:resourcekey="hypRegister" CssClass="btn btn-primary"></asp:HyperLink>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-7">
            <div class="row">
                <div class="col-sm-12">
                    <br />
                    <h2><%= GetLocalResourceObject("strWeOffer").ToString() %></h2>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <ul>
                        <li><%= GetLocalResourceObject("strBenefit1").ToString() %></li>
                        <li><%= GetLocalResourceObject("strBenefit2").ToString()%></li>
                        <li><%= GetLocalResourceObject("strBenefit6").ToString()%></li>
                        <li><%= GetLocalResourceObject("strBenefit7").ToString()%></li>
                        <li><%= GetLocalResourceObject("strBenefit8").ToString()%></li>
                        <li><%= GetLocalResourceObject("strBenefit3").ToString()%></li>
                        <li><%= GetLocalResourceObject("strBenefit4").ToString()%></li>
                        <li><%= GetLocalResourceObject("strBenefit5").ToString()%></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="col-sm-5">
            <br />
            <h2><%= GetLocalResourceObject("strAcceptingApplications").ToString() %></h2>
            <div>
                <asp:Label ID="lblAcceptingApplications" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblPublishWithUs" runat="server"></asp:Label></h2>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <br />
            <asp:HyperLink ID="hypRegister2" runat="server" meta:resourcekey="hypRegister" CssClass="btn btn-primary"></asp:HyperLink>
        </div>
    </div>
</asp:Content>

