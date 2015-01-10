Fissoft.SSOClient
=================

### global.asax

ApplicationStart: register cookie domain
``` C#
    AuthClient.RegisterCookieDomain("mydomain.com");
```
### web.config

``` xml
    <add key="sso.appKey" value="app1" />
    <add key="sso.secret" value="sec1" />
    <add key="sso.serverUrl" value="http://sso.mydomain.com" />
    <add key="sso.application" value="cookieid" />
```
### how to install
``` powershell
Install-Package Fissoft.SSOClient
```
