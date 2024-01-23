<%@ Control Language="C#" AutoEventWireup="true" CodeFile="blogedit.ascx.cs" Inherits="HristoEvtimov.Websites.Work.Web.Admin.Controls.blogedit" %>
<%@ Register TagPrefix="he" TagName="formvalidator" Src="~/controls/_formvalidator.ascx" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function (jQuery) {
        addEditBlogOnLoadScript();
    });

    function addEditBlogOnLoadScript() {
        jQuery('#<%= txtBlogTitle.ClientID %>').keypress(OnKeyAddEditBlog);
        jQuery('#<%= txtBlogSubTitle.ClientID %>').keypress(OnKeyAddEditBlog);
        jQuery('#<%= txtBlogSeUrl.ClientID %>').keypress(OnKeyAddEditBlog);
        jQuery('#<%= txtBlogSeTitle.ClientID %>').keypress(OnKeyAddEditBlog);
        jQuery('#<%= txtBlogSeDescription.ClientID %>').keypress(OnKeyAddEditBlog);
        jQuery('#<%= txtBlogKeywords.ClientID %>').keypress(OnKeyAddEditBlog);
        jQuery('#<%= lstTopics.ClientID %>').keyup(OnKeyAddEditBlog);

        jQuery('#<%= txtBlogTitle.ClientID %>').focus();

        jQuery('#<%= txtBlogPublishDate.ClientID %>').datepicker({ keyboardNavigation: false, calendarWeeks: true,
            autoclose: true, todayHighlight: true
        });
    }

    function OnKeyAddEditBlog(e) {
        if (e.which == 13) {
            jQuery('#<%= btnAddUpdateBlog.ClientID %>').click();
            return false;
        }
    }
</script>

<div class="form-group row" id="divBlogAuthor" runat="server">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogAuthor" runat="server" meta:resourcekey="lblBlogAuthor" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:DropDownList ID="ddlBlogAuthor" runat="server" ValidationGroup="AddUpdateBlog" 
            CssClass="form-control input-sm"></asp:DropDownList>
    </div>
    <div class="col-sm-3"></div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogTitle" runat="server" meta:resourcekey="lblBlogTitle" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtBlogTitle" runat="server" MaxLength="512" ValidationGroup="AddUpdateBlog" 
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqBlogTitle" runat="server" ControlToValidate="txtBlogTitle"
            ValidationGroup="AddUpdateBlog" meta:resourcekey="reqBlogTitle" Display="Dynamic" CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogSubTitle" runat="server" meta:resourcekey="lblBlogSubTitle" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtBlogSubTitle" runat="server" MaxLength="512" ValidationGroup="AddUpdateBlog" 
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3"></div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogSummary" runat="server" meta:resourcekey="lblBlogSummary" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:TextBox id="txtBlogSummary" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regBlogSummary" runat="server" ControlToValidate="txtBlogSummary" 
            ValidationGroup="AddUpdateBlog" ValidationExpression="^.{0,25000}$" meta:resourcekey="regBlogSummary"
            CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogContent" runat="server" meta:resourcekey="lblBlogContent" CssClass="control-label"></asp:Label>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:TextBox id="txtBlogContent" runat="server" TextMode="MultiLine" MaxLength="25000"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regBlogContent" runat="server" ControlToValidate="txtBlogContent" 
            ValidationGroup="AddUpdateBlog" ValidationExpression="^.{0,25000}$" meta:resourcekey="regBlogContent"
            CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogSeUrl" runat="server" meta:resourcekey="lblBlogSeUrl" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtBlogSeUrl" runat="server" MaxLength="256" ValidationGroup="AddUpdateBlog" 
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3"></div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogSeTitle" runat="server" meta:resourcekey="lblBlogSeTitle" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtBlogSeTitle" runat="server" MaxLength="256" ValidationGroup="AddUpdateBlog" 
            CssClass="form-control input-sm" Rows="3" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="col-sm-3"></div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogSeDescription" runat="server" meta:resourcekey="lblBlogSeDescription" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtBlogSeDescription" runat="server" MaxLength="256" ValidationGroup="AddUpdateBlog" 
            CssClass="form-control input-sm" Rows="3" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="col-sm-3"></div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogKeywords" runat="server" meta:resourcekey="lblBlogKeywords" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtBlogKeywords" runat="server" MaxLength="512" ValidationGroup="AddUpdateBlog" 
            CssClass="form-control input-sm" Rows="3" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="col-sm-3"></div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblTopics" runat="server" meta:resourcekey="lblTopics" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:ListBox ID="lstTopics" runat="server" DataTextField="Name" DataValueField="TopicId" 
            meta:resourcekey="lstTopics" SelectionMode="Multiple" CssClass="form-control input-sm topics-listbox">
        </asp:ListBox>
    </div>
    <div class="col-sm-3">
        <asp:RequiredFieldValidator ID="reqTopics" runat="server" ControlToValidate="lstTopics"
            ValidationGroup="AddUpdateBlog" meta:resourcekey="reqTopics" Display="Dynamic"
            CssClass="text-danger">
        </asp:RequiredFieldValidator>
    </div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label ID="lblBlogPublished" runat="server" meta:resourcekey="lblBlogPublished" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:CheckBox ID="chkbBlogPublished" runat="server" meta:resourcekey="chkbBlogPublished" CssClass="checkbox"/>
    </div>
    <div class="col-sm-3"></div>
</div>

<div class="form-group row">
    <div class="col-sm-3">
        <asp:Label id="lblBlogPublishDate" runat="server" meta:resourcekey="lblBlogPublishDate" CssClass="control-label"></asp:Label>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtBlogPublishDate" runat="server" ValidationGroup="AddUpdateBlog" MaxLength="10"
            CssClass="form-control input-sm"></asp:TextBox>
    </div>
    <div class="col-sm-3">    
        <asp:RegularExpressionValidator ID="regBlogPublishDate" runat="server" ControlToValidate="txtBlogPublishDate"
            ValidationGroup="AddUpdateBlog" meta:resourcekey="regBlogPublishDate" Display="Dynamic"
            ValidationExpression="\d{2,2}/\d{2,2}/\d{4,4}" CssClass="text-danger">
        </asp:RegularExpressionValidator>
    </div>
</div>

<he:formvalidator id="heFormValidator" runat="server"/>

<br />
<div class="form-group row">
    <div class="col-sm-3">
        <asp:Button id="btnAddUpdateBlog" runat="server" ValidationGroup="AddUpdateBlog" 
            OnClick="btnAddUpdateBlog_Click" meta:resourcekey="btnAddUpdateBlog" CssClass="btn btn-default"/>
    </div>
</div>  