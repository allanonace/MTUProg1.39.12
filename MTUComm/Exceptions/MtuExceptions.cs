using System;

namespace MTUComm.Exceptions
{
    public class MtuTypeIsNotFoundException : OwnExceptionsBase
    {
        public MtuTypeIsNotFoundException () { }
        public MtuTypeIsNotFoundException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class MtuNotSupportMeterException : OwnExceptionsBase
    {
        public MtuNotSupportMeterException () { }
        public MtuNotSupportMeterException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class MtuHasChangeBeforeFinishActionException : OwnExceptionsBase
    {
        public MtuHasChangeBeforeFinishActionException () { }
        public MtuHasChangeBeforeFinishActionException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class PuckCantCommWithMtuException : OwnExceptionsBase
    {
        public PuckCantCommWithMtuException () { }
        public PuckCantCommWithMtuException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class PuckCantReadFromMtuAfterWritingException : OwnExceptionsBase
    {
        public PuckCantReadFromMtuAfterWritingException () { }
        public PuckCantReadFromMtuAfterWritingException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class AttemptNotAchievedTurnOffException : OwnExceptionsBase
    {
        public AttemptNotAchievedTurnOffException () { }
        public AttemptNotAchievedTurnOffException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ActionNotAchievedTurnOffException : OwnExceptionsBase
    {
        public ActionNotAchievedTurnOffException () { }
        public ActionNotAchievedTurnOffException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptingAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public ScriptingAlarmForCurrentMtuException () { }
        public ScriptingAlarmForCurrentMtuException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class SelectedAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public SelectedAlarmForCurrentMtuException () { }
        public SelectedAlarmForCurrentMtuException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class NumberOfDialsTagMissingScript : OwnExceptionsBase
    {
        public NumberOfDialsTagMissingScript () { }
        public NumberOfDialsTagMissingScript ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class DriveDialSizeTagMissingScript : OwnExceptionsBase
    {
        public DriveDialSizeTagMissingScript () { }
        public DriveDialSizeTagMissingScript ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class UnitOfMeasureTagMissingScript : OwnExceptionsBase
    {
        public UnitOfMeasureTagMissingScript () { }
        public UnitOfMeasureTagMissingScript ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptingAutoDetectMeterException : OwnExceptionsBase
    {
        public ScriptingAutoDetectMeterException () { }
        public ScriptingAutoDetectMeterException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ProcessingParamsScriptException : OwnExceptionsBase
    {
        public ProcessingParamsScriptException () { }
        public ProcessingParamsScriptException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptForOnePortButTwoEnabledException : OwnExceptionsBase
    {
        public ScriptForOnePortButTwoEnabledException () { }
        public ScriptForOnePortButTwoEnabledException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptForTwoPortsButMtuOnlyOneException : OwnExceptionsBase
    {
        public ScriptForTwoPortsButMtuOnlyOneException () { }
        public ScriptForTwoPortsButMtuOnlyOneException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptLogfileInvalidException : OwnExceptionsBase
    {
        public ScriptLogfileInvalidException () { }
        public ScriptLogfileInvalidException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptActionTypeInvalidException : OwnExceptionsBase
    {
        public ScriptActionTypeInvalidException () { }
        public ScriptActionTypeInvalidException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptEmptyException : OwnExceptionsBase
    {
        public ScriptEmptyException () { }
        public ScriptEmptyException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ScriptWrongStructureException : OwnExceptionsBase
    {
        public ScriptWrongStructureException () { }
        public ScriptWrongStructureException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class MtuIsNotTwowayICException : OwnExceptionsBase
    {
        public MtuIsNotTwowayICException () { }
        public MtuIsNotTwowayICException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class MtuIsAlreadyTurnedOffICException : OwnExceptionsBase
    {
        public MtuIsAlreadyTurnedOffICException () { }
        public MtuIsAlreadyTurnedOffICException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class ActionNotAchievedICException : OwnExceptionsBase
    {
        public ActionNotAchievedICException () { }
        public ActionNotAchievedICException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    public class AttemptNotAchievedICException : OwnExceptionsBase
    {
        public AttemptNotAchievedICException () { }
        public AttemptNotAchievedICException ( string message, int port = 1 ) : base ( message, port ) { }
    }
    
    // Encryption [ 7xx ]
    
    public class ActionNotAchievedEncryptionException : OwnExceptionsBase
    {
        public ActionNotAchievedEncryptionException () { }
        public ActionNotAchievedEncryptionException ( string message, int port = 1 ) : base ( message, port ) { }
    }
}
