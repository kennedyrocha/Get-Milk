using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GetMilk.Database
{
    public static class Constants
    {
        public const string DatabaseFilename = "GetMilk.db3";

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
