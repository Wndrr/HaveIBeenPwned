using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HaveIBeenPwned.Password
{
    public class HaveIBeenPwned
    {
        private readonly string _apiAddress;

        /// <summary>
        /// Initializes an instance of the <see cref="HaveIBeenPwned"/> class
        /// </summary>
        public HaveIBeenPwned()
        {
            _apiAddress = "https://api.pwnedpasswords.com/range";
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HaveIBeenPwned"/> class
        /// </summary>
        /// <exception cref="UriFormatException"></exception>
        /// <param name="apiAddress">The rest endpoint to use for the api calls</param>
        public HaveIBeenPwned(string apiAddress)
        {
            if(!Uri.IsWellFormedUriString(apiAddress, UriKind.Absolute))
                throw new UriFormatException($"The \"{apiAddress}\" value for the {nameof(apiAddress)} parameter must be a valid URI.");

            _apiAddress = apiAddress.TrimEnd('/');
        }

        /// <summary>
        /// Calls the HaveIBeenPwned web API with the provided password and returns the number of times it was leaked
        /// </summary>
        /// <exception cref="WebException">Unknown host</exception>
        /// <param name="plainTextPassword">The password to test</param>
        /// <returns>Number of times the password was found</returns>
        public int GetNumberOfTimesPasswordPwned(string plainTextPassword)
        {
            var hash = Utils.GetSha1Hash(plainTextPassword);
            var hashPrefix = hash.Substring(0, 5);
            var hashSufix = hash.Substring(5);
            
            // The haveibeenpwned API takes the 5 first chars of the password's SHA-1 hash and returns a list of all the known leaked passwords whose hash starts with these 5 characters.
            using (var client = new WebClient())
            using (var data = client.OpenRead($"{_apiAddress}/{hashPrefix}"))
            {
                if (data == null)
                    return 0;

                using (var reader = new StreamReader(data))
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (line == null)
                            continue;

                        // Each line of the returned hash list has the following form "hash:numberOfTimesFound"
                        // The first element is the hash
                        // The second element is the numberOfTimesFound
                        var splitLIne = line.Split(':');

                        var lineHashedSuffix = splitLIne[0];
                        var numberOfTimesPasswordPwned = int.Parse(splitLIne[1]);

                        if (lineHashedSuffix == hashSufix)
                            return numberOfTimesPasswordPwned;
                    }

            }

            return 0;
        }

        /// <summary>
        /// Calls the HaveIBeenPwned web API with the provided password and returns true if the password was leaked at least once, false otherwise
        /// </summary>
        /// <exception cref="WebException">Unknown host</exception>
        /// <param name="plainTextPassword">The password to test</param>
        /// <returns>Whether the password was found at all in the database</returns>
        public bool IsPasswordPwned(string plainTextPassword)
        {
            return GetNumberOfTimesPasswordPwned(plainTextPassword) > 0;
        }
    }
}
