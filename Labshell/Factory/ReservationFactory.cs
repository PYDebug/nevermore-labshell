using Labshell.Result;
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
    class ReservationFactory
    {
        public ReservationResult GetValidity(int labId, String token)
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/reservation/validity", Method.GET);
            request.AddHeader("x-auth-token", token);
            request.AddQueryParameter("labId", labId+"");
            var response = client.Execute<ReservationResult>(request);
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
