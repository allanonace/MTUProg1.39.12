using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Xml;

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

        private string CreateBasicStructure ()
        {
            Configuration config = Configuration.GetInstance ();
        
            string base_stream = "<?xml version=\"1.0\" encoding=\"ASCII\"?>";
            base_stream += "<StarSystem>";
            base_stream += "    <AppInfo>";
            base_stream += "        <AppName>" + config.getApplicationName() + "</AppName>";
            base_stream += "        <Version>" + config.GetApplicationVersion() + "</Version>";
            base_stream += "        <Date>" + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm") + "</Date>";
            base_stream += "        <UTCOffset>" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
            base_stream += "        <UnitId>" + config.GetDeviceUUID() + "</UnitId>";
            base_stream += "        <AppType>" + ( Action.IsFromScripting ? "Scripted" : "Interactive" ) + "</AppType>";
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
            Configuration config = Configuration.GetInstance ();
        
            string base_stream = "<?xml version=\"1.0\" encoding=\"ASCII\"?>";
            base_stream += "<StarSystem>";
            base_stream += "    <AppInfo>";
            base_stream += "        <AppName>" + config.getApplicationName() + "</AppName>";
            base_stream += "        <Version>" + config.GetApplicationVersion() + "</Version>";
            base_stream += "        <Date>" + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm") + "</Date>";
            base_stream += "        <UTCOffset>" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
            base_stream += "        <UnitId>" + config.GetDeviceUUID() + "</UnitId>";
            base_stream += "        <AppType>Scripted</AppType>";
            base_stream += "    </AppInfo>";
            
            if ( Action.currentAction != null )
            {
            base_stream += "    <Action display=\"" + Action.displays[ Action.currentAction.type ] + "\" type=\"" + Action.tag_types[ Action.currentAction.type ] + "\" reason=\"" + Action.tag_reasons[ Action.currentAction.type ] + "\">";
            base_stream += "        <Date display=\"Date/Time\">" + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm") + "</Date>";
            base_stream += "        <User display=\"User\">" + Action.currentAction.user + "</User>";
            base_stream += "    </Action>";
            }
            
            base_stream += "</StarSystem>";
            
            config = null;
            
            string uri = Path.Combine ( Mobile.ConfigPath, "___tmp.xml" );
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
    
                string full_new_path = Path.Combine ( Mobile.LogPath, rel_path );
    
                if ( ! Directory.Exists ( full_new_path ) )
                    Directory.CreateDirectory ( full_new_path );
    
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
            string uri = Path.Combine ( Mobile.LogPath, file_name );

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

            Parameter(element, new Parameter("Date", null, DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

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
            string    time    = DateTime.UtcNow.ToString ( "MM/dd/yyyy HH:mm:ss" );

            // The log send to the explorer should be only the last error performed, not full current activity log
            string    uriLog  = this.CreateLogBase_Scripting_Error ();
            XDocument docLog  = XDocument.Load ( uriLog );

            XElement error;
            XElement message;
            foreach ( Error e in Errors.GetErrorsToLog () )
            {
                error = new XElement ( "AppError" );

                Parameter ( error, new Parameter ( "Date", null, time ) );
            
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
            Parameter ( element, new Parameter ( "Date", null, time ) );
            message = new XElement ( "Message" );
            element.Add ( message );
            docLog.Root.Add ( element );
            
            string docLogText = docLog.ToString ();
            
            if ( File.Exists ( uriLog ) )
                File.Delete ( uriLog );
            
            return docLogText;
        }

        public string ReadMTU ( Action action, ActionResult result, Mtu mtu )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);
            PrepareLog_ReadMTU(doc.Root.Element("Mtus"), action, result, mtu );
            doc.Save(uri);
            
            // Launching multiple times scripts with the same output path, concatenates the actions logs,
            // but the log send to the explorer should be only the last action performed
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                mtu.Id + "-" + action.type + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.CreateFileIfNotExist ( false, uniUri );
            
            XDocument uniDoc = XDocument.Load ( uniUri );
            PrepareLog_ReadMTU ( uniDoc.Root.Element("Mtus"), action, result, mtu );
            
            #if DEBUG
            
            uniDoc.Save ( uniUri );
            
            #endif
            
            // Write in ActivityLog
            if ( Action.IsFromScripting &&
                 ! Configuration.GetInstance ().global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.ResetFixedName ();
                
                uri = CreateFileIfNotExist ();
                doc = XDocument.Load( uri );
                PrepareLog_ReadMTU(doc.Root.Element("Mtus"), action, result, mtu );
                doc.Save(uri);
            }

            return uniDoc.ToString ();
        }

        private void PrepareLog_ReadMTU ( XElement parent, Action action, ActionResult result, Mtu mtu )
        {
            try
            {
            
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.DisplayText);
            AddAtrribute(element, "type", action.LogText);
            AddAtrribute(element, "reason", action.Reason);

            InterfaceParameters[] parameters = Configuration.GetInstance ().getLogInterfaceFields( mtu, ActionType.ReadMtu );
            foreach ( InterfaceParameters parameter in parameters )
            {
                try
                {
            
                if ( parameter.Name == "Port" )
                {
                    ActionResult[] ports = result.getPorts();
                    for ( int i = 0; i < ports.Length; i++ )
                        Port(i, element, ports[i], parameter.Parameters.ToArray());
                }
                else this.ComplexParameter(element, result, parameter);
                
                }
                catch ( Exception ex )
                {
                
                }
            }
            parent.Add(element);

            }
            catch ( Exception ex )
            {

            }
        }

        public void ReadData ( Action action, ActionResult result, Mtu mtu )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);

            PrepareLog_ReadData(doc.Root.Element("Mtus"), action, result, mtu );
            doc.Save(uri);
        }

        private void PrepareLog_ReadData ( XElement parent, Action action, ActionResult result, Mtu mtu )
        {
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.DisplayText);
            AddAtrribute(element, "type", action.LogText);
            AddAtrribute(element, "reason", action.Reason);

            InterfaceParameters[] parameters = Configuration.GetInstance ().getLogInterfaceFields ( mtu, ActionType.ReadData );
            foreach (InterfaceParameters parameter in parameters)
            {
                if (parameter.Name == "Port")
                {
                    ActionResult[] ports = result.getPorts();
                    for (int i = 0; i < ports.Length; i++)
                        Port(i, element, ports[i], parameter.Parameters.ToArray());
                }
                else this.ComplexParameter(element, result, parameter);
            }

            parent.Add(element);
        }

        public void Port ( int portnumber, XElement parent, ActionResult result, InterfaceParameters[] parameters )
        {
            XElement element = new XElement("Port");

            AddAtrribute(element, "display", "Port "+ (portnumber+1).ToString());
            AddAtrribute(element, "number", (portnumber + 1).ToString());

            foreach (InterfaceParameters parameter in parameters)
                this.ComplexParameter(element, result, parameter, portnumber );

            parent.Add(element);
        }

        public string TurnOnOff ( Action action, Mtu mtu, uint mtuId )
        {
            String uri = CreateFileIfNotExist ();
            XDocument doc = XDocument.Load ( uri );
            PrepareLog_TurnOff(doc.Root.Element("Mtus"), action.DisplayText, action.LogText, action.user, mtuId );
            doc.Save(uri);
            
            // Launching multiple times scripts with the same output path, concatenates the actions logs,
            // but the log send to the explorer should be only the last action performed
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                mtu.Id + "-" + action.type + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.CreateFileIfNotExist ( false, uniUri );
            
            XDocument uniDoc = XDocument.Load ( uniUri );
            PrepareLog_TurnOff ( uniDoc.Root.Element("Mtus"), action.DisplayText, action.LogText, action.user, mtuId );
            
            #if DEBUG
            
            uniDoc.Save ( uniUri );
            
            #endif
            
            // Write in ActivityLog
            if ( Action.IsFromScripting &&
                 ! Configuration.GetInstance ().global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.ResetFixedName ();
                
                uri = CreateFileIfNotExist ();
                doc = XDocument.Load ( uri );
                PrepareLog_TurnOff(doc.Root.Element("Mtus"), action.DisplayText, action.LogText, action.user, mtuId );
                doc.Save(uri);
            }
            
            return uniDoc.ToString ();;
        }

        private void PrepareLog_TurnOff (XElement parent, string display, string type, string user, uint MtuId )
        {
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", display);
            AddAtrribute(element, "type", type );

            Parameter(element, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (user != null)
                Parameter(element, new Parameter("User", "User", user));

            Parameter(element, new Parameter("MtuId", "MTU ID", MtuId.ToString()));

            parent.Add(element);
        }

        public void TurnOn ( Action action, uint MtuId )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);
            
            PrepareLog_TurnOn(doc.Root.Element("Mtus"), action.DisplayText, action.LogText, action.user, MtuId);
            doc.Save(uri);
        }

        private void PrepareLog_TurnOn ( XElement parent, string display, string type, string user, uint MtuId )
        {
            XElement element = new XElement("Action");

            AddAtrribute(element, "display", display);
            AddAtrribute(element, "type", type);

            Parameter(element, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (user != null)
                Parameter(element, new Parameter("User", "User", user));

            Parameter(element, new Parameter("MtuId", "MTU ID", MtuId.ToString()));

            parent.Add(element);
        }

        public void Cancel ( Action action, String cancel, String reason )
        {
            String uri = CreateFileIfNotExist();
            XDocument doc = XDocument.Load(uri);

            XElement element = new XElement("Action");

            AddAtrribute(element, "display", action.DisplayText);
            AddAtrribute(element, "type", action.LogText);
            AddAtrribute(element, "reason", action.Reason);

            Parameter(element, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));
            Parameter(element, new Parameter("User", "User", action.user));

            Parameter(element, new Parameter("Cancel", "Cancel Action", cancel));
            Parameter(element, new Parameter("Reason", "Cancel Reason", reason));

            doc.Root.Element("Mtus").Add(element);
            doc.Save(uri);
        }

        public string ReadDataEntries ( string mtu_id, DateTime start, DateTime end, List<LogDataEntry> Entries )
        {
            String    path     = CreateEventFileIfNotExist(mtu_id);
            XDocument doc      = XDocument.Load(path);
            XElement  transfer = doc.Root.Element("Transfer");
            XElement  events   = new XElement("Events");

            AddAtrribute(events, "FilterMode", "Match");
            AddAtrribute(events, "FilterValue", "MeterRead");
            AddAtrribute(events, "RangeStart", start.ToString("yyyy-MM-dd HH:mm:ss"));
            AddAtrribute(events, "RangeStop", end.ToString("yyyy-MM-dd HH:mm:ss"));

            foreach (LogDataEntry entry in Entries)
                PrepareLog_ReadDataEntries(events, entry);

            transfer.Add(events);
            doc.Save(path);

            return path;
        }
        
        private void PrepareLog_ReadDataEntries ( XElement events, LogDataEntry entry )
        {
            XElement read_event = new XElement("MeterReadEvent");

            AddAtrribute(read_event, "FormatVersion", entry.FormatVersion.ToString());

            read_event.Add(new XElement("TimeStamp", entry.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")));
            read_event.Add(new XElement("MeterRead", entry.MeterRead.ToString()));
            read_event.Add(new XElement("ErrorStatus", entry.ErrorStatus.ToString()));
            read_event.Add(new XElement("ReadInterval", "PT"+entry.ReadInterval.ToString()+"M"));
            read_event.Add(new XElement("PortNumber", "PORT"+(entry.PortNumber + 1 ).ToString()));
            read_event.Add(new XElement("IsDailyRead", entry.IsDailyRead.ToString()));
            read_event.Add(new XElement("IsTopOfHourRead", entry.IsTopOfHourRead.ToString()));
            read_event.Add(new XElement("ReadReason", entry.ReasonForRead.ToString()));
            read_event.Add(new XElement("IsSynchronized", entry.IsSynchronized.ToString()));

            events.Add(read_event);
        }

        public void ComplexParameter ( XElement parent, ActionResult result, InterfaceParameters parameter, int portNumber = 0 )
        {
            Parameter param = result.getParameterByTag ( parameter.Name, parameter.Source, portNumber );

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
                Parameter ( parent, param );
        }

        public void Parameter ( XElement parent, Parameter parameter )
        {
            XElement xml_parameter = new XElement ( parameter.getLogTag(), parameter.Value );
            AddAtrribute(xml_parameter, "display", parameter.getLogDisplay());
            
            if ( parameter.Optional )
                AddAtrribute(xml_parameter, "option", "1");
                
            parent.Add ( xml_parameter );
        }
    }
}
