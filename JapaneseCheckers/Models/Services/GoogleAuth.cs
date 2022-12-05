using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using JapaneseCheckers.ViewModels;
using static JapaneseCheckers.Models.Services.Encryption;

namespace JapaneseCheckers.Models.Services;

public class GoogleAuth : MvvmBase
{
    private const string clientId = "581786658708-elflankerquo1a6vsckabbhn25hclla0.apps.googleusercontent.com";
    private const string clientSecret = "3f6NggMbPtrmIBpgx-MK2xXK";
    private const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

    private string userinfoResponseText = "";

    public string UserinfoResponseText
    {
        get => userinfoResponseText;
        set => Set(ref userinfoResponseText, value);
    }

    public static int GetRandomUnusedPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    public async void Auth()
    {
        var state = RandomDataBase64Url(32);
        var codeVerifier = RandomDataBase64Url(32);
        var codeChallenge = Base64UrlencodeNoPadding(Sha256Bytes(codeVerifier));
        const string codeChallengeMethod = "S256";

        var redirectUri = $"http://{IPAddress.Loopback}:{GetRandomUnusedPort()}/";

        var http = new HttpListener();
        http.Prefixes.Add(redirectUri);
        http.Start();

        var authorizationRequest = string.Format(
            "{0}?response_type=code&scope=openid%20profile%20email&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
            authorizationEndpoint,
            Uri.EscapeDataString(redirectUri),
            clientId,
            state,
            codeChallenge,
            codeChallengeMethod);

        var ps = new ProcessStartInfo(authorizationRequest)
        {
            UseShellExecute = true,
            Verb = "open"
        };

        Process.Start(ps);

        var context = await http.GetContextAsync();


        var response = context.Response;
        var responseString =
            "<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body><h1>You can close this page now.</h1></body></html>";
        var buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        var responseOutput = response.OutputStream;
        await responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith(_ =>
        {
            responseOutput.Close();
            http.Stop();
            Console.WriteLine("HTTP server stopped.");
        });

        var code = context.Request.QueryString.Get("code");
        var incomingState = context.Request.QueryString.Get("state");

        if (context.Request.QueryString.Get("error") is not null) return;
        if (code is null) return;
        if (incomingState is null) return;
        if (incomingState != state) return;

        PerformCodeExchange(code, codeVerifier, redirectUri);
    }

    private async void PerformCodeExchange(string code, string codeVerifier, string redirectUri)
    {
        const string tokenRequestUri = "https://www.googleapis.com/oauth2/v4/token";
        var tokenRequestBody = string.Format(
            "code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope=email&grant_type=authorization_code",
            code,
            Uri.EscapeDataString(redirectUri),
            clientId,
            codeVerifier,
            clientSecret
        );
        try
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenRequestUri)
            {
                Content = new StringContent(tokenRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(tokenRequestUri),
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
            var client = new HttpClient();
            var tokenResponse = await client.SendAsync(tokenRequest);

            if (!tokenResponse.IsSuccessStatusCode) return;

            var responseText = await tokenResponse.Content.ReadAsStringAsync();
            var accessToken = responseText.Split(':')[1].Substring(2, 210);
            UserinfoCall(accessToken);
        }
        catch
        {
            //
        }
    }


    private async void UserinfoCall(string accessToken)
    {
        const string userinfoRequestUri = "https://www.googleapis.com/oauth2/v3/userinfo";
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, userinfoRequestUri);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode) return;
        var responseText = await response.Content.ReadAsStringAsync();
        UserinfoResponseText = responseText;
    }
}