@ECHO OFF
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" /c /n:Kontrax.EAuth.Schema saml-schema-protocol-2.0.xsd saml-schema-assertion-2.0.xsd bg-egov-eauthentication.xsd xmldsig-core-schema.xsd xenc-schema.xsd
IF %ERRORLEVEL% NEQ 0 GOTO END
DEL GeneratedClasses.cs
REN "saml-schema-protocol-2_0_saml-schema-assertion-2_0_bg-egov-eauthentication_xmldsig-core-schema_xenc-schema.cs" GeneratedClasses.cs
:END
PAUSE
