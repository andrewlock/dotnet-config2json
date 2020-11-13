dotnet-config2json
============

[![AppVeyor build status][appveyor-badge]](https://ci.appveyor.com/project/andrewlock/dotnet-config2json/branch/master)

[appveyor-badge]: https://img.shields.io/appveyor/ci/andrewlock/dotnet-config2json/master.svg?label=appveyor&style=flat-square

[![NuGet][main-nuget-badge]][main-nuget] [![MyGet][main-myget-badge]][main-myget]

[main-nuget]: https://www.nuget.org/packages/dotnet-config2json/
[main-nuget-badge]: https://img.shields.io/nuget/v/dotnet-config2json.svg?style=flat-square&label=nuget
[main-myget]: https://www.myget.org/feed/andrewlock-ci/package/nuget/dotnet-config2json
[main-myget-badge]: https://img.shields.io/www.myget/andrewlock-ci/vpre/dotnet-config2json.svg?style=flat-square&label=myget

A simple tool to convert a web.config file to an appsettings.json file.

## Installation

The latest release of dotnet-config2json requires the [2.1.300](https://dotnet.microsoft.com/download/dotnet-core/2.1) .NET Core SDK or newer. It also works with .NET Core 2.2. and .NET Core 3.1.
Once installed, run this command:

```
dotnet tool install --global dotnet-config2json
```

## Usage

```
Usage: dotnet config2json [arguments] [options]

Arguments:
  path          Path to the file or directory to migrate
  delimiter     The character in keys to replace with the section delimiter (:)
  prefix        If provided, an additional namespace to prefix on generated keys

Options:
  -?|-h|--help  Show help information

Performs basic migration of an xml .config file to
a JSON file. Uses the 'key' value as the key, and the
'value' as the value. Can optionally replace a given
character with the section marker (':').
