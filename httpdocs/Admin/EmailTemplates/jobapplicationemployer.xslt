<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="FirstName"/>
    <xsl:param name="LastName"/>
    <xsl:param name="Email"/>
    <xsl:param name="Phone"/>
    <xsl:param name="CoverLetter"/>
    <xsl:param name="Resume"/>
    <xsl:param name="DashboardLink"/>
    <xsl:param name="CustomerServiceEmail"/>
    <xsl:param name="CustomerServicePhone"/>
    <xsl:param name="Domain"/>

    <xsl:template match="/">
        Dear <xsl:value-of select="root/User/FirstName"/>&#160;<xsl:value-of select="root/User/LastName"/>,
        <br /><br />
        A new job application was submitted on <xsl:value-of select="$Domain"/> under your job posting titled "<xsl:value-of select="root/JobPost/Title"/>".
        Here are the details
        <br /><br />
        <b>First name</b>: <xsl:value-of select="$FirstName"/><br />
        <b>Last name</b>: <xsl:value-of select="$LastName"/><br />
        <b>Email</b>: <xsl:value-of select="$Email"/><br />
        <b>Phone</b>: <xsl:value-of select="$Phone"/><br />
        <b>Cover letter</b>: <xsl:value-of select="$CoverLetter" disable-output-escaping="yes"/><br />
        <b>Resume</b>: <xsl:value-of select="$Resume" disable-output-escaping="yes"/><br />

        <br /><br />
        You can also view all job applications by visiting your employer
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$DashboardLink"/>
            </xsl:attribute>
            dashboard
        </xsl:element>.

        <br /><br />
        Sincerely,
        <br /><br />
        The <xsl:value-of select="$Domain"/> Team!
        <br />Contact Us by Email at <xsl:element name="a"><xsl:attribute name="href">mailto:<xsl:value-of select="$CustomerServiceEmail"/></xsl:attribute><xsl:value-of select="$CustomerServiceEmail"/></xsl:element>, Toll Free at <xsl:value-of select="$CustomerServicePhone"/>, or simply reply to this email.
</xsl:template>
</xsl:stylesheet>
