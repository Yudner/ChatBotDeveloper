﻿using Chatbot.Constants;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Chatbot.Services.QnAMaker
{
    public class QnAMakerService : IQnAMakerService
    {
        public string GetAnswer(string query)
        {
            var client = new RestClient(QnaMakerConstants.Host + "/knowledgebases/" + QnaMakerConstants.KnowledgeBaseId + "/generateAnswer");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "EndpointKey " + QnaMakerConstants.EndPointKey);
            request.AddParameter(QnaMakerConstants.FormatJson, "{\"question\": \"" + query + "\"}", ParameterType.RequestBody);
            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<QnAMakerModel>(response.Content);

            if (result.Answers.Count > 0)
            {
                var respuesta = result.Answers[0].Answer;
                var score = result.Answers[0].Score;
                if (!respuesta.ToLower().Equals(QnaMakerConstants.AnswerNotFound) && score > 40)
                    return respuesta;
            }
            return QnaMakerConstants.AnswerNotFound;
        }
    }
}