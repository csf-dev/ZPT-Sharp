using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZPT.Models
{
    public class HomeModel
    {
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public long Number { get; set; }

        public Version Version { get; set; }

        public string GetVersion()
        {
            return Version.ToString();
        }
    }
}