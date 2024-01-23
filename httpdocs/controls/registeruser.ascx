<%@ Control Language="C#" AutoEventWireup="true" CodeFile="registeruser.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.registeruser" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<div class="form-group row">
    <div class="col-sm-12">
        <h1><asp:Label ID="lblMessage" runat="server"></asp:Label></h1>
    </div>
</div>

<asp:Panel ID="pnlForm" runat="server">
    <script type="text/javascript">
        jQuery(document).ready(function (jQuery) {
            registerUserOnLoadScript();
        });
                
        function registerUserOnLoadScript() {        
            
            jQuery('#<%= txtFirstName.ClientID %>').keypress(OnKeyPressRegisterUser);
            jQuery('#<%= txtLastName.ClientID %>').keypress(OnKeyPressRegisterUser);
            jQuery('#<%= chkbSaveCoverLetter.ClientID %>').keyup(OnKeyPressRegisterUser);
            jQuery('#<%= chkbSaveResume.ClientID %>').keyup(OnKeyPressRegisterUser);
            jQuery('#<%= txtEmail.ClientID %>').keypress(OnKeyPressRegisterUser);
            jQuery('#<%= txtPhone.ClientID %>').keypress(OnKeyPressRegisterUser);

            <% if(pnlRegisterChoice.Visible) { %>
            jQuery('#<%= rdbRegisterYes.ClientID %>').keypress(OnKeyPressRegisterUser);
            jQuery('#<%= rdbRegisterNo.ClientID %>').keypress(OnKeyPressRegisterUser);    
            <% } %>
            
            jQuery('#<%= chkbPublicResume.ClientID %>').keyup(OnKeyPressRegisterUser);
            jQuery('#<%= lstbUserCategories.ClientID %>').keyup(OnKeyPressRegisterUser);
            jQuery('#<%= chkbOkToEmail.ClientID %>').keyup(OnKeyPressRegisterUser);
            jQuery('#<%= txtUserPassword.ClientID %>').keypress(OnKeyPressRegisterUser);
            jQuery('#<%= txtUserPasswordRepeat.ClientID %>').keypress(OnKeyPressRegisterUser);
            
            jQuery('#<%= txtFirstName.ClientID %>').focus();
            jQuery('#<%= lstbUserCategories.ClientID %>').tooltip({ placement:"top", html:true, trigger: "focus" });
            jQuery('#<%= txtUserPassword.ClientID %>').tooltip({ placement:"top", html:true, trigger: "focus" });
        }

        function OnKeyPressRegisterUser(e)
        {
            if (e.which == 13) {
                jQuery('#<%= btnSubmit.ClientID %>').click();
                return false;
            }
        }
        
        function ChangeRegisterOption(value) {
            value = parseInt(value);
            if (value == 0) {
                jQuery('#<%= pnlRegister.ClientID %>').slideUp('slow');
            }
            else {
                jQuery('#<%= pnlRegister.ClientID %>').slideDown('slow');
            }
        }

        function ValidateFormSubmit() {
            if (<%= IsJobApplication.ToString().ToLower() %>) {
                if(jQuery("#<%= rdbRegisterNo.ClientID %>").is(':checked'))
                {
                    return Page_ClientValidate('Application');
                }
            }
            return Page_ClientValidate('Application') && Page_ClientValidate('RegisterUser');
        }
    </script>

    <div class="form-group row">
        <div class="col-sm-12">
            <h1><asp:Label ID="lblHeading" runat="server"></asp:Label></h1>
            <div class="help-block"><%= GetLocalResourceObject("strRequiredLegend").ToString() %></div>
        </div>
    </div>

    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="100" CssClass="form-control input-sm"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName"
                ValidationGroup="Application" meta:resourcekey="reqFirstName" Display="Dynamic" CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtLastName" runat="server" MaxLength="100" CssClass="form-control input-sm"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName"
                ValidationGroup="Application" meta:resourcekey="reqLastName" Display="Dynamic" CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:Label ID="lblCoverLetter" runat="server" meta:resourcekey="lblCoverLetter" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:TextBox id="txtCoverLetter" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
            <asp:RegularExpressionValidator ID="regCoverLetter" runat="server" ControlToValidate="txtCoverLetter" 
                ValidationGroup="Application" ValidationExpression="^.{0,25000}$" meta:resourcekey="regCoverLetter"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <asp:Panel ID="pnlSaveCoverLetter" runat="server" CssClass="form-group row">
        <div class="col-sm-12">
            <asp:CheckBox ID="chkbSaveCoverLetter" runat="server" meta:resourcekey="chkbSaveCoverLetter" CssClass="checkbox"/>
        </div>
    </asp:Panel>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:Label ID="lblResume" runat="server" meta:resourcekey="lblResume" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:TextBox id="txtResume" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
            <asp:RegularExpressionValidator ID="regResume" runat="server" ControlToValidate="txtResume" 
                ValidationGroup="Application" ValidationExpression="^.{0,25000}$" meta:resourcekey="regResume"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <asp:Panel ID="pnlSaveResume" runat="server" CssClass="form-group row">
        <div class="col-sm-12">
            <asp:CheckBox ID="chkbSaveResume" runat="server" meta:resourcekey="chkbSaveResume" CssClass="checkbox"/>
        </div>
    </asp:Panel>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" CssClass="form-control input-sm"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail"
                ValidationGroup="Application" meta:resourcekey="reqEmail" Display="Dynamic" CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail"
                ValidationGroup="Application" meta:resourcekey="regEmail" Display="Dynamic" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblPhone" runat="server" meta:resourcekey="lblPhone" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtPhone" runat="server" MaxLength="25" CssClass="form-control input-sm">
            </asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:CheckBox id="chkbResumeHelp" runat="server" meta:resourcekey="chkbResumeHelp" CssClass="checkbox"/>
        </div>
    </div>

    <asp:Panel ID="pnlRegisterChoice" runat="server">
        <div class="row">
            <div class="col-sm-12">
                <asp:Label ID="lblRegisterChoice" runat="server" meta:resourcekey="lblRegisterChoice" 
                    CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <asp:RadioButton id="rdbRegisterYes" runat="server" meta:resourcekey="rdbRegisterYes" GroupName="RegisterChoice" CssClass="radio"/>
                <asp:RadioButton id="rdbRegisterNo" runat="server" meta:resourcekey="rdbRegisterNo" GroupName="RegisterChoice" CssClass="radio"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlRegister" runat="server">
        <div class="form-group row">
            <div class="col-sm-12">
                <asp:CheckBox id="chkbPublicResume" runat="server" meta:resourcekey="chkbPublicResume" CssClass="checkbox"/>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblUserCategories" runat="server" meta:resourcekey="lblUserCategories" 
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:ListBox ID="lstbUserCategories" runat="server" DataValueField="CategoryId" DataTextField="Name" 
                    SelectionMode="Multiple" CssClass="form-control categories-listbox" Rows="6"
                    meta:resourcekey="lstbUserCategories"></asp:ListBox>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <asp:CheckBox id="chkbOkToEmail" runat="server" CssClass="checkbox" meta:resourcekey="chkbOkToEmail" Checked="true"/>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblUserPassword" runat="server" CssClass="control-label" 
                    meta:resourcekey="lblUserPassword"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtUserPassword" runat="server" ValidationGroup="RegisterUser" TextMode="Password" 
                    MaxLength="100" CssClass="form-control input-sm" meta:resourcekey="txtUserPassword"></asp:TextBox>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqUserPassword" runat="server" ControlToValidate="txtUserPassword"
                    meta:resourcekey="reqUserPassword" Display="Dynamic" ValidationGroup="RegisterUser"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regUserPassword" runat="server" ControlToValidate="txtUserPassword"
                    meta:resourcekey="regUserPassword" ValidationGroup="RegisterUser" Display="Dynamic"
                    ValidationExpression="^((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,})$" CssClass="text-danger">
                </asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblUserPasswordRepeat" runat="server" meta:resourcekey="lblUserPasswordRepeat"
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtUserPasswordRepeat" runat="server" ValidationGroup="RegisterUser" 
                    TextMode="Password" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqUserPasswordRepeat" runat="server" ControlToValidate="txtUserPasswordRepeat"
                    meta:resourcekey="reqUserPasswordRepeat" Display="Dynamic" ValidationGroup="RegisterUser"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="compUserPasswordRepeat" runat="server" ControlToValidate="txtUserPasswordRepeat"
                    ControlToCompare="txtUserPassword" meta:resourcekey="compUserPasswordRepeat" Display="Dynamic"
                    ValidationGroup="RegisterUser" CssClass="text-danger">
                </asp:CompareValidator>
            </div>
        </div>
    </asp:Panel>

    <he:formvalidator id="heFormValidator" runat="server"/>
    <div class="form-group row">
        <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit" OnCommand="btnSubmit_Click"
            OnClientClick="javascript:return ValidateFormSubmit();" CssClass="btn btn-primary"/>
    </div>
</asp:Panel>