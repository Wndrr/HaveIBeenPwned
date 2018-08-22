using System;
using System.Linq;
using Xunit;

namespace HaveIBeenTested.Password.Pwned
{
    public class HaveIBeenPwnedTest
    {
        private readonly HaveIBeenPwned.Password.HaveIBeenPwned _haveIBeenPwned = new HaveIBeenPwned.Password.HaveIBeenPwned();

        [Theory]
        [InlineData("password", true)]
        [InlineData("12345", true)]
        [InlineData("sjgfsdkvkjcxfbxkcvbksjdfbvc", false)]
        [InlineData("skeftbgfdmlh!:h d!;,:fgg352", false)]
        public void IsPasswordPwned_default(string password, bool isPwned)
        {
            var isPwnedResult = _haveIBeenPwned.IsPasswordPwned(password);

            Assert.Equal(isPwned, isPwnedResult);
        }

        [Theory]
        [InlineData("password", true)]
        [InlineData("12345", false)]
        [InlineData("sjgfsdkvkjcxfbxkcvbksjdfbvc", false)]
        [InlineData("skeftbgfdmlh!:h d!;,:fgg352", false)]
        public void IsPasswordPwned_(string password, bool isPwned)
        {
            var pwned = new HaveIBeenPwned.Password.HaveIBeenPwned(2500000);
            var isPwnedResult = pwned.IsPasswordPwned(password);

            Assert.Equal(isPwned, isPwnedResult);
        }

        [Theory]
        [InlineData(2, "password", "12345", "sjgfsdkvkjcxfbxkcvbksjdfbvc", "skeftbgfdmlh!:h d!;,:fgg352")]
        [InlineData(0, "thisisaverylongandcomplexpasswordthatshouldwork")]
        [InlineData(1, "Password-1")]
        public void GetPwned_array(int expectedNumberOfmatch, params string[] passwords)
        {
            var pwnedPasswords = _haveIBeenPwned.GetPwned(passwords);

            Assert.Equal(expectedNumberOfmatch, pwnedPasswords.Count);
        }

        [Theory]
        [InlineData(2, "password", "12345", "sjgfsdkvkjcxfbxkcvbksjdfbvc", "skeftbgfdmlh!:h d!;,:fgg352")]
        [InlineData(0, "thisisaverylongandcomplexpasswordthatshouldwork")]
        [InlineData(1, "Password-1")]
        public void GetPwned_enumerable(int expectedNumberOfmatch, params string[] passwords)
        {
            var pwnedPasswords = _haveIBeenPwned.GetPwned(passwords.ToList());

            Assert.Equal(expectedNumberOfmatch, pwnedPasswords.Count);
        }

        #region CONSTRUCTOR

        [Fact]
        public void Only_Url_ThrowIfInvalidUri()
        {
            Assert.Throws<UriFormatException>(() => new HaveIBeenPwned.Password.HaveIBeenPwned("invaliduri"));
        }

        [Fact]
        public void Only_Url_DoesNotThrowIfValidUri()
        {
            var ex = Record.Exception(() => new HaveIBeenPwned.Password.HaveIBeenPwned("https://api.pwnedpasswords.com/range"));

            Assert.Null(ex);
        }

        [Fact]
        public void Url_and_NumberOfTimes_ThrowIfInvalidUri()
        {
            Assert.Throws<UriFormatException>(() => new HaveIBeenPwned.Password.HaveIBeenPwned("invaliduri", 5));
        }

        [Fact]
        public void Url_and_NumberOfTimes_DoesNotThrowIfValidUri()
        {
            var ex = Record.Exception(() => new HaveIBeenPwned.Password.HaveIBeenPwned("https://api.pwnedpasswords.com/range", 5));

            Assert.Null(ex);
        }

        [Fact]
        public void Url_and_NumberOfTimes_ThrowIfInvalidInt()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new HaveIBeenPwned.Password.HaveIBeenPwned("https://api.pwnedpasswords.com/range", -1));
        }

        [Fact]
        public void Url_and_NumberOfTimes_DoesNotThrowIfValidint()
        {
            var ex = Record.Exception(() => new HaveIBeenPwned.Password.HaveIBeenPwned("https://api.pwnedpasswords.com/range", 5));

            Assert.Null(ex);
        }

        [Fact]
        public void Only_NumberOfTimes_ThrowIfInvalidInt()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new HaveIBeenPwned.Password.HaveIBeenPwned(-1));
        }

        [Fact]
        public void Only_NumberOfTimes_DoesNotThrowIfValidInt()
        {
            var ex = Record.Exception(() => new HaveIBeenPwned.Password.HaveIBeenPwned(5));

            Assert.Null(ex);
        }

        #endregion
    }
}
