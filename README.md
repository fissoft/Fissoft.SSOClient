Fissoft.SSOClient
=================
[![install from nuget](http://img.shields.io/nuget/v/Fissoft.SSOClient.svg?style=flat-square)](https://www.nuget.org/packages/Fissoft.SSOClient)
[![downloads](http://img.shields.io/nuget/dt/Fissoft.SSOClient.svg?style=flat-square)](https://www.nuget.org/packages/Fissoft.SSOClient)
[![release](https://img.shields.io/github/release/fissoft/Fissoft.SSOClient.svg?style=flat-square)](https://github.com/fissoft/Fissoft.SSOClient/releases)

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
### NuGet Install
``` powershell
Install-Package Fissoft.SSOClient
```
