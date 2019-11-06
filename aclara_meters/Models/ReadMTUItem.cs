using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aclara_meters.Models
{
    public class ReadMTUItem
    {

        public ReadMTUItem(){
            Title = "-";
            Description = "-";
            isMeter = "false";
            isMTU = "true";
            isDetailMeter = "false";

            Title1 = "-";
            Title2 = "-";
            Title3 = "-";

            Description1 = "-";
            Description2 = "-";
            Description3 = "-";

            isDisplayed = "true";
            Height = "60";
            FontColor = "#7A868C";
            BackgroundColor = "#FFFFFF";
        }

        public string Height
        { get; set; }

        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }

        public string Title
        { get; set; }

        public string Description
        { get; set; }

        public string isMeter
        { get; set; }

        public string isDisplayed
        { get; set; }

        public string isMTU
        { get; set; }


        public string isDetailMeter
        { get; set; }

        public string Title1
        { get; set; }

        public string Description1
        { get; set; }

        public string Title2
        { get; set; }

        public string Description2
        { get; set; }

        public string Title3
        { get; set; }

        public string Description3
        { get; set; }


   
    }
}

