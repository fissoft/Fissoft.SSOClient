using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Fissoft.SSOClient.Util
{
    //for http post util
    internal class HttpProcUtil
    {
        public static string Post(Uri uri, string param)
        {
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.ReadWriteTimeout = 10000;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if (request.ServicePoint != null)
            {
                request.ServicePoint.Expect100Continue = false;
            }

            // Append params to post-request
            string paramlist = param;
            request.ContentLength = paramlist.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(paramlist);
                postStream.Write(bytes, 0, bytes.Length);
            }

            // Perform request
            string responseText;
            try
            {
                responseText = GetResponseText(request);
            }
            catch (WebException ex)
            {
                throw new ApplicationException(
                    String.Format(CultureInfo.InvariantCulture, "An error occured accesing page {0}", uri)
                    , ex);
            }
            return responseText;
        }

        private static string GetResponseText(HttpWebRequest request)
        {
            string responseText;
            var response = (HttpWebResponse) request.GetResponse();
            using (Stream respStream = response.GetResponseStream())
            {
                Uri respuri = response.ResponseUri;
                responseText = GetStreamText(respStream);
                HttpStatusCode statusCode = response.StatusCode;
                response.Close();

                // Check if call was successfull
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        break;

                    case HttpStatusCode.Forbidden:
                        throw new ApplicationException(responseText + " " + respuri);

                    case HttpStatusCode.NotFound:
                        throw new ApplicationException(responseText + " " + respuri);

                    default:
                        var numericStatus = (int) statusCode;
                        if ((numericStatus >= 500) && (numericStatus <= 600))
                        {
                            throw new ApplicationException(responseText + " " + respuri);
                        }
                        bool rateLimitExceeded = responseText.Contains("Rate limit exceeded");
                        if (rateLimitExceeded)
                        {
                            throw new ApplicationException(responseText);
                        }
                        throw new ApplicationException(responseText + " " + respuri);
                }
            }
            return responseText;
        }

        private static string GetStreamText(Stream respStream)
        {
            string responseText;
            using (var sr = new StreamReader(respStream))
            {
                responseText = sr.ReadToEnd();
            }
            return responseText;
        }

    }
}