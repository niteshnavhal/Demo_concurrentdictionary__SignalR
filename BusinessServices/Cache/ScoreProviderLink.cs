using BusinessServices.Implementation;
using BusinessServices.Interface;
using Modal.ConcurrentDic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Cache
{
    public class ScoreProviderLink
    {
        private readonly ScoreLink _ScoreLink;
        private readonly string _Id;
        private ScoreProviderLinkSnap _snap;
        public ScoreProviderLink(ScoreLink ScoreLink, string ID)
        {
            _ScoreLink = ScoreLink;
            _Id = ID;
            if (_snap == null)
            {
                _snap = new ScoreProviderLinkSnap();
                _snap.id = _Id;
                _snap.eventid = _ScoreLink.Snap.EventID;
            }
        }
        internal async Task OnScoreLink(int linkid, int providerId, string ScoreLink, string streamingUrl)
        {
            try
            {
                _snap.link = linkid;
                _snap.providerType = providerId;
                _snap.ScoreUrl = ScoreLink;
                _snap.StreamingUrl = streamingUrl;
            }
            catch (Exception ex)
            {
            }
        }

        public ScoreProviderLinkSnap Snap
        {
            get
            {
                return _snap;
            }
        }
    }
}
