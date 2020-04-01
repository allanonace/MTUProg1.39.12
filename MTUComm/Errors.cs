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

        private const string ERROR_INFO = "Information";

        private Dictionary<Exception,int> ex2id = 
        new Dictionary<Exception,int> ()
        {
            // Dynamic MemoryMap [ 0xx ]
            //------------------
            // ...
        
            // MTU [ 1xx ]
            //----
            { new MtuHasChangeBeforeFinishActionException (),   100 },
            { new PuckCantCommWithMtuException (),              101 },
            { new LexiReadingException (),                      101 },
            { new LexiWritingException (),                      101 },
            { new LexiWritingAckException (),                   101 },
            { new PuckCantReadFromMtuAfterWritingException (),  102 },
            { new MtuMissingException (),                       103 },
            { new MtuQueryEventLogsException (),                104 },
            { new AttemptNotAchievedGetEventsLogException (),   105 }, // NOTE: Not used because it fills the log list
            { new MtuIsBusyToGetEventsLogException (),          106 },
            { new ActionNotAchievedGetEventsLogException (),    107 },
            { new MtuIsNotOnDemandCompatibleDevice (),          108 },
            { new NoEventsLogException (),                      109 },
            { new PreparingLogInterfaceException (),            110 },
            { new NumberToBcdIsLargerThanBytesRegister (),      111 },
            { new MtuDoesNotBelongToAnyFamilyException (),      112 },
            { new MtuIsNotRDDCompatibleDevice (),               113 },
        
            // Meter [ 2xx ]
            //------
            { new ScriptingAutoDetectMeterMissing (),           200 },
            { new ScriptingAutoDetectNotSupportedException (),  201 },
            { new ScriptingAutoDetectMeterException (),         202 },
            { new NumberOfDialsTagMissingScript (),             203 },
            { new DriveDialSizeTagMissingScript (),             204 },
            { new UnitOfMeasureTagMissingScript (),             205 },
            { new ScriptingAutoDetectTagsMissingScript (),      206 },
            { new EncoderMeterFFException (),                   207 },
            { new EncoderMeterFEException (),                   208 },
            { new EncoderMeterFDException (),                   209 },
            { new EncoderMeterFCException (),                   210 },
            { new EncoderMeterFBException (),                   211 },
            { new EncoderMeterUnknownException (),              212 },
            { new EncoderAutodetectNotAchievedException (),     213 },
            { new EncoderAutodetectException (),                214 },
            { new RDDDesiredStatusIsUnknown (),                 215 },
            { new RDDStatusIsDisabled (),                       216 },
            { new RDDStatusIsNotBusyAfterLExICommand (),        217 },
            { new RDDContinueInTransitionAfterMaxTime (),       218 },
            { new RDDStatusIsUnknownAfterMaxTime (),            219 },
            { new RDDStatusIsDifferentThanExpected (),          220 },
                       
            // Scripting Parameters [ 3xx ]
            //---------------------
            { new ProcessingParamsScriptException (),           300 },
            { new ScriptForOnePortButTwoEnabledException (),    301 },
            { new ScriptForTwoPortsButMtuOnlyOneException (),   302 },
            { new ScriptLogfileInvalidException (),             303 },
            { new ScriptActionTypeInvalidException (),          304 },
            { new ScriptWrongStructureException (),             305 },
            { new ScriptEmptyException (),                      306 },
            { new MandatoryMeterSerialHiddenScriptException (), 307 }, // DEPRECATED
            { new SameParameterRepeatScriptException (),        308 },
            { new ScriptUserNameMissingException (),            309 },
            { new ScriptingTagMissingException (),              310 },
            { new ScriptRepeatedParametersException (),         311 },
            
            // Alarm [ 4xx ]
            //------
            { new ScriptingAlarmForCurrentMtuException (),      400 },
            { new SelectedAlarmForCurrentMtuException (),       401 },

            // Alarm [ 45x ]
            //------
            { new ScriptingDemandForCurrentMtuException (),     450 },
            { new SelectedDemandForCurrentMtuException (),      451 },
            
            // Turn Off [ 5xx ]
            //---------
            { new AttemptNotAchievedTurnOffException (),        500 },
            { new ActionNotAchievedTurnOffException (),         501 },
            
            // Install Confirmation [ 6xx ]
            //---------------------
            { new MtuIsNotTwowayICException (),                 600 },
            { new MtuIsAlreadyTurnedOffICException (),          601 },
            { new AttemptNotAchievedICException (),             602 },
            { new ActionNotAchievedICException (),              603 },
            { new NodeDiscoveryNotInitializedException (),      604 }, // DEPRECATED
            { new NodeDiscoveryNotStartedException (),          605 }, // DEPRECATED
            { new ActionStoppedNodeDiscoveryException (),       606 },
            { new ActionNotAchievedNodeDiscoveryException (),   607 },

            // Encryption [ 7xx ]
            //-----------
            { new ActionNotAchievedEncryptionException (),      700 },
            { new ODEncryptionPublicKeyNotSetException (),      701 },
            { new ODEncryptionPublicKeyFormatException (),      702 },
            { new ODEncryptionBroadcastKeyNotSetException (),   703 },
            { new ODEncryptionBroadcastKeyFormatException (),   704 },
            { new EncryptionIndexLimitReachedException (),      705 },
            
            // Configuration Files and System [ 8xx ]
            //-------------------------------
            { new ConfigFilesNotFoundException (),              800 },
            { new ConfigFilesCorruptedException (),             801 },
            { new CertificateFileNotValidException (),          802 },
            { new CertificateInstalledNotValidException (),     803 },
            { new CertificateInstalledExpiredException (),      804 },
            { new AndroidPermissionsException (),               805 }, // DEPRECATED
            { new FtpDownloadException (),                      806 },
            { new NoInternetException (),                       807 },
            { new DeviceMinDateAllowedException (),             808 },
            { new FtpCredentialsMissingException (),            809 },
            { new FtpConnectionException (),                    810 },
            { new FtpUpdateLogsException (),                    811 },
            { new ConfigFilesChangedException (),               812 }, // DEPRECATED
            { new ConfigFilesNewVersionException (),            813 },
            { new IntuneCredentialsException (),                814 },
            { new CameraException (),                           815 },
            { new GlobalChangedException (),                    816 },
            { new PortTypeMissingMTUException (),               817 },
            { new ConfigFilesCorruptedSettingsException (),     818 },
            
            // DEBUG - Configuration Files and System [ 85x ]
            //-------------------------------
            { new InterfaceNotFoundException_Internal (),       850 },
            { new ActionInterfaceNotFoundException_Internal (), 851 },
            { new AlarmNotFoundException_Internal (),           852 },
            { new DemandNotFoundException_Internal (),          853 },

            // Internal [ 9xx ]
            //-------------------------------
            { new MemoryMapXmlValidationException (),           900 },
            { new MemoryMapParseXmlException (),                901 },
            { new MemoryRegisterNotExistException (),           902 },
            { new CustomMethodNotExistException (),             903 },
            { new OverloadEmptyCustomException (),              904 },
            { new SetMemoryFormatException (),                  905 },
            { new SetMemoryTypeLimitException (),               906 },
            { new MemoryOverloadsAreReadOnly (),                907 },
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
