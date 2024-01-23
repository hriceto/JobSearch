<%@ Control Language="C#" AutoEventWireup="true" CodeFile="publishjob.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Employer.Controls.publishjob" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript">
    jQuery(document).ready(function (jQuery) {
        publishJobOnLoadScript();
    });

    function publishJobOnLoadScript() {
        jQuery('#<%= rdbPrice.ClientID %> input').keypress(OnKeyPublishJob);
        jQuery('#<%= txtReplyEmail.ClientID %>').keypress(OnKeyPublishJob);
        jQuery('#<%= txtReplyUrl.ClientID %>').keypress(OnKeyPublishJob);
        jQuery('#<%= txtStartDate.ClientID %>').keypress(OnKeyPublishJob);

        jQuery('#<%= rdbPrice.ClientID %> input').first().focus();
        jQuery('#<%= txtKeywords.ClientID %>').tooltip({ placement: "top", html: true, trigger: "focus" });
        jQuery('#<%= txtReplyEmail.ClientID %>').tooltip({ placement: "top", html: true, trigger: "focus" });
        jQuery('#<%= txtReplyUrl.ClientID %>').tooltip({ placement: "top", html: true, trigger: "focus" });
        jQuery('#<%= txtStartDate.ClientID %>').tooltip({ placement: "right", html: true, trigger: "focus" });

        jQuery('#<%= txtStartDate.ClientID %>').datepicker({ keyboardNavigation: false, calendarWeeks: true, 
            autoclose: true, todayHighlight: true });
    }

    function OnKeyPublishJob(e) {
        if (e.which == 13) {
            jQuery('#<%= btnPublish.ClientID %>').click();
            return false;
        }
    }
    
    function ChangePriceOption(value) {
        value = parseInt(value);
        if (value > 0) {
            jQuery('#<%= divPaid.ClientID %>').slideDown('slow');
        }
        else {
            jQuery('#<%= divPaid.ClientID %>').slideUp('slow');
        }
    }

    function ValidatePaidOptions() {
        if (jQuery("#<%= rdbPrice.ClientID %> input:checked").val() > 0) {
            return Page_ClientValidate('PaidAd');
        }
        return true;
    }
</script>

<div class="form-group row">
    <div class="col-sm-12">
        <h2><asp:Label ID="lblTitle" runat="server"></asp:Label></h2>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-9">
        <asp:RadioButtonList ID="rdbPrice" runat="server" ValidationGroup="PriceOption" CssClass="radio" RepeatLayout="Flow">
            <asp:ListItem meta:resourcekey="rdbPrice_0" OnClick="javascript:ChangePriceOption(this.value);"></asp:ListItem>
            <asp:ListItem meta:resourcekey="rdbPrice_1" OnClick="javascript:ChangePriceOption(this.value);"></asp:ListItem>
            <asp:ListItem meta:resourcekey="rdbPrice_2" OnClick="javascript:ChangePriceOption(this.value);"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqPrice" runat="server" ControlToValidate="rdbPrice" 
            Display="Dynamic" ValidationGroup="PriceOption" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>

<div id="divPaid" runat="server" style="display:none;">
    <div class="row">
        <div class="col-sm-12">
            <asp:Label ID="lblKeywords" runat="server" meta:resourcekey="lblKeywords" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-9">
            <asp:TextBox ID="txtKeywords" runat="server" ValidationGroup="PaidAd" TextMode="MultiLine" Rows="3" 
                MaxLength="512" CssClass="form-control input-sm" meta:resourcekey="txtKeywords"></asp:TextBox>
            <asp:RegularExpressionValidator ID="regKeywords" runat="server" ControlToValidate="txtKeywords" 
                ValidationGroup="PaidAd" ValidationExpression="^.{0,512}$" meta:resourcekey="regKeywords"
                CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12">
            <asp:Label id="lblReplyEmail" runat="server" meta:resourcekey="lblReplyEmail" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-9">
            <asp:TextBox ID="txtReplyEmail" runat="server" ValidationGroup="PaidAd" MaxLength="100"
                CssClass="form-control input-sm" meta:resourcekey="txtReplyEmail"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RegularExpressionValidator ID="regReplyEmail" runat="server" ControlToValidate="txtReplyEmail"
                ValidationGroup="PaidAd" meta:resourcekey="regReplyEmail" Display="Dynamic" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:Label id="lblReplyUrl" runat="server" meta:resourcekey="lblReplyUrl" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-9">
            <asp:TextBox ID="txtReplyUrl" runat="server" ValidationGroup="PaidAd" MaxLength="256"
                CssClass="form-control input-sm" meta:resourcekey="txtReplyUrl"></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <asp:RegularExpressionValidator ID="regReplyUrl" runat="server" ValidationGroup="PaidAd"
                ControlToValidate="txtReplyUrl" meta:resourcekey="regReplyUrl" Display="Dynamic"
                ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" 
                CssClass="text-danger"></asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:Label id="lblStartDate" runat="server" meta:resourcekey="lblStartDate" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-sm-9">
            <asp:TextBox ID="txtStartDate" runat="server" ValidationGroup="PaidAd" MaxLength="10"
                CssClass="form-control input-sm" meta:resourcekey="txtStartDate"></asp:TextBox>
        </div>
        <div class="col-sm-3">    
            <asp:RegularExpressionValidator ID="regStartDate" runat="server" ControlToValidate="txtStartDate"
                ValidationGroup="PaidAd" meta:resourcekey="regStartDate" Display="Dynamic"
                ValidationExpression="\d{2,2}/\d{2,2}/\d{4,4}" CssClass="text-danger">
            </asp:RegularExpressionValidator>
        </div>
    </div>
</div>

<br />
<he:formvalidator id="heFormValidator" runat="server"/>
<div class="row">
    <div class="col-sm-3">
        <asp:Button id="btnPublish" runat="server" meta:resourcekey="btnPublish" OnClientClick="javascript:return ValidatePaidOptions();" 
            OnClick="btnPublish_Click" ValidationGroup="PriceOption" CssClass="btn btn-primary"/>
    </div>
    <div class="col-sm-3">
        <asp:Button id="btnCancel" runat="server" meta:resourcekey="btnCancel" 
            OnClick="btnCancel_Click" CausesValidation="false" CssClass="btn btn-default"/>
    </div>
</div>