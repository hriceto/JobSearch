<%@ Control Language="C#" AutoEventWireup="true" CodeFile="login.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.login" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        loginOnLoadScript();
    });

    function loginOnLoadScript() {
        jQuery('#<%= txtUserName.ClientID %>').keypress(function (e) {
            if (e.which == 13) {
                jQuery('#<%= btnLogin.ClientID %>').click();
                return false;
            }
        });

        jQuery('#<%= txtPassword.ClientID %>').keypress(function (e) {
            if (e.which == 13) {
                jQuery('#<%= btnLogin.ClientID %>').click();
                return false;
            }
        });

        jQuery('#<%= txtUserName.ClientID %>').focus();
    }

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(loginOnLoadScript);
</script>

<div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Label ID="lblUserName" runat="server" meta:ResourceKey="lblUserName" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtUserName" runat="server" ValidationGroup="Login" CssClass="form-control"
                MaxLength="100">
            </asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqUserName" runat="server" ValidationGroup="Login"
                ControlToValidate="txtUserName" meta:resourcekey="reqUserName" Display="Dynamic" 
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regUserName" runat="server" ValidationGroup="Login"
                ControlToValidate="txtUserName" meta:resourcekey="regUserName"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Label ID="lblPassword" runat="server" meta:ResourceKey="lblPassword" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"
                MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqPassword" runat="server" ValidationGroup="Login"
                ControlToValidate="txtPassword" meta:resourcekey="reqPassword" Display="Dynamic"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Button id="btnLogin" runat="server" OnClick="btnLogin_Click" meta:resourcekey="btnLogin" 
            ValidationGroup="Login" CssClass="btn btn-primary"/>
        </div>
        <div class="col-sm-6">
            <asp:HyperLink id="hypForgotPassword" runat="server" meta:resourcekey="hypForgotPassword" CssClass="text-muted pull-right"></asp:HyperLink><br />
            <asp:HyperLink id="hypRegisterJobSeeker" runat="server" meta:resourcekey="hypRegisterJobSeeker" CssClass="text-muted pull-right"></asp:HyperLink><br />
            <asp:HyperLink id="hypRegisterEmployer" runat="server" meta:resourcekey="hypRegisterEmployer" CssClass="text-muted pull-right"></asp:HyperLink><br />
        </div>
        <div class="col-sm-4">
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:Panel id="pnlMessage" runat="server" CssClass="alert alert-dismissable alert-warning" Visible="false">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:Panel>
        </div>
    </div>
</div>
