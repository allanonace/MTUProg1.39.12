using System;
using Xamarin.Forms;

namespace aclara_meters
{
    public partial class BorderlessEntry : Entry
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public bool   Flag { get; set; }
        public string PrevValue { get; set; }
        
        public BorderlessEntry ()
        {
            this.PrevValue = string.Empty;
        }
    }
}
