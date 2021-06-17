using System.ComponentModel;

namespace HACS.WPF.ViewModels
{
	public class SerialController : StateManager
	{
		[Browsable(false)]
		public new Components.ISerialController Component
		{
			get => base.Component as Components.ISerialController;
			protected set => base.Component = value;
		}
		public Utilities.SerialDevice SerialDevice => Component.SerialDevice;
		public bool LogCommands { get => Component.LogCommands; set => Component.LogCommands = value; }
		public bool LogResponses { get => Component.LogResponses; set => Component.LogResponses = value; }
		public bool TokenizeCommands { get => Component.TokenizeCommands; set => Component.TokenizeCommands = value; }
		public bool IgnoreUnexpectedResponses { get => Component.IgnoreUnexpectedResponses; set => Component.IgnoreUnexpectedResponses = value; }
		public int ResponseTimeout { get => Component.ResponseTimeout; set => Component.ResponseTimeout = value; }
		public bool Responsive => Component.Responsive;
		public int TooManyResponseTimeouts { get => Component.TooManyResponseTimeouts; set => Component.TooManyResponseTimeouts = value; }
		public bool Idle => Component.Idle;
		public bool Free => Component.Free;
		public uint CommandCount => Component.CommandCount;
		public uint ResponseCount => Component.ResponseCount;
		public bool Hurry { get => Component.Hurry; set => Component.Hurry = value; }
		public string ServiceCommand => Component.ServiceCommand;
		public string CommandMessage => Component.CommandMessage;
		public int ResponseTimeouts => Component.ResponseTimeouts;
		public string Response => Component.Response;
	}
}
