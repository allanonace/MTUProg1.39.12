using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Library;
using Library.Exceptions;
using Xml;

namespace MTUComm
{
    public sealed class Errors
    {    
        #region Constants

        private const string ERROR_INFO  = "Information";

        private Dictionary<Exception,int> ex2id = 
        new Dictionary<Exception,int> ()
        {
            // Dynamic MemoryMap [ 0xx ]
            //------------------
            // ...
        
            // MTU [ 1xx ]
            //----
            // MTU is not the same as at the beginning of the process
            { new MtuHasChangeBeforeFinishActionException (),   100 },
            // Puck can't write or read to/from MTU
            { new PuckCantCommWithMtuException (),              101 },
            { new LexiReadingException (),                      101 },
            { new LexiWritingException (),                      101 },
            { new LexiWritingAckException (),                   101 },
            // Puck can't read from MTU after has completed writing process
            { new PuckCantReadFromMtuAfterWritingException (),  102 },
            // The Mtu.xml file does no contain the MTU ID specified
            { new MtuMissingException (),                       103 },
            // Error trying to start the event log query
            { new MtuQueryEventLogsException (),                104 },
            // Get next event log process has failed trying to recover data from the MTU
            { new AttemptNotAchievedGetEventsLogException (),   105 },
            // Historical Read process can't be performed because the MTU is busy
            { new MtuIsBusyToGetEventsLogException (),          106 },
            // Historical Read process can't be performed after having tried it several times
            { new ActionNotAchievedGetEventsLogException (),    107 },
            // The MtU is not an OnDemand device
            { new MtuIsNotOnDemandCompatibleDevice (),          108 },
            // There are no records to retrieve for the Historical Read process
            { new NoEventsLogException (),                      109 },
            // Preparing values for the log. Check puck position and try again
            { new PreparingLogInterfaceException (),            110 },
            // The value to write in BCD format for the Meter ID is greater ( ... ) than the number of available bytes
            { new NumberToBcdIsLargerThanBytesRegister (),      111 },
            // The MTU does not belong to any of the families currently supported
            { new MtuDoesNotBelongToAnyFamilyException (),      112 },
        
            // Meter [ 2xx ]
            //------
            // The Meter.xml file does not contain the Meter ID specified
            { new ScriptingAutoDetectMeterMissing (),           200 },
            // Selected Meter is not supported for current MTU
            { new ScriptingAutoDetectNotSupportedException (),  201 },
            // The Meter.xml file does not contain the Meter type specified with the tags NumberOfDials, DriveDialSize and UnitOfMeasure
            { new ScriptingAutoDetectMeterException (),         202 },
            // The script does not contain the tag NumberOfDials needed to select the MTU
            { new NumberOfDialsTagMissingScript (),             203 },
            // The script does not contain the tag DriveDialSize needed to select the MTU
            { new DriveDialSizeTagMissingScript (),             204 },
            // The script does not contain the tag UnitOfMeasure needed to select the MTU
            { new UnitOfMeasureTagMissingScript (),             205 },
            // Some of the necessary parameters for Meter autodetection are missing in the script
            { new ScriptingAutoDetectTagsMissingScript (),      206 },
            // The selected Meter is not compatible with the Encoder connected'
            { new EncoderMeterFFException (),                   207 },
            // The selected Meter is not compatible with the Encoder because of 'Encoder has bad digit in reading'
            { new EncoderMeterFEException (),                   208 },
            // The selected Meter is not compatible with the Encoder because of 'Delta overflow'
            { new EncoderMeterFDException (),                   209 },
            // The selected Meter is not compatible with the Encoder because of 'Deltas purged / New install / Reset'
            { new EncoderMeterFCException (),                   210 },
            // The selected Meter is not compatible with the Encoder because of 'Encoder clock shorted'
            { new EncoderMeterFBException (),                   211 },
            // The selected Meter is not compatible with the Encoder because of an unknown error code
            { new EncoderMeterUnknownException (),              212 },
            // Encoder auto-detection process cannot be performed after having tried it for _var_ seconds
            { new EncoderAutodetectNotAchievedException (),     213 },
            // Encoder auto-detection process cannot be performed, perhaps due to a problem with the MTU
            { new EncoderAutodetectException (),                214 },
            // The status of the RDD device is unknown
            { new RDDDesiredStatusIsUnknown (),                 215 },
            // The RDD device is disabled
            { new RDDStatusIsDisabled (),                       216 },
            // The status of the RDD device is not busy after executing the LExI command
            { new RDDStatusIsNotBusyAfterLExICommand (),        217 },
            // The RDD device continues in transition status after ... seconds
            { new RDDContinueInTransitionAfterMaxTime (),       218 },
            // The status of the RDD device is unknown after ... seconds
            { new RDDStatusIsUnknownAfterMaxTime (),            219 },
            // The status of the RDD device is different than expected after ... seconds
            { new RDDStatusIsDifferentThanExpected (),          220 },
                       
            // Scripting Parameters [ 3xx ]
            //---------------------
            // Error translating or validating parameters from script/trigger file
            { new ProcessingParamsScriptException (),           300 },
            // The script is only for one port but the MTU has two port and both activated
            { new ScriptForOnePortButTwoEnabledException (),    301 },
            // The script is for two ports but the MTU has one port only or second port is disabled
            { new ScriptForTwoPortsButMtuOnlyOneException (),   302 },
            // Logfile element in the script is empty or contains some invalid character
            { new ScriptLogfileInvalidException (),             303 },
            // Action type specified in the script is empty or is not one of the available options
            { new ScriptActionTypeInvalidException (),          304 },
            // The script file used has not valid structure or format
            { new ScriptWrongStructureException (),             305 },
            // The script file used is empty
            { new ScriptEmptyException (),                      306 },
            // The script does not contain the ( Old|New ) Meter serial number parameter that is mandatory in writing actions
            { new MandatoryMeterSerialHiddenScriptException (), 307 }, // NOT USED
            // The script contains the same parameter more than once
            { new SameParameterRepeatScriptException (),        308 },
            // User name missing in the script file
            { new ScriptUserNameMissingException (),            309 },
            // The script does not contain the required parameters 
            { new ScriptingTagMissingException (),              310 },
            
            // Alarm [ 4xx ]
            //------
            // The alarm profile Scripting for current MTU is not defined in the Alarm.xml file
            { new ScriptingAlarmForCurrentMtuException (),      400 },
            // No alarm was selected but the MTU entry in Mtu.xml requires using an alarm
            { new SelectedAlarmForCurrentMtuException (),       401 },

            // Alarm [ 45x ]
            //------
            // The demand profile Scripting for current MTU is not defined in the DemandConf.xml file
            { new ScriptingDemandForCurrentMtuException (),     450 },
            // No demand was selected but the MTU entry in Mtu.xml requires using one
            { new SelectedDemandForCurrentMtuException (),      451 },
            
            // Turn Off [ 5xx ]
            //---------
            // Turn off MTU process has failed trying to activated the Ship Bit
            { new AttemptNotAchievedTurnOffException (),        500 },
            // MTU can not be turned off after having tried it several times
            { new ActionNotAchievedTurnOffException (),         501 },
            
            // Install Confirmation [ 6xx ]
            //---------------------
            // The MTU has not support for two-way
            { new MtuIsNotTwowayICException (),                 600 },
            // The MTU is turned off
            { new MtuIsAlreadyTurnedOffICException (),          601 },
            // Installation Confirmation process has failed trying to communicate with the DCU
            { new AttemptNotAchievedICException (),             602 },
            // Installation Confirmation can't be performed after having tried it several times
            { new ActionNotAchievedICException (),              603 },
            // Node Discovery not initialized correctly
            { new NodeDiscoveryNotInitializedException (),      604 }, // NOT USED
            // Node Discovery not started correctly
            { new NodeDiscoveryNotStartedException (),          605 }, // NOT USED
            // Node Discovery can't be performed because it ended prematurely due to an exception
            { new ActionStoppedNodeDiscoveryException (),       606 },
            // Node Discovery can't be performed after having tried it for ... seconds
            { new ActionNotAchievedNodeDiscoveryException (),   607 },

            // Encryption [ 7xx ]
            //-----------
            // The MTU encryption process can't be performed after having tried it several times
            { new ActionNotAchievedEncryptionException (),      700 },
            // The MTU encryption process can't be performed because public key is not present in Global.xml
            { new ODEncryptionPublicKeyNotSetException (),      701 },
            // The MTU encryption process can't be performed because public key does not have the correct format
            { new ODEncryptionPublicKeyFormatException (),      702 },
            // The MTU encryption process can't be performed because broadcast key is not present in Global.xml
            { new ODEncryptionBroadcastKeyNotSetException (),   703 },
            // The MTU encryption process can't be performed because broadcast key does not have the correct format
            { new ODEncryptionBroadcastKeyFormatException (),   704 },
            // The MTU encryption index has reached its limit
            { new EncryptionIndexLimitReachedException (),      705 },
            
            // Configuration Files and System [ 8xx ]
            //-------------------------------
            // Some of the configuration files are not present in the root folder. Contact your IT administrator
            { new ConfigFilesNotFoundException (),              800 },
            // There is a problem with the configuration files and some of them are corrupted or may not have a the port type defined. Contact your IT administrator
            { new ConfigFilesCorruptedException (),             801 },
            // The certificate that you tried to install is not a valid certificate file ( *.cer )
            { new CertificateFileNotValidException (),          802 },
            // It is not possible to use the currently installed certificate. Contact your IT administrator
            { new CertificateInstalledNotValidException (),     803 },
            // Download or install a new certificate because the one that is currently used has expired
            { new CertificateInstalledExpiredException (),      804 },
            // Not all necessary permissions have been granted on the Android device.
            { new AndroidPermissionsException (),               805 }, // NOT USED
            // There is a problem with the FTP and configuration files could not be downloaded. Contact your IT administrator
            { new FtpDownloadException (),                      806 },
            // The Device has not internet connection
            { new NoInternetException (),                       807 },
            // The current date of the device is lower than allowed
            { new DeviceMinDateAllowedException (),             808 },
            // Missing credentials for SFTP in global.xml
            { new FtpCredentialsMissingException (),            809 },
            // App cannot comunnicate with the FTP
            { new FtpConnectionException (),                    810 },
            // Sending activity logs to the FTP has failed, only _var_ files have been uploaded
            { new FtpUpdateLogsException (),                    811 },
 		    // Changed configuration files
            { new ConfigFilesChangedException (),               812 }, // NOT USED
            // New version files corrupted
            { new ConfigFilesNewVersionException (),            813 },
            // Intune credentials missing
            { new IntuneCredentialsException (),                814 },
            // Exception with camera
            { new CameraException (),                           815 },
            // Modification of the content of Global.xml file has failed
            { new GlobalChangedException (),                    816 },
            // Exception MTU without portType
            { new PortTypeMissingMTUException (),               817 },
            // The configuration files are corrupted and the app will continue with the current files. Contact your IT administrator
            { new ConfigFilesCorruptedSettingsException (),     818 },
            // The current device date is less than allowed and the app will continues with the actual ones when restart it. Contact your IT administrator
            { new DeviceMinDateAllowedSettingsException (),     819 },
            
            // DEBUG - Configuration Files and System [ 85x ]
            //-------------------------------
            // The interface for the specified MTU ID was not found
            { new InterfaceNotFoundException_Internal (),       850 },
            // The interface for the specified MTU ID and action was not found
            { new ActionInterfaceNotFoundException_Internal (), 851 },
            // The Alarm list for the specified MTU ID was not found
            { new AlarmNotFoundException_Internal (),           852 },
            // The DemandaConfig for the specified MTU ID was not found
            { new DemandNotFoundException_Internal (),          853 },

            // Internal [ 9xx ]
            //-------------------------------
            // Deserializing MemoryMap has failed due to validate required fields ...
            { new MemoryMapXmlValidationException (),           900 },
            // Error generating MemoryMap in the ... register
            { new MemoryMapParseXmlException (),                901 },
            // The register ... does not exist in the MemoryMap
            { new MemoryRegisterNotExistException (),           902 },
            // Custom method ... does not exist in MTU family class
            { new CustomMethodNotExistException (),             903 },
            // Overload register ... must have the custom field configured
            { new OverloadEmptyCustomException (),              904 },
            // String argument can't be casted to ...
            { new SetMemoryFormatException (),                  905 },
            // Argument value is outside ... limits
            { new SetMemoryTypeLimitException (),               906 },
            // All overload registers are readonly members
            { new MemoryOverloadsAreReadOnly (),                907 },
            // Setting value to a Memory Register
            { new MemoryRegisterSetValueException (),           908 },
        };

