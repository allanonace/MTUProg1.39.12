using MTUComm;
using MTUComm.MemoryMap;
using System;
using System.Linq;
using System.IO;
using Xml;
using Xml.UnitTest;
using Xunit;
using Library;
using System.Threading.Tasks;

using ActionType       = MTUComm.Action.ActionType;
using Action           = MTUComm.Action;
using ActionFinishArgs = MTUComm.Delegates.ActionFinishArgs;
using ProgressArgs     = MTUComm.Delegates.ProgressArgs;

namespace UnitTest.Tests
{
    public class Test_Actions
    {
        #region Constants

        private const string FOLDER = "Aclara_Test_Files";
        private const string ERROR  = "ERROR: ";
        private const string ERROR_VAL_STR = ERROR + "String parameter is not a valid numeric value";

        #endregion

        #region Attributes

        Configuration config;
        Action action;
        dynamic map;
        private TaskCompletionSource<bool> semaphore;
        private string exceptionError;

        #endregion

        #region Test methods

        // XMLs FOLDER:
        // Create "Aclara_Test_Files" on your OS desktop and put inside all configuration xml files
        // and MTU families xmls to test, adding all them ( MTU xmls ) using [InlineData] attributes
        private string GetPath (
            int mtuId = -1,
            ActionType actionType = ActionType.ReadMtu )
        {
            if ( mtuId > -1 )
                return Path.Combine (
                    Environment.GetFolderPath ( Environment.SpecialFolder.Desktop ),
                    FOLDER, "Tests", mtuId + "_" + actionType );
            else
                return Path.Combine (
                    Environment.GetFolderPath ( Environment.SpecialFolder.Desktop ),
                    FOLDER );
        }

        private bool TestExpression ( Func<dynamic> func )
        {
            try
            {
                func.Invoke ();
            }
            catch ( Exception e )
            {
                this.exceptionError = e.Message;
                return false;
            }
            return true;
        }

        private string Error ( string constMessage = "" )
        {
            if ( ! string.IsNullOrEmpty ( constMessage ) )
                return constMessage + " => Message: " + this.exceptionError;
            return ERROR + this.exceptionError;
        }

        #endregion

        #region Tests

        [Theory]
        [InlineData(198,ActionType.ReadMtu,true)]
        [InlineData(198,ActionType.DataRead,true)]
        [InlineData(198,ActionType.RFCheck,true)]
        [InlineData(198,ActionType.ValveOperation,true)]
        [InlineData(198,ActionType.TurnOffMtu,true)]
        [InlineData(198,ActionType.TurnOnMtu,true)]
        [InlineData(198,ActionType.AddMtu,true)]
        public async Task Test_Action (
            int mtuId,
            ActionType actionType,
            bool isIOS )
        {
            string ds = "1111111111";
            string fs = string.Format ( "{0:D2}", ds );

            Console.WriteLine ( "-----> " + fs );

            return;

            try
            {
                // Executes the action
                Assert.True (
                    await this.LaunchAction ( mtuId, actionType, isIOS ),
                    $"The action '{actionType}' has failed" );

                // Log generated result
                this.LogResults ();

                // Compare generate result with result load from the xml
                Assert.True (
                    this.CompareResults (),
                    $"The result validation for the action '{actionType}' has failed" );

                Console.WriteLine ( $"The action '{actionType}' has successfully completed the unit test" );
            }
            catch ( Exception e )
            {
                Console.WriteLine ( "ERROR: " + e.Message );
            }
        }

        private async Task<bool> LaunchAction (
            int mtuId,
            ActionType actionType,
            bool isIOS )
        {
            try
            {
                // Loads the simulation data
                UnitTest_Data testData = new UnitTest_Data ( this.GetPath ( mtuId, actionType ) );
                Data.Set ( "UnitTestData", testData );

                // Loads configuration files
                this.config = Configuration.GetInstance ( this.GetPath () );

                // Prepare data that would be set during app initialization
                Data.Set ( "IsFromScripting", false );
                Data.Set ( "IsIOS",       isIOS );
                Data.Set ( "IsAndroid", ! isIOS );

                this.config.setPlatform   ( ( Data.Get.IsIOS ) ? "iOS" : "Android" );
                this.config.setAppName    ( "Aclara MTU Programmer" );
                this.config.setVersion    ( "UNIT TEST" );
                this.config.setDeviceUUID ( "COMPUTER" );

                // Prepare values to use from Library.Data and Global ( requires Global, that it is inside config )
                testData.PrepareValues ();

                // Replaces alarm, demand and meter ids by the instances
                if ( Data.Contains ( "Meter" ) )
                    Data.SetTemp ( "Meter",
                        config.MeterTypes.FindByMterId ( int.Parse ( Data.Get.Meter ) ) );

                if ( Data.Contains ( "Alarm" ) )
                    Data.SetTemp ( "Alarm",
                        config.Alarms.FindByMtuTypeAndName ( mtuId, Data.Get.Alarm ) );

                if ( Data.Contains ( "Demand" ) )
                    Data.SetTemp ( "Demand",
                        config.Demands.FindByMtuTypeAndName ( mtuId, Data.Get.Demand ) );

                // Generates the memory map for the family of the MTU indicated
                string family = this.config.Interfaces.GetFamilyByMtuId ( mtuId );
                this.map = new MemoryMap ( null, family, false );

                this.map.FillMemory ( testData.MemoryMap );

                // All LExI functions will use preloaded data
                Lexi.Lexi.Map = this.map;

                // Performs the basic reading that all actions need before executing
                await new Action ( null, ActionType.BasicRead ).Run ();

                // Prepares the action to be launched
                this.action = new Action ( null, actionType, "PC" );

                this.action.OnProgress -= this.OnProgress;
                this.action.OnProgress += this.OnProgress;
                this.action.OnFinish   -= this.OnFinish;
                this.action.OnFinish   += this.OnFinish;
                this.action.OnError    -= this.OnError;
                this.action.OnError    += this.OnError;

                this.semaphore = new TaskCompletionSource<bool>();

                await this.action.Run ();
                
                return await semaphore.Task;
            }
            catch ( Exception e )
            {
                return false;
            }
        }

