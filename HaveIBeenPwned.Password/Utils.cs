using System.Security.Cryptography;
using System.Text;

namespace HaveIBeenPwned.Password
{
    /// <summary>
    /// Contains the utility methods for the project
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Hashes a string using the SHA-1 algorithm
        /// </summary>
        /// <param name="input">The string to hash</param>
        /// <returns>Hashed string in lowecase</returns>
        public static string GetSha1Hash(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var stringBuilder = new StringBuilder(hash.Length * 2);

                foreach (var b in hash)
                {
                    // can be "x2" if you want lowercase
                    stringBuilder.Append(b.ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}