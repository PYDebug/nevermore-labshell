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
using Labshell.JsonForm;

namespace Labshell.Factory
{
    class MachineFactory
    {
        public MachineResult AddMachine(String mac, int labId, String launchPath, List<String> listenPath)
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/manage/machine", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("x-auth-token", CacheService.GetAdminToken());
            request.AddBody(new Machine {
                macAddress = mac,
                labId = labId,
                launchPath = launchPath,
                listenPath = listenPath
            });
            var response = client.Execute<MachineResult>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                return null;
            }
        }

        public MachineResult UpdateMachine(int id, String mac, int labId, String launchPath, List<String> listenPath)
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/manage/machine/{id}", Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("x-auth-token", CacheService.GetAdminToken());
            request.AddUrlSegment("id", id+"");
            request.AddBody(new Machine
            {
                macAddress = mac,
                labId = labId,
                launchPath = launchPath,
                listenPath = listenPath
            });
            var response = client.Execute<MachineResult>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                return null;
            }
        }

        public MachineResult GetMachine(String mac)
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/manage/machine", Method.GET);
            request.AddQueryParameter("macAddress", mac);
            var response = client.Execute<MachineResult>(request);
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
