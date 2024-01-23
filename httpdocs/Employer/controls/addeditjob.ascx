<%@ Control Language="C#" AutoEventWireup="true" CodeFile="addeditjob.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.addeditjob" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        addEditJobOnLoadScript();
    });

    function addEditJobOnLoadScript() {
        jQuery('#<%= txtJobTitle.ClientID %>').keypress(OnKeyAddEditJob);
        jQuery('#<%= txtPosition.ClientID %>').keypress(OnKeyAddEditJob);
        jQuery('#<%= txtJobLocation.ClientID %>').keypress(OnKeyAddEditJob);
        jQuery('#<%= txtJobZip.ClientID %>').keypress(OnKeyAddEditJob);
        jQuery('#<%= ddlEmploymentType.ClientID %>').keyup(OnKeyAddEditJob);
        jQuery('#<%= lstCategories.ClientID %>').keyup(OnKeyAddEditJob);

        jQuery('#<%= txtJobTitle.ClientID %>').focus();
        jQuery('#<%= txtJobLocation.ClientID %>').tooltip({ placement: "top", html: true, trigger: "focus" });
        jQuery('#<%= txtJobZip.ClientID %>').tooltip({ placement: "top", html: true, trigger: "focus" });
        jQuery('#<%= lstCategories.ClientID %>').tooltip({ placement: "top", html: true, trigger: "focus" });
    }

    function OnKeyAddEditJob(e) {
        if (e.which == 13) {
            jQuery('#<%= btnAddUpdatePublish.ClientID %>').click();
            return false;
        }
    }
    
    function showTextbox(showTextboxContainer, textboxContainer) {
        jQuery('#' + showTextboxContainer).slideUp({ duration: 900 });
        jQuery('#' + textboxContainer).slideDown({duration:900});
    }
</script>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblJobTitle" runat="server" meta:resourcekey="lblJobTitle" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtJobTitle" runat="server" MaxLength="256" ValidationGroup="AddUpdateJob" 
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqJobTitle" runat="server" ControlToValidate="txtJobTitle"
            ValidationGroup="AddUpdateJob" meta:resourcekey="reqJobTitle" Display="Dynamic" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>    

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblJobDescription" runat="server" meta:resourcekey="lblJobDescription" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:TextBox id="txtJobDescription" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regJobDescription" runat="server" ControlToValidate="txtJobDescription" 
            ValidationGroup="AddUpdateJob" ValidationExpression="^.{0,25000}$" meta:resourcekey="regJobDescription"
            CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>

<div id="divShowJobRequirements" runat="server" class="form-group row">
    <div class="col-sm-12">
        <a href="javascript:void(0);" onclick="javascript:showTextbox('<%= divShowJobRequirements.ClientID %>', '<%= divJobRequirements.ClientID %>');"><%= GetLocalResourceObject("strShowRequirements").ToString() %></a>
    </div>
</div>
<div id="divJobRequirements" runat="server">
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobRequirements" runat="server" meta:resourcekey="lblJobRequirements" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:TextBox id="txtJobRequirements" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
            <asp:RegularExpressionValidator ID="regJobRequirements" runat="server" ControlToValidate="txtJobRequirements" 
                ValidationGroup="AddUpdateJob" ValidationExpression="^.{0,25000}$" meta:resourcekey="regJobRequirements"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
</div>

<div id="divShowJobBenefits" runat="server" class="form-group row">
    <div class="col-sm-12">
        <a href="javascript:void(0);" onclick="javascript:showTextbox('<%= divShowJobBenefits.ClientID %>', '<%= divJobBenefits.ClientID %>');"><%= GetLocalResourceObject("strShowBenefits").ToString() %></a>
    </div>
</div>
<div id="divJobBenefits" runat="server">
    <div class="form-group row">
        <div class="col-sm-3">
            <asp:Label ID="lblJobBenefits" runat="server" meta:resourcekey="lblJobBenefits" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-12">
            <asp:TextBox id="txtJobBenefits" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
            <asp:RegularExpressionValidator ID="regJobBenefits" runat="server" ControlToValidate="txtJobBenefits" 
                ValidationGroup="AddUpdateJob" ValidationExpression="^.{0,25000}$" meta:resourcekey="regJobBenefits"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblPosition" runat="server" meta:resourcekey="lblPosition" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtPosition" runat="server" MaxLength="256" ValidationGroup="AddUpdateJob"
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqPosition" runat="server" ControlToValidate="txtPosition"
            ValidationGroup="AddUpdateJob" meta:resourcekey="reqPosition" Display="Dynamic" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>    

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblJobLocation" runat="server" meta:resourcekey="lblJobLocation" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtJobLocation" runat="server" MaxLength="256" ValidationGroup="AddUpdateJob"
            meta:resourcekey="txtJobLocation" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqJobLocation" runat="server" ControlToValidate="txtJobLocation"
            ValidationGroup="AddUpdateJob" meta:resourcekey="reqJobLocation" Display="Dynamic"
            CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>    

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblJobZip" runat="server" meta:resourcekey="lblJobZip" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtJobZip" runat="server" MaxLength="12" ValidationGroup="AddUpdateJob"
            meta:resourcekey="txtJobZip" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqJobZip" runat="server" ControlToValidate="txtJobZip"
            ValidationGroup="AddUpdateJob" meta:resourcekey="reqJobZip" Display="Dynamic" CssClass="text-danger">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regJobZip" runat="server" ValidationExpression="\d{5}(-\d{4})?"
            ControlToValidate="txtJobZip" ValidationGroup="AddUpdateJob" meta:resourcekey="regJobZip"
            Display="Dynamic" CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>    

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblEmploymentType" runat="server" meta:resourcekey="lblEmploymentType" 
            CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:DropDownList ID="ddlEmploymentType" runat="server" CssClass="form-control input-sm"
            DataTextField="Name" DataValueField="EmploymentTypeId" AppendDataBoundItems="true">
            <asp:ListItem meta:resourceKey="ddlEmploymentType_0"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqEmploymentType" runat="server" ControlToValidate="ddlEmploymentType"
            ValidationGroup="AddUpdateJob" meta:resourcekey="reqEmploymentType" Display="Dynamic"
            CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblCategories" runat="server" meta:resourcekey="lblCategories"
            CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:ListBox ID="lstCategories" runat="server" DataTextField="Name" DataValueField="CategoryId" 
            meta:resourcekey="lstCategories" SelectionMode="Multiple" CssClass="form-control input-sm categories-listbox">
        </asp:ListBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqCategories" runat="server" ControlToValidate="lstCategories"
            ValidationGroup="AddUpdateJob" meta:resourcekey="reqCategories" Display="Dynamic"
            CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>

<he:formvalidator id="heFormValidator" runat="server"/>

<br />
<div class="form-group row">
    <div class="col-sm-3">
        <asp:Button id="btnAddUpdatePublish" runat="server" ValidationGroup="AddUpdateJob" 
            OnCommand="btnAddUpdate_Command" meta:resourcekey="btnAddUpdatePublish" CssClass="btn btn-primary"/>
    </div>
    <div class="col-sm-3">
        <asp:Button id="btnAddUpdate" runat="server" ValidationGroup="AddUpdateJob" 
            OnCommand="btnAddUpdate_Command" meta:resourcekey="btnAddUpdate" CssClass="btn btn-default"/>
    </div>
</div>  