        private bool CompareResults ()
        {
            UnitTest_Results xmlResults = ( ( UnitTest_Data )Data.Get.UnitTestData ).Results;

            // For actions without response, such as turn on/off
            if ( xmlResults.ListValues == null &&
                 this.action.LastResults.Length <= 0 )
                return true;

            int num, xmlNum;
            if ( ( num = xmlResults.ListValues.Length ) != ( xmlNum = this.action.LastResults.Length ) )
            {
                Console.WriteLine (
                    "ERROR: Different number of result lists ->" +
                    " Results.Length = " + num +
                    " Xml.Length = " + xmlNum );

                return false;
            }

            bool resultOk = true;
            ActionResult    result;
            UnitTest_Result xmlResult;
            UnitTest_Output xmlParam;
            for ( int i = 0; i < this.action.LastResults.Length; i++ )
            {
                result    = action.LastResults[ i ];
                xmlResult = xmlResults.GetInterface ( result.ActionType.ToString () );

                // Both ( test and xml results ) must have the same number of parameters
                int resultPorts = result.getPorts ().Sum ( r => r.NumParameters );

                if ( ( ( num = result.getParameters ().Length + resultPorts ) != ( xmlNum = xmlResult.Outputs.Length ) ) )
                {
                    Console.WriteLine (
                        "ERROR: Different number of parameters ->" +
                        " Result.Length = " + num +
                        " Xml.Length = " + xmlNum );

                    return false;
                }

                // All parameters must have the same value
                // Root properties
                foreach ( Parameter param in result.getParameters () )
                {
                    if ( ( xmlParam = xmlResult[ param.CustomParameter ] ) != null )
                    {
                        if ( ! param.Value.ToString ().Equals ( xmlParam.Value.ToString () ) )
                        {
                            Console.WriteLine (
                                "ERROR: " + param.CustomParameter + " ->" +
                                " Result = " + param.Value +
                                " Xml = " + xmlParam.Value );
                            
                            resultOk = false;
                        }
                    }
                }
                // Port properties
                foreach ( ActionResult portResult in result.getPorts () )
                    foreach ( Parameter param in portResult.getParameters () )
                    {
                        if ( ( xmlParam = xmlResult[ param.CustomParameter ] ) != null )
                        {
                            if ( ! param.Value.ToString ().Equals ( xmlParam.Value.ToString () ) ||
                                 param.Port != xmlParam.Port )
                            {
                                Console.WriteLine (
                                    "ERROR: " + param.CustomParameter + " ->" +
                                    " Result = " + param.Value + " , " + param.Port +
                                    " Xml = " + xmlParam.Value + " , " + xmlParam.Port );
                                
                                resultOk = false;
                            }
                        }
                    }
            }

            return resultOk;
        }

        private void LogResults ()
        {
            for ( int i = 0; i < this.action.LastResults.Length; i++ )
            {
                ActionResult result = action.LastResults[ i ];

                Console.WriteLine ( "<Results>" );
                Console.WriteLine ( "\t<Result interface=\"{0}\">", result.ActionType.ToString () );
                
                // Root properties
                foreach ( Parameter param in result.getParameters () )
                    Console.WriteLine (
                        "\t\t<Output id=\"{0}\" value=\"{1}\"/>",
                        param.CustomParameter, param.Value );

                // Port properties
                foreach ( ActionResult portResult in result.getPorts () )
                    foreach ( Parameter param in portResult.getParameters () )
                        Console.WriteLine (
                            "\t\t<Output port=\"{0}\" id=\"{1}\" value=\"{2}\"/>",
                            param.Port, param.CustomParameter, param.Value );

                Console.WriteLine ("\t</Result>");
                Console.WriteLine ( "</Results>" );
            }
        }

        private void OnProgress ( object sender, ProgressArgs e )
        {
            string mensaje = e.Message;

            Console.WriteLine ( "OnProgress: " + mensaje );
        }

        public async Task OnFinish ( object sender, ActionFinishArgs args )
        {
            this.semaphore.SetResult ( true );
        }

        public void OnError ()
        {
            Error error = Errors.LastError;

            Console.WriteLine ( "OnError: " + error.Id + " -> " + error.Message );

            this.semaphore.SetResult ( false );
        }

        #endregion
    }
}
