namespace Library.Exceptions
{
    #region MTU [ 1xx ]

    /// <summary>
    /// Exception thrown when the MTU detected is not the same
    /// as the one used at the beginning of the process.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.CheckIsTheSameMTU ()"/>.
    /// </para>
    /// </summary>
    public class MtuHasChangeBeforeFinishActionException : OwnExceptionsBase
    {
        public MtuHasChangeBeforeFinishActionException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the communication between the application/puck and the MTU fails
    /// but it is also used as generic error when some unknown error happens, because it is preferable
    /// to show this own exception to the user and not a rare .NET error.
    /// </summary>
    public class PuckCantCommWithMtuException : OwnExceptionsBase
    {
        public PuckCantCommWithMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class PuckCantReadFromMtuAfterWritingException : OwnExceptionsBase
    {
        public PuckCantReadFromMtuAfterWritingException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when it is not possible to recover an MTU entry by the specified ID.
    /// </summary>
    public class MtuMissingException : OwnExceptionsBase
    {
        public MtuMissingException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class MtuQueryEventLogsException : OwnExceptionsBase
    {
        public MtuQueryEventLogsException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when an attempt to recover a DataRead event log from the MTU fails but
    /// there are still attempts to retry the action, and for that reason the action is not over yet.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.DataRead ()"/>.
    /// </para>
    /// </summary>
    public class AttemptNotAchievedGetEventsLogException : OwnExceptionsBase
    {
        public AttemptNotAchievedGetEventsLogException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when an attempt to recover a DataRead event log from the MTU fails,
    /// due the MTU for som reason is busy and it can't serve the information, but there are
    /// still attempts to retry the action, and for that reason the action is not over yet.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.DataRead ()"/>.
    /// </para>
    /// </summary>
    public class MtuIsBusyToGetEventsLogException : OwnExceptionsBase
    {
        public MtuIsBusyToGetEventsLogException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when the DataRead process fails, not recovering all the MeterRead
    /// events stored in the MTU for the selected dates and not understanding as partially
    /// successfull the process when some events are recovered, only the full stack is desirable.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.DataRead ()"/>.
    /// </para>
    /// </summary>
    public class ActionNotAchievedGetEventsLogException : OwnExceptionsBase
    {
        public ActionNotAchievedGetEventsLogException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when the OnDemand tag has not been set within the MTU entry
    /// in the MTU.xml file ( by default false ) or it is set to false, that indicates
    /// the MTU is not an OnDemand compatible device.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.DataRead ()"/>.
    /// </para>
    /// </summary>
    public class MtuIsNotOnDemandCompatibleDevice : OwnExceptionsBase
    {
        public MtuIsNotOnDemandCompatibleDevice ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    #endregion

    #region Meter [ 2xx ]

    /// <summary>
    /// Exception thrown when the Meter ID specified in the script file
    /// does not match any Meter in the Meter.xml file.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class ScriptingAutoDetectMeterMissing : OwnExceptionsBase
    {
        public ScriptingAutoDetectMeterMissing ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the selected Meter is not supported by the MTU port, due the
    /// type ( Pulse, Encoder or Ecoder ) or when only certain specific Meter IDs are allowed.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class ScriptingAutoDetectNotSupportedException : OwnExceptionsBase
    {
        public ScriptingAutoDetectNotSupportedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the parameters NumberOfDials, DriveDialSize
    /// and UnitOfMeasure specified in the script do not match any Meter.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class ScriptingAutoDetectMeterException : OwnExceptionsBase
    {
        public ScriptingAutoDetectMeterException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the parameter NumberOfDials is not present in the script.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class NumberOfDialsTagMissingScript : OwnExceptionsBase
    {
        public NumberOfDialsTagMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the parameter DriveDialSize is not present in the script.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class DriveDialSizeTagMissingScript : OwnExceptionsBase
    {
        public DriveDialSizeTagMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the parameter UnitOfMeasure is not present in the script.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class UnitOfMeasureTagMissingScript : OwnExceptionsBase
    {
        public UnitOfMeasureTagMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the script does not contain some of the required tags
    /// ( NumberOfDials, DriveDialSize and UnitOfMeasure ) to auto-detect the Meter.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class ScriptingAutoDetectTagsMissingScript : OwnExceptionsBase
    {
        public ScriptingAutoDetectTagsMissingScript ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the Meter
    /// type selected is not compatible with the Meter connected to the MTU.
    /// <para>
    /// The error code generated is 0xFE = "No reading / No response from Encoder".
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.CheckSelectedEncoderMeter ()"/>.
    /// </para>
    /// </summary>
    public class EncoderMeterFFException : OwnExceptionsBase
    {
        public EncoderMeterFFException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the Meter
    /// type selected is not compatible with the Meter connected to the MTU.
    /// <para>
    /// The error code generated is 0xFE = "Encoder has bad digit in reading".
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.CheckSelectedEncoderMeter ()"/>.
    /// </para>
    /// </summary>
    public class EncoderMeterFEException : OwnExceptionsBase
    {
        public EncoderMeterFEException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the Meter
    /// type selected is not compatible with the Meter connected to the MTU.
    /// <para>
    /// The error code generated is 0xFD = "Delta overflow".
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.CheckSelectedEncoderMeter ()"/>.
    /// </para>
    /// </summary>
    public class EncoderMeterFDException : OwnExceptionsBase
    {
        public EncoderMeterFDException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the Meter
    /// type selected is not compatible with the Meter connected to the MTU.
    /// <para>
    /// The error code generated is 0xFC = "Deltas purged / New install / Reset".
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.CheckSelectedEncoderMeter ()"/>.
    /// </para>
    /// </summary>
    public class EncoderMeterFCException : OwnExceptionsBase
    {
        public EncoderMeterFCException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the Meter
    /// type selected is not compatible with the Meter connected to the MTU.
    /// <para>
    /// The error code generated is 0xFB = "Encoder clock shorted".
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.CheckSelectedEncoderMeter ()"/>.
    /// </para>
    /// </summary>
    public class EncoderMeterFBException : OwnExceptionsBase
    {
        public EncoderMeterFBException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the Meter
    /// type selected is not compatible with the Meter connected to the MTU.
    /// <para>
    /// The error code generated is unknown.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.CheckSelectedEncoderMeter ()"/>.
    /// </para>
    /// </summary>
    public class EncoderMeterUnknownException : OwnExceptionsBase
    {
        public EncoderMeterUnknownException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the
    /// Meter type auto-detection process fails.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AutodetectMeterEcoders ()"/>.
    /// </para>
    /// </summary>
    public class EncoderAutodetectNotAchievedException : OwnExceptionsBase
    {
        public EncoderAutodetectNotAchievedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown working with Encoders or Ecoders when the Meter type
    /// auto-detection process fails, but due to an exception that is not own ( .Net ).
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AutodetectMeterEcoders ()"/>.
    /// </para>
    /// </summary>
    public class EncoderAutodetectException : OwnExceptionsBase
    {
        public EncoderAutodetectException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class RDDDesiredStatusIsUnknown : OwnExceptionsBase
    {
        public RDDDesiredStatusIsUnknown ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class RDDStatusIsDisabled : OwnExceptionsBase
    {
        public RDDStatusIsDisabled ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class RDDStatusIsNotBusyAfterLExICommand : OwnExceptionsBase
    {
        public RDDStatusIsNotBusyAfterLExICommand ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class RDDContinueInTransitionAfterMaxTime : OwnExceptionsBase
    {
        public RDDContinueInTransitionAfterMaxTime ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class RDDStatusIsUnknownAfterMaxTime : OwnExceptionsBase
    {
        public RDDStatusIsUnknownAfterMaxTime ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class RDDStatusIsDifferentThanExpected : OwnExceptionsBase
    {
        public RDDStatusIsDifferentThanExpected ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
  
    #endregion

    #region Scripting Parameters [ 3xx ]

    /// <summary>
    /// Exception thrown when some of the tags included in the
    /// script file has not passed the validation process.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class ProcessingParamsScriptException : OwnExceptionsBase
    {
        public ProcessingParamsScriptException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the script used is for only one port
    /// but the MTU has two ports and the second port is enabled.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
    public class ScriptForOnePortButTwoEnabledException : OwnExceptionsBase
    {
        public ScriptForOnePortButTwoEnabledException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the script used is for two ports
    /// but the MTU has not second port or it is disabled.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.ScriptAux.ValidateParams ()"/>.
    /// </para>
    /// </summary>
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

    public class ScriptingTagMissingException : OwnExceptionsBase
    {
        public ScriptingTagMissingException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
    }

    #endregion

    #region Alarm [ 4xx ]

    /// <summary>
    /// Exception thrown in scripted mode when the MTU entry in Mtu.xml file
    /// has the tag RequiresAlarmProfile set to true, but in Alarm.xml file
    /// there is no an "Scripting" entry for the MTU ID.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// </summary>
    public class ScriptingAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public ScriptingAlarmForCurrentMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when an alarm profile has not been selected before to start with the new installation.
    /// </summary>
    public class SelectedAlarmForCurrentMtuException : OwnExceptionsBase
    {
        public SelectedAlarmForCurrentMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    #endregion
    
    #region Demand [ 45x ]

    /// <summary>
    /// Exception thrown in scripted mode when in DemandConf.xml
    /// file there is no an "Scripting" entry for the MTU ID.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.AddMtu ( Action action )"/>.
    /// </para>
    /// </summary>
    public class ScriptingDemandForCurrentMtuException : OwnExceptionsBase
    {
        public ScriptingDemandForCurrentMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when an demand profile has not been selected before to start with the new installation.
    /// </summary>
    public class SelectedDemandForCurrentMtuException : OwnExceptionsBase
    {
        public SelectedDemandForCurrentMtuException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    #endregion

    #region Turn Off [ 5xx ]
    
    /// <summary>
    /// Exception thrown when an attempt to switch off the MTU but there are still
    /// attempts to retry the action, and for that reason the action is not over yet.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.TurnOnOffMtu_Logic ()"/>.
    /// </para>
    /// </summary>
    public class AttemptNotAchievedTurnOffException : OwnExceptionsBase
    {
        public AttemptNotAchievedTurnOffException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the MTU shutdown fails.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.TurnOnOffMtu_Logic ()"/>.
    /// </para>
    /// </summary>
    public class ActionNotAchievedTurnOffException : OwnExceptionsBase
    {
        public ActionNotAchievedTurnOffException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    #endregion
    
    #region Install Confirmation [ 6xx ]
    
    /// <summary>
    /// Exception thrown when the MTU does not support two-way communication,
    /// which does not allow the Install Confirmation process.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.InstallConfirmation_Logic ()"/>.
    /// </para>
    /// </summary>
    public class MtuIsNotTwowayICException : OwnExceptionsBase
    {
        public MtuIsNotTwowayICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when trying to execute an Install
    /// Confirmation process but the MTU is already turned off.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.InstallConfirmation_Logic ()"/>.
    /// </para>
    /// </summary>
    public class MtuIsAlreadyTurnedOffICException : OwnExceptionsBase
    {
        public MtuIsAlreadyTurnedOffICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when an attempt to perform an Install Confirmation between the MTU and a DCU
    /// but there are still attempts to retry the action, and for that reason the action is not over yet.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.InstallConfirmation_Logic ()"/>.
    /// </para>
    /// </summary>
    public class AttemptNotAchievedICException : OwnExceptionsBase
    {
        public AttemptNotAchievedICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when the Install Confirmation process fails, not getting to
    /// update the "InstallConfirmationNotSynced" flag in the memory register to false.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.InstallConfirmation_Logic ()"/>.
    /// </para>
    /// </summary>
    public class ActionNotAchievedICException : OwnExceptionsBase
    {
        public ActionNotAchievedICException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class NodeDiscoveryNotInitializedException : OwnExceptionsBase
    {
        public NodeDiscoveryNotInitializedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class NodeDiscoveryNotStartedException : OwnExceptionsBase
    {
        public NodeDiscoveryNotStartedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class AttemptNotAchievedNodeDiscoveryException : OwnExceptionsBase
    {
        public AttemptNotAchievedNodeDiscoveryException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class ActionNotAchievedNodeDiscoveryException : OwnExceptionsBase
    {
        public ActionNotAchievedNodeDiscoveryException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    #endregion

    #region Encryption [ 7xx ]

    /// <summary>
    /// Exception thrown when the Encryption of the MTU fails during an installation.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.Encrypt_Old ( dynamic )"/>.
    /// </para>
    /// </summary>
    public class ActionNotAchievedEncryptionException : OwnExceptionsBase
    {
        public ActionNotAchievedEncryptionException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when the public key used during the new encryption process
    /// for OnDemand 1.2 MTUs is not set in global.xml file, using the PublicKey tag.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.Encrypt_OD12 ( dynamic )"/>.
    /// </para>
    /// </summary>
    public class ODEncryptionPublicKeyNotSetException : OwnExceptionsBase
    {
        public ODEncryptionPublicKeyNotSetException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when the public key used during the new encryption process for
    /// OnDemand 1.2 MTUs has not the correct format ( in base64 and more than 63 bytes ).
    /// <para>
    /// See <see cref="MTUComm.MTUComm.Encrypt_OD12 ( dynamic )"/>.
    /// </para>
    /// </summary>
    public class ODEncryptionPublicKeyFormatException : OwnExceptionsBase
    {
        public ODEncryptionPublicKeyFormatException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when the broadcast key used during the new encryption process
    /// for OnDemand 1.2 MTUs is not set in global.xml file, using the BroadcastSet tag.
    /// <para>
    /// See <see cref="MTUComm.MTUComm.Encrypt_OD12 ( dynamic )"/>.
    /// </para>
    /// </summary>
    public class ODEncryptionBroadcastKeyNotSetException : OwnExceptionsBase
    {
        public ODEncryptionBroadcastKeyNotSetException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when the broadcast key used during the new encryption process for
    /// OnDemand 1.2 MTUs has not the correct format ( in base64 and 32 bytes ).
    /// <para>
    /// See <see cref="MTUComm.MTUComm.Encrypt_OD12 ( dynamic )"/>.
    /// </para>
    /// </summary>
    public class ODEncryptionBroadcastKeyFormatException : OwnExceptionsBase
    {
        public ODEncryptionBroadcastKeyFormatException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when the encryption index has reached the register/byte limit ( 255 ).
    /// <para>
    /// See <see cref="MTUComm.MTUComm.Encrypt_Old ( dynamic )"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MTUComm.Encrypt_OD12 ( dynamic )"/>.
    /// </para>
    /// </summary>
    public class EncryptionIndexLimitReachedException : OwnExceptionsBase
    {
        public EncryptionIndexLimitReachedException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
    }

    #endregion

    #region Configuration Files and System [ 8xx ]

    public class ConfigurationFilesNotFoundException : OwnExceptionsBase
    {
        public ConfigurationFilesNotFoundException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class ConfigurationFilesCorruptedException : OwnExceptionsBase
    {
        public ConfigurationFilesCorruptedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class ConfigurationFilesNewVersionException : OwnExceptionsBase
    {
        public ConfigurationFilesNewVersionException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
    }

    public class IntuneCredentialsException : OwnExceptionsBase
    {
        public IntuneCredentialsException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
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
    
    public class FtpConnectionException : OwnExceptionsBase
    {
        public FtpConnectionException ( string message = "", int port = 1, string messagePopup = "" ) : base( message, port, messagePopup ) { }
    }
    
    public class FtpUpdateLogsException : OwnExceptionsBase
    {
        public FtpUpdateLogsException ( string message = "", int port = 1, string messagePopup = "" ) : base( message, port, messagePopup ) { }
    }

    public class ConfigFilesChangedException : OwnExceptionsBase
    {
        public ConfigFilesChangedException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
    }

    public class CameraException : OwnExceptionsBase
    {
        public CameraException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
    }
    
    public class GlobalChangedException : OwnExceptionsBase
    {
        public GlobalChangedException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    // Internal

    public class InterfaceNotFoundException_Internal : OwnExceptionsBase
    {
        public InterfaceNotFoundException_Internal ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class ActionInterfaceNotFoundException_Internal : OwnExceptionsBase
    {
        public ActionInterfaceNotFoundException_Internal ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class MtuNotFoundException_Internal : OwnExceptionsBase
    {
        public MtuNotFoundException_Internal ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MeterNotFoundException_Internal : OwnExceptionsBase
    {
        public MeterNotFoundException_Internal ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class AlarmNotFoundException_Internal : OwnExceptionsBase
    {
        public AlarmNotFoundException_Internal ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class DemandNotFoundException_Internal : OwnExceptionsBase
    {
        public DemandNotFoundException_Internal ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class PortTypeMissingMTUException : OwnExceptionsBase
    {
        public PortTypeMissingMTUException(string message = "", int port = 1, string messagePopup = "") : base(message, port, messagePopup) { }
    }
    #endregion
}
