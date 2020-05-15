using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using System;
using System.Net;

namespace DinoPassKeepassPlugin
{
    class DinoPassGenerator : CustomPwGenerator
    {
        public override PwUuid Uuid { get; }

        private readonly string _url;
        private readonly WebClient _webClient;

        public override string Name { get; }

        public DinoPassGenerator(string name, Guid pwUuid, string getPasswordUrl, WebClient webClient)
        {
            _url = getPasswordUrl ?? throw new ArgumentNullException(nameof(getPasswordUrl));
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));           

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Uuid = new PwUuid(pwUuid.ToByteArray());
        }

        public override ProtectedString Generate(PwProfile prf, CryptoRandomStream crsRandomSource)
        {
            var pw = _webClient.DownloadString(_url).Trim();
            return new ProtectedString(true, pw);
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }

        public static DinoPassGenerator CreateStrong(WebClient client) =>
            new DinoPassGenerator("DinoPass (Strong)",
                Guid.Parse("c9e67bb1-de65-4774-bc4a-57b66b0b95d5"),
                "https://www.dinopass.com/password/strong", client);

        public static DinoPassGenerator CreateSimple(WebClient client) =>
            new DinoPassGenerator("DinoPass (Simple)",
                Guid.Parse("476d9fbf-8baf-4e18-b70a-0c26d541acaf"),
                "https://www.dinopass.com/password/simple", client);
    }
}