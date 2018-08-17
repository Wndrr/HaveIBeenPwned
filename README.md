# HaveIBeenPwned Password C# API
A C#.NET Standard 2.0 wrapper for the [haveibeenpwned](https://haveibeenpwned.com/API/v2#PwnedPasswords) password REST API

## Compatibility

This library targets .NET Standard 2.0 which makes it compatible with both .NET Core 2.0 and above aswell as .NET Framework 4.6.1 and above. See the [compatibility page](http://immo.landwerth.net/netstandard-versions/#) for a more exhaustive list of compatible frameworks.

## NuGet package

The NuGet package can be found on [NuGet.org](https://www.nuget.org/packages/Wndrr.HaveIBeenPwned.Password/) or by searching `Wndrr.HaveIBeenPwned.Password` in the NuGet explorer.

## Usage

The API has two methods: `IsPasswordPwned` and `GetNumberOfTimesPasswordPwned`.

### The `IsPasswordPwned` method

Calls the HaveIBeenPwned REST API and returns a `bool` indicating if the password has been leaked at least once. Internally, the `IsPasswordPwned` method uses a call to `GetNumberOfTimesPasswordPwned`.

Usage example

```csharp
// Instanciate the HaveIBeenPwned class
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();
// Call IsPasswordPwned with a plain text string password to know if the password was leaked at least once
bool isPasswordPwned = pwned.IsPasswordPwned("aVeryBadPassword");
```

### The `GetNumberOfTimesPasswordPwned` method

Calls the HaveIBeenPwned REST API and returns an `int` indicating how many times the password has been leaked.

Usage example

```csharp
// Instanciate the HaveIBeenPwned class
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();
// Call GetNumberOfTimesPasswordPwned with a plain text string password to know the number of times it was leaked
int numberOfTimesPwned = pwned.GetNumberOfTimesPasswordPwned("aVeryBadPassword");
```
