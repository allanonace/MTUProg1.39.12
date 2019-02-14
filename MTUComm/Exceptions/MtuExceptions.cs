using System;

namespace MTUComm.Exceptions
{
    public class MtuTypeIsNotFoundException : OwnExceptionsBase
    {
        public MtuTypeIsNotFoundException () { }
        public MtuTypeIsNotFoundException ( string message ) : base ( message ) { }
    }
    
    public class MtuHasChangeBeforeFinishActionException : OwnExceptionsBase
    {
        public MtuHasChangeBeforeFinishActionException () { }
        public MtuHasChangeBeforeFinishActionException ( string message ) : base ( message ) { }
    }
    
    public class PuckCantReadFromMtuException : OwnExceptionsBase
    {
        public PuckCantReadFromMtuException () { }
        public PuckCantReadFromMtuException ( string message ) : base ( message ) { }
    }
    
    public class PuckCantReadFromMtuAfterWritingException : OwnExceptionsBase
    {
        public PuckCantReadFromMtuAfterWritingException () { }
        public PuckCantReadFromMtuAfterWritingException ( string message ) : base ( message ) { }
    }
    
    public class PuckCantComWithMtuTurnOffException : OwnExceptionsBase
    {
        public PuckCantComWithMtuTurnOffException () { }
        public PuckCantComWithMtuTurnOffException ( string message ) : base ( message ) { }
    }
    
    public class AttemptNotAchievedTurnOffException : OwnExceptionsBase
    {
        public AttemptNotAchievedTurnOffException () { }
        public AttemptNotAchievedTurnOffException ( string message ) : base ( message ) { }
    }
    
    public class ActionNotAchievedTurnOffException : OwnExceptionsBase
    {
        public ActionNotAchievedTurnOffException () { }
        public ActionNotAchievedTurnOffException ( string message ) : base ( message ) { }
    }
    
    public class ScriptingAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public ScriptingAlarmForCurrentMtuException () { }
        public ScriptingAlarmForCurrentMtuException ( string message ) : base ( message ) { }
    }
    
    public class SelectedAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public SelectedAlarmForCurrentMtuException () { }
        public SelectedAlarmForCurrentMtuException ( string message ) : base ( message ) { }
    }
    
    public class NumberOfDialsTagMissingScript : OwnExceptionsBase
    {
        public NumberOfDialsTagMissingScript () { }
        public NumberOfDialsTagMissingScript ( string message ) : base ( message ) { }
    }
    
    public class DriveDialSizeTagMissingScript : OwnExceptionsBase
    {
        public DriveDialSizeTagMissingScript () { }
        public DriveDialSizeTagMissingScript ( string message ) : base ( message ) { }
    }
    
    public class UnitOfMeasureTagMissingScript : OwnExceptionsBase
    {
        public UnitOfMeasureTagMissingScript () { }
        public UnitOfMeasureTagMissingScript ( string message ) : base ( message ) { }
    }
    
    public class ScriptingAutoDetectMeterException : OwnExceptionsBase
    {
        public ScriptingAutoDetectMeterException () { }
        public ScriptingAutoDetectMeterException ( string message ) : base ( message ) { }
    }
    
    public class TranslatingParamsScriptException : OwnExceptionsBase
    {
        public TranslatingParamsScriptException () { }
        public TranslatingParamsScriptException ( string message ) : base ( message ) { }
    }
}