        #endregion

        #region Attributes

        public  static string lastErrorLogGenerated { get; private set; }

        private Logger logger;
        private Dictionary<int,Error> errors;
        private List<Error> errorsToLog;
        private Error lastError;
        private Error[] xmlErrors;

        #endregion

        #region Properties

        /// <summary>
        /// An indexer to easiest recover errors without having to use a methods,
        /// doing the structure more logical, associating the class itself with the errors.
        /// </summary>
        /// <param name="id">Error identifier in numeric format</param>
        public Error this[ int id ]
        {
            get
            {
                if ( this.errors.ContainsKey ( id ) )
                    return errors[ id ];
                return null;
            }
        }
        
        /// <summary>
        /// Indicates if the error ID should be written or only the messages.
        /// </summary>
        /// <remarks>
        /// This value is red from Global.xml file, searching for the tag 'ErrorId'.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the client wants to show error IDs
        /// </returns>
        public static bool ShowId
        {
            get
            {
                Global global = Singleton.Get.Configuration.Global;
                return ( global != null ) ? global.ErrorId : true;
            }
        }

        public static Error LastError
        {
            get { return Errors.GetInstance ().lastError; }
        }

        #endregion

        #region Initialization

        private Errors ()
        {
            Action currentAction = Singleton.Get.Action;
        
            this.logger      = ( currentAction != null ) ? currentAction.Logger : new Logger ();
            this.errors      = new Dictionary<int,Error> ();
            this.errorsToLog = new List<Error> ();
            this.xmlErrors   = Utils.DeserializeXml<ErrorList> ( "Error.xml", true ).List;

            if ( this.xmlErrors != null )
                foreach ( Error errorXml in this.xmlErrors )
                    this.errors.Add ( errorXml.Id, errorXml );
        }

