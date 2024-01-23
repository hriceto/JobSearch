<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
    exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="freeJobAvailableFor"/>
    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>
    <xsl:param name="JobsPage"/>
    <xsl:param name="freeAdsRefreshInterval"/>
    <xsl:param name="unsubscribeUrl"/>

    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        This is a reminder to let you know that you have a free job posting available for use. Just visit
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$JobsPage"/>
            </xsl:attribute>
            <xsl:value-of select="$JobsPage"/>
        </xsl:element> to create and publish a new job posting. No credit card required. <br /><br />
        
        We encourage you to use this free job posting soon. Additional free job postings are awarded 
        <xsl:value-of select="$freeAdsRefreshInterval"/> days after the publication date of the last free job posting.<br /><br />
        
        Please feel free to contact us with any questions, comments or concerns. We will be happy to help. 
        <br /><br />
        
        Like our service? Please consider using one of our paid ad options.
        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
        <br /><br /><small>No longer wish to receive emails about website news, services, and promotions? <xsl:element name="a"><xsl:attribute name="href"><xsl:value-of select="$unsubscribeUrl"/></xsl:attribute>Unsubscribe.</xsl:element></small>
    </xsl:template>
</xsl:stylesheet>
