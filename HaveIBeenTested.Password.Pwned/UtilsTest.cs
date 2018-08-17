using System.Runtime.InteropServices.ComTypes;
using HaveIBeenPwned.Password;
using Xunit;

namespace HaveIBeenTested.Password.Pwned
{
    public class UtilsTest
    {
        [Theory]
        [InlineData("password", "5baa61e4c9b93f3f0682250b6cf8331b7ee68fd8")]
        [InlineData("testPassword", "82f8809f42d911d1bd5199021d69d15ea91d1fad")]
        [InlineData("otherPassword", "92ce34b7e0230671ec31a0b522cffafa6b5bd39b")]
        [InlineData("andAFourthpassword", "4b723ddf44953990304ee1a2abf370e456591195")]
        public void GetSha1HashGivesCorrectHash(string input, string expectedHash)
        {
            var hashed = Utils.GetSha1Hash(input);

            Assert.Equal(expectedHash.ToLower(), hashed.ToLower());
        }
    }
}