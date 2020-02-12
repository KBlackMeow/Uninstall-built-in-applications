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
        public String GetPackageName()
        {
            return this.pckname;
        }

        public String GetName()
        {
            return this.name;
        }
        public void SetName(String name)
        {
            this.name = name;
        }
        public AndroidApp(string pckname, string name)
        {
            this.name = name;
            this.pckname = pckname;
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
