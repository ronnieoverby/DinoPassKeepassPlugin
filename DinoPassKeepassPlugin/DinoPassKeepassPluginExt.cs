using KeePass.Plugins;
using System.Net;

namespace DinoPassKeepassPlugin
{
    public sealed class DinoPassKeepassPluginExt : Plugin
    {
        private WebClient _client;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) 
                return false;

            _client = new WebClient();
            host.PwGeneratorPool.Add(DinoPassGenerator.CreateStrong(_client));
            host.PwGeneratorPool.Add(DinoPassGenerator.CreateSimple(_client));

            return true;
        }

        public override void Terminate()
        {
            _client.Dispose();
            _client = null;
        }
    }
}