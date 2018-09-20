using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFilingApiClient
{
    public class ApiClient
    {
        private static string _subscriptionkey;
        private static RestClient _client;

        /// <summary>
        /// Sample API Client for ESD Filing API.
        /// </summary>
        /// <param name="baseUrl">Base URL for api calls.</param>
        /// <param name="subscriptionKey">Subscription key for authorization.</param>
        public ApiClient(string baseUrl, string subscriptionKey)
        {
            if (string.IsNullOrWhiteSpace(baseUrl)) throw new ArgumentException("Parameter `baseUrl` cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(subscriptionKey)) throw new ArgumentException("Parameter `subscriptionKey` cannot be null or empty.");

            _client = new RestClient(baseUrl);
            _subscriptionkey = subscriptionKey;
        }

        /// <summary>
        /// Submits request.
        /// </summary>
        private string SubmitRequest(RestRequest request)
        {
            request.AddHeader("Ocp-Apim-Subscription-Key", _subscriptionkey);
            IRestResponse response = _client.Execute(request);

            return response.Content;
        }

        /// <summary>
        /// FilingStatus_GetTaxAndWageReportStatus.
        /// </summary>
        public string GetTaxAndWageReportStatus(string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId)) throw new ArgumentException("Parameter `trackingId` cannot be null or empty");

            RestRequest request = new RestRequest($"status/tax-and-wage-report/{trackingId}", Method.POST);

            return SubmitRequest(request);
        }

        /// <summary>
        /// TaxAndWageFiling_SubmitAsync
        /// </summary>
        public string Submit(string filingJson)
        {
            if (string.IsNullOrWhiteSpace(filingJson)) throw new ArgumentException("Parameter `filingJson` cannot be null or empty");

            RestRequest request = new RestRequest($"file/single", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", filingJson, ParameterType.RequestBody);
            string result = SubmitRequest(request);

            return result.Substring(1, result.Length - 2);
        }

        /// <summary>
        /// TaxAndWageInfo_GetTaxRate
        /// </summary>
        public string GetTaxRate(string esdNumber, string ubiNumber, string feinNumber, int month, int year)
        {
            if (string.IsNullOrWhiteSpace(esdNumber)) throw new ArgumentException("Parameter `esdNumber` cannot be null or empty");
            if (string.IsNullOrWhiteSpace(ubiNumber)) throw new ArgumentException("Parameter `ubiNumber` cannot be null or empty");
            if (string.IsNullOrWhiteSpace(feinNumber)) throw new ArgumentException("Parameter `feinNumber` cannot be null or empty");
            if (month < 1 || month > 12) throw new ArgumentException("Parameter `month` must be between 1 and 12 inclusive");
            if (year < 0) throw new ArgumentException("Parameter `year` must not be negative");

            RestRequest request = new RestRequest($"info/tax-rate/{esdNumber}/{ubiNumber}/{feinNumber}/{month}/{year}", Method.GET);

            return SubmitRequest(request);
        }
    }
}
