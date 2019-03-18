namespace MTUComm.Exceptions
{
    // MTU [ 1xx ]
    //----
    public class MtuHasChangeBeforeFinishActionException : OwnExceptionsBase
    {
        public MtuHasChangeBeforeFinishActionException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class PuckCantCommWithMtuException : OwnExceptionsBase
    {
        public PuckCantCommWithMtuException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class PuckCantReadFromMtuAfterWritingException : OwnExceptionsBase
    {
        public PuckCantReadFromMtuAfterWritingException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    // Meter [ 2xx ]
    //------

    public class MtuTypeIsNotFoundException : OwnExceptionsBase
    {
        public MtuTypeIsNotFoundException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class MtuNotSupportMeterException : OwnExceptionsBase
    {
        public MtuNotSupportMeterException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptingAutoDetectMeterException : OwnExceptionsBase
    {
        public ScriptingAutoDetectMeterException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class NumberOfDialsTagMissingScript : OwnExceptionsBase
    {
        public NumberOfDialsTagMissingScript ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class DriveDialSizeTagMissingScript : OwnExceptionsBase
    {
        public DriveDialSizeTagMissingScript ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class UnitOfMeasureTagMissingScript : OwnExceptionsBase
    {
        public UnitOfMeasureTagMissingScript ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    // Scripting Parameters [ 3xx ]
    //---------------------
    
    public class ProcessingParamsScriptException : OwnExceptionsBase
    {
        public ProcessingParamsScriptException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptForOnePortButTwoEnabledException : OwnExceptionsBase
    {
        public ScriptForOnePortButTwoEnabledException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptForTwoPortsButMtuOnlyOneException : OwnExceptionsBase
    {
        public ScriptForTwoPortsButMtuOnlyOneException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptLogfileInvalidException : OwnExceptionsBase
    {
        public ScriptLogfileInvalidException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptActionTypeInvalidException : OwnExceptionsBase
    {
        public ScriptActionTypeInvalidException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptWrongStructureException : OwnExceptionsBase
    {
        public ScriptWrongStructureException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptEmptyException : OwnExceptionsBase
    {
        public ScriptEmptyException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    // Alarm [ 4xx ]
    //------
    
    public class ScriptingAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public ScriptingAlarmForCurrentMtuException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class SelectedAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public SelectedAlarmForCurrentMtuException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    // Turn Off [ 5xx ]
    //---------
    
    public class AttemptNotAchievedTurnOffException : OwnExceptionsBase
    {
        public AttemptNotAchievedTurnOffException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ActionNotAchievedTurnOffException : OwnExceptionsBase
    {
        public ActionNotAchievedTurnOffException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    // Install Confirmation [ 6xx ]
    //---------------------
    
    public class MtuIsNotTwowayICException : OwnExceptionsBase
    {
        public MtuIsNotTwowayICException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class MtuIsAlreadyTurnedOffICException : OwnExceptionsBase
    {
        public MtuIsAlreadyTurnedOffICException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class AttemptNotAchievedICException : OwnExceptionsBase
    {
        public AttemptNotAchievedICException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ActionNotAchievedICException : OwnExceptionsBase
    {
        public ActionNotAchievedICException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    // Encryption [ 7xx ]
    //-----------
    
    public class ActionNotAchievedEncryptionException : OwnExceptionsBase
    {
        public ActionNotAchievedEncryptionException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    // Configuration Files and System [ 8xx ]
    //-------------------------------
    
    public class ConfigurationFilesNotFoundException : OwnExceptionsBase
    {
        public ConfigurationFilesNotFoundException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class ConfigurationFilesCorruptedException : OwnExceptionsBase
    {
        public ConfigurationFilesCorruptedException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class CertificateFileNotValidException : OwnExceptionsBase
    {
        public CertificateFileNotValidException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class CertificateInstalledNotValidException : OwnExceptionsBase
    {
        public CertificateInstalledNotValidException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class CertificateInstalledExpiredException : OwnExceptionsBase
    {
        public CertificateInstalledExpiredException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class AndroidPermissionsException : OwnExceptionsBase
    {
        public AndroidPermissionsException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class FtpDownloadException : OwnExceptionsBase
    {
        public FtpDownloadException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class NoInternetException : OwnExceptionsBase
    {
        public NoInternetException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
    
    public class DeviceMinDateAllowedException : OwnExceptionsBase
    {
        public DeviceMinDateAllowedException ( string message = "", int port = 1 ) : base ( message, port ) { }
    }
}
