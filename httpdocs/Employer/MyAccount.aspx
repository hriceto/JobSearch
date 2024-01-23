<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyAccount.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.MyAccount" %>
<%@ Register TagPrefix="he" TagName="updatepassword" Src="~/controls/updatepassword.ascx" %>
<%@ Register TagPrefix="he" TagName="updatecompany" Src="~/Employer/controls/updatecompany.ascx" %>
<%@ Register TagPrefix="he" TagName="updateuser" Src="~/Employer/controls/updateuser.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        jQuery(document).ready(function (jQuery) {
            jQuery('#<%= liUpdateUser.ClientID %> a').on('shown.bs.tab', function (e) {
                SetFocus<%= heUpdateUser.ClientID %>();
            });
            jQuery('#<%= liUpdateCompany.ClientID %> a').on('shown.bs.tab', function (e) {
                SetFocus<%= heUpdateCompany.ClientID %>();
            });
            jQuery('#<%= liUpdatePassword.ClientID %> a').on('shown.bs.tab', function (e) {
                SetFocus<%= heUpdatePassword.ClientID %>();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-9">
            <ul class="nav nav-tabs">
                <li id="liUpdateUser" runat="server" class="active">
                    <a href="#<%= divUpdateUser.ClientID %>" data-toggle="tab"><%= GetLocalResourceObject("strUpdateProfile").ToString() %></a>
                </li>
                <li id="liUpdateCompany" runat="server">
                    <a href="#<%= divUpdateCompany.ClientID %>" data-toggle="tab"><%= GetLocalResourceObject("strUpdateCompany").ToString() %></a>
                </li>
                <li id="liUpdatePassword" runat="server">
                    <a href="#<%= divUpdatePassword.ClientID %>" data-toggle="tab"><%= GetLocalResourceObject("strUpdatePassword").ToString() %></a>
                </li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane fade in active" id="divUpdateUser" runat="server">
                    <br />
                    <he:updateuser id="heUpdateUser" runat="server"/>
                </div>
                <div class="tab-pane fade" id="divUpdateCompany" runat="server">
                    <br />
                    <he:updatecompany id="heUpdateCompany" runat="server"/>
                </div>
                <div class="tab-pane fade" id="divUpdatePassword" runat="server">
                    <br />
                    <he:updatepassword id="heUpdatePassword" runat="server"/>
                </div>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h2 class="panel-title"><asp:Label ID="lblAdditionalOptions" runat="server" meta:resourcekey="lblAdditionalOptions"></asp:Label></h2>
                </div>
                <div class="panel-body">
                    <p>
                        <asp:HyperLink ID="hypMyCoupons" runat="server" meta:resourcekey="hypMyCoupons"></asp:HyperLink>
                    </p>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>

