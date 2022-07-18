using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GetMilk.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GetMilk.Droid
{
    [Service(Name = "GetMilk.Service.IntegrationService",
         Permission = "android.permission.BIND_JOB_SERVICE")]
    public class IntegrationService : JobService
    {
        public override bool OnStartJob(JobParameters jobParams)
        {
            Task.Run(async () =>
            {
                // Work is happening asynchronously
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    ServicoService ser = new ServicoService();
                    ColetaService col = new ColetaService();
                    await ser.integrarServicos();
                    await col.integrarColetas();
                }
                // Have to tell the JobScheduler the work is done. 
                JobFinished(jobParams, false);
            });

            // Return true because of the asynchronous work
            return true;
        }

        public override bool OnStopJob(JobParameters jobParams)
        {
            Toast.MakeText(this, "Integração realizada", ToastLength.Long).Show();

            return true;
        }
    }
}