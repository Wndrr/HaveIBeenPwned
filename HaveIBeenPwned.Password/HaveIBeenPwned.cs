using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace HaveIBeenPwned.Password
{
    public class HaveIBeenPwned
    {
        /// <summary>
        /// Determines how many times a password must have been leaked for it to be considered pwned
        /// </summary>
        private readonly int _numberOfLeaksForPwn = 1;
        private readonly string _apiAddress = "https://api.pwnedpasswords.com/range";

        #region CONSTRUCTORS
        
        /// <summary>
        /// Initializes an instance of the <see cref="T:HaveIBeenPwned.Password.HaveIBeenPwned" /> class
        /// </summary>
        /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
        /// <param name="numberOfLeaksForPwn">Used by the <see cref="M:HaveIBeenPwned.Password.HaveIBeenPwned.IsPasswordPwned(System.String)" /> and <see cref="!:GetPwned" />.</param>
        public HaveIBeenPwned(int numberOfLeaksForPwn = 1)
        {
            if(numberOfLeaksForPwn < 1 )
                throw new ArgumentOutOfRangeException($"The {nameof(numberOfLeaksForPwn)} parameter's value can't be under 0");

            _numberOfLeaksForPwn = numberOfLeaksForPwn;
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
        /// Initializes an instance of the <see cref="HaveIBeenPwned"/> class
        /// </summary>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <param name="apiAddress">The rest endpoint to use for the api calls</param>
        /// <param name="numberOfLeaksForPwn">Used by the <see cref="IsPasswordPwned"/> and <see cref="GetPwned"/>.</param>
        public HaveIBeenPwned(string apiAddress, int numberOfLeaksForPwn) : this(apiAddress)
        {
            if (numberOfLeaksForPwn < 1)
                throw new ArgumentOutOfRangeException($"The {nameof(numberOfLeaksForPwn)} parameter's value can't be under 0");

            _numberOfLeaksForPwn = numberOfLeaksForPwn;
        }
        
        #endregion

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
            var numberOfTimesPasswordPwned = GetNumberOfTimesPasswordPwned(plainTextPassword);
            return numberOfTimesPasswordPwned >= _numberOfLeaksForPwn;
        }

        /// <summary>
        /// Calls the HaveIBeenPwned web API for each provided password and returns the list of password that were leaked
        /// </summary>
        /// <exception cref="WebException">Unknown host</exception>
        /// <param name="passwords">The passwords to test</param>
        /// <returns></returns>
        public List<string> GetPwned(params string[] passwords)
        {
            var pwned = passwords.Where(IsPasswordPwned);

            return pwned.ToList();
        }

        /// <summary>
        /// Calls the HaveIBeenPwned web API for each provided password and returns the list of password that were leaked
        /// </summary>
        /// <exception cref="WebException">Unknown host</exception>
        /// <param name="passwords">The passwords to test</param>
        /// <returns></returns>
        public List<string> GetPwned(IEnumerable<string> passwords)
        {
            return GetPwned(passwords.ToArray());
        }
    }
}
