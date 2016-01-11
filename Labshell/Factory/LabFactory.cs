using Labshell.Model;
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
    class LabFactory
    {
        public List<Lab> AllLab()
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/manage/lab/labs", Method.GET);
            request.AddQueryParameter("scope","all");
            request.AddParameter("pageSize", 10000);
            var response = client.Execute<LabResult>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                List<Lab> list = new List<Lab>();
                foreach (LabResult.Lab lr in response.Data.data)
                {
                    Lab lab = new Lab() { Id=lr.id, Name=lr.name, Number=lr.number, MachineNumber = 0, StudentNumber=lr.capacity};
                    list.Add(lab);
                }
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
