using Labshell.Result;
using Labshell.Service;
using Labshell.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Factory
{
    class RecordFactory
    {
        public FileResult UploadFile(String file_path, String token)
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/file/upload", Method.POST);
            request.AddHeader("x-auth-token", token);
            request.AddFile("file",file_path);
            var response = client.Execute<FileResult>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                return null;
            }
        }
    }
}
