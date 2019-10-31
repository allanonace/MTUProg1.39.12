using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Library;
using Xml;
using System.Linq;

using ActionType = MTUComm.Action.ActionType;

namespace MTUComm
{
    public class Logger
    {
        #region Constants

        public enum BasicFileType
        {
            READ,
            NODE_DISCOVERY,
            DATA_READ,
            REMOTE_DISCONNECT
        }

        #endregion

        #region Attributes

        private string fixed_name = String.Empty;

        #endregion

        #region Properties



        #endregion

        #region Initialization

        public Logger ( string outFileName = "" )
        {
            fixed_name = outFileName;
        }

        #endregion

        #region Logic

        private Boolean IsFixedName ()
        {
            return ! string.IsNullOrEmpty ( fixed_name.Trim () );
        }

        public void ResetFixedName ()
        {
            fixed_name = string.Empty;
        }

        #endregion

        #region Structure

        public string CreateBasicStructure (
            BasicFileType basicFileType = BasicFileType.READ )
        {
            Configuration config = Singleton.Get.Configuration;
        
            string base_stream = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            switch ( basicFileType )
            {
                case BasicFileType.READ:
                base_stream += "<StarSystem>";
                base_stream += "    <AppInfo>";
                base_stream += "        <AppName>"   + config.getApplicationName() + "</AppName>";
                base_stream += "        <Version>"   + config.GetApplicationVersion() + "</Version>";
                base_stream += "        <Date>"      + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") + "</Date>";
                base_stream += "        <UTCOffset>" + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
                base_stream += "        <UnitId>"    + config.GetDeviceUUID() + "</UnitId>";
                base_stream += "        <AppType>"   + ( Data.Get.IsFromScripting ? "Scripted" : "Interactive" ) + "</AppType>";
                base_stream += "    </AppInfo>";
                base_stream += "    <Message />";
                base_stream += "    <Mtus />";
                base_stream += "    <Warning />";
                base_stream += "    <Error />";
                base_stream += "</StarSystem>";
                break;

                case BasicFileType.NODE_DISCOVERY:
                base_stream += "<NodeDiscoveryReports>";
                base_stream += "    <AppName>"   + config.getApplicationName() + "</AppName>";
                base_stream += "    <Version>"   + config.GetApplicationVersion() + "</Version>";
                base_stream += "    <MtuId />";
                base_stream += "    <Date>"      + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") + "</Date>";
                base_stream += "    <UTCOffset>" + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
                base_stream += "    <UnitId>"    + config.GetDeviceUUID() + "</UnitId>";
                base_stream += "    <AppType>"   + ( Data.Get.IsFromScripting ? "Scripted" : "Interactive" ) + "</AppType>";
                base_stream += "</NodeDiscoveryReports>";
                break;

                case BasicFileType.DATA_READ:
                base_stream += "<Log>";
                base_stream += "    <Transfer>";
                base_stream += "        <AppName>"   + config.getApplicationName() + "</AppName>";
                base_stream += "        <Version>"   + config.GetApplicationVersion() + "</Version>";
                base_stream += "        <MtuId />";
                base_stream += "        <Date>"      + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") + "</Date>";
                base_stream += "        <UTCOffset>" + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
                base_stream += "        <UnitId>"    + config.GetDeviceUUID() + "</UnitId>";
                base_stream += "        <AppType>"   + ( Data.Get.IsFromScripting ? "Scripted" : "Interactive" ) + "</AppType>";
                base_stream += "        <Events />";
                base_stream += "    </Transfer>";
                base_stream += "</Log>";
                break;
            }            
                     
            return base_stream;
        }
        
