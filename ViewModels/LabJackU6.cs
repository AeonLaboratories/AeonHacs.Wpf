using System.ComponentModel;
namespace HACS.WPF.ViewModels
{
	public class LabJackU6 : DeviceManager
	{
		[Browsable(false)]
		public new Components.ILabJackU6 Component
		{
			get => base.Component as Components.ILabJackU6;
			protected set => base.Component = value;
		}

		public bool IsUp => Component.IsUp;
		public bool IsStreaming => Component.IsStreaming;
		public bool DataAcquired => Component.DataAcquired;
		public int ScanFrequency => Component.ScanFrequency;
		public string Error => Component.Error;
		public int LocalId { get => Component.LocalId; set => Component.LocalId = value; }
		public int MinimumRetrievalInterval { get => Component.MinimumRetrievalInterval; set => Component.MinimumRetrievalInterval = value; }
		public int SettlingTimeIndex { get => Component.SettlingTimeIndex; set => Component.SettlingTimeIndex = value; }
		public int ResolutionIndex { get => Component.ResolutionIndex; set => Component.ResolutionIndex = value; }
		public int OutputPaceMilliseconds { get => Component.OutputPaceMilliseconds; set => Component.OutputPaceMilliseconds = value; }
		public double HardwareVersion => Component.HardwareVersion;
		public double SerialNumber => Component.SerialNumber;
		public double FirmwareVersion => Component.FirmwareVersion;
		public double BootloaderVersion => Component.BootloaderVersion;
		public double ProductId => Component.ProductId;
		public double U6Pro => Component.U6Pro;
		public int StreamingBacklogHardware => Component.StreamingBacklogHardware;
		public int StreamingBacklogDriver => Component.StreamingBacklogDriver;
		public int StreamSamplesPerPacket => Component.StreamSamplesPerPacket;
		public int StreamReadsPerSecond => Component.StreamReadsPerSecond;
		public long ScansReceived => Component.ScansReceived;
		public int RetrievalInterval => Component.RetrievalInterval;
		public long ScanMilliseconds => Component.ScanMilliseconds;
		public int MinimumScanTime => Component.MinimumScanTime;
		public int MinimumCommandResponseTime => Component.MinimumCommandResponseTime;
		public int MinimumStreamResponseTime => Component.MinimumStreamResponseTime;
		public int ExpectedScanTime => Component.ExpectedScanTime;
		public int ExpectedStreamResponseTime => Component.ExpectedStreamResponseTime;
	}
}
