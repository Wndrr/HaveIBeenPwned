using System;
using System.Linq;
using Xunit;

namespace HaveIBeenTested.Password.Pwned
{
    public class HaveIBeenPwnedTest
    {
        [Theory]
        [InlineData("password", true)]
        [InlineData("12345", true)]
        [InlineData("sjgfsdkvkjcxfbxkcvbksjdfbvc", false)]
        [InlineData("skeftbgfdmlh!:h d!;,:fgg352", false)]
        public void IsPasswordPwned(string password, bool isPwned)
        {
            var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();

            var isPwnedResult = pwned.IsPasswordPwned(password);

            Assert.Equal(isPwned, isPwnedResult);

        }

        [Theory]
        [InlineData(2, "password", "12345", "sjgfsdkvkjcxfbxkcvbksjdfbvc", "skeftbgfdmlh!:h d!;,:fgg352")]
        [InlineData(0, "thisisaverylongandcomplexpasswordthatshouldwork")]
        [InlineData(1, "Password-1")]
        public void GetPwned_array(int expectedNumberOfmatch, params string[] passwords)
        {
            var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();

            var pwnedPasswords = pwned.GetPwned(passwords);

            Assert.Equal(expectedNumberOfmatch, pwnedPasswords.Count);

        }

        [Theory]
        [InlineData(2, "password", "12345", "sjgfsdkvkjcxfbxkcvbksjdfbvc", "skeftbgfdmlh!:h d!;,:fgg352")]
        [InlineData(0, "thisisaverylongandcomplexpasswordthatshouldwork")]
        [InlineData(1, "Password-1")]
        public void GetPwned_enumerable(int expectedNumberOfmatch, params string[] passwords)
        {
            var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned();

            var pwnedPasswords = pwned.GetPwned(passwords.ToList());

            Assert.Equal(expectedNumberOfmatch, pwnedPasswords.Count);

        }

        [Fact]
        public void ThrowIfInvalidUri()
        {
            Assert.Throws<UriFormatException>(() => new HaveIBeenPwned.Password.HaveIBeenPwned("invaliduri"));
        }

        [Fact]
        public void DoesNotThrowIfValidUri()
        {
            var ex = Record.Exception(() => new HaveIBeenPwned.Password.HaveIBeenPwned("https://api.pwnedpasswords.com/range"));

            Assert.Null(ex);
        }
    }
}
