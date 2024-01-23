<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyAccount.aspx.cs" Inherits="HristoEvtimov.Websites.Work.Web.JobSeeker.MyAccount" ValidateRequest="false"%>
<%@ Register TagPrefix="he" TagName="updatepassword" Src="~/controls/updatepassword.ascx" %>
<%@ Register TagPrefix="he" TagName="updateuser" Src="controls/updateuser.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/controls/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        jQuery(document).ready(function (jQuery) {
            jQuery('#<%= liUpdateUser.ClientID %> a').on('shown.bs.tab', function (e) {
                SetFocus<%= heUpdateUser.ClientID %>();
            });
            jQuery('#<%= liUpdatePassword.ClientID %> a').on('shown.bs.tab', function (e) {
                SetFocus<%= heUpdatePassword.ClientID %>();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPanel" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <ul class="nav nav-tabs">
                <li id="liUpdateUser" runat="server" class="active">
                    <a href="#<%= divUpdateUser.ClientID %>" data-toggle="tab">
                        <%= GetLocalResourceObject("strUpdateProfile").ToString() %>
                    </a>
                </li>
                <li id="liUpdatePassword" runat="server">
                    <a href="#<%= divUpdatePassword.ClientID %>" data-toggle="tab">
                        <%= GetLocalResourceObject("strUpdatePassword").ToString() %>
                    </a>
                </li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane fade in active" id="divUpdateUser" runat="server">
                    <br />
                    <he:updateuser id="heUpdateUser" runat="server"/>
                </div>
                <div class="tab-pane fade" id="divUpdatePassword" runat="server">
                    <br />
                    <he:updatepassword id="heUpdatePassword" runat="server"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

