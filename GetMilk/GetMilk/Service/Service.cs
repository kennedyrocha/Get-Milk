using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace GetMilk.Service
{
    public abstract class Service
    {
        protected HttpClient _client;
        private string BaseApiUrl = "https://getmilk.bdsoft.com.br/getmilk/API";
        protected string ApiUrlLogin
        {
            get
            {
                return BaseApiUrl + "/controller_login.php";
            }
        }
        protected string ApiGetServices
        {
            get
            {
                return BaseApiUrl + "/controller_get_services.php";
            }
        }
        protected string ApiGetColetas
        {
            get
            {
                return BaseApiUrl + "/controller_get_collects.php";
            }
        }
        protected string ApiGetVeiculos
        {
            get
            {
                return BaseApiUrl + "/controller_get_vehicles.php";
            }
        }
        protected string ApiStartService
        {
            get
            {
                return BaseApiUrl + "/controller_start_service.php";
            }
        }
        protected string ApiEndService
        {
            get
            {
                return BaseApiUrl + "/controller_end_service.php";
            }
        }
        protected string ApiControllerColeta
        {
            get
            {
                return BaseApiUrl + "/controller_collect.php";
            }
        }

        public string Name { get; set; }

        public Service()
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromSeconds(10);
        }
    }
}
