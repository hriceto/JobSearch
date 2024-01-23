<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
    exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="DateUpdated"/>
    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>
    
    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        Your job posting titled "<xsl:value-of select="root/JobPost/Title"/>" has been reviewed by a site administrator on <xsl:value-of select="msxsl:format-date(root/JobPost/ReviewedDate, 'MM/dd/yyyy')"/>. 
        <xsl:if test="$DateUpdated='false'">
            It will be added to our search index as scheduled.
        </xsl:if>
        <br />
        <br />

        <xsl:if test="$DateUpdated='true'">
            <hr></hr>
            The job posting start date has been updated to <xsl:value-of select="msxsl:format-date(root/JobPost/StartDate, 'MM/dd/yyyy')"/>.
            <hr></hr>
            <br />
        </xsl:if>

        Please feel free to contact us with any questions, comments or concerns. We will be happy to help.
        <br /><br />

        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.

</xsl:template>
</xsl:stylesheet>
