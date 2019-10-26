using System;

namespace Micro.KeyStore.Api.Workers
{
    [Serializable]
    public class Key
    {
        public string Id { set; get; }
        public string Body { set; get; }
    }
}
