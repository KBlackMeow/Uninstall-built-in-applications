using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUninstall
{
    class AndroidApp:Object
    {
        private String name;
        private String pckname;
        private String suggestion;
        public String GetPackageName()
        {
            return this.pckname;
        }

        public String GetName()
        {
            return this.name;
        }
        public String GetSuggestion()
        {
            return this.suggestion;
        }
        public void SetSuggesion(String suggestion)
        {
            this.suggestion = suggestion;
        }
        public AndroidApp(string pckname, string name ,string suggestion="")
        {
            this.name = name;
            this.pckname = pckname;
            this.suggestion = suggestion;
        }

        public bool Equals(AndroidApp a)
        {
            return this.GetPackageName().Equals(a.GetPackageName());
        }
        public override int GetHashCode()
        {
            return this.GetPackageName().GetHashCode();
        }
    }
}
