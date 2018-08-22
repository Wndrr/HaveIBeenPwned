# HaveIBeenPwned Password C# API
A C#.NET Standard 2.0 wrapper for the [haveibeenpwned](https://haveibeenpwned.com/API/v2#PwnedPasswords) password REST API

## Compatibility

This library targets .NET Standard 2.0 which makes it compatible with both .NET Core 2.0 and above aswell as .NET Framework 4.6.1 and above. See the [compatibility page](http://immo.landwerth.net/netstandard-versions/#) for a more exhaustive list of compatible frameworks.

## NuGet package

The NuGet package can be found on [NuGet.org](https://www.nuget.org/packages/Wndrr.HaveIBeenPwned.Password/) or by searching `Wndrr.HaveIBeenPwned.Password` in the NuGet explorer.

## Usage

### Config

The library can be configured throught it constructor. below are the configurable properties:
- The API endpoint
- The number of times a password must be leaked to be considered "pwned"

```csharp
// Default instanciation. The official API endpoint will be used and the numberOfLeaksForPwn will be 0
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();
// Overrides only the API endpoint
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned("https://custom.end.point");
// Overrides only the numberOfLeaksForPwn.
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned(10);
// Overrides both the API endpoint and the numberOfLeaksForPwn property
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned("https://custom.end.point", 10);
```

#### The `NumberOfLeaksForPwn` property
This property must have a value between `1` and `int.MaxValue`. It will be used in the `IsPasswordPwned` and `GetPwned` methods to determine whether a password is "pwned". Default is `1`.

### The `IsPasswordPwned` method

Calls the HaveIBeenPwned REST API and returns a `bool` indicating if the password has been leaked at least once. Internally, the `IsPasswordPwned` method uses a call to `GetNumberOfTimesPasswordPwned`.

```csharp
// Instanciate the HaveIBeenPwned class
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();
// Call IsPasswordPwned with a plain text string password to know if the password was leaked at least once
bool isPasswordPwned = pwned.IsPasswordPwned("aVeryBadPassword");
```

### The `GetNumberOfTimesPasswordPwned` method

Calls the HaveIBeenPwned REST API and returns an `int` indicating how many times the password has been leaked.

```csharp
// Instanciate the HaveIBeenPwned class
var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();
// Call GetNumberOfTimesPasswordPwned with a plain text string password to know the number of times it was leaked
int numberOfTimesPwned = pwned.GetNumberOfTimesPasswordPwned("aVeryBadPassword");
```

### The `GetPwned` method

Calls the HaveIBeenPwned web API for each provided password and returns the list of passwords that were leaked
It can be called by passing either an `IEnumerable<string>`, a `string[]` or a set of `string`. 

```csharp
var rawPass = new[]{"poopPassword_1", "iAtePizzaAtLunchI'mSoFull(this_is_a_decent_length_password)"};
var pwnedArray = GetPwned(rawPass); //string[]
var pwnedIEnumerable = GetPwned(rawPass.ToList()); // IEnumerable<string>
var pwnedParams = GetPwned("poopPassword_1", "passPoopWord_2", "3_3", "123456" /*, [...] */); // params string
```