        private string CreateLogBase_Scripting_Error ()
        {
            Configuration config = Singleton.Get.Configuration;
        
            string base_stream = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            base_stream += "<StarSystem>";
            base_stream += "    <AppInfo>";
            base_stream += "        <AppName>" + config.getApplicationName() + "</AppName>";
            base_stream += "        <Version>" + config.GetApplicationVersion() + "</Version>";
            base_stream += "        <Date>" + DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "</Date>";
            base_stream += "        <UTCOffset>" + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
            base_stream += "        <UnitId>" + config.GetDeviceUUID() + "</UnitId>";
            base_stream += "        <AppType>Scripted</AppType>";
            base_stream += "    </AppInfo>";
            
            Action currentAction;
            if ( ( currentAction = Singleton.Get.Action ) != null )
            {
                base_stream += "    <Action display=\"" + Action.LogDisplays[ currentAction.Type ] + "\" type=\"" + Action.LogTypes[ currentAction.Type ] + "\" reason=\"" + Action.LogReasons[ currentAction.Type ] + "\">";
                base_stream += "        <Date display=\"Date/Time\">" + DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "</Date>";
                base_stream += "        <User display=\"User\">" + currentAction.User + "</User>";
                base_stream += "    </Action>";
            }
            
            base_stream += "</StarSystem>";
                        
            string uri = Path.Combine ( Mobile.ConfigPath, "___tmp.xml" );
            Mobile.CreateDirectoryIfNotExist(Mobile.ConfigPath);

            using ( System.IO.StreamWriter file = new System.IO.StreamWriter ( uri, false ) )
            {
                file.WriteLine ( base_stream );
            }
            
            return uri;
        }

        #endregion

        #region File

