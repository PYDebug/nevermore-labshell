using Labshell.JsonForm;
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

        public RecordResult GetRecord(int classId, int experimentId, int studentId, int labId, int machineId, String token)
        {
            RestClient client = new RestClient(ServerURL.URL);
            RestRequest request = new RestRequest("/experiment/{id}/userRecord", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("x-auth-token", token);
            request.AddUrlSegment("id", experimentId + "");
            request.AddBody(new UserRecord 
            { 
                classId = classId,
                experimentId = experimentId,
                studentId = studentId,
                labId = labId,
                machineId = machineId,
                slotId = 1
            });
            var response = client.Execute<RecordResult>(request);
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