        private static Errors GetInstance ()
        {
            if ( ! Singleton.Has<Errors> () )
                Singleton.Set = new Errors ();
            
            return Singleton.Get.Errors;
        }

        #endregion

        #region Logic

        #region Get Error

        private Error GetErrorById (
            int id,
            Exception e,
            int portIndex = 1 )
        {
            Error error = null;
        
            if ( this[ id ] != null )
            {
                error           = ( Error )this[ id ].Clone ();
                error.Port      = portIndex;
                error.Exception = e;
            }
            
            return error;
        }

        private Error GetErrorByException (
            Exception e,
            int portIndex = 1 )
        {
            Type  typeExp = e.GetType ();
            Error error   = null;
            
            // Own exception
            if ( this.ex2id.Any ( item => item.Key.GetType () == typeExp ) )
            {
                int id = this.ex2id.Single ( item => item.Key.GetType () == typeExp ).Value;
                
                error = this.GetErrorById ( id, e, portIndex );
                error.Exception = e;
            }
            // .NET exception
            else
            {
                error = ( Error )this.TryToTranslateDotNet ( e ).Clone ();
                error.Port         = portIndex;
                error.Exception    = e;
                error.Message      = e.Message;
                error.MessagePopup = e.Message;
            }
            
            return error;
        }
        
