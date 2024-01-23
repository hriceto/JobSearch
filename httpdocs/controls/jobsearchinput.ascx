<%@ Control Language="C#" AutoEventWireup="true" Inherits="HristoEvtimov.Websites.Work.WorkLibrary.UserControls.JobSearchInput" %>

<script type="text/javascript" language="javascript">
    jQuery(document).ready(function ($) {
        jQuery('#<%= txtSearch.ClientID %>').keypress(function (e) {
            if (e.which == 13) {
                jQuery('#<%= btnSearch.ClientID %>').click();
                return false;
            }
        });
    });
</script>


<div class="form-group">
    <div class="input-group">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control job_search_input" 
            meta:resourcekey="txtSearch" MaxLength="100"></asp:TextBox>
        <span class="input-group-btn job_search_button">
            <asp:Button id="btnSearch" runat="server" OnClick="btnSearch_Click" meta:resourcekey="btnSearch" CssClass="btn btn-default"/>
        </span>
    </div>
</div>