<%@ Control Language="C#" AutoEventWireup="true" CodeFile="companyadmin.ascx.cs"
    Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.companyadmin" %>
<%@ Import Namespace="HristoEvtimov.Websites.Work.WorkDal" %>
<%@ Register TagPrefix="he" TagName="paging" Src="~/controls/_paging.ascx" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        addCompanyAdminScript();
    });

    function addCompanyAdminScript() {
        jQuery('#<%= txtSearchCompanyName.ClientID %>').keypress(OnKeySearchCompany);
        jQuery('#<%= txtSearchEmailAddress.ClientID %>').keypress(OnKeySearchCompany);
        jQuery('#<%= txtSearchCreatedDateFrom.ClientID %>').keypress(OnKeySearchCompany);
        jQuery('#<%= txtSearchCreatedDateTo.ClientID %>').keypress(OnKeySearchCompany);

        jQuery('#<%= txtSearchCreatedDateFrom.ClientID %>').datepicker({ keyboardNavigation: false, calendarWeeks: true,
            autoclose: true, todayHighlight: true
        });
        jQuery('#<%= txtSearchCreatedDateTo.ClientID %>').datepicker({ keyboardNavigation: false, calendarWeeks: true,
            autoclose: true, todayHighlight: true
        });

        jQuery('#<%= txtSearchCompanyName.ClientID %>').focus();
    }

    function OnKeySearchCompany(e) {
        if (e.which == 13) {
            jQuery('#<%= btnSearchCompany.ClientID %>').click();
            return false;
        }
    }
</script>


<div class="row">
    <div class="col-sm-8">
        <h4>
            <%= GetLocalResourceObject("strCompaniesPendingReview").ToString() %>
        </h4>
    </div>
</div>
<asp:Repeater ID="rptrCompaniesPendingReview" runat="server" OnItemDataBound="rptrCompanies_ItemDataBound">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row">
            <div class="col-sm-4">
                <%# ((Company)Container.DataItem).Name %>
            </div>
            <div class="col-sm-4">
                <asp:Literal ID="litUsers" runat="server"></asp:Literal>
            </div>
            <div class="col-sm-4">
                <asp:HyperLink ID="hypReview" runat="server" meta:resourcekey="hypReview" class="btn btn-primary"></asp:HyperLink>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
    <SeparatorTemplate>
        <div class="row">
            <div class="col-sm-12">
                &nbsp;
            </div>
        </div>
    </SeparatorTemplate>
</asp:Repeater>

<div class="form-group row">
    <div class="col-sm-12">
        <hr />
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-8">
        <h4>
            <%= GetLocalResourceObject("strCompanySearch").ToString() %>
        </h4>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblSearchCompanyName" runat="server" meta:resourcekey="lblSearchCompanyName" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:Label ID="lblSearchEmailAddress" runat="server" meta:resourcekey="lblSearchEmailAddress" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-4">
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:TextBox ID="txtSearchCompanyName" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtSearchEmailAddress" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:CheckBox id="chkbSearchApproved" runat="server" meta:resourcekey="chkbSearchApproved" CssClass="checkbox"/>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Label ID="lblSearchCreatedDateFrom" runat="server" meta:resourcekey="lblSearchCreatedDateFrom" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-4">
        <asp:Label ID="lblSearchCreatedDateTo" runat="server" meta:resourcekey="lblSearchCreatedDateTo" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-4"></div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:TextBox ID="txtSearchCreatedDateFrom" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:TextBox ID="txtSearchCreatedDateTo" runat="server" CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-4">
        <asp:RegularExpressionValidator ID="regSearchCreatedDateFrom" runat="server" ControlToValidate="txtSearchCreatedDateFrom"
            meta:resourcekey="regSearchCreatedDateFrom" Display="Dynamic"
            ValidationExpression="\d{2,2}/\d{2,2}/\d{4,4}" CssClass="text-danger">
        </asp:RegularExpressionValidator>
        <asp:RegularExpressionValidator ID="regSearchCreatedDateTo" runat="server" ControlToValidate="txtSearchCreatedDateTo"
            meta:resourcekey="regSearchCreatedDateTo" Display="Dynamic"
            ValidationExpression="\d{2,2}/\d{2,2}/\d{4,4}" CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-4">
        <asp:Button id="btnSearchCompany" runat="server" meta:resourcekey="btnSearchCompany" class="btn btn-primary"
            OnClick="btnSearchCompany_Click"/>
    </div>
</div>


<asp:Repeater ID="rptrCompanySearch" runat="server" OnItemDataBound="rptrCompanies_ItemDataBound">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row">
            <div class="col-sm-4">
                <%# ((Company)Container.DataItem).Name %>
            </div>
            <div class="col-sm-4">
                <asp:Literal ID="litUsers" runat="server"></asp:Literal>
            </div>
            <div class="col-sm-4">
                <asp:HyperLink ID="hypReview" runat="server" meta:resourcekey="hypReview" class="btn btn-primary"></asp:HyperLink>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
    <SeparatorTemplate>
        <div class="row">
            <div class="col-sm-12">
                &nbsp;
            </div>
        </div>
    </SeparatorTemplate>
</asp:Repeater>

<div>
    <he:paging id="hePaging" runat="server"/>
</div>