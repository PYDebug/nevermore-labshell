using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labshell.Util;
using System.Net;
using Labshell.Result;

namespace Labshell.Factory
{
    class AccountFactory
    {
        public LoginResult Login(String username, String password)
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/api/account/authentication", Method.POST);
            request.AddHeader("X-Username", username);
            request.AddHeader("X-Password", MD5Tool.GetMD5(MD5Tool.GetMD5(MD5Tool.GetMD5(password) + username) + "1234"));
            var response = client.Execute<LoginResult>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (Parameter header in response.Headers)
                {
                    if (header.Name.Equals("X-Auth-Token"))
                    {
                        response.Data.token = header.Value.ToString();
                    }
                }
                return response.Data;
            }
            else
            {
                return null;
            }
        }
    }
}
