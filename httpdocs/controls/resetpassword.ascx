<%@ Control Language="C#" AutoEventWireup="true" CodeFile="resetpassword.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.resetpassword" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        resetPasswordOnLoadScript();
    });

    function resetPasswordOnLoadScript() {
        jQuery('#<%= txtRequestResetPasswordEmail.ClientID %>').keypress(OnEnterKeyRequestPasswordChange);
        jQuery('#<%= txtRequestResetPasswordFirstName.ClientID %>').keypress(OnEnterKeyRequestPasswordChange);

        jQuery('#<%= txtChangePasswordEmail.ClientID %>').keypress(OnEnterKeyChangePassword);
        jQuery('#<%= txtChangePasswordFirstName.ClientID %>').keypress(OnEnterKeyChangePassword);
        jQuery('#<%= txtChangePasswordPassword.ClientID %>').keypress(OnEnterKeyChangePassword);
        jQuery('#<%= txtChangePasswordRepeatPassword.ClientID %>').keypress(OnEnterKeyChangePassword);
        
        <% if(pnlRequestResetPassword.Visible) { %>
        jQuery('#<%= txtRequestResetPasswordEmail.ClientID %>').focus();
        <% } %>
        <% if(pnlChangePassword.Visible) { %>
        jQuery('#<%= txtChangePasswordEmail.ClientID %>').focus();
        jQuery('#<%= txtChangePasswordPassword.ClientID %>').tooltip({ placement:"top", html:true, trigger: "focus" });
        <% } %>
    }

    function OnEnterKeyRequestPasswordChange(e)
    {
        if (e.which == 13) {
            jQuery('#<%= btnRequestResetPassword.ClientID %>').click();
            return false;
        }
    }

    function OnEnterKeyChangePassword(e)
    {
        if (e.which == 13) {
            jQuery('#<%= btnChangePassword.ClientID %>').click();
            return false;
        }
    }

</script>
    
<asp:Panel ID="pnlRequestResetPassword" runat="server">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblRequestResetPasswordHeader" runat="server" CssClass="control-label" 
                meta:resourcekey="lblRequestResetPasswordHeader"></asp:Label></h2>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Label ID="lblRequestResetPasswordEmail" runat="server" CssClass="control-label" 
                meta:resourcekey="lblRequestResetPasswordEmail"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtRequestResetPasswordEmail" runat="server" CssClass="form-control"
                ValidationGroup="ResetPassword" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqRequestResetPasswordEmail" runat="server" ControlToValidate="txtRequestResetPasswordEmail"
                meta:resourcekey="reqRequestResetPasswordEmail" Display="Dynamic" ValidationGroup="ResetPassword"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regRequestResetPasswordEmail" runat="server" ControlToValidate="txtRequestResetPasswordEmail" 
                meta:resourcekey="regRequestResetPasswordEmail" ValidationGroup="ResetPassword" 
                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Label ID="lblRequestResetPasswordFirstName" runat="server" CssClass="control-label" 
                meta:resourcekey="lblRequestResetPasswordFirstName"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtRequestResetPasswordFirstName" runat="server" CssClass="form-control" 
                ValidationGroup="ResetPassword" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqRequestResetPasswordFirstName" runat="server" ControlToValidate="txtRequestResetPasswordFirstName"
                meta:resourcekey="reqRequestResetPasswordFirstName" Display="Dynamic" ValidationGroup="ResetPassword"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Button ID="btnRequestResetPassword" runat="server" meta:resourcekey="btnRequestResetPassword" OnCommand="btnRequestResetPassword_Click"
                CssClass="btn btn-primary" ValidationGroup="ResetPassword" />
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlChangePassword" runat="server" Visible="false">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><asp:Label ID="lblChangePasswordHeader" runat="server" CssClass="control-label" 
                meta:resourcekey="lblChangePasswordHeader"></asp:Label></h2>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblChangePasswordEmail" runat="server" CssClass="control-label" 
                meta:resourcekey="lblChangePasswordEmail"></asp:Label>
        </div>
        <div class="col-sm-5">
            <asp:TextBox ID="txtChangePasswordEmail" runat="server" CssClass="form-control" 
                ValidationGroup="ChangePassword" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqChangePasswordEmail" runat="server" ControlToValidate="txtChangePasswordEmail"
                meta:resourcekey="reqChangePasswordEmail" Display="Dynamic" ValidationGroup="ChangePassword"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regChangePasswordEmail" runat="server" ControlToValidate="txtChangePasswordEmail" 
                meta:resourcekey="regChangePasswordEmail" ValidationGroup="ChangePassword" 
                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblChangePasswordFirstName" runat="server" CssClass="control-label" 
                meta:resourcekey="lblChangePasswordFirstName"></asp:Label>
        </div>
        <div class="col-sm-5">
            <asp:TextBox ID="txtChangePasswordFirstName" runat="server" CssClass="form-control"
                ValidationGroup="ChangePassword" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqChangePasswordFirstName" runat="server" ControlToValidate="txtChangePasswordFirstName"
                meta:resourcekey="reqChangePasswordFirstName" Display="Dynamic" ValidationGroup="ChangePassword"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>        
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblChangePasswordPassword" runat="server" CssClass="control-label" 
                meta:resourcekey="lblChangePasswordPassword"></asp:Label>
        </div>
        <div class="col-sm-5">
            <asp:TextBox ID="txtChangePasswordPassword" runat="server" ValidationGroup="ChangePassword" 
                CssClass="form-control" TextMode="Password" meta:resourcekey="txtChangePasswordPassword"
                MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqChangePasswordPassword" runat="server" ControlToValidate="txtChangePasswordPassword"
                meta:resourcekey="reqChangePasswordPassword" Display="Dynamic" ValidationGroup="ChangePassword"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regChangePasswordPassword" runat="server" ControlToValidate="txtChangePasswordPassword"
                meta:resourcekey="regChangePasswordPassword" ValidationGroup="ChangePassword" CssClass="text-danger"
                Display="Dynamic" ValidationExpression="^((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,})$">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblChangePasswordRepeatPassword" runat="server" CssClass="control-label"
                meta:resourcekey="lblChangePasswordRepeatPassword"></asp:Label>
        </div>
        <div class="col-sm-5">
            <asp:TextBox ID="txtChangePasswordRepeatPassword" runat="server" ValidationGroup="ChangePassword" 
                CssClass="form-control" TextMode="Password" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-4">
            <asp:RequiredFieldValidator ID="reqChangePasswordRepeatPassword" runat="server" ControlToValidate="txtChangePasswordRepeatPassword"
                meta:resourcekey="reqChangePasswordRepeatPassword" Display="Dynamic" ValidationGroup="ChangePassword"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:CompareValidator ID="compChangePasswordRepeatPassword" runat="server" ControlToValidate="txtChangePasswordRepeatPassword"
                ControlToCompare="txtChangePasswordPassword" meta:resourcekey="compChangePasswordRepeatPassword" 
                Display="Dynamic" ValidationGroup="ChangePassword" CssClass="text-danger">
            </asp:CompareValidator>
        </div>
    </div>

    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Button ID="btnChangePassword" runat="server" meta:resourcekey="btnChangePassword" OnCommand="btnChangePassword_Click"
                CssClass="btn btn-primary" ValidationGroup="ChangePassword" />
        </div>
    </div>
</asp:Panel>

<he:formvalidator id="heFormValidator" runat="server"/>