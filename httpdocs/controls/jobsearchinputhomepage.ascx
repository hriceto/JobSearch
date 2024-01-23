<%@ Control Language="C#" AutoEventWireup="true" Inherits="HristoEvtimov.Websites.Work.WorkLibrary.UserControls.JobSearchInput" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function ($) {
        jQuery('#<%= txtSearch.ClientID %>').keypress(function (e) {
            if (e.which == 13) {
                jQuery('#<%= btnSearch.ClientID %>').click();
                return false;
            }
        });

        jQuery('#<%= txtSearch.ClientID %>').focus();
    });
</script>


<div class="form-group row">
    <div class="col-sm-12">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm" 
            meta:resourcekey="txtSearch" MaxLength="100"></asp:TextBox>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <asp:Button id="btnSearch" runat="server" OnClick="btnSearch_Click" meta:resourcekey="btnSearch" CssClass="btn btn-default"/>
    </div>
</div>