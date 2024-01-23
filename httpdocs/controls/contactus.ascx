<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contactus.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Controls.contactus" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        contactUsOnLoadScript();
    });

    function contactUsOnLoadScript() {
        jQuery('#<%= txtEmail.ClientID %>').keypress(OnKeyDownContactUs);
        jQuery('#<%= txtName.ClientID %>').keypress(OnKeyDownContactUs);
        
        jQuery('#<%= txtEmail.ClientID %>').focus();
    }

    function OnKeyDownContactUs(e) {
        if (e.which == 13) {
            jQuery('#<%= btnSubmit.ClientID %>').click();
            return false;
        }
    }
</script>


<div class="form-group row">
    <div class="col-sm-12">
        <h1><%= GetLocalResourceObject("strContactUsHeader").ToString() %></h1>
        <div><asp:Literal ID="litContactUsDescription" runat="server"></asp:Literal></div>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-2">
        <asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail"
            meta:resourcekey="reqEmail" Display="Dynamic" ValidationGroup="ContactUs" CssClass="text-danger">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail"
            meta:resourcekey="regEmail" Display="Dynamic" ValidationGroup="ContactUs" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-2">
        <asp:Label ID="lblName" runat="server" meta:resourcekey="lblName" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control input-sm" MaxLength="100"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
            meta:resourcekey="reqName" Display="Dynamic" ValidationGroup="ContactUs" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-2">
        <asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessage" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control input-sm" 
            Rows="5" TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RequiredFieldValidator ID="reqMessage" runat="server" ControlToValidate="txtMessage"
            meta:resourcekey="reqMessage" Display="Dynamic" ValidationGroup="ContactUs" CssClass="text-danger">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regMessage" runat="server" ControlToValidate="txtMessage" 
            ValidationGroup="ContactUs" ValidationExpression="^.{0,1000}$" meta:resourcekey="regMessage"
            CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>
<he:formvalidator id="heFormValidator" runat="server"/>
<div class="form-group row">
    <div class="col-sm-2">
    </div>
    <div class="col-sm-3">
        <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmit" OnClick="btnSubmit_Click"
            CssClass="btn btn-primary" ValidationGroup="ContactUs" />
    </div>
</div>