using betfaireScore.Hubs;
using BusinessServices.Interface;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Modal;
using Modal.betfairscore;
using Modal.CommanrtyScore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Modal.Enums;

namespace betfaireScore.Services
{
    public class getScore
    {
        private int iGetScoreInterval = 1000;
        private int iShowTimeGetScoreinterval = 1000;
        private BackgroundWorker bwScore;
        private BackgroundWorker bwCommantryScore;
        private IScoreServices _IScoreServices;
        private readonly IbetfairScoreServices _ibetfairScoreServices;
        private IConfiguration _IConfiguration;
        private static string _CommantrySocketUrl = "";
        public static bool isCommantryScoketActive = false;
        public static int _IntervalDoWhileLoop = 1000;
        public static bool isScoketthreadStart = false;
        public static bool _IsWebSocketActive = false; 
        public static string _ScoketConnectionMethod = "";
        public static string _ScoketConnectionLiveEventMethod = "";
        public static string _WebScoketConnectionUri = "";
        private IHubContext<bfScore> Hub { get; set; }

        public getScore(IHubContext<bfScore> hub, IConfiguration IConfiguration, IScoreServices IScoreServices, IbetfairScoreServices ibetfairScoreServices)
        {
            try
            {


                _IConfiguration = IConfiguration;
                _IScoreServices = IScoreServices;
                _ibetfairScoreServices = ibetfairScoreServices;

                iGetScoreInterval = Convert.ToInt32(_IConfiguration["betfair:getScoreInterval"]);
                iShowTimeGetScoreinterval = Convert.ToInt32(_IConfiguration["betfair:bfShowTimeGetScoreinterval"]);
                _CommantrySocketUrl = _IConfiguration["commentry:SocketUrl"];
                isCommantryScoketActive = Convert.ToBoolean(_IConfiguration["commentry:isActive"]);
                _IntervalDoWhileLoop = Convert.ToInt32(_IConfiguration["commentry:IntervalDoWhileLoop"]);
                _ScoketConnectionMethod = _IConfiguration["commentry:ScoketConnectionMethod"];
                _ScoketConnectionLiveEventMethod = _IConfiguration["commentry:ScoketConnectionLiveEventMethod"];
                _WebScoketConnectionUri = _IConfiguration["onlyscore:socketURI"];
                _IsWebSocketActive = Convert.ToBoolean(_IConfiguration["onlyscore:IsSocketActive"]);
                Hub = hub;

                this.bwScore = new BackgroundWorker();
                this.bwScore.DoWork += new DoWorkEventHandler(bwScore_DoWork);
                this.bwScore.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwScore_RunWorkerCompleted);
                this.bwScore.WorkerReportsProgress = true;
                this.bwScore.RunWorkerAsync();

                this.bwCommantryScore = new BackgroundWorker();
                this.bwCommantryScore.DoWork += new DoWorkEventHandler(bwCommantryScore_DoWork);
                this.bwCommantryScore.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCommantryScore_RunWorkerCompleted);
                this.bwCommantryScore.WorkerReportsProgress = true;
                this.bwCommantryScore.RunWorkerAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private async void bwScore_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    System.DateTime objStartTime = DateTime.UtcNow.AddHours(5.5);
                    var vtemplstscore = await _IScoreServices.getallScore();
                    if (vtemplstscore != null)
                    {
                        //var vDate = DateTime.UtcNow.AddHours(5.5).AddMilliseconds(iShowTimeGetScoreinterval * (-1));
                        var vlstEventID = vtemplstscore.Where(p => p.Value.isfinished != true //&& p.Value.showtime >= vDate && p.Value.eventtypeid != 1
                        )
                                                 .Select(p => p.Key)
                                                 .ToList();
                        if (vlstEventID.Count > 0)
                        {
                            for (int i = 0; i < vlstEventID.Count; i++)
                            {
                                var EventScore = await _IScoreServices.getupdatebfScore(vlstEventID[i].ToString());
                                BroadcastScore(vlstEventID[i].ToString(), EventScore.score, EventScore.shortscore);
                            }
                        }

                        //var vlstSoccreEventID = vtemplstscore.Where(p => p.Value.isfinished != true && p.Value.showtime >= vDate && p.Value.eventtypeid == 1)
                        //                         .Select(p => p.Key)
                        //                         .ToList();
                        //if (vlstSoccreEventID.Count > 0)
                        //{
                        //    for (int i = 0; i < vlstSoccreEventID.Count; i++)
                        //    {
                        //        await _IScoreServices.getupdatebfScoreTimeline(vlstSoccreEventID[i].ToString());
                        //    }
                        //}
                    }
                    System.DateTime objEndTime = DateTime.UtcNow.AddHours(5.5);
                    TimeSpan spDate = (objEndTime - objStartTime);
                    var iMilliseconds = spDate.Milliseconds;
                    if (iMilliseconds < Convert.ToInt32(iGetScoreInterval))
                    {
                        Thread.Sleep((Convert.ToInt32(iGetScoreInterval) - iMilliseconds));
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        private void bwScore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private async void bwCommantryScore_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (isCommantryScoketActive)
                    {
                        System.DateTime objStartTime = getdate();
                        if (!isScoketthreadStart)
                        {
                            isScoketthreadStart = true;
                            System.Threading.Thread vSocketThread = new System.Threading.Thread(SocketConnectivity);
                            object[] vThreadparam = new object[1]
                            {vSocketThread};
                            vSocketThread.Name = "vSocketCommanrtyThread";
                            vSocketThread.Start(vThreadparam);
                        }

                        System.DateTime objEndTime = getdate();
                        TimeSpan spDate = (objEndTime - objStartTime);
                        var iMilliseconds = spDate.Milliseconds;
                        if (iMilliseconds < 1000)
                        {
                            Thread.Sleep((1000 - iMilliseconds));
                        }
                    }
                    if (_IsWebSocketActive)
                    {
                        System.DateTime objStartTime = getdate();
                        if (!isScoketthreadStart)
                        {
                            isScoketthreadStart = true;
                            System.Threading.Thread vSocketThread = new System.Threading.Thread(WebsocketConn);
                            object[] vThreadparam = new object[1]
                            {vSocketThread};
                            vSocketThread.Name = "vWebSocketNodeThread";
                            vSocketThread.Start(vThreadparam);
                        }

                        System.DateTime objEndTime = getdate();
                        TimeSpan spDate = (objEndTime - objStartTime);
                        var iMilliseconds = spDate.Milliseconds;
                        if (iMilliseconds < 1000)
                        {
                            Thread.Sleep((1000 - iMilliseconds));
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        private void bwCommantryScore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BroadcastScore(string strEventID, object vinfo, object vShortScore)
        {
            Hub.Clients.Group(strEventID).SendAsync("Score", vinfo);
            Hub.Clients.Group(common.ShortScoreGroupName).SendAsync("ShortScore", vShortScore);
        }
        private void BroadcastShortScore(string strEventID, object vShortScore)
        {
            Hub.Clients.Group(common.ShortScoreGroupName).SendAsync("ShortScore", vShortScore);
        }

        private void BroadcastScoreToclient(string Connectionid, object vinfo)
        {
            Hub.Clients.Clients(Connectionid).SendAsync("SubScribescore", vinfo);
        }

        #region SocketConnection
        public async void SocketConnectivity(object vParam)
        {
            string strSocket = _CommantrySocketUrl.ToString();
            object[] objInfo = vParam as object[];
            System.Threading.Thread _signalr = (System.Threading.Thread)objInfo[0];
            if (isCommantryScoketActive)
            {
                bool isConnected = false;
                try
                {
                    HubConnection vCommantrySignalR = new HubConnectionBuilder()
                    .WithUrl(strSocket, HttpTransportType.WebSockets)
                    .Build();

                    do
                    {
                        try
                        {
                            vCommantrySignalR.StartAsync().ContinueWith(task =>
                            {
                                if (!task.IsFaulted)

                                {
                                    isConnected = true;

                                }
                            }).Wait(_IntervalDoWhileLoop);
                        }
                        catch (Exception ex)
                        {

                        }
                        if (isConnected == false)
                        {

                        }
                    } while (isConnected == false);

                    vCommantrySignalR.Closed += async (error) =>
                    {
                        isConnected = false;
                    };

                    vCommantrySignalR.On<ShortScore>("ShortScore", (async param =>
                    {
                        try
                        {
                            param.eti = Convert.ToInt32(Enum_SportsType.Cricket);
                            BroadcastShortScore(param.eid, param);
                        }
                        catch (Exception ex) { }
                    }));

                    vCommantrySignalR.On<SubScribeShortScore>("subScribetShortScore", (async param =>
                    {
                        try
                        {
                            if (param != null)
                            {
                                BroadcastScoreToclient(param.id, param.scr);
                            }
                        }
                        catch (Exception ex) { }
                    }));

                    await vCommantrySignalR.InvokeAsync(_ScoketConnectionMethod);

                    do
                    {
                        var obj = await _IScoreServices.getallConnectionID();
                        if (obj != null && obj.Count > 0)
                        {
                            foreach (var kvp in obj)
                            {
                                //Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
                                await vCommantrySignalR.InvokeAsync(_ScoketConnectionLiveEventMethod, kvp.Value);
                                await _IScoreServices.RemoveSubscribetEventtoSocket(kvp.Key);
                            }
                        }
                    }
                    while (isConnected);

                    _signalr.Abort();
                }
                catch (ThreadAbortException ex)
                {

                }
                catch (Exception ex)
                {
                    isConnected = false;
                }
                finally
                {
                    isScoketthreadStart = false;
                }
            }
        }


        public async void WebsocketConn(object vParam) 
        {

            string strWebSocket = _WebScoketConnectionUri.ToString();
            object[] objInfo = vParam as object[];
            System.Threading.Thread _websocket = (System.Threading.Thread)objInfo[0];
            if (_IsWebSocketActive)
            {
                var uri = new Uri(strWebSocket);

                using (var client = new ClientWebSocket())
                {
                    try
                    {
                        await client.ConnectAsync(uri, CancellationToken.None);
                        Console.WriteLine("Connected to WebSocket server.");

                        var sendTask = Task.Run(async () =>
                        {
                            while (client.State == WebSocketState.Open)
                            {
                                var obj = await _IScoreServices.getallConnectionID();
                                if (obj != null && obj.Count > 0)
                                {
                                    foreach (var kvp in obj)
                                    {
                                        string sendDataForSocketUpdate = "{\"event\": \"subScribetShortScore\", \"data\": \"" + kvp.Value.eventids + "\",\"connectionID\": \"" + kvp.Value.connectionID + "\"}";
                                        await SendMessageAsync(client, sendDataForSocketUpdate);
                                        await _IScoreServices.RemoveSubscribetEventtoSocket(kvp.Key);
                                    }
                                }
                            }
                        });

                        var receiveTask = Task.Run(async () =>
                        {
                            while (client.State == WebSocketState.Open)
                            {
                                try { 
                                    List<ShortScore> _shortScores = new List<ShortScore>();
                                    string receivedMessage = await ReceiveMessageAsync(client);
                                    WebSocketMessage message = JsonConvert.DeserializeObject<WebSocketMessage>(receivedMessage);
                                    if (!string.IsNullOrEmpty(message.connectionID))
                                    {
                                        _shortScores = JsonConvert.DeserializeObject<List<ShortScore>>(message.data);
                                        BroadcastScoreToclient(message.connectionID, _shortScores);
                                    }
                                    else
                                    {
                                        ShortScore shortScore = JsonConvert.DeserializeObject<ShortScore>(message.data);
                                        BroadcastShortScore(shortScore.eid, shortScore); 
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        });

                        await Task.WhenAll(sendTask, receiveTask);
                        _websocket.Abort();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine($"Error: {ex.Message}");
                    }
                    finally
                    {
                        if (client.State == WebSocketState.Open)
                            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

                        isScoketthreadStart = false;
                    }
                }
            }
        }

        static async Task SendMessageAsync(ClientWebSocket client, string message)
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await client.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        static async Task<string> ReceiveMessageAsync(ClientWebSocket client)
        {
            var buffer = new ArraySegment<byte>(new byte[4096]);
            WebSocketReceiveResult result = await client.ReceiveAsync(buffer, CancellationToken.None);
            return Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
        }

        public static DateTime getdate()
        {
            return DateTime.Now;
        }

        public async Task getLiveEventScore(string connectionID, string strEventIDs)
        {

            var vtemplstscoreCard = await _ibetfairScoreServices.getAllScorecard();
            if (vtemplstscoreCard != null && vtemplstscoreCard.Count > 0)
            {
                if (_IsWebSocketActive) 
                { 
                     var vlstEventID = vtemplstscoreCard.SelectMany(k => k.links.Where(p => p.providerType == Convert.ToInt32(Enum_ProviderType.NewOnlyScore))).Select(p => p).ToList();
                    //var vlstEventID = vtemplstscoreCard.Where(p => p.providerType == Convert.ToInt32(Enum_ProviderType.OnlyScore)).Select(p => p).ToList();
                    if (vlstEventID.Count > 0)
                    {
                        string sendEventids = "";
                        strEventIDs = strEventIDs.Trim(',');
                        string[] objEventIds = strEventIDs.Split(',');
                        if (objEventIds.Length > 0)
                        {
                            for (int i = 0; i < objEventIds.Length; i++)
                            {
                                if (objEventIds[i].ToString() != "")
                                {
                                    if (vlstEventID.Where(k => k.eventid == Convert.ToDouble(objEventIds[i].ToString())).FirstOrDefault() != null)
                                    {
                                        sendEventids += objEventIds[i].ToString() + ',';
                                    }

                                }
                            }
                        }

                        sendEventids = sendEventids.Trim(',');

                        if (!string.IsNullOrEmpty(sendEventids))
                        {
                                await _IScoreServices.SubscribetEventtoSocket(sendEventids, connectionID);
                        }
                    }
                }

                if (isCommantryScoketActive)
                {
                    var vlstEventID = vtemplstscoreCard.SelectMany(k => k.links.Where(p => p.providerType == Convert.ToInt32(Enum_ProviderType.OnlyScore))).Select(p => p).ToList();
                    //var vlstEventID = vtemplstscoreCard.Where(p => p.providerType == Convert.ToInt32(Enum_ProviderType.OnlyScore)).Select(p => p).ToList();
                    if (vlstEventID.Count > 0)
                    {
                        string sendEventids = "";
                        strEventIDs = strEventIDs.Trim(',');
                        string[] objEventIds = strEventIDs.Split(',');
                        if (objEventIds.Length > 0)
                        {
                            for (int i = 0; i < objEventIds.Length; i++)
                            {
                                if (objEventIds[i].ToString() != "")
                                {
                                    if (vlstEventID.Where(k => k.eventid == Convert.ToDouble(objEventIds[i].ToString())).FirstOrDefault() != null)
                                    {
                                        sendEventids += objEventIds[i].ToString() + ',';
                                    }

                                }
                            }
                        }

                        sendEventids = sendEventids.Trim(',');

                        if (!string.IsNullOrEmpty(sendEventids))
                        {
                            await _IScoreServices.SubscribetEventtoSocket(sendEventids, connectionID);
                        }
                    }
                }
                //var vlstBFEventID = vtemplstscoreCard.Where(p => p.providerType != Convert.ToInt32(Enum_ProviderType.OnlyScore)).Select(p => p).ToList();
                var vlstBFEventID = vtemplstscoreCard.SelectMany(k => k.links.Where(p => p.providerType == Convert.ToInt32(Enum_ProviderType.BetFair))).Select(p => p).ToList();
                if (vlstBFEventID.Count > 0)
                {
                    foreach (var item in vlstBFEventID)
                    {
                        await _IScoreServices.getScore(item.eventid.ToString());
                    }
                }
            }
        }
    }
    #endregion
}
