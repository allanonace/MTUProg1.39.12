using System.Collections.Generic;
using Xml;
using System;
using System.Linq;
using System.Dynamic;
using MTUComm.Exceptions;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace MTUComm
{
    public sealed class Errors
    {    
        #region Constants

        private const string ERROR_TITLE = "Controlled Exception";
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
            // Puck can't read from MTU after has completed writing process
            { new PuckCantReadFromMtuAfterWritingException (),  102 },
            // The Mtu.xml file does no contain the MTU ID specified
            { new MtuMissingException (),                       103 },
        
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
            { new MandatoryMeterSerialHiddenScriptException (), 307 },
            // The script contains the same parameter more than once
            { new SameParameterRepeatScriptException (),        308 },
            // User name missing in the script file
            { new ScriptUserNameMissingException (),            309 },

            
            // Alarm [ 4xx ]
            //------
            // The alarm profile Scripting for current MTU is not defined in the Alarm.xml file
            { new ScriptingAlarmForCurrentMtuException (),      400 },
            // No alarm was selected but the MTU entry in Mtu.xml requires using an alarm
            { new SelectedAlarmForCurrentMtuException (),       401 },
            
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

            // Encryption [ 7xx ]
            //-----------
            // The MTU encryption process can't be performed after having tried it several times
            { new ActionNotAchievedEncryptionException (),      700 },
            
            // Configuration Files and System [ 7xx ]
            //-------------------------------
            // Some of the configuration files are not present in the root folder. Contact your IT administrator
            { new ConfigurationFilesNotFoundException (),       800 },
            // There is a problem with the configuration files and some of them are corrupted. Contact your IT administrator
            { new ConfigurationFilesCorruptedException (),      801 },
            // The certificate that you tried to install is not a valid certificate file ( *.cer )
            { new CertificateFileNotValidException (),          802 },
            // It is not possible to use the currently installed certificate. Contact your IT administrator
            { new CertificateInstalledNotValidException (),     803 },
            // Download or install a new certificate because the one that is currently used has expired
            { new CertificateInstalledExpiredException (),      804 },
            // Not all necessary permissions have been granted on the Android device.
            { new AndroidPermissionsException (),               805 },
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
            { new ConfigFilesChangedException (),               812 }
        };

        #endregion

        #region Attributes

        private static Errors instance;
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
        /// doing the structure more logical, associating the class itself with errors
        /// </summary>
        /// <param name="id">Error identifier</param>
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
        /// Indicates if the error id should be written or only the messages
        /// </summary>
        /// <value><c>true</c> if the client wants to show error IDs in the log; otherwise, <c>false</c></value>
        public static bool ShowId
        {
            get
            {
                Configuration config = Configuration.GetInstance ();
                return ( config.global != null ) ? config.global.ErrorId : true;
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
            this.logger      = ( Action.currentAction != null ) ? Action.currentAction.logger : new Logger ();
            this.errors      = new Dictionary<int,Error> ();
            this.errorsToLog = new List<Error> ();
            this.xmlErrors   = Aux.DeserializeXml<ErrorList> ( "Error.xml", true ).List;

            if ( this.xmlErrors != null )
                foreach ( Error errorXml in this.xmlErrors )
                    this.errors.Add ( errorXml.Id, errorXml );
        }

        private static Errors GetInstance ()
        {
            if ( Errors.instance == null )
                Errors.instance = new Errors ();
            
            return Errors.instance;
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
                error.MessagePopup = ( ( OwnExceptionsBase )e ).MessagePopup;
            }
            // .NET exception
            else
            {
                error = ( Error )this.TryToTranslateDotNet ( e ).Clone ();
                error.Port      = portIndex;
                error.Exception = e;
                error.Message   = e.Message;
                error.MessagePopup = e.Message;
            }
            
            return error;
        }
        
        /// <summary>
        /// Returns all registered errors and by default clear list after that
        /// </summary>
        /// <returns>The errors to log.</returns>
        /// <param name="clearList">If set to <c>true</c> clear list of registered errors</param>
        private Error[] _GetErrorsToLog (
            bool clearList = true )
        {
            Error[] errors = new Error[ this.errorsToLog.Count ];
            Array.Copy ( this.errorsToLog.ToArray (), errors, this.errorsToLog.Count );
            
            if ( clearList )
                this.errorsToLog.Clear ();
            
            return errors;
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
            bool kill = false)
        {
            Error error = this.AddErrorByException ( e, portIndex );
            PageLinker.ShowAlert ( ERROR_TITLE, error, kill );
            
            // Method can be invoked when Configuration is not instantiated yet
            if ( Configuration.HasInstance )
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
                PageLinker.ShowAlert ( ERROR_TITLE, this.lastError );
                
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
            
            await PageLinker.ShowAlert ( ERROR_INFO, error );
        }

        #endregion

        #endregion

        #region Direct Singleton

        /// <summary>
        /// Gets all errors registered to log, used from class Logger
        /// </summary>
        /// <returns>Array of registered errors</returns>
        /// <param name="clearList">Clear list of registered errors after being returned</param>
        public static Error[] GetErrorsToLog (
            bool clearList = true )
        {
            return Errors.GetInstance ()._GetErrorsToLog ( clearList );
        }
        
        /// <summary>
        /// Registers a new error based on an ( own or .Net ) exception
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
        /// Registers a new error based on an exception, shows an popup alert using
        /// this last error and also registers in the ( activity or result ) log file
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
        /// this last error, but does not register in the ( activity or result ) log file
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
        /// is not loaded yet and the error forces to close the app
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        public static void LogErrorNowAndKill (
            Exception e )
        {
            Errors.GetInstance ()._LogErrorNow ( e, 1, false, true ); // Port index has not importance in this case
        }

        /// <summary>
        /// Both options will log all registered exceptions that remain, but in
        /// the first case previously the last exception launched will be added
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        public static void LogRemainExceptions (
            Exception e )
        {
            // Last exception was not added yet
            if ( ! Errors.GetInstance ().IsLastExceptionUsed ( e ) )
                Errors.LogErrorNow ( e );
            
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
