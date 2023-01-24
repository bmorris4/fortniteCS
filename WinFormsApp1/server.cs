using Newtonsoft.Json;
using System.Net;
using System.Text;
using File = System.IO.File;

namespace WinFormsApp1
{
    internal class server
    {
        public static async Task StartServer()
        {
            int port = 3642;
            string username = "username";
            var server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:" + port + "/");
            server.Start();
            Console.WriteLine("Server started on port " + port);

            while (true)
            {
                var context = server.GetContext();
                if (context.Request.Url.PathAndQuery.Contains("/datarouter/api/v1/public/"))
                {
                    context.Response.StatusCode = 204;
                }

                if (context.Request.Url.PathAndQuery.Contains("/notifications/undelivered/count"))
                {
                    context.Response.StatusCode = 204;
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/v2/versioncheck/Windows"))
                {
                    var data = JsonConvert.SerializeObject(new
                    {
                        type = "NO_UPDATE"
                    }
                    );
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/game/v2/tryPlayOnPlatform/account/bmorris44?platform=PC"))
                {
                    try
                    {

                        string data = "{\"errorCode\":\"errors.com.epicgames.common.not_found\",\"errorMessage\":\"Sorry the resource you were trying to find could not be found\",\"messageVars\":[],\"numericErrorCode\":1004,\"originatingService\":\"fortnite\",\"intent\":\"prod-live\"}";
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "text/plain";
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                    }
                    catch
                    {
                        string data = "{\"errorCode\":\"errors.com.epicgames.common.not_found\",\"errorMessage\":\"Sorry the resource you were trying to find could not be found\",\"messageVars\":[],\"numericErrorCode\":1004,\"originatingService\":\"fortnite\",\"intent\":\"prod-live\"}";
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "text/plain";
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                    }
                }

                if (context.Request.Url.PathAndQuery == "/account/api/oauth/token")
                {
                    var body = new StreamReader(context.Request.InputStream).ReadToEnd();
                    string[] split = body.Split('=');
                    string grant_type1 = split[1];
                    string[] split2 = grant_type1.Split('&');
                    string grantType = split2[0];
                    var displayName = "";
                    var accountId = "";
                    switch (grantType)
                    {
                        case "client_credentials":
                            displayName = null;
                            accountId = null;
                            break;

                        case "password":
                            displayName = username;
                            accountId = username;
                            break;

                        case "authorization_code":
                            displayName = username;
                            accountId = username;
                            break;

                        case "device_auth":
                            displayName = username;
                            accountId = username;
                            break;

                        case "exchange_code":
                            displayName = username;
                            accountId = username;
                            break;
                        default:
                            break;
                    }
                    var random = new Random();
                    var randomString = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 16).Select(s => s[random.Next(s.Length)]).ToArray());
                    var data1 = JsonConvert.SerializeObject(new
                    {
                        access_token = randomString,
                        expires_in = 28800,
                        expires_at = "9999-12-31T23:59:59.999Z",
                        token_type = "bearer",
                        account_id = accountId,
                        client_id = "ec684b8c687f479fadea3cb2ad83f5c6",
                        internal_client = true,
                        client_service = "fortnite",
                        refresh_token = "eyJ0IjoiZXBpY19pZCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiV01TN0Vua0lHcGNIOURHWnN2MldjWTl4c3VGblpDdHhaamo0QWhiLV84RSJ9.eyJzdWIiOiJlMjE0ODYyMjgzMjA0YjE5OTcyODU3ZjU2MGJhZDhlMCIsInBmc2lkIjoiZm4iLCJpc3MiOiJodHRwczpcL1wvYXBpLmVwaWNnYW1lcy5kZXZcL2VwaWNcL29hdXRoXC92MSIsImRuIjoiUyDOlCBNIFUiLCJwZnBpZCI6InByb2QtZm4iLCJhdWQiOiJlYzY4NGI4YzY4N2Y0NzlmYWRlYTNjYjJhZDgzZjVjNiIsInBmZGlkIjoiNjJhOTQ3M2EyZGNhNDZiMjljY2YxNzU3N2ZjZjQyZDciLCJ0IjoiZXBpY19pZCIsImFwcGlkIjoiZmdoaTQ1NjdGTkZCS0Z6M0U0VFJPYjBibVBTOGgxR1ciLCJzY29wZSI6ImJhc2ljX3Byb2ZpbGUgZnJpZW5kc19saXN0IG9wZW5pZCBwcmVzZW5jZSIsImV4cCI6MTY2ODU1NjkzOSwiaWF0IjoxNjY4NTI4MTM5LCJqdGkiOiI1YzI1ODVkZDZmYzE0MTQ3ODRhNmJjNzM1MDg1YjJjMiJ9.k6n-oFrrQF2x5eNn1BWO7-buauGWSlCcDnc6m-p_-1KK2WZv1cjSFQbfdC3rPPKtABGhfyvy7TNkgZGmCr7W4Kh2PgXT_zJMnRIZ49ibZqKzsCcg-AU3MrNgPz4lqfwwi7uU5oLc6LdgXym2KUADLYygMQn0tM5oYJHGM2FzHhFvgdjigdFIxp94wNG7DiWKpYHB5XkvOJfcctF20RtCufuy9VswvmIXSe443RvWJFsfO0pZZ4vlxbz3FUV9b3Dc-0UQRdg-RaSMLebT7GoaQL7uajYglXEL6WEYQEJccopitAJtjqAvr_5F-7w2fbVyBLWD4xByTcAzLa3KGrWrLQ",
                        refresh_expires = 115200,
                        refresh_expires_at = "9999-12-31T23:59:59.999Z",
                        account_display_name = displayName,
                        app = "fortnite",
                        in_app_id = accountId,
                        device_id = "5dcab5dbe86a7344b061ba57cdb33c4f"
                    }
                    );
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);

                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/game/v2/enabled_features"))
                {
                    string data1 = "[]";
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);

                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/cloudstorage/user/"))
                {
                    if (context.Request.Url.PathAndQuery.Contains("ClientSettings.Sav"))
                    {
                        string data1 = System.IO.File.ReadAllText("ClientSettings.Sav");
                        context.Response.StatusCode = 200;
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                    }
                    else
                    {
                        var data1 = JsonConvert.SerializeObject(new
                        {
                            uniqueFilename = "ClientSettingsSwitch.Sav",
                            filename = "ClientSettingsSwitch.Sav",
                            hash = "b565046375e82a4d12b990c77b5785430780a60f",
                            hash256 = "577c4975694c4c1c07d25eb47283207cdfb6f5e4a485937d197be89358c9fc70",
                            length = 14917,
                            contentType = "text/plain",
                            uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                            storageType = "S3",
                            storageIds = new { },
                            metadata = new { },
                            accountId = username
                        });
                        context.Response.StatusCode = 200;
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                    }
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/receipts/v1/account/"))
                {
                    string data1 = "[]";
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/friends/api/public/blocklist/"))
                {
                    string data = "[]";
                    var data1 = JsonConvert.SerializeObject(new
                    {
                        blockedUsers = data
                    });
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/catalog/api/shared/bulk/offers"))
                {
                    var data1 = JsonConvert.SerializeObject(new
                    { });
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("friends/api/public/friends/"))
                {
                    var data1 = JsonConvert.SerializeObject(new
                    {
                        accountId = "test",
                        status = "ACCEPTED",
                        direction = "INBOUND",
                        created = "2018-12-06T04:46:01.296Z",
                        favorite = false

                    });
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/friends/api/v1/"))
                {
                    string accountId = context.Request.Url.PathAndQuery.Split(new string[] { "/v1/" }, StringSplitOptions.None)[1];
                    if (accountId.Contains("settings"))
                    {
                        var data1 = JsonConvert.SerializeObject(new
                        {
                            acceptInvites = "public"
                        });
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);

                    }
                    else if (accountId.Contains("/friends"))
                    {
                        string message = "{ \"errorCode\": \"errors.com.epicgames.Neonite.common.forbidden\", \"errorMessage\": \"You cannot remove the bot\", \"messageVars\": [], \"numericErrorCode\": 14004, \"originatingService\": \"party\", \"intent\": \"prod\" }";
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(message).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(message), 0, Encoding.UTF8.GetBytes(message).Length);
                    }
                    else if (accountId.Contains("/blocklist"))
                    {
                        string data1 = "[]";
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                    }
                    else if (accountId.Contains("/recent/fortnite"))
                    {
                        string data1 = "[]";
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                    }
                    else if (accountId.Contains("/summary"))
                    {
                        string groups;
                        int mutual;
                        string alias;
                        string note;
                        bool favorite;
                        string created;


                        var data1 = JsonConvert.SerializeObject(new
                        {
                            friends = new object[] {
                            accountId= "test",
                            groups= "[]",
                            mutual = 0,
                            alias= "",
                            note= "",
                            favorite= true,
                            created= "2021-01-17T16:42:04.125Z"
                            },
                            incoming = "[]",
                            suggested = "[]",
                            blocklist = "[]",
                            settings = new
                            {
                                acceptInvites = "public"
                            },
                            limitsReached = new
                            {
                                incoming = false,
                                outgoing = false,
                                accepted = false
                            }

                        });
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);

                    }
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/game/v2/grant_access/"))
                {
                    context.Response.StatusCode = 204;
                }

                if (context.Request.Url.PathAndQuery.Contains("/waitingroom/api/waitingroom"))
                {
                    context.Response.StatusCode = 204;
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/calendar/v1/timeline"))
                {
                    string version;
                    version = context.Request.Headers["User-Agent"].Split('-')[1].Split(new string[] { "-CL" }, StringSplitOptions.None)[0];
                    string season = version.Split('.')[0];
                    DateTime date = DateTime.Now;
                    string s = "{            \"channels\": {                \"standalone-store\": {},                \"client-matchmaking\": {},                \"tk\": {},                \"featured-islands\": {},                \"community-votes\": {},                \"client-events\": {                    \"states\": [{                        \"validFrom\": \"2020-05-21T18:36:38.383Z\",                        \"activeEvents\": [                            {                                \"eventType\": \"EventFlag.LobbySeason" + season + "\",                                \"activeUntil\": \"9999-12-31T23:59:59.999Z\",                                \"activeSince\": \"2019-12-31T23:59:59.999Z\"                            }                        ],                        \"state\": {                            \"activeStorefronts\": [],                            \"eventNamedWeights\": {},                            \"activeEvents\": [],                            \"seasonNumber\": " + Convert.ToInt64(season) + ",                            \"seasonTemplateId\": \"AthenaSeason:athenaseason" + season + "\",                            \"matchXpBonusPoints\": 0,                            \"eventPunchCardTemplateId\": \"\",                            \"seasonBegin\": \"9999-12-31T23:59:59.999Z\",                            \"seasonEnd\": \"9999-12-31T23:59:59.999Z\",                            \"seasonDisplayedEnd\": \"9999-12-31T23:59:59.999Z\",                            \"weeklyStoreEnd\": \"9999-12-31T23:59:59.999Z\",                            \"stwEventStoreEnd\": \"9999-12-31T23:59:59.999Z\",                            \"stwWeeklyStoreEnd\": \"9999-12-31T23:59:59.999Z\",                            \"dailyStoreEnd\": \"9999-12-31T23:59:59.999Z\"                        }                    }],                    \"cacheExpire\": \"9999-12-31T23:59:59.999Z\"                }            },            \"cacheIntervalMins\": 99999,            \"currentTime\": " + date + "        }";
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(s).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(s), 0, Encoding.UTF8.GetBytes(s).Length);

                }

                if (context.Request.Url.PathAndQuery.Contains("/content/api/pages/fortnite-game"))
                {
                    string json = File.ReadAllText("fortnite-game.json");

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(json).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(json), 0, Encoding.UTF8.GetBytes(json).Length);

                }

                if (context.Request.Url.PathAndQuery.Contains("/api/v1/events/Fortnite/download/"))
                {
                    string data = "[]";
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);

                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/storefront/v2/catalog"))
                {
                    string data = File.ReadAllText("store.json");
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/friends/api/public/list/fortnite/"))
                {
                    string data1 = "[]";
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/storefront/v2/keychain"))
                {
                    string data1 = File.ReadAllText("keychain.json");
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/account/api/public/account?accountId="))
                {
                    string accountId = context.Request.Url.PathAndQuery.Replace("/account/api/public/account?accountId=", "");
                    var data1 = JsonConvert.SerializeObject(new
                    {
                        id = username
                    }
                    );
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data1).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data1), 0, Encoding.UTF8.GetBytes(data1).Length);

                }

                if (context.Request.Url.PathAndQuery == "/fortnite/api/cloudstorage/system")
                {
                    var data = JsonConvert.SerializeObject(new
                    {
                        uniqueFilename = "3460cbe1c57d4a838ace32951a4d7171",
                        filename = "DefaultGame.ini",
                        hash = "603E6907398C7E74E25C0AE8EC3A03FFAC7C9BB4",
                        hash256 = "973124FFC4A03E66D6A4458E587D5D6146F71FC57F359C8D516E0B12A50AB0D9",
                        length = File.ReadAllText("DefaultGame.ini").Length,
                        contentType = "application/octet-stream",
                        uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        storageType = "S3",
                        doNotCache = false
                    });

                    var data2 = JsonConvert.SerializeObject(new
                    {
                        uniqueFilename = "3460cbe1c57d4a838ace32951a4d7172",
                        filename = "DefaultEngine.ini",
                        hash = "603E6907398C7E74E25C0AE8EC3A03FFAC7C9BB5",
                        hash256 = "973124FFC4A03E66D6A4458E587D5D6146F71FC57F359C8D516E0B12A50AB0D8",
                        length = File.ReadAllText("DefaultEngine.ini").Length,
                        contentType = "application/octet-stream",
                        uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        storageType = "S3",
                        doNotCache = false
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.PathAndQuery.Contains("/fortnite/api/game/v2/profile"))
                {
                    if (context.Request.Url.PathAndQuery.Contains("QueryProfile"))
                    {
                        if (context.Request.Url.PathAndQuery.Contains("athena"))
                        {
                            string data = File.ReadAllText("athena.json");
                            data = data.Replace("replace me", username);
                            data = data.Replace("date replace", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"));
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                            context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                        }
                        else if (context.Request.Url.PathAndQuery.Contains("common_core"))
                        {
                            string data = File.ReadAllText("common_core.json");
                            data = data.Replace("replace me", username);
                            data = data.Replace("date replace", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"));
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                            context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                        }
                        else if (context.Request.Url.PathAndQuery.Contains("ClientQuestLogin"))
                        {
                            string data = File.ReadAllText("quest.json");
                            data = data.Replace("replace me", username);
                            data = data.Replace("date replace", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"));
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                            context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                        }
                        else if (context.Request.Url.PathAndQuery.Contains("common_public"))
                        {
                            string data = File.ReadAllText("common_public.json");
                            data = data.Replace("replace me", username);
                            data = data.Replace("date replace", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"));
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                            context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                        }
                        else if (context.Request.Url.PathAndQuery.Contains("creative"))
                        {
                            string data = File.ReadAllText("creative.json");
                            data = data.Replace("replace me", username);
                            data = data.Replace("date replace", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"));
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                            context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                        }
                        else if (context.Request.Url.PathAndQuery.Contains("collections"))
                        {
                            string data = File.ReadAllText("collections.json");
                            data = data.Replace("replace me", username);
                            data = data.Replace("date replace", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"));
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 200;
                            context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                        }
                    }
                    else
                    {
                        var data = JsonConvert.SerializeObject(new
                        {
                            errorCode = "errors.com.epicgames.common.not_found",
                            errorMessage = "Sorry the resource you were trying to find could not be found",
                            messageVars = new { },
                            numericErrorCode = 1004,
                            originatingService = "fortnite",
                            intent = "prod-live"
                        });
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 200;
                        context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                        context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                    }
                }

                if (context.Request.Url.PathAndQuery.Contains("/api/public/account"))
                {
                    var data = JsonConvert.SerializeObject(new
                    {
                        id = username,
                        displayName = username,
                        name = "skidalot",
                        email = "skidder@iskid.com",
                        failedLoginAttempts = 0,
                        lastLogin = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        numberOfDisplayNameChanges = 3,
                        ageGroup = "UNKNOWN",
                        headless = false,
                        country = "BR",
                        countryUpdatedTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        lastName = "skidalot",
                        preferredLanguage = "en",
                        lastDisplayNameChange = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        canUpdateDisplayName = true,
                        tfaEnabled = true,
                        emailVerified = true,
                        minorVerified = false,
                        minorExpected = false,
                        minorStatus = "UNKNOWN"
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);

                }



                if (context.Request.Url.PathAndQuery == "/fortnite/api/cloudstorage/system/config")
                {
                    var data = JsonConvert.SerializeObject(new
                    {
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }


                if (context.Request.Url.PathAndQuery.Contains("/lightswitch/api/service/bulk/status"))
                {
                    var data = "[{\"serviceInstanceId\":\"fortnite\",\"status\":\"UP\",\"message\":\"GO AWAY KID\",\"maintenanceUri\":null,\"allowedActions\":[\"PLAY\",\"DOWNLOAD\"],\"banned\":false}]";

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }
                if (context.Request.Url.PathAndQuery == "/fortnite/api/cloudstorage/system/3460cbe1c57d4a838ace32951a4d7171")
                {
                    var defaultGame = File.ReadAllBytes("DefaultGame.ini");

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = defaultGame.Length;
                    context.Response.OutputStream.Write(defaultGame, 0, defaultGame.Length);
                }
                if (context.Request.Url.PathAndQuery == "/fortnite/api/cloudstorage/system/3460cbe1c57d4a838ace32951a4d7172")
                {
                    var defaultGame = File.ReadAllBytes("DefaultEngine.ini");

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = defaultGame.Length;
                    context.Response.OutputStream.Write(defaultGame, 0, defaultGame.Length);
                }
            }
        }
    }
}
