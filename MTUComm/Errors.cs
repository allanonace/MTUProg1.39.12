using System.Collections.Generic;
using Xml;
using System;
using System.Linq;
using System.Dynamic;

namespace MTUComm
{
    public sealed class Errors
    {
        #region Attributes

        private static Errors instance;
        
        private Global global;
        private Logger logger;

        private Dictionary<int,Error> errors;
        private List<Error> errorsToLog;

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
        private bool _AddError (
            int id )
        {
            // Error ID exists and is not registered already
            if ( this[ id ] != null )
            {
                this.errorsToLog.Add ( this[ id ] );
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Register .NET errors trying to found some error that match
        /// </summary>
        /// <param name="e">.NET exception</param>
        private void _AddError (
            Exception e )
        {
            this.errorsToLog.Add ( this.TryToTranslate ( e ) );
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
        
        private Error TryToTranslate (
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
        /// <param name="id">Error identifier</param>
        private void _LogErrorNow (
            int id )
        {
            if ( this._AddError ( id ) )
                this.logger.LogError ();
        }
        
        private void _LogErrorNow (
            Exception e )
        {
            this._AddError ( e );
            this.logger.LogError ();
        }
        
        /// <summary>
        /// Write errors registered using AddError method
        /// Usually used when performing an action
        /// </summary>
        private void _LogRegisteredErrors ()
        {
            if ( this.errorsToLog.Count > 0 )
                this.logger.LogError ();
        }

        /// <summary>
        /// Clears the list of registered errors
        /// Usually no need to use this method because is already used by default for GetErrorsToLog
        /// </summary>
        private void _ClearList ()
        {
            this.errorsToLog.Clear ();
        }

        #endregion

        #region Direct Singleton

        public static Error[] GetErrorsToLog (
            bool clearList = true )
        {
            return Errors.GetInstance ()._GetErrorsToLog ( clearList );
        }

        public static void AddError (
            int id )
        {
            Errors.GetInstance ()._AddError ( id );
        }
        
        public static void AddError (
            Exception e )
        {
            Errors.GetInstance ()._AddError ( e );
        }

        public static void LogErrorNow (
            int id )
        {
            Errors.GetInstance ()._LogErrorNow ( id );
        }
        
        public static void LogErrorNow (
            Exception e )
        {
            Errors.GetInstance ()._LogErrorNow ( e );
        }
        
        public static void LogRegisteredErrors ()
        {
            Errors.GetInstance ()._LogRegisteredErrors ();
        }

        #endregion
    }
}
