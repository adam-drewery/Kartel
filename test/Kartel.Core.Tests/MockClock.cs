namespace Kartel.Core.Tests;

public class MockClock : IClock
{
	public Game Game { get; set; }

	public bool Started { get; private set; }

	public short Interval { get; set; }

	public DateTime Time { get; private set; } = new(2000, 01, 01);

	public TimeSpan Delta { get; private set; }

	public event EventHandler Tick;

	public double MinimumTickSpeed { get; set; }

	public DateTime LastUpdate { get; set; }
		
	public float SpeedFactor { get; set; }
		
	public void Start() => Started = true;

	public void Stop() => Started = false;

	public void InvokeTick()
	{
		Game.OnTick();
		Tick?.Invoke(this, System.EventArgs.Empty);
	}

	public void IncrementTime(TimeSpan increment)
	{
		LastUpdate = Time;
		Time += increment;
		Delta = Time - LastUpdate;
	}
}