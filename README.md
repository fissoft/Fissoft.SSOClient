Fissoft.SSOClient
=================

### global.asax

ApplicationStart: register cookie domain

    AuthClient.RegisterCookieDomain("mydomain.com");

### web.config


    <add key="sso.appKey" value="app1" />
    <add key="sso.secret" value="sec1" />
    <add key="sso.serverUrl" value="http://sso.mydomain.com" />
    <add key="sso.application" value="cookieid" />

### how to install

Install-Package Fissoft.SSOClient
