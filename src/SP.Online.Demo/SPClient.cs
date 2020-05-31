using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SP.Online.Demo
{
    public class SPClient
    {

        public string AuthToken { get; set; }
        private readonly SPConfiguration _spConfig;
        private HttpClient _client;

        public SPClient(SPConfiguration sPConfiguration)
        {
            _spConfig = sPConfiguration;
        }

        public bool BeginSesion()
        {

            try
            {

                using (_client = new HttpClient())
                {

                    var formContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>( "grant_type"  , _spConfig.GrantType),
                        new KeyValuePair<string, string>( "resource"  , _spConfig.Resource),
                        new KeyValuePair<string, string>( "client_secret"  , _spConfig.ClientSecret),
                        new KeyValuePair<string, string>( "client_id"  , _spConfig.AuthClientID)
                    });

                    HttpResponseMessage response = _client.PostAsync(new Uri($"{_spConfig.AuthURL}"), formContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        AuthToken = JsonConvert.DeserializeObject<SPAuthResponse>(response.Content.ReadAsStringAsync().Result).access_token;
                    }

                    return true;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<SPFile> GetFilesFromFolder(string folderName)
        {
            try
            {
                using (_client = new HttpClient())
                {

                    string oDataUrl = $"https://{_spConfig.SPUrl}/_api/web/GetFolderByServerRelativeUrl('{_spConfig.LibraryURL}/{folderName}')/Files?$select=Name";

                    HttpRequestMessage httpReqMsg = new HttpRequestMessage(HttpMethod.Get, oDataUrl);

                    httpReqMsg.Headers.Add("authorization", $"Bearer {AuthToken}");
                    httpReqMsg.Headers.Add("Accept", "application/json;odata=light;q=1,application/json;odata=verbose;q=0.5");

                    HttpResponseMessage response = _client.SendAsync(httpReqMsg).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;

                        SPFileResponse files = JsonConvert.DeserializeObject<SPFileResponse>(content);

                        files.d.results.ForEach(f =>
                        {
                            f.Folder = folderName;
                        });

                        return files.d.results;

                    }

                    return null;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SPFile LoadMetadata(SPFile spFile)
        {
            try
            {
                using (_client = new HttpClient())
                {


                    string oDataUrl =
                        string.Format("https://{0}/_api/web/GetFolderByServerRelativeUrl('{1}/{2}')/Files('{3}')/ListItemAllFields?$select=Title",
                        _spConfig.SPUrl, _spConfig.LibraryURL, spFile.Folder, spFile.Name);

                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, oDataUrl);

                    httpRequestMessage.Headers.Add("authorization", "Bearer " + AuthToken);
                    httpRequestMessage.Headers.Add("Accept", "application/json;odata=light;q=1,application/json;odata=verbose;q=0.5");

                    HttpResponseMessage response = _client.SendAsync(httpRequestMessage).Result;

                    if (response.IsSuccessStatusCode)
                    {

                        string content = response.Content.ReadAsStringAsync().Result;

                        SPFileObjectResponse fileResponse = JsonConvert.DeserializeObject<SPFileObjectResponse>(content);

                        spFile.Title = fileResponse.d.Title;

                        return spFile;

                    }

                }

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DownloadFile(string folderName, string fileName)
        {
            try
            {
                using (_client = new HttpClient())
                {

                    string oDataUrl = $"https://{_spConfig.SPUrl}/_api/web/GetFileByServerRelativeUrl('{_spConfig.LibraryURL}/{folderName}/{fileName}')/?$value";


                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, oDataUrl);

                    httpRequestMessage.Headers.Add("authorization", $"Bearer {AuthToken}");

                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);

                      var c =  _client.GetAsync(oDataUrl).Result;

                    MemoryStream response = (MemoryStream)_client.GetAsync(oDataUrl).Result.Content.ReadAsStreamAsync().Result;


                    if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, folderName)))
                        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, folderName));

                    File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, folderName, fileName), response.ToArray());

                    var inputAsString = Convert.ToBase64String(response.ToArray());

                    return null;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EndSession()
        {
            this.AuthToken = null;
        }
    }
}