        private Error[] _GetErrorsToLog (
            bool clearList = true )
        {
            Error[] errs = new Error[ this.errorsToLog.Count ];
            Array.Copy ( this.errorsToLog.ToArray (), errs, this.errorsToLog.Count );
            
            if ( clearList )
                this.errorsToLog.Clear ();
            
            return errs;
        }
        
        private bool IsLastExceptionUsed (
            Exception e )
        {
            return ( this.lastError != null &&
                     this.lastError.Exception == e );
        }

        #endregion

        #region Translate .NET Errors
        
        private Error TryToTranslateDotNet (
            Exception e )
        {
            dynamic dynException = new ExpandoObject ();
            dynException.Message = e.Message;
            dynException.HResult = int.Parse ( e.HResult.ToString ( "X" ) );
        
            int idTranslated = this.GetIdForDotNetError ( dynException );
        
            // The exception has translation in Error.xml
            if ( idTranslated > -1 )
                return this[ idTranslated ];
            
            // Register the .Net exception directly, without error ID
            return new Error ( dynException.Message );
        }

        private int GetIdForDotNetError (
            dynamic e ) // e is an Exception
        {
            if ( this.IsRegisteredDotNetError ( e ) )
                return this.errors.Single ( item => item.Value.DotNetId == e.HResult ).Value.Id;
            return -1;
        }
        
