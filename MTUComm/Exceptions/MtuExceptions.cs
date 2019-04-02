namespace MTUComm.Exceptions
{
    // MTU [ 1xx ]
    //----
    public class MtuHasChangeBeforeFinishActionException : OwnExceptionsBase
    {
        public MtuHasChangeBeforeFinishActionException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class PuckCantCommWithMtuException : OwnExceptionsBase
    {
        public PuckCantCommWithMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class PuckCantReadFromMtuAfterWritingException : OwnExceptionsBase
    {
        public PuckCantReadFromMtuAfterWritingException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MtuMissingException : OwnExceptionsBase
    {
        public MtuMissingException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    // Meter [ 2xx ]
    //------

    public class ScriptingAutoDetectMeterMissing : OwnExceptionsBase
    {
        public ScriptingAutoDetectMeterMissing ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptingAutoDetectNotSupportedException : OwnExceptionsBase
    {
        public ScriptingAutoDetectNotSupportedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptingAutoDetectMeterException : OwnExceptionsBase
    {
        public ScriptingAutoDetectMeterException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class NumberOfDialsTagMissingScript : OwnExceptionsBase
    {
        public NumberOfDialsTagMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class DriveDialSizeTagMissingScript : OwnExceptionsBase
    {
        public DriveDialSizeTagMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class UnitOfMeasureTagMissingScript : OwnExceptionsBase
    {
        public UnitOfMeasureTagMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptingAutoDetectTagsMissingScript : OwnExceptionsBase
    {
        public ScriptingAutoDetectTagsMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    // Scripting Parameters [ 3xx ]
    //---------------------
    
    public class ProcessingParamsScriptException : OwnExceptionsBase
    {
        public ProcessingParamsScriptException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptForOnePortButTwoEnabledException : OwnExceptionsBase
    {
        public ScriptForOnePortButTwoEnabledException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptForTwoPortsButMtuOnlyOneException : OwnExceptionsBase
    {
        public ScriptForTwoPortsButMtuOnlyOneException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptLogfileInvalidException : OwnExceptionsBase
    {
        public ScriptLogfileInvalidException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptActionTypeInvalidException : OwnExceptionsBase
    {
        public ScriptActionTypeInvalidException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptWrongStructureException : OwnExceptionsBase
    {
        public ScriptWrongStructureException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ScriptEmptyException : OwnExceptionsBase
    {
        public ScriptEmptyException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MandatoryMeterSerialHiddenScriptException : OwnExceptionsBase
    {
        public MandatoryMeterSerialHiddenScriptException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class SameParameterRepeatScriptException : OwnExceptionsBase
    {
        public SameParameterRepeatScriptException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class ScriptUserNameMissingException : OwnExceptionsBase
    {
        public ScriptUserNameMissingException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
    }


    // Alarm [ 4xx ]
    //------

    public class ScriptingAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public ScriptingAlarmForCurrentMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class SelectedAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public SelectedAlarmForCurrentMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    // Turn Off [ 5xx ]
    //---------
    
    public class AttemptNotAchievedTurnOffException : OwnExceptionsBase
    {
        public AttemptNotAchievedTurnOffException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ActionNotAchievedTurnOffException : OwnExceptionsBase
    {
        public ActionNotAchievedTurnOffException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    // Install Confirmation [ 6xx ]
    //---------------------
    
    public class MtuIsNotTwowayICException : OwnExceptionsBase
    {
        public MtuIsNotTwowayICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MtuIsAlreadyTurnedOffICException : OwnExceptionsBase
    {
        public MtuIsAlreadyTurnedOffICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class AttemptNotAchievedICException : OwnExceptionsBase
    {
        public AttemptNotAchievedICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ActionNotAchievedICException : OwnExceptionsBase
    {
        public ActionNotAchievedICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    // Encryption [ 7xx ]
    //-----------
    
    public class ActionNotAchievedEncryptionException : OwnExceptionsBase
    {
        public ActionNotAchievedEncryptionException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    // Configuration Files and System [ 8xx ]
    //-------------------------------
    
    public class ConfigurationFilesNotFoundException : OwnExceptionsBase
    {
        public ConfigurationFilesNotFoundException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ConfigurationFilesCorruptedException : OwnExceptionsBase
    {
        public ConfigurationFilesCorruptedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class CertificateFileNotValidException : OwnExceptionsBase
    {
        public CertificateFileNotValidException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class CertificateInstalledNotValidException : OwnExceptionsBase
    {
        public CertificateInstalledNotValidException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class CertificateInstalledExpiredException : OwnExceptionsBase
    {
        public CertificateInstalledExpiredException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class AndroidPermissionsException : OwnExceptionsBase
    {
        public AndroidPermissionsException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class FtpDownloadException : OwnExceptionsBase
    {
        public FtpDownloadException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class NoInternetException : OwnExceptionsBase
    {
        public NoInternetException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class DeviceMinDateAllowedException : OwnExceptionsBase
    {
        public DeviceMinDateAllowedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class FtpCredentialsMissingException : OwnExceptionsBase
    {
        public FtpCredentialsMissingException ( string message = "", int port = 1, string messagePopup = "" ) : base( message, port, messagePopup ) { }
    }
}
