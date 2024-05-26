using BusinessServices.Implementation;
using Modal;
using Modal.ConcurrentDic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Cache
{
    public class ScoreLink
    {
        private readonly betfairScoreServices _betfairScoreServices;
        private readonly int _Id;
        private Scoreinfo _snap;
        private readonly ConcurrentDictionary<string, ScoreProviderLink> _providers = new ConcurrentDictionary<string, ScoreProviderLink>();
        public ScoreLink(betfairScoreServices BetfairScoreServices, int eventId)
        {
            _betfairScoreServices = BetfairScoreServices;
            _Id = eventId;
            if (_snap == null)
            {
                _snap = new Scoreinfo();
                _snap.EventID = _Id;
            }
        }
        internal async Task<ScoreProviderLink> GetOrAddproviders(string id)
        {
            ScoreProviderLink provider;
            if (!_providers.TryGetValue(id, out provider))
            {
                provider = new ScoreProviderLink(this, id);
                _providers[id] = provider;
            }
            return provider;
        }
        internal async Task OnAddScoreLink(int linkid, int providerId, string ScoreLink, string streamingUrl)
        {
            try
            {
                string strIds = providerId.ToString() + "_" + linkid.ToString();

                ScoreProviderLink provider = await GetOrAddproviders(strIds);
                await provider.OnScoreLink(linkid, providerId, ScoreLink, streamingUrl);

                _snap.links = _providers.Select(k => k.Value.Snap).ToList();
            }
            catch (Exception ex)
            {
            }
        }

        public Scoreinfo Snap
        {
            get
            {
                return _snap;
            }
        }
    }
}
