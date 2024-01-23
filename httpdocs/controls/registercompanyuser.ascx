<%@ Control Language="C#" AutoEventWireup="true" CodeFile="registercompanyuser.ascx.cs"
    Inherits="HristoEvtimov.Websites.Work.Web.Controls.registercompanyuser" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        registerCompanyUserOnLoadScript();
    });

    function registerCompanyUserOnLoadScript() {        
        <% if(pnlStep1.Visible) { %>
        jQuery('#<%= txtUserEmail.ClientID %>').keypress(OnEnterKeyChangeCompany1);
        jQuery('#<%= txtUserEmail.ClientID %>').focus();
        jQuery('#<%= txtUserFirstName.ClientID %>').keypress(OnEnterKeyChangeCompany1);
        jQuery('#<%= txtUserLastName.ClientID %>').keypress(OnEnterKeyChangeCompany1);
        jQuery('#<%= txtUserPassword.ClientID %>').keypress(OnEnterKeyChangeCompany1);
        jQuery('#<%= txtUserPasswordRepeat.ClientID %>').keypress(OnEnterKeyChangeCompany1);
        jQuery('#<%= chkbOkToEmail.ClientID %>').keyup(OnEnterKeyChangeCompany1);
        jQuery('#<%= txtUserPassword.ClientID %>').tooltip({ placement:"top", html:true, trigger: "focus" });
        <% } %>

        <% if(pnlStep2a.Visible) { %>
        jQuery('#<%= rbtnlCompanyIsRecruiter.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyName.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyAddress1.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyAddress2.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyCity.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= ddlCompanyState.ClientID %>').keyup(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyZip.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= ddlCompanyCountry.ClientID %>').keyup(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyPhoneNumber.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyWebsite.ClientID %>').keypress(OnEnterKeyChangeCompany2a);
        jQuery('#<%= txtCompanyName.ClientID %>').focus();
        jQuery('#<%= txtCompanyZip.ClientID %>').tooltip({ placement:"top", html:true, trigger: "focus" });
        <% } %>
    }

    function OnEnterKeyChangeCompany1(e)
    {
        if (e.which == 13) {
            jQuery('#<%= btnNext1.ClientID %>').click();
            return false;
        }
    }

    function OnEnterKeyChangeCompany2a(e)
    {
        if (e.which == 13) {
            jQuery('#<%= btnSubmit2a.ClientID %>').click();
            return false;
        }
    }

    function ChangeCompanyApplicationOption(control) {
        if (jQuery(control).is(":checked")) {
            jQuery('#<%= pnlCompanyApplication.ClientID %>').slideDown('slow');
        }
        else {
            jQuery('#<%= pnlCompanyApplication.ClientID %>').slideUp('slow');
        }
    }

</script>

