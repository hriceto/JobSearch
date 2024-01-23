<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="LoginLink"/>
    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>
    <xsl:param name="JobsPage"/>
    
    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        <xsl:if test="root/Company/Reviewed='false'">
            Your new account has been verified successfully. Your account now has access the following access:
            <br /><br />
        </xsl:if>
        <xsl:if test="root/Company/Reviewed='true'">
            Your new account has been verified successfully. Your account now has access the following access:
            <br /><br />
        </xsl:if>

        <hr></hr>
        <b>Price level</b>
        <xsl:if test="root/Company/IsRecruiter='true'">
            <br /><xsl:value-of select="root/Company/Name"/> is now at recruiter pricing level.
        </xsl:if>
        <xsl:if test="root/Company/IsRecruiter='false'">
            <br /><xsl:value-of select="root/Company/Name"/> is now at employer pricing level.
        </xsl:if>
        <hr></hr>

        <b>Access to Junior Advertisements</b>
        <xsl:if test="root/Company/AllowFreeJobPosts='true'">
            <br /><xsl:value-of select="root/Company/Name"/> has been granted access to Junior Advertisements (free ads).
        </xsl:if>
        <xsl:if test="root/Company/AllowFreeJobPosts='false'">
            <br /><xsl:value-of select="root/Company/Name"/> has been denied access to Junior Advertisements (free ads).
            <xsl:if test="root/Company/Reviewed='false'">
                <br />Denial has most likely occurred due to registering with a public domain email address (such as yourname@yahoo.com).
                An administrator will review your account and make a final decision on junior ad availability.
            </xsl:if>
        </xsl:if>
        <hr></hr>

        <br />
        To create a job posting please visit 
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$JobsPage"/>
            </xsl:attribute>
            <xsl:value-of select="$JobsPage"/>
        </xsl:element>.
        <br /><br />
        Thank you for allowing us to server you.
        <br /><br />
        Please feel free to contact us with any questions, comments or concerns. We will be happy to help.
        <br /><br />

        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
</xsl:template>
</xsl:stylesheet>