        private bool IsRegisteredDotNetError (
           dynamic e ) // e is an Exception
        {
            return e.HResult > -1 &&
                   this.errors.Any ( item => item.Value.DotNetId == e.HResult );
        }

        #endregion

        #region Register Error

        /// <summary>
        /// Register a new error to be written into the log after be recovered using GetErrorsToLog
        /// </summary>
        /// <returns>Exception that represents the last error happened</returns>
        /// <param name="id">Error identifier</param>
        /// <param name="portIndex">MTU port index related</param>
        private Error AddErrorById (
            int id,
            Exception e,
            int portIndex = 1 )
        {
            // Error ID exists and is not registered yet
            if ( this[ id ] != null )
            {
                Error error = this.GetErrorById ( id, e, portIndex );
                this.errorsToLog.Add ( error );
                
                return ( this.lastError = this.errorsToLog.Last () );
            }
            return null;
        }
        
        /// <summary>
        /// Register .NET errors trying to found some error that match
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        /// <param name="portIndex">MTU port index related</param>
        private Error AddErrorByException (
            Exception e,
            int portIndex = 1 )
        {
            Error error = this.GetErrorByException ( e, portIndex );
            this.errorsToLog.Add ( error );
            
            return ( this.lastError = this.errorsToLog.Last () );
        }

        #endregion

        #region Log and Alert

        /// <summary>
        /// Write an error into the log right now, without have to invoke AddError method
        /// Usually used outside actions logic, for example trying to detect and connect with a puck
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        private void _LogErrorNow (
            Exception e,
            int portIndex,
            bool forceException,
            bool kill = false )
        {
            Error error = this.AddErrorByException ( e, portIndex );
            PageLinker.ShowErrorAlert ( ERROR_INFO, error, kill );
            
            // Method can be invoked when Configuration is not instantiated yet
            if ( Singleton.Has<Configuration> () )
                lastErrorLogGenerated = this.logger.Error ();
            
            if ( forceException )
                throw error.Exception;
        }

        /// <summary>
        /// Writes the errors registered with AddError method. It is almost the same as
        /// the _LogErrorNow method but using the last registered error for the alert pop-up
        /// </summary>
        private void _LogRegisteredErrors (
            bool forceException )
        {
            if ( this.errorsToLog.Count > 0 )
            {
                Error error = this.errorsToLog[ this.errorsToLog.Count - 1 ];
                PageLinker.ShowErrorAlert ( ERROR_INFO, this.lastError );
                
                lastErrorLogGenerated = this.logger.Error ();

                if ( forceException )
                    throw error.Exception;
            }
        }
        
