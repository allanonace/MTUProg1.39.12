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
        public static string fixed_name = String.Empty;

        public void ResetFixedName ()
        {
            fixed_name = string.Empty;
        }

        public Logger ( string outFileName = "" )
        {
            fixed_name = outFileName;
        }

        private Boolean IsFixedName ()
        {
            return ! string.IsNullOrEmpty ( fixed_name.Trim () );
        }

        public string CreateBasicStructure ()
        {
            Configuration config = Singleton.Get.Configuration;
        
            string base_stream = "<?xml version=\"1.0\" encoding=\"ASCII\"?>";
            base_stream += "<StarSystem>";
            base_stream += "    <AppInfo>";
            base_stream += "        <AppName>" + config.getApplicationName() + "</AppName>";
            base_stream += "        <Version>" + config.GetApplicationVersion() + "</Version>";
            base_stream += "        <Date>" + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm") + "</Date>";
            base_stream += "        <UTCOffset>" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
            base_stream += "        <UnitId>" + config.GetDeviceUUID() + "</UnitId>";
            base_stream += "        <AppType>" + ( Data.Get.IsFromScripting ? "Scripted" : "Interactive" ) + "</AppType>";
            base_stream += "    </AppInfo>";
            base_stream += "    <Message />";
            base_stream += "    <Mtus />";
            base_stream += "    <Warning />";
            base_stream += "    <Error />";
            base_stream += "</StarSystem>";
            
            config = null;
            
            return base_stream;
        }
        
        private string CreateLogBase_Scripting_Error ()
        {
            Configuration config = Singleton.Get.Configuration;
        
            string base_stream = "<?xml version=\"1.0\" encoding=\"ASCII\"?>";
            base_stream += "<StarSystem>";
            base_stream += "    <AppInfo>";
            base_stream += "        <AppName>" + config.getApplicationName() + "</AppName>";
            base_stream += "        <Version>" + config.GetApplicationVersion() + "</Version>";
            base_stream += "        <Date>" + DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "</Date>";
            base_stream += "        <UTCOffset>" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
            base_stream += "        <UnitId>" + config.GetDeviceUUID() + "</UnitId>";
            base_stream += "        <AppType>Scripted</AppType>";
            base_stream += "    </AppInfo>";
            
            Action currentAction;
            if ( ( currentAction = Singleton.Get.Action ) != null )
            {
                base_stream += "    <Action display=\"" + Action.displays[ currentAction.type ] + "\" type=\"" + Action.tag_types[ currentAction.type ] + "\" reason=\"" + Action.tag_reasons[ currentAction.type ] + "\">";
                base_stream += "        <Date display=\"Date/Time\">" + DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "</Date>";
                base_stream += "        <User display=\"User\">" + currentAction.user + "</User>";
                base_stream += "    </Action>";
            }
            
            base_stream += "</StarSystem>";
            
            config = null;
            
            string uri = Path.Combine ( Mobile.ConfigPath, "___tmp.xml" );
            Mobile.CreateIfNotExist(Mobile.ConfigPath);

            using ( System.IO.StreamWriter file = new System.IO.StreamWriter ( uri, false ) )
            {
                file.WriteLine ( base_stream );
            }
            
            return uri;
        }

        public string CreateFileIfNotExist (
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

                Mobile.CreateIfNotExist(Mobile.LogPath);

                Mobile.CreateIfNotExist(full_new_path);

                uri = Path.Combine ( full_new_path, filename_clean );
            }

            if ( ! File.Exists ( uri ) )
            { 
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, append ))
                {
                    file.WriteLine ( CreateBasicStructure (  ) );
                }
            }
            else if ( ! append )
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, false ))
                {
                    file.WriteLine ( CreateBasicStructure () );
                }
            }
            else
            {
                try
                {
                    XDocument.Load(uri);
                }
                catch ( Exception e )
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, false ))
                    {
                        file.WriteLine ( CreateBasicStructure () );
                    }
                }
            }

            return uri;
        }
        
        public string CreateEventFileIfNotExist ( string mtu_id )
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

        public void AddAtrribute ( XElement node, String attname, String att_value )
        {
            if ( att_value != null && att_value.Length > 0 )
                node.Add(new XAttribute(attname, att_value));
        }

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

        private void AddAdditionalParameters ( XElement xmlAction, ActionResult result )
        {
            // Add additional parameters to the log
            result.getParameters ()
                .Where ( param => param.Optional )
                .ToList ()
                .ForEach ( param => AddParameter ( xmlAction, param ) );
        }

        public string ReadMTU ( Action action, ActionResult resultAllInterfaces, Mtu mtu )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);
            PrepareLog_ReadMTU(doc.Root.Element("Mtus"), action, resultAllInterfaces, mtu );
            doc.Save(uri);

            // Launching multiple times scripts with the same output path, concatenates the actions logs,
            // but the log send to the explorer should be only the last action performed
            byte[] byteArray = Encoding.UTF8.GetBytes(CreateBasicStructure());
            Stream BasicStruct = new MemoryStream(byteArray);
            XDocument uniDoc = XDocument.Load(BasicStruct);
            PrepareLog_ReadMTU(uniDoc.Root.Element("Mtus"), action, resultAllInterfaces, mtu);
            
            #if DEBUG
            
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                mtu.Id + "-" + action.type + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.CreateFileIfNotExist ( false, uniUri );
 
            uniDoc.Save ( uniUri );
            
            #endif
            
            // Write in ActivityLog
            if ( Data.Get.IsFromScripting &&
                 ! Singleton.Get.Configuration.Global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.ResetFixedName ();
                
                uri = CreateFileIfNotExist ();
                doc = XDocument.Load( uri );
                PrepareLog_ReadMTU(doc.Root.Element("Mtus"), action, resultAllInterfaces, mtu );
                doc.Save(uri);
            }

            return uniDoc.ToString ();
        }

        private void PrepareLog_ReadMTU ( XElement parent, Action action, ActionResult resultAllInterfaces, Mtu mtu )
        {
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.DisplayText);
            AddAtrribute(element, "type", action.LogText);
            AddAtrribute(element, "reason", action.Reason);

            InterfaceParameters[] parameters = Singleton.Get.Configuration.getLogInterfaceFields( mtu, ActionType.ReadMtu );
            foreach ( InterfaceParameters parameter in parameters )
            {
                try
                {
                    if ( parameter.Name == "Port" )
                    {
                        ActionResult[] ports = resultAllInterfaces.getPorts();
                        for ( int i = 0; i < ports.Length; i++ )
                            Port(i, element, ports[i], parameter.Parameters.ToArray());
                    }
                    else
                        this.ComplexParameter(element, resultAllInterfaces, parameter);
                }
                catch ( Exception ex )
                {
                
                }
            }
            
            // Add additional parameters
            if ( action.type == ActionType.ReadMtu ||
                 action.type == ActionType.MtuInstallationConfirmation )
                this.AddAdditionalParameters ( element, resultAllInterfaces );
            
            parent.Add ( element );
        }

        public void ReadData ( Action action, ActionResult result, Mtu mtu )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);

            PrepareLog_ReadData(doc.Root.Element("Mtus"), action, result, mtu );
            doc.Save(uri);
        }

        private void PrepareLog_ReadData ( XElement parent, Action action, ActionResult resultAllInterfaces, Mtu mtu )
        {
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.DisplayText);
            AddAtrribute(element, "type", action.LogText);
            AddAtrribute(element, "reason", action.Reason);

            InterfaceParameters[] parameters = Singleton.Get.Configuration.getLogInterfaceFields ( mtu, ActionType.ReadData );
            foreach (InterfaceParameters parameter in parameters)
            {
                if (parameter.Name == "Port")
                {
                    ActionResult[] ports = resultAllInterfaces.getPorts();
                    for (int i = 0; i < ports.Length; i++)
                        Port(i, element, ports[i], parameter.Parameters.ToArray());
                }
                else this.ComplexParameter(element, resultAllInterfaces, parameter);
            }

            parent.Add(element);
        }

        public void Port ( int portnumber, XElement parent, ActionResult resultAllInterfaces, InterfaceParameters[] parameters )
        {
            XElement element = new XElement("Port");

            AddAtrribute(element, "display", "Port "+ (portnumber+1).ToString());
            AddAtrribute(element, "number", (portnumber + 1).ToString());

            foreach (InterfaceParameters parameter in parameters)
                this.ComplexParameter(element, resultAllInterfaces, parameter, portnumber );

            parent.Add(element);
        }

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
                mtu.Id + "-" + action.type + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.CreateFileIfNotExist ( false, uniUri );
             
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

            AddAtrribute(element, "display", action.DisplayText );
            AddAtrribute(element, "type", action.LogText );

            AddParameter(element, new Parameter("Date", "Date/Time", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( action.user != null )
                AddParameter(element, new Parameter("User", "User", action.user ));

            AddParameter ( element, resultBasic.getParameters()[ 2 ] ); // MtuType ( e.g. 138 )
            AddParameter ( element, resultBasic.getParameters()[ 3 ] ); // MtuID / Serial number

            // Add additional parameters
            if ( action.type == ActionType.TurnOnMtu ||
                 action.type == ActionType.TurnOffMtu )
                this.AddAdditionalParameters ( element, resultBasic );

            parent.Add(element);
        }

        #endregion

        public void Cancel ( Action action, String cancel, String reason )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);

            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.DisplayText);
            AddAtrribute(element, "type", action.LogText);
            AddAtrribute(element, "reason", action.Reason);

            AddParameter(element, new Parameter("Date", "Date/Time", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
            AddParameter(element, new Parameter("User", "User", action.user));

            AddParameter(element, new Parameter("Cancel", "Cancel Action", cancel));
            AddParameter(element, new Parameter("Reason", "Cancel Reason", reason));

            doc.Root.Element("Mtus").Add(element);
            doc.Save(uri);
        }

        public string ReadDataEntries ( string mtu_id, DateTime start, DateTime end, EventLogList eventLogList )
        {
            String    path     = CreateEventFileIfNotExist(mtu_id);
            XDocument doc      = XDocument.Load(path);
            XElement  transfer = doc.Root.Element("Transfer");
            XElement  events   = new XElement("Events");

            AddAtrribute(events, "FilterMode", "Match");
            AddAtrribute(events, "FilterValue", "MeterRead");
            AddAtrribute(events, "RangeStart", start.ToString("yyyy-MM-dd HH:mm:ss"));
            AddAtrribute(events, "RangeStop", end.ToString("yyyy-MM-dd HH:mm:ss"));

            foreach ( EventLog eventLog in eventLogList.Entries )
                PrepareLog_ReadDataEntries ( events, eventLog );

            transfer.Add(events);
            doc.Save(path);

            return path;
        }
        
        private void PrepareLog_ReadDataEntries ( XElement events, EventLog eventLog )
        {
            XElement read_event = new XElement("MeterReadEvent");

            AddAtrribute(read_event, "FormatVersion", eventLog.FormatVersion.ToString());

            read_event.Add(new XElement("TimeStamp", eventLog.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")));
            read_event.Add(new XElement("MeterRead", eventLog.MeterRead.ToString()));
            read_event.Add(new XElement("ErrorStatus", eventLog.ErrorStatus.ToString()));
            read_event.Add(new XElement("ReadInterval", "PT"+eventLog.ReadInterval.ToString()+"M"));
            read_event.Add(new XElement("PortNumber", "PORT"+(eventLog.PortNumber + 1 ).ToString()));
            read_event.Add(new XElement("IsDailyRead", eventLog.IsDailyRead.ToString()));
            read_event.Add(new XElement("IsTopOfHourRead", eventLog.IsTopOfHourRead.ToString()));
            read_event.Add(new XElement("ReadReason", eventLog.ReasonForRead.ToString()));
            read_event.Add(new XElement("IsSynchronized", eventLog.IsSynchronized.ToString()));

            events.Add(read_event);
        }

        public void ComplexParameter ( XElement parent, ActionResult resultAllInterfaces, InterfaceParameters parameter, int portNumber = 0 )
        {
            Parameter param = resultAllInterfaces.getParameterByTag ( parameter.Name, parameter.Source, portNumber );
            
            Utils.Print ( "Log Param: " + parameter.Name + " " + parameter.Source + " " + portNumber +
                " = " + ( ( param == null ) ? "..." : param.Value ) );

            /*
            if ( ! string.IsNullOrEmpty ( parameter.Source ) )
            {
                try
                {
                    param = result.getParameterByTag ( parameter.Source.Split(new char[] { '.' })[1], parameter.Source, 0 );
                }
                catch (Exception e)
                {
                
                }
            }
            if (param == null)
                param = result.getParameterByTag ( parameter.Name, parameter.Source, 0 );
            */

            if (param != null)
                AddParameter ( parent, param );
        }

        public void AddParameter ( XElement parent, Parameter parameter )
        {
            XElement xml_parameter = new XElement ( parameter.getLogTag(), parameter.Value );
            AddAtrribute(xml_parameter, "display", parameter.getLogDisplay());
            
            //if ( parameter.Optional )
            //    AddAtrribute(xml_parameter, "option", "1");
            
            parent.Add ( xml_parameter );
        }
    }
}
