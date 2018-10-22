using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml;

namespace MTUComm
{
    public class Configuration
    {
        private String mbase_path;
        private MtuTypes mtuTypes;
        private MeterTypes meterTypes;

        public Configuration(String base_path)
        {
            mbase_path = base_path;

            Config config = new Config();
            mtuTypes = config.GetMtu(Path.Combine(mbase_path, "Mtu.xml"));
            meterTypes = config.GetMeters(Path.Combine(mbase_path, "Meter.xml"));

        }


        public String GetBasePath()
        {
            return mbase_path;
        }


        public Mtu[] GetMtuTypes()
        {
            return mtuTypes.Mtus.ToArray();

        }

        public Mtu GetMtuTypeById(int mtuId)
        {
            return mtuTypes.FindByMtuId(mtuId);
        }

        public Meter[] GetMeterType()
        {
            return meterTypes.Meters.ToArray();
        }

        public Meter getMeterTyoeById(int meterId)
        {
            return meterTypes.FindByMterId(meterId);
        }


        public List<string>  GetVendorsFromMeters()
        {
            return meterTypes.GetVendorsFromMeters(meterTypes.Meters);
        }


        public List<string> GetModelsByVendorFromMeters(String vendor)
        {
            return meterTypes.GetModelsByVendorFromMeters(meterTypes.Meters, vendor);
        }


        public Boolean useDummyDigits()
        {
            return true; //should be taen from globals, hardcoded for dev
        }

        public String GetDevideUUID()
        {
            return "ACLARATECH-CLE5478L-KGUILER"; //get UUID from Xamarin
        }

        public String GetApplicationVersion()
        {
            return "2.2.5.0"; //get build version from manifest/pkg manager
        }

        public String getApplicationName()
        {
            return "AclaraStarSystemMobile";
        }

    }
}
