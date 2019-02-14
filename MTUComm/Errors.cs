using System.Collections.Generic;
using Xml;
using System;
using System.Linq;
using System.Dynamic;
using MTUComm.Exceptions;

namespace MTUComm
{
    public sealed class Errors
    {
        #region Constants

        private Dictionary<Exception,int> ex2id = 
        new Dictionary<Exception,int> ()
        {
            // Mtu can't be recovered using MTU type
            { new MtuTypeIsNotFoundException (), 123 },
            // MTU is not the same as at the beginning of the process
            { new MtuHasChangeBeforeFinishActionException (), 180 },
            // Puck can't read from MTU
            { new PuckCantReadFromMtuException (), 165 },
            // Puck can't read from MTU after has completed writing process
            { new PuckCantReadFromMtuAfterWritingException (), 166 },
            // Puck can't communicate with MTU performing the turn off
            { new PuckCantComWithMtuTurnOffException (), 163 },
            // Turn off MTU process has failed trying to activated the Ship Bit
            { new AttemptNotAchievedTurnOffException (), 162 },
            // MTU can not be turned off after having tried it several times
            { new ActionNotAchievedTurnOffException (), 160 },
            // Alarm profile selected for current MTU is not defined in the Alarm.xml file
            { new ScriptingAlarmForCurrentMtuException (), 200 },
            // The  alarm profile "Scripting" for current MTU is not defined in the Alarm.xml file
            { new SelectedAlarmForCurrentMtuException (), 201 },
            // The Meter.xml does not contain the Meter type specified with tags NumberOfDials, DriveDialSize and UnitOfMeasure
            { new ScriptingAutoDetectMeterException (), 100 },
            // The script does not contain the tag NumberOfDials needed to select the MTU
            { new NumberOfDialsTagMissingScript (), 101 },
            // The script does not contain the tag DriveDialSize needed to select the MTU
            { new DriveDialSizeTagMissingScript (), 102 },
            // The script does not contain the tag UnitOfMeasure needed to select the MTU
            { new UnitOfMeasureTagMissingScript (), 103 },
            // Error translatin parameters from script/trigger file
            { new TranslatingParamsScriptException (), 113 }
        };

        #endregion

        #region Attributes

        private static Errors instance;
        
        private Global global;
        private Logger logger;

        private Dictionary<int,Error> errors;
        private List<Error> errorsToLog;
        
        private Exception lastRegistered;

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
                return Errors.GetInstance ().global.ErrorId;
            }
        }

        #endregion

        #region Initialization

        private Errors ()
        {
            Configuration config = Configuration.GetInstance ();
        
            this.global      = config.global;
            this.logger      = new Logger ( config );
            this.errors      = new Dictionary<int,Error> ();
            this.errorsToLog = new List<Error> ();
        
            Error[] errorsXml = config.errors;

            if ( errorsXml != null )
                foreach ( Error errorXml in errorsXml )
                    this.errors.Add ( errorXml.Id, errorXml );
                    
            config = null;
        }
        
        private static Errors GetInstance ()
        {
            if ( Errors.instance == null )
                Errors.instance = new Errors ();
            
            return Errors.instance;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Register a new error to be written into the log after be recovered using GetErrorsToLog
        /// </summary>
        /// <returns><c>true</c>, if error was added, <c>false</c> otherwise.</returns>
        /// <param name="id">Error identifier</param>
        private bool AddErrorById (
            int id,
            Exception e,
            int portIndex )
        {
            // Error ID exists and is not registered already
            if ( this[ id ] != null )
            {
                Error error = ( Error )this[ id ].Clone ();
                error.Port  = portIndex;
                error.Exception = e;
            
                this.errorsToLog.Add ( error );
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Register .NET errors trying to found some error that match
        /// </summary>
        /// <param name="e">.NET exception</param>
        private void AddErrorByException (
            Exception e,
            int portIndex )
        {
            Type typeExp = e.GetType ();
            
            // Own exception
            if ( this.ex2id.Any ( item => item.Key.GetType () == typeExp ) )
            {
                int id = this.ex2id.Single ( item => item.Key.GetType () == typeExp ).Value;
                
                this.AddErrorById ( id, e, portIndex );
            }
            // .NET exception
            else
            {
                Error error = ( Error )this.TryToTranslateDotNet ( e ).Clone ();
                error.Port  = portIndex;
                error.Exception = e;
            
                this.errorsToLog.Add ( error );
            }
                
            this.lastRegistered = e;
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
            this.AddErrorByException ( e, portIndex );
            this.logger.LogError ();
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
                
                this.logger.LogError ();

                if ( forceException )
                    throw lastException;
            }
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
            return ( this.lastRegistered != null &&
                     this.lastRegistered == e );
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
            int portIndex = 1,
            bool forceException = true )
        {
            Errors.GetInstance ()._LogErrorNow ( e, portIndex );
            Errors.LaunchException ( e, forceException );
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
                Errors.LogRegisteredErrors ();
        }
        
        public static bool IsOwnException (
            Exception e )
        {
            return ( e.GetType () == typeof( OwnExceptionsBase ) );
        }

        #endregion
    }
}