        private async Task _ShowAlert (
            Exception e )
        {
            Error error = this.GetErrorByException ( e );
            error.Id = -1;
            
            await PageLinker.ShowErrorAlert ( ERROR_INFO, error );
        }

        #endregion

        #endregion

        #region Direct Singleton

        /// <summary>
        /// Returns all errors registered to log, used from class Logger.
        /// </summary>
        /// <returns>Array of registered errors</returns>
        /// <param name="clearList">Clear list of registered errors after being returned</param>
        public static Error[] GetErrorsToLog (
            bool clearList = true )
        {
            return Errors.GetInstance ()._GetErrorsToLog ( clearList );
        }
        
        /// <summary>
        /// Registers a new error based on an ( own or .Net ) exception.
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        /// <param name="portIndex">MTU port index related</param>
        public static void AddError (
            Exception e,
            int portIndex = 1 )
        {
            Errors.GetInstance ().AddErrorByException ( e, portIndex );
        }
        
        /// <summary>
        /// Registers a new error based on an exception, shows a popup alert using
        /// this last error and also writes it in the ( activity or result ) log file.
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        /// <param name="portIndex">Index of MTU port associated to the error</param>
        /// <param name="forceException">Forces to launch/throw the exception</param>
        public static void LogErrorNow (
            Exception e,
            int portIndex = -1,
            bool forceException = true )
        {
            if ( ! IsOwnException ( e ) )
                 Errors.GetInstance ()._LogErrorNow ( e, portIndex, forceException );
            else Errors.GetInstance ()._LogErrorNow ( e, ( ( portIndex > -1 ) ? portIndex : ( ( OwnExceptionsBase )e ).Port ), forceException );
        }
        
        /// <summary>
        /// Registers a new error based on an exception and shows an popup alert using
        /// this last error, but does not write it in the ( activity or result ) log file.
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        /// <param name="portindex">Index of MTU port associated to the error</param>
        public static void LogErrorNowAndContinue (
            Exception e,
            int portindex = -1 )
        {
            LogErrorNow ( e, portindex, false );
        }

        /// <summary>
        /// Used during the initialization process, when the app
        /// has not yet loaded and the error forces the application to close.
        /// </summary>
        /// <param name="e">Exception that represents the last error</param>
        public static void LogErrorNowAndKill (
            Exception e )
        {
            Errors.GetInstance ()._LogErrorNow ( e, 1, false, true ); // Port index has not importance in this case
        }

        /// <summary>
        /// Both options will log all registered exceptions that remain, but in
        /// the first case previously the last exception launched will be added.
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        public static void LogRemainExceptions (
            Exception e )
        {
            // Last exception was not added yet
            if ( ! Errors.GetInstance ().IsLastExceptionUsed ( e ) )
                Errors.LogErrorNow ( e, -1, false );
            
            // Last exception was already added
            else
                Errors.LogRegisteredErrors ();
        }
        
        public static void LogRegisteredErrors (
            bool forceException = false )
        {
            Errors.GetInstance ()._LogRegisteredErrors ( forceException );
        }
        
        public async static Task ShowAlert (
            Exception e )
        {
            await Errors.GetInstance ()._ShowAlert ( e );
        }

        /// <summary>
        /// Launched exception is an own exception or is from .Net framework
        /// </summary>
        /// <returns><c>true</c>, if own exception was ised, <c>false</c> otherwise.</returns>
        /// <param name="e">Exception that represents the last error happened</param>
        public static bool IsOwnException (
            Exception e )
        {
            return ( e.GetType ().IsSubclassOf ( typeof( OwnExceptionsBase ) ) );
        }

        #endregion
    }
}
