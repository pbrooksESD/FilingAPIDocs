using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleFilingApiClient;

namespace SampleConsoleApp
{
    class SampleConsoleApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("API URL: https://ess-api-management.azure-api.net/file/api/");
            string url = " https://ess-api-management.azure-api.net/file/api/"; // Console.ReadLine();
            Console.Write("Subscription Key: ");
            string subscriptionKey = Console.ReadLine();
            ApiClient client = new ApiClient(url, subscriptionKey);
            bool loop = true;
            while (loop)
            {
                Console.WriteLine();
                Console.WriteLine("Sample Application Menu:");
                Console.WriteLine("1. Check tax rate");
                Console.WriteLine("2. File quarterly tax report");
                Console.WriteLine("3. Check tax report status");
                Console.WriteLine("0. Exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        CheckTaxRate(client);
                        break;
                    case "2":
                        FileQuarterlyReport(client);
                        break;
                    case "3":
                        CheckTaxReportStatus(client);
                        break;
                    case "0":
                        loop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void CheckTaxRate(ApiClient client)
        {
            try
            {
                Console.Write("ESD Number: ");
                string esdNumber = Console.ReadLine();
                Console.Write("UBI Number: ");
                string ubiNumber = Console.ReadLine();
                Console.Write("FEIN Number: ");
                string feinNumber = Console.ReadLine();
                Console.Write("Month (as number): ");
                int month = Convert.ToInt32(Console.ReadLine());
                Console.Write("Year: ");
                int year = Convert.ToInt32(Console.ReadLine());
                string result = client.GetTaxRate(esdNumber, ubiNumber, feinNumber, month, year);
                Console.WriteLine(result);
            }
            catch { }

        }

        private static void FileQuarterlyReport(ApiClient client)
        {
            Console.Write("Quarterly Report: ");
            string previousLine = string.Empty;
            string report = string.Empty;
            bool read = true;
            while (read)
            {
                string currentLine = Console.ReadLine();
                if (currentLine == string.Empty && previousLine == string.Empty)
                    read = false;
                else
                {
                    previousLine = currentLine;
                    report += currentLine + Environment.NewLine;
                }
            }
            string result = client.Submit(report.Substring(0, report.Length - 4));
            Console.WriteLine($"Tracking ID: {result}");
        }

        private static void CheckTaxReportStatus(ApiClient client)
        {
            Console.Write("Tracking ID: ");
            string trackingId = Console.ReadLine();
            string result = client.GetTaxAndWageReportStatus(trackingId);
            Console.WriteLine(result);
        }
    }
}
