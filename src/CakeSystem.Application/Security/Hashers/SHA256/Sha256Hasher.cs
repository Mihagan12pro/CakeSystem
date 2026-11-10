using Net.Cryptography.SHA256;

namespace CakeSystem.Application.Security.Hashers.SHA256
{
    internal class Sha256Hasher : ISha256Hasher
    {
        public string Hash(string input)
            => input.ToSha256();
    }
}
