using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace GetMilk.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.SetFlags(new string[] { "SwipeView_Experimental" });
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            Android.App.Job.JobInfo.Builder jobBuilder = this.CreateJobBuilderUsingJobId<IntegrationService>(1).SetMinimumLatency(10000);
            Android.App.Job.JobInfo jobInfo = jobBuilder.Build();

            _jobScheduler = (Android.App.Job.JobScheduler)GetSystemService(JobSchedulerService);

            var result = _jobScheduler.Schedule(jobInfo);

            if (result != Android.App.Job.JobScheduler.ResultSuccess)
            {
                Console.WriteLine("Success!");
            }

            LoadApplication(new App());
        }

        Android.App.Job.JobScheduler _jobScheduler;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}