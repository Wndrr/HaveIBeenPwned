using System;
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
