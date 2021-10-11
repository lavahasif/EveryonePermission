using System;
using RestSharp;

namespace EveryonePermission
{
    public class whatsapp
    {
        public whatsapp()
        {
        }

        public String byTest()
        {
            try
            {
                var client =
                    new RestClient(
                        "https://panel.rapiwha.com/send_message.php?apikey=EKTH6T5CQHDN14BSIT60&number=919747200785&text=poratta");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public String byurl(String url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public String bymannual(String tomob, string tomessage, String apikey)
        {
            try
            {
                var client =
                    new RestClient(
                        "https://panel.rapiwha.com/send_message.php?apikey=" + apikey + "&number=" + tomob + "&text=" +
                        tomessage);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}