<asp:Panel ID="pnlStep1" runat="server" Visible="true">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2><%= GetLocalResourceObject("strStep1Header").ToString() %></h2>
            <strong><%= GetLocalResourceObject("strStep1Step").ToString() %></strong>&nbsp;&nbsp;<span class="glyphicon glyphicon-chevron-right"></span>&nbsp;&nbsp;<span class="text-muted"><%= GetLocalResourceObject("strStep2Step").ToString() %></span>
            <br /><br />
            <div class="help-block"><%= GetLocalResourceObject("strRequiredLegend").ToString() %></div>
            <div><%= GetLocalResourceObject("strStep1HeaderDescription").ToString() %></div>
            <br />
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblUserEmail" runat="server" meta:resourcekey="lblUserEmail" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtUserEmail" runat="server" ValidationGroup="User" CssClass="form-control input-sm"
                MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqUserEmail" runat="server" ControlToValidate="txtUserEmail"
                meta:resourcekey="reqUserEmail" Display="Dynamic" ValidationGroup="User" CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regUserEmail" runat="server" ControlToValidate="txtUserEmail" 
                meta:resourcekey="regUserEmail" Display="Dynamic" ValidationGroup="User" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblUserFirstName" runat="server" meta:resourcekey="lblUserFirstName" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtUserFirstName" runat="server" ValidationGroup="User" 
                CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqUserFirstName" runat="server" ControlToValidate="txtUserFirstName"
                meta:resourcekey="reqUserFirstName" Display="Dynamic" ValidationGroup="User" CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblUserLastName" runat="server" meta:resourcekey="lblUserLastName" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtUserLastName" runat="server" ValidationGroup="User" CssClass="form-control input-sm"
                MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqUserLastName" runat="server" ControlToValidate="txtUserLastName"
                meta:resourcekey="reqUserLastName" Display="Dynamic" ValidationGroup="User" CssClass="text-danger">
            </asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblUserPassword" runat="server" meta:resourcekey="lblUserPassword" CssClass="control-label"></asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtUserPassword" runat="server" ValidationGroup="User" TextMode="Password"
                CssClass="form-control input-sm" meta:resourcekey="txtUserPassword" MaxLength="100">
            </asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqUserPassword" runat="server" ControlToValidate="txtUserPassword"
                meta:resourcekey="reqUserPassword" Display="Dynamic" ValidationGroup="User" CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regUserPassword" runat="server" ControlToValidate="txtUserPassword"
                meta:resourcekey="regUserPassword" ValidationGroup="User" Display="Dynamic"
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
            <asp:TextBox ID="txtUserPasswordRepeat" runat="server" ValidationGroup="User" TextMode="Password"
                CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RequiredFieldValidator ID="reqUserPasswordRepeat" runat="server" ControlToValidate="txtUserPasswordRepeat"
                meta:resourcekey="reqUserPasswordRepeat" Display="Dynamic" ValidationGroup="User"
                CssClass="text-danger">
            </asp:RequiredFieldValidator>
            <asp:CompareValidator ID="compUserPasswordRepeat" runat="server" ControlToValidate="txtUserPasswordRepeat"
                ControlToCompare="txtUserPassword" meta:resourcekey="compUserPasswordRepeat" Display="Dynamic"
                ValidationGroup="User" CssClass="text-danger">
            </asp:CompareValidator>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:CheckBox ID="chkbOkToEmail" runat="server" meta:resourcekey="chkbOkToEmail" CssClass="checkbox" Checked="true"/>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-2">
            <asp:Button ID="btnNext1" runat="server" meta:resourcekey="btnNext1" OnCommand="btnStep1Next_Command"
                CommandArgument="2" CausesValidation="true" ValidationGroup="User" CssClass="btn btn-default"/>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="pnlStep2" runat="server" Visible="false">
    <asp:Panel ID="pnlStep2a" runat="server">
        <div class="form-group row">
            <div class="col-sm-12">
                <h2><%= GetLocalResourceObject("strStep2aHeader").ToString() %></h2>
                <span class="text-success"><span class="glyphicon glyphicon-ok-circle"></span>&nbsp;<%= GetLocalResourceObject("strStep1Step").ToString() %></span>&nbsp;&nbsp;<span class="glyphicon glyphicon-chevron-right"></span>&nbsp;&nbsp;<strong><%= GetLocalResourceObject("strStep2Step").ToString() %></strong>
                <br /><br />
                <div class="help-block"><%= GetLocalResourceObject("strRequiredLegend").ToString() %></div>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyName" runat="server" meta:resourcekey="lblCompanyName" CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtCompanyName" runat="server" ValidationGroup="Company" 
                    CssClass="form-control input-sm" MaxLength="256"></asp:TextBox>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqCompanyName" runat="server" ControlToValidate="txtCompanyName"
                    meta:resourcekey="reqCompanyName" Display="Dynamic" ValidationGroup="Company" CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:Label ID="lblCompanyIsRecruiter" runat="server" meta:resourcekey="lblCompanyIsRecruiter" 
                    CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-2">
                <asp:RadioButtonList ID="rbtnlCompanyIsRecruiter" runat="server" CssClass="radio" RepeatLayout="Flow">
                    <asp:ListItem Value="true" meta:resourcekey="rbtnlCompanyIsRecruiter_Item1"></asp:ListItem>
                    <asp:ListItem Value="false" meta:resourcekey="rbtnlCompanyIsRecruiter_Item2"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-sm-10">
                <asp:RequiredFieldValidator ID="reqCompanyIsRecruiter" runat="server" ControlToValidate="rbtnlCompanyIsRecruiter"
                    meta:resourcekey="reqCompanyIsRecruiter" Display="Dynamic" ValidationGroup="Company"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyAddress1" runat="server" meta:resourcekey="lblCompanyAddress1" 
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtCompanyAddress1" runat="server" ValidationGroup="Company" 
                    CssClass="form-control input-sm" MaxLength="256"></asp:TextBox>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqCompanyAddress1" runat="server" ControlToValidate="txtCompanyAddress1"
                    meta:resourcekey="reqCompanyAddress1" Display="Dynamic" ValidationGroup="Company"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyAddress2" runat="server" meta:resourcekey="lblCompanyAddress2" 
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtCompanyAddress2" runat="server" ValidationGroup="Company" 
                    CssClass="form-control input-sm" MaxLength="256"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyCity" runat="server" meta:resourcekey="lblCompanyCity" CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtCompanyCity" runat="server" ValidationGroup="Company" 
                    CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqCompanyCity" runat="server" ControlToValidate="txtCompanyCity"
                    meta:resourcekey="reqCompanyCity" Display="Dynamic" ValidationGroup="Company" 
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyState" runat="server" meta:resourcekey="lblCompanyState" CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:DropDownList ID="ddlCompanyState" runat="server" ValidationGroup="Company" CssClass="form-control input-sm"
                    DataValueField="StateCode" DataTextField="StateName">
                </asp:DropDownList>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqCompanyState" runat="server" ControlToValidate="ddlCompanyState"
                    meta:resourcekey="reqCompanyState" Display="Dynamic" ValidationGroup="Company"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyZip" runat="server" meta:resourcekey="lblCompanyZip" CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtCompanyZip" runat="server" ValidationGroup="Company" 
                    CssClass="form-control input-sm" meta:resourcekey="txtCompanyZip"
                    MaxLength="12"></asp:TextBox>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqCompanyZip" runat="server" ControlToValidate="txtCompanyZip"
                    meta:resourcekey="reqCompanyZip" Display="Dynamic" ValidationGroup="Company"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regCompanyZip" runat="server" ControlToValidate="txtCompanyZip"
                    meta:resourcekey="regCompanyZip" Display="Dynamic" ValidationGroup="Company" 
                    ValidationExpression="[0-9]{5}(-[0-9]{4})?" CssClass="text-danger">
                </asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyCountry" runat="server" meta:resourcekey="lblCompanyCountry" CssClass="control-label">
                </asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:DropDownList ID="ddlCompanyCountry" runat="server" ValidationGroup="Company" CssClass="form-control input-sm"
                    DataValueField="CountryCode" DataTextField="CountryName">
                </asp:DropDownList>
            </div>
            <div class="col-sm-3">
                <asp:RequiredFieldValidator ID="reqCompanyCountry" runat="server" ControlToValidate="ddlCompanyCountry"
                    meta:resourcekey="reqCompanyCountry" Display="Dynamic" ValidationGroup="Company"
                    CssClass="text-danger">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyPhoneNumber" runat="server" meta:resourcekey="lblCompanyPhoneNumber"
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtCompanyPhoneNumber" runat="server" ValidationGroup="Company" 
                    CssClass="form-control input-sm" MaxLength="25">
                </asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Label ID="lblCompanyWebsite" runat="server" meta:resourcekey="lblCompanyWebsite" 
                    CssClass="control-label"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtCompanyWebsite" runat="server" ValidationGroup="Company" 
                    CssClass="form-control input-sm" MaxLength="50"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <asp:CheckBox ID="chkbCompanyApplication" runat="server" meta:resourcekey="chkbCompanyApplication" CssClass="checkbox" 
                    Checked="false" onclick="javascript:ChangeCompanyApplicationOption(this)" />
            </div>
        </div>
        <asp:Panel ID="pnlCompanyApplication" runat="server">
            <div class="form-group row">
                <div class="col-sm-12">
                    <div class="help-block"><%= GetLocalResourceObject("strCompanyApplicationHelp").ToString() %></div>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-12">
                    <div class="col-sm-3">
                        <asp:Label ID="lblCompanyNumberOfEmployees" runat="server" meta:resourcekey="lblCompanyNumberOfEmployees" 
                            CssClass="control-label"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <asp:DropDownList ID="ddlCompanyNumberOfEmployees" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Text="1 - 50" Value="1 - 50"></asp:ListItem>
                            <asp:ListItem Text="51 - 100" Value="51 - 100"></asp:ListItem>
                            <asp:ListItem Text="101 - 250" Value="101 - 250"></asp:ListItem>
                            <asp:ListItem Text="251 - 500" Value="251 - 500"></asp:ListItem>
                            <asp:ListItem Text="501 - 1000" Value="501 - 1000"></asp:ListItem>
                            <asp:ListItem Text="1000+" Value="1000+"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-12">
                    <div class="col-sm-3">
                        <asp:Label ID="lblCompanyPostingsPerYear" runat="server" meta:resourcekey="lblCompanyPostingsPerYear" 
                            CssClass="control-label"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlCompanyPostingsPerYear" runat="server" CssClass="form-control input-sm">
                            <asp:ListItem Text="1 - 5" Value="1 - 5"></asp:ListItem>
                            <asp:ListItem Text="6 - 15" Value="6 - 15"></asp:ListItem>
                            <asp:ListItem Text="16 - 25" Value="16 - 25"></asp:ListItem>
                            <asp:ListItem Text="25+" Value="25+"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <he:formvalidator id="heFormValidator" runat="server"/>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Button ID="btnSubmit2a" runat="server" meta:resourcekey="btnSubmit" OnCommand="btnSubmit_Click"
                    CssClass="btn btn-primary" ValidationGroup="Company" />
            </div>
            <div class="col-sm-3">
                <asp:Button ID="btnPrevious2a" runat="server" meta:resourcekey="btnPrevious2" OnCommand="btnNextPrevious_Command"
                    CssClass="btn" CommandArgument="1" CausesValidation="false" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlStep2b" runat="server">
        <div class="form-group row">
            <div class="col-sm-12">
                <h2><%= GetLocalResourceObject("strStep2bHeader").ToString() %></h2>
                <span class="text-success"><span class="glyphicon glyphicon-ok-circle"></span>&nbsp;<%= GetLocalResourceObject("strStep1Step").ToString() %></span>&nbsp;&nbsp;<span class="glyphicon glyphicon-chevron-right"></span>&nbsp;&nbsp;<strong><%= GetLocalResourceObject("strStep2Step").ToString() %></strong>
                <br /><br />
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <%= GetLocalResourceObject("strExistingCompanyName").ToString() %>
            </div>
            <div class="col-sm-9">
                <asp:Label ID="lblExistingCompanyName" runat="server" CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <%= GetLocalResourceObject("strExistingCompanyDomain").ToString() %>
            </div>
            <div class="col-sm-9">
                <asp:Label ID="lblExistingCompanyDomain" runat="server" CssClass="control-label"></asp:Label>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-3">
                <asp:Button ID="btnSubmit2b" runat="server" meta:resourcekey="btnSubmit" OnCommand="btnSubmit_Click"
                    CssClass="btn btn-primary" CausesValidation="false" />
            </div>
            <div class="col-sm-3">
                <asp:Button ID="btnPrevious2b" runat="server" meta:resourcekey="btnPrevious2" OnCommand="btnNextPrevious_Command"
                    CssClass="btn" CommandArgument="1" CausesValidation="false"/>
            </div>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:Panel ID="pnlStep3" runat="server" Visible="false">
    <div class="form-group row">
        <div class="col-sm-12">
            <h2 class="text-center"><%= GetLocalResourceObject("strStep3Header").ToString() %></h2>
        </div>
    </div>
</asp:Panel>
