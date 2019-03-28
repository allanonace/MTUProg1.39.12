using System.Collections.Generic;
using Xml;
using System;
using System.Linq;
using System.Dynamic;
using MTUComm.Exceptions;
using System.Xml.Serialization;
using System.IO;

namespace MTUComm
{
    public sealed class Errors
    {    
        #region Constants

        private const string ERROR_TITLE = "Controlled Exception";

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
            // Script is only for one port but the MTU has two port and both activated
            { new ScriptForOnePortButTwoEnabledException (),    301 },
            // Script is for two ports but the MTU has one port only or second port is disabled
            { new ScriptForTwoPortsButMtuOnlyOneException (),   302 },
            // Logfile element in the script is empty or contains some invalid character
            { new ScriptLogfileInvalidException (),             303 },
            // Action type specified in the script is empty or is not one of the available options
            { new ScriptActionTypeInvalidException (),          304 },
            // The script file used has not valid structure or format
            { new ScriptWrongStructureException (),             305 },
            // The script file used is empty
            { new ScriptEmptyException (),                      306 },
            
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
            { new FtpCredentialsMissingException (),             809 },
        };

        #endregion

        #region Attributes

        private static Errors instance;
        
        //private Global global;
        //private Logger logger;

        private Logger logger;
        private Dictionary<int,Error> errors;
        private List<Error> errorsToLog;
        private Error lastError;
        private Error[] xmlErrors;

        public static Error configError;
        
        public static string lastErrorLogGenerated;

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
            }
            // .NET exception
            else
            {
                error = ( Error )this.TryToTranslateDotNet ( e ).Clone ();
                error.Port  = portIndex;
                error.Exception = e;
            }
            
            return error;
        }

        /// <summary>
        /// Register a new error to be written into the log after be recovered using GetErrorsToLog
        /// </summary>
        /// <returns><c>true</c>, if error was added, <c>false</c> otherwise.</returns>
        /// <param name="id">Error identifier</param>
        private bool AddErrorById (
            int id,
            Exception e,
            int portIndex = 1 )
        {
            // Error ID exists and is not registered already
            if ( this[ id ] != null )
            {
                Error error = this.GetErrorById ( id, e, portIndex );
                this.errorsToLog.Add ( error );
                
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Register .NET errors trying to found some error that match
        /// </summary>
        /// <param name="e">.NET exception</param>
        private Error AddErrorByException (
            Exception e,
            int portIndex = 1 )
        {
            Error error = this.GetErrorByException ( e, portIndex );
            this.errorsToLog.Add ( error );
            
            return ( this.lastError = this.errorsToLog.Last () );
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
                this._ClearList ();
            
            return errors;
        }
        
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
        
        /// <summary>
        /// Write an error into the log right now, without have to invoke AddError method
        /// Usually used outside actions logic, for example trying to detect and connect with a puck
        /// </summary>
        /// <param name="e">Exception launched</param>
        private void _LogErrorNow (
            Exception e,
            int portIndex )
        {
            Error error = this.AddErrorByException ( e, portIndex );
            PageLinker.ShowAlert ( ERROR_TITLE, error );
            
            lastErrorLogGenerated = this.logger.Error ();
        }
        
        /// <summary>
        /// Write errors registered using AddError method
        /// Usually used when performing an action
        /// </summary>
        private void _LogRegisteredErrors (
            bool forceException,
            Exception e )
        {
            if ( this.errorsToLog.Count > 0 )
            {
                //Error lastError = ( Error )this.errorsToLog[ this.errorsToLog.Count - 1 ].Clone ();
                Exception lastException = ( e != null ) ? e : this.errorsToLog[ this.errorsToLog.Count - 1 ].Exception;
                
                if ( forceException )
                    PageLinker.ShowAlert ( ERROR_TITLE, this.lastError );
                
                lastErrorLogGenerated = this.logger.Error ();

                if ( forceException )
                    throw lastException;
            }
        }

        private void _ShowErrorAndKill (
            Exception e,
            int portIndex = 1 )
        {
            Error error = this.AddErrorByException ( e, portIndex );
            PageLinker.ShowAlert ( ERROR_TITLE, error, true );
        
            lastErrorLogGenerated = this.logger.Error ();
        }

        /// <summary>
        /// Clears the list of registered errors
        /// Usually no need to use this method because is already used by default for GetErrorsToLog
        /// </summary>
        private void _ClearList ()
        {
            this.errorsToLog.Clear ();
        }

        private bool IsLastExceptionUsed (
            Exception e )
        {
            return ( this.lastError != null &&
                     this.lastError.Exception == e );
        }

        private static void LaunchException (
            Exception e,
            bool forceException )
        {
            if ( forceException )
                throw e;
        }

        #endregion

        #region Direct Singleton

        public static Error[] GetErrorsToLog (
            bool clearList = true )
        {
            return Errors.GetInstance ()._GetErrorsToLog ( clearList );
        }
        
        public static void AddError (
            Exception e,
            int portIndex = 1 )
        {
            Errors.GetInstance ().AddErrorByException ( e, portIndex );
        }
        
        public static void LogErrorNow (
            Exception e,
            int portIndex = -1,
            bool forceException = true )
        {
            if ( ! IsOwnException ( e ) )
                 Errors.GetInstance ()._LogErrorNow ( e, portIndex );
            else Errors.GetInstance ()._LogErrorNow ( e, ( ( portIndex > -1 ) ? portIndex : ( ( OwnExceptionsBase )e ).Port ) );
            
            Errors.LaunchException ( e, forceException );
        }
        
        /// <summary>
        /// Only log registered errors and shows error message/pop-up of the last,
        /// without launching the exception, allowing to continue executing process logic
        /// </summary>
        /// <param name="e">Exception that represents the last error happened</param>
        /// <param name="portindex">Index of MTU port associated to the error</param>
        public static void LogErrorNowAndContinue (
            Exception e,
            int portindex = -1 )
        {
            LogErrorNow ( e, portindex, false );
        }

        public static void LogRegisteredErrors (
            bool forceException = false,
            Exception e = null )
        {
            Errors.GetInstance ()._LogRegisteredErrors ( forceException, e );
        }

        /// <summary>
        /// Both options will log all registered exceptions that remain, but in
        /// the first case, previously the last exception launched will be added
        /// </summary>
        /// <param name="e">Exception</param>
        public static void LogRemainExceptions (
            Exception e )
        {
            // Last exception was not added yet
            if ( ! Errors.GetInstance ().IsLastExceptionUsed ( e ) )
                Errors.LogErrorNow ( e );
            
            // Last exception was already added
            else
                Errors.LogRegisteredErrors (); // ! ( e is OwnExceptionsBase ) );
        }
        
        public static bool IsOwnException (
            Exception e )
        {
            return ( e.GetType ().IsSubclassOf ( typeof( OwnExceptionsBase ) ) );
        }

        public static void ShowErrorAndKill (
            Exception e )
        {
            Errors.GetInstance ()._ShowErrorAndKill ( e );
        }

        #endregion
    }
}