        public string CreateFileIfNotExist (
            BasicFileType basicFileType = BasicFileType.READ,
            bool   append     = true,
            string customPath = "" )
        {
            string uri = customPath;
        
            if ( string.IsNullOrEmpty ( customPath ) )
            {
                string file_name = ( IsFixedName () ) ? fixed_name : DateTime.Now.ToString("MMddyyyyHH")+"Log.xml";
                String filename_clean = Path.GetFileName(file_name);
                String rel_path = file_name.Replace(filename_clean, "");
    
                if ( rel_path.Length > 1 && rel_path.StartsWith ( "/" ) )
                    rel_path = rel_path.Substring ( 1 );
    
                string full_new_path = Path.Combine (String.IsNullOrEmpty(Mobile.LogUserPath)?Mobile.LogPath:Mobile.LogUserPath, rel_path );

                Mobile.CreateDirectoryIfNotExist(Mobile.LogPath);

                Mobile.CreateDirectoryIfNotExist(full_new_path);

                uri = Path.Combine ( full_new_path, filename_clean );
            }

            if ( ! File.Exists ( uri ) )
            { 
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, append ))
                {
                    file.WriteLine ( CreateBasicStructure ( basicFileType ) );
                }
            }
            else if ( ! append )
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, false ))
                {
                    file.WriteLine ( CreateBasicStructure ( basicFileType ) );
                }
            }
            else
            {
                try
                {
                    XDocument.Load(uri);
                }
                catch ( Exception )
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, false ))
                    {
                        file.WriteLine ( CreateBasicStructure ( basicFileType ) );
                    }
                }
            }

            return uri;
        }
        
        public string CreateEventFileIfNotExist (
            string mtu_id )
        {
            string file_name = "MTUID"+ mtu_id+ "-" + System.DateTime.Now.ToString("MMddyyyyHH") + "-" + DateTime.Now.Ticks.ToString() + "DataLog.xml"; 
            string uri = Path.Combine ( Mobile.LogUserPath, file_name );

            if (!File.Exists(uri))
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, true))
                {
                    string base_stream = "<?xml version=\"1.0\" encoding=\"ASCII\"?>";
                    base_stream += "<Log>";
                    base_stream += "    <Transfer/>";
                    base_stream += "</Log>";
                
                    file.WriteLine ( base_stream );
                }

            return uri;
        }

        #endregion
        
        #region Actions

        #region Login, Error and Cancel

        public void Login ( String username )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);

            XElement element = new XElement("AppMessage");

            AddParameter(element, new Parameter("Date", null, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));

            XElement message = new XElement("Message", "User Login: "+ username);
            element.Add(message);

            doc.Root.Element("Message").Add(element);
            doc.Save(uri);
        }

        /// <summary>
        /// Write errors in log file, using Errors singleton class
        /// that contains all catched errors since the last writting
        /// <Error>
        ///   <AppError>
        ///     <Date></Date>
        ///     <Message ErrorId="n">...</Message>
        ///   </AppError>
        /// </Error>
        /// </summary>
        public string Error ()
        {
            String    uri     = CreateFileIfNotExist ();
            XDocument doc     = XDocument.Load ( uri );
            XElement  element = doc.Root.Element ( "Error" );
            string    time    = DateTime.Now.ToString ( "MM/dd/yyyy HH:mm:ss" );

            // The log send to the explorer should be only the last error performed, not full current activity log
            string    uriLog  = this.CreateLogBase_Scripting_Error ();
            XDocument docLog  = XDocument.Load ( uriLog );

            XElement error;
            XElement message;
            foreach ( Error e in Errors.GetErrorsToLog () )
            {
                error = new XElement ( "AppError" );

                AddParameter ( error, new Parameter ( "Date", null, time ) );
            
                message = new XElement ( "Message", e.Message );
                
                if ( Errors.ShowId &&
                     e.Id > -1 )
                    AddAtrribute ( message, "ErrorId", e.Id.ToString () );
                    
                if ( e.Port > 1 )
                    AddAtrribute ( message, "Port", e.Port.ToString () );
                
                error.Add ( message );
                
                element.Add ( error );     // Activity log
                docLog.Root.Add ( error ); // Result ( send to explorer )
            }
            
            doc.Save ( uri );
            
            // Only for the result log
            element = new XElement("AppMessage");
            AddParameter ( element, new Parameter ( "Date", null, time ) );
            message = new XElement ( "Message" );
            element.Add ( message );
            docLog.Root.Add ( element );
            
            string docLogText = docLog.ToString ();
            
            if ( File.Exists ( uriLog ) )
                File.Delete ( uriLog );
            
            return docLogText;
        }

        public void Cancel ( Action action, String cancel, String reason )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);

            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.LogDisplay);
            AddAtrribute(element, "type", action.LogType);
            AddAtrribute(element, "reason", action.LogReason);

            AddParameter(element, new Parameter("Date", "Date/Time", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
            AddParameter(element, new Parameter("User", "User", action.User));

            AddParameter(element, new Parameter("Cancel", "Cancel Action", cancel));
            AddParameter(element, new Parameter("Reason", "Cancel Reason", reason));

            doc.Root.Element("Mtus").Add(element);
            doc.Save(uri);
        }

        #endregion

        #region ReadMTU

        public string ReadMTU (
            Action action,
            ActionResult allParamsFromInterface,
            Mtu mtu,
            bool isSubAction = false )
        {
            // Create or write in the result file in scripted mode or
            // create or write in the activity log in interactive mode
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);
            ReadMTU_Logic(doc.Root.Element("Mtus"), action.Type, allParamsFromInterface, mtu, isSubAction );
            doc.Save(uri);

            // Launching multiple times scripts with the same output path, concatenates the actions logs,
            // but the log send to the explorer should be only the last action performed
            XDocument uniDoc;
            using ( Stream BasicStruct = new MemoryStream ( Encoding.UTF8.GetBytes ( CreateBasicStructure () ) ) )
            {
                uniDoc = XDocument.Load(BasicStruct);
                ReadMTU_Logic(uniDoc.Root.Element("Mtus"), action.Type, allParamsFromInterface, mtu, isSubAction );
            }
            
            #if DEBUG
            
            // Single file for debugging purposes
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                mtu.Id + "-" + action.Type + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.CreateFileIfNotExist ( BasicFileType.READ, false, uniUri );
            uniDoc.Save ( uniUri );
            
            #endif
            
            // Create or write in the ActivityLog in scripted mode
            if ( Data.Get.IsFromScripting &&
                 ! Singleton.Get.Configuration.Global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.ResetFixedName ();
                
                uri = CreateFileIfNotExist ();
                doc = XDocument.Load( uri );
                ReadMTU_Logic(doc.Root.Element("Mtus"), action.Type, allParamsFromInterface, mtu, isSubAction );
                doc.Save(uri);
            }

            return uniDoc.ToString ();
        }

        private void ReadMTU_Logic (
            XElement parent,
            ActionType actionType,
            ActionResult allParamsFromInterface,
            Mtu mtu,
            bool isSubAction )
        {
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", Action.LogDisplays[ actionType ] );
            AddAtrribute(element, "type", Action.LogTypes[ actionType ] );

            if ( ! isSubAction )
                AddAtrribute(element, "reason", Action.LogReasons[ actionType ] );

            InterfaceParameters[] parameters = Singleton.Get.Configuration.getLogParamsFromInterface ( mtu, ActionType.ReadMtu );
            foreach ( InterfaceParameters parameter in parameters )
            {
                try
                {
                    if ( parameter.Name == "Port" )
                    {
                        ActionResult[] ports = allParamsFromInterface.getPorts();
                        for ( int i = 0; i < ports.Length; i++ )
                            if (ports[i].getParameters().Length > 0) // Does not write an empty port ( <Port ... /> )
                                Port(i, element, ports[i], parameter.Parameters.ToArray());
                    }
                    else
                        this.ComplexParameter(element, allParamsFromInterface, parameter);
                }
                catch (Exception e) { Console.WriteLine($"logger.cs_ {e.Message}"); }
            }
            
            // Add additional parameters
            if ( actionType == ActionType.ReadMtu ||
                 actionType == ActionType.MtuInstallationConfirmation )
                this.AddAdditionalParameters ( element, allParamsFromInterface );
            
            parent.Add ( element );
        }

        #endregion

        #region Data Read

        public string DataRead (
            ActionResult dataRead_allParamsFromInterface,
            ActionResult readMtu_allParamsFromInterface,
            EventLogList eventList,
            Mtu mtu )
        {
            #region Events Log

            string eventsUri = Data.Get.ProcessResultFileFullDR;
            using ( Stream BasicStruct = new MemoryStream (
                Encoding.UTF8.GetBytes ( CreateBasicStructure ( BasicFileType.DATA_READ ) ) ) )
            {
                XDocument eventsDoc = XDocument.Load ( BasicStruct );
                XElement  events    = eventsDoc.Root.Element ( "Transfer" ).Element ( "Events" );

                // Events attributes
                AddAtrribute ( events, "FilterMode",  eventList.FilterMode.ToString () );
                AddAtrribute ( events, "FilterValue", eventList.EntryType .ToString () );
                AddAtrribute ( events, "RangeStart",  eventList.DateStart .ToString ( "yyyy-MM-dd HH:mm:ss" ) );
                AddAtrribute ( events, "RangeStop",   eventList.DateEnd   .ToString ( "yyyy-MM-dd HH:mm:ss" ) );

                // Mtu ID value
                eventsDoc.Root.Element ( "Transfer" ).Element ( "MtuId" ).Value = Data.Get.MtuId;

                // Add events to the log
                DataRead_Events ( events, eventList );

                this.CreateFileIfNotExist ( BasicFileType.DATA_READ, true, eventsUri );
                eventsDoc.Save ( eventsUri );
            }

            #endregion

            return this.CompoundActivityLog (
                ActionType.DataRead,
                dataRead_allParamsFromInterface,
                readMtu_allParamsFromInterface,
                mtu );
        }

        private void DataRead_Events (
            XElement eventsParent,
            EventLogList eventList )
        {
            foreach ( EventLog log in eventList.Entries )
            {
                XElement evnt = new XElement ( "MeterReadEvent" );
                evnt.Add ( new XAttribute ( "FormatVersion",   log.FormatVersion ) );
                evnt.Add ( new XElement   ( "TimeStamp",       log.TimeStamp.ToString ( "yyyy-MM-dd HH:mm:ss" ) ) );
                evnt.Add ( new XElement   ( "MeterRead",       log.MeterRead ) );
                evnt.Add ( new XElement   ( "ErrorStatus",     log.ErrorStatus ) );
                evnt.Add ( new XElement   ( "ReadInterval",    "PT" + log.ReadInterval + "M" ) );
                evnt.Add ( new XElement   ( "PortNumber",      "PORT" + log.PortNumber ) );
                evnt.Add ( new XElement   ( "IsDailyRead",     Utils.FirstCharToCapital ( log.IsDailyRead.ToString () ) ) );
                evnt.Add ( new XElement   ( "IsTopOfHourRead", Utils.FirstCharToCapital ( log.IsTopOfHourRead ) ) );
                evnt.Add ( new XElement   ( "ReadReason",      log.ReasonForRead.ToString () ) );
                evnt.Add ( new XElement   ( "IsSynchronized",  Utils.FirstCharToCapital ( log.IsSynchronized ) ) );
 
                eventsParent.Add ( evnt );
            }
        }

        #endregion

        #region Node Discovery

        public void NodeDiscovery (
            NodeDiscoveryList nodeList,
            Mtu mtu )
        {
            string eventsUri = Data.Get.ProcessResultFileFullND;
            using ( Stream BasicStruct = new MemoryStream (
                Encoding.UTF8.GetBytes(CreateBasicStructure ( BasicFileType.NODE_DISCOVERY ) ) ) )
            {
                XDocument eventsDoc = XDocument.Load ( BasicStruct );
                XElement  nodes     = eventsDoc.Root;

                // Mtu ID value
                eventsDoc.Root.Element ( "MtuId" ).Value = Data.Get.MtuId;

                // Add events to the log
                NodeDiscovery_Nodes ( nodes, nodeList );

                this.CreateFileIfNotExist ( BasicFileType.NODE_DISCOVERY, true, eventsUri );
                eventsDoc.Save ( eventsUri );
            }
        }

        private void NodeDiscovery_Nodes (
            XElement nodesParent,
            NodeDiscoveryList nodeList )
        {
            bool first;
            foreach ( List<NodeDiscovery> listNodes in nodeList.AllAttempts )
            {
                first = true;
                foreach ( NodeDiscovery node in listNodes )
                {
                    XElement evnt;

                    if ( first )
                    {
                        first = false;

                        evnt = new XElement ( "NodeDiscoveryReportGeneral" );
                        evnt.Add ( new XElement ( "FrequencyChannelOfResponse",  node.FreqChannelResponse ) );
                        evnt.Add ( new XElement ( "RequestorNoiseFloorEstimate", node.NoiseFloorRequest   ) );
                    }
                    else
                    {
                        evnt = new XElement ( "NodeDiscoveryReportNode" );
                        evnt.Add ( new XElement ( "NodeID",                      node.NodeId ) );
                        evnt.Add ( new XElement ( "NodeType",                    node.NodeType ) );
                        evnt.Add ( new XElement ( "RSSIOfRequest",               node.RSSIRequest ) );
                        evnt.Add ( new XElement ( "FreqencyErrorOfRequest",      node.FreqErrorRequest ) );
                        evnt.Add ( new XElement ( "TimeDeltaRequest",            node.TimeDeltaRequest ) );
                        evnt.Add ( new XElement ( "RSSIOfResponse",              node.RSSIResponse ) );
                        evnt.Add ( new XElement ( "FreqencyErrorOfResponse",     node.FreqErrorResponse ) );
                        evnt.Add ( new XElement ( "TimeDeltaResponse",           node.TimeDeltaResponse ) );
                        evnt.Add ( new XElement ( "ResponderReceiverID",         node.ResponderId ) );
                        evnt.Add ( new XElement ( "FrequencyChannelOfRequest",   node.FreqChannelRequest ) );
                        evnt.Add ( new XElement ( "ResponderNoiseFloorEstimate", node.NoiseFloorResponse ) );
                    }
    
                    nodesParent.Add ( evnt );
                }
            }
        }

        #endregion

        #region Remote Disconnect

        public string RemoteDisconnect (
            ActionResult rdd_allParamsFromInterface,
            ActionResult readMtu_allParamsFromInterface,
            Mtu mtu )
        {
            return this.CompoundActivityLog (
                ActionType.ValveOperation,
                rdd_allParamsFromInterface,
                readMtu_allParamsFromInterface,
                mtu );
        }

        #endregion

        #region TurnOnOff

        public string TurnOnOff ( Action action, Mtu mtu, ActionResult resultBasic )
        {
            String uri = CreateFileIfNotExist ();
            XDocument doc = XDocument.Load ( uri );
            PrepareLog_TurnOnOff(doc.Root.Element("Mtus"), action, resultBasic );
            doc.Save(uri);

            // Launching multiple times scripts with the same output path, concatenates the actions logs,
            // but the log send to the explorer should be only the last action performed
            byte[]    byteArray   = Encoding.UTF8.GetBytes(CreateBasicStructure());
            Stream    BasicStruct = new MemoryStream(byteArray);
            XDocument uniDoc      = XDocument.Load(BasicStruct);
            PrepareLog_TurnOnOff(uniDoc.Root.Element("Mtus"), action, resultBasic );
            
            #if DEBUG
            
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                mtu.Id + "-" + action.Type + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.CreateFileIfNotExist ( BasicFileType.READ, false, uniUri );
             
            uniDoc.Save ( uniUri );
            
            #endif
            
            // Write in ActivityLog
            if ( Data.Get.IsFromScripting &&
                 ! Singleton.Get.Configuration.Global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.ResetFixedName ();
                
                uri = CreateFileIfNotExist ();
                doc = XDocument.Load ( uri );
                PrepareLog_TurnOnOff(doc.Root.Element("Mtus"), action, resultBasic );
                doc.Save(uri);
            }
            
            return uniDoc.ToString ();
        }

        private void PrepareLog_TurnOnOff ( XElement parent, Action action, ActionResult resultBasic )
        {
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.LogDisplay );
            AddAtrribute(element, "type", action.LogType );

            AddParameter(element, new Parameter("Date", "Date/Time", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( action.User != null )
                AddParameter(element, new Parameter("User", "User", action.User ));

            AddParameter ( element, resultBasic.getParameters()[ 2 ] ); // MtuType ( e.g. 138 )
            AddParameter ( element, resultBasic.getParameters()[ 3 ] ); // MtuID / Serial number

            // Add additional parameters
            if ( action.Type == ActionType.TurnOnMtu ||
                 action.Type == ActionType.TurnOffMtu )
                this.AddAdditionalParameters ( element, resultBasic );

            parent.Add(element);
        }

        #endregion

        #endregion

        #region Logic

        private string CompoundActivityLog (
            ActionType actionType,
            ActionResult mainAction_allParamsFromInterface,
            ActionResult readMtu_allParamsFromInterface,
            Mtu mtu )
        {
            // Create or write in the result file in scripted mode or
            // create or write in the activity log in interactive mode
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);
            CompoundActivityLog_Logic (
                actionType,
                doc.Root.Element("Mtus"),
                mainAction_allParamsFromInterface,
                readMtu_allParamsFromInterface,
                mtu );
            doc.Save(uri);

            // Launching multiple times scripts with the same output path, concatenates the actions logs,
            // but the log send to the explorer should be only the last action performed
            XDocument uniDoc;
            using ( Stream BasicStruct = new MemoryStream (
                    Encoding.UTF8.GetBytes ( CreateBasicStructure () ) ) )
            {
                uniDoc = XDocument.Load(BasicStruct);
                CompoundActivityLog_Logic (
                    actionType,
                    uniDoc.Root.Element("Mtus"),
                    mainAction_allParamsFromInterface,
                    readMtu_allParamsFromInterface,
                    mtu );
            }
            
            #if DEBUG
            
            // Single file for debugging purposes
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                mtu.Id + "-" + actionType + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" )
                + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.CreateFileIfNotExist ( BasicFileType.DATA_READ, false, uniUri );
            uniDoc.Save ( uniUri );
            
            #endif
            
            // Create or write in the ActivityLog in scripted mode
            if ( Data.Get.IsFromScripting &&
                 ! Singleton.Get.Configuration.Global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.ResetFixedName ();
                
                uri = CreateFileIfNotExist ();
                doc = XDocument.Load( uri );
                CompoundActivityLog_Logic (
                    actionType,
                    doc.Root.Element("Mtus"),
                    mainAction_allParamsFromInterface,
                    readMtu_allParamsFromInterface,
                    mtu );
                doc.Save(uri);
            }

            return uniDoc.ToString ();
        }

        private void CompoundActivityLog_Logic (
            ActionType actionType,
            XElement parent,
            ActionResult mainAction_allParamsFromInterface,
            ActionResult readMtu_allParamsFromInterface,
            Mtu mtu )
        {
            XElement element = new XElement ( "Action" );

            // DataRead log
            //ActionType actionType = ActionType.DataRead;
            AddAtrribute(element, "display", Action.LogDisplays[ actionType ] );
            AddAtrribute(element, "type",    Action.LogTypes   [ actionType ] );
            AddAtrribute(element, "reason",  Action.LogReasons [ actionType ] );

            InterfaceParameters[] parameters = Singleton.Get.Configuration.getLogParamsFromInterface ( mtu, actionType );
            foreach (InterfaceParameters parameter in parameters)
            {
                if (parameter.Name == "Port")
                {
                    ActionResult[] ports = mainAction_allParamsFromInterface.getPorts();
                    for (int i = 0; i < ports.Length; i++)
                        if ( ports[ i ].getParameters ().Length > 0 ) // Does not write an empty port ( <Port ... /> )
                            Port(i, element, ports[i], parameter.Parameters.ToArray());
                }
                else this.ComplexParameter(element, mainAction_allParamsFromInterface, parameter);
            }

            // Add additional parameters, not present in the interface
            foreach ( Parameter parameter in mainAction_allParamsFromInterface.getParameters () )
                if ( parameter.Optional )
                    AddParameter ( element, parameter );

            // ReadMtu log as subaction
            ReadMTU_Logic ( element, ActionType.ReadMtu, readMtu_allParamsFromInterface, mtu, true );

            parent.Add ( element );
        }

        public void Port ( int portnumber, XElement parent, ActionResult allParamsFromInterface, InterfaceParameters[] parameters )
        {
            XElement element = new XElement("Port");

            AddAtrribute(element, "display", "Port "+ (portnumber+1).ToString());
            AddAtrribute(element, "number", (portnumber + 1).ToString());

            foreach (InterfaceParameters parameter in parameters)
                this.ComplexParameter(element, allParamsFromInterface, parameter, portnumber );

            parent.Add(element);
        }

        public void AddParameter ( XElement parent, Parameter parameter )
        {
            XElement xml_parameter = new XElement ( parameter.getLogTag(), parameter.Value );
            AddAtrribute(xml_parameter, "display", parameter.getLogDisplay());
            
            //if ( parameter.Optional )
            //    AddAtrribute(xml_parameter, "option", "1");
            
            parent.Add ( xml_parameter );
        }

        public void ComplexParameter ( XElement parent, ActionResult allParamsFromInterface, InterfaceParameters parameter, int portNumber = 0 )
        {
            Parameter param = allParamsFromInterface.getParameterByTag ( parameter.Name, parameter.Source, portNumber );
            
            string source = string.IsNullOrEmpty ( parameter.Source ) ? string.Empty : " , Source: " + parameter.Source;

            Utils.PrintDeep ( "Add Param to Log: " + parameter.Name + source + " , Port: " + portNumber +
                " = " + ( ( param == null ) ? "NOT USED" : param.Value ) );
            
            if (param != null)
                AddParameter ( parent, param );
        }

        public void AddAtrribute ( XElement node, String attname, String att_value )
        {
            if ( att_value != null && att_value.Length > 0 )
                node.Add(new XAttribute(attname, att_value));
        }

        private void AddAdditionalParameters ( XElement xmlAction, ActionResult result )
        {
            // Add additional parameters to the log
            result.getParameters ()
                .Where ( param => param.Optional )
                .ToList ()
                .ForEach ( param => AddParameter ( xmlAction, param ) );
        }

        #endregion
   }
}
