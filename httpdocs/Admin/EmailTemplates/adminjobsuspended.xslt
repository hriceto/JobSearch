<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
    exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="ReasonText"/>
    <xsl:param name="CustomerServiceEmail" />
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>


    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        Your job posting titled "<xsl:value-of select="root/JobPost/Title"/>" has been reviewed by a site administrator on <xsl:value-of select="msxsl:format-date(root/JobPost/ReviewedDate, 'MM/dd/yyyy')"/>.
        <br />
        <br />

        <hr></hr>
        <xsl:if test="root/JobPost/Suspended='true'">
            We are sorry to inform you that the job posting named above has been found to violate one or more of our site rules. 
            An administrator has marked the job posting as "suspended" which means that it will be taken off of our 
            search results. 
            <br />The reason provided by the administrator was : <xsl:value-of select="$ReasonText"/>
            <xsl:if test="root/JobPost/IsPaidAd='true'">
                <br />It looks like you may be entitled to a refund. Generally we will process refunds within 48 hours.
                <br />Feel free to contact us at <xsl:value-of select="$CustomerServiceEmail"/> with any questions.
            </xsl:if>
        </xsl:if>
        <hr></hr>

        <br /><br />
        Please feel free to contact us with any questions, comments or concerns. We will be happy to help.
        <br /><br />

        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.

</xsl:template>
</xsl:stylesheet>
