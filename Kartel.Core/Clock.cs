using System;
using System.Timers;

namespace Kartel;

public class Clock : IClock
{
    private readonly Game _game;
    private DateTime _lastUpdate;
    private readonly Timer _timer = new();
    private TimeSpan _sinceLastUpdate;

    public bool Started { get; private set; }

    public Clock(Game game)
    {
        _game = game;
        _timer.Elapsed += (_, _) => OnTick();
    }

    public double Interval
    {
        get => _timer.Interval;
        set => _timer.Interval = value;
    }
        
    /// <summary>The current game-time.</summary>
    public DateTime Time { get; private set; } = DateTime.UtcNow;
        
    /// <summary>Amount of game-time since the last tick.</summary>
    public TimeSpan Delta { get; private set; } = TimeSpan.Zero;

    public DateTime LastUpdate { get; set; }

    public void UpdateTime()
    {
        _sinceLastUpdate = (DateTime.UtcNow - _lastUpdate);
        _lastUpdate = DateTime.UtcNow;

        var scaled = _sinceLastUpdate.Ticks * SpeedFactor;
        LastUpdate = Time;
        Time = Time.AddTicks((long) scaled);
        Delta = Time - LastUpdate;
    }

    public event EventHandler Tick;

    public double TickSpeed => _sinceLastUpdate.TotalMilliseconds; 
        
    /// <summary>Specifies the number of milliseconds per tick.</summary>
    public double MinimumTickSpeed
    {
        get => _timer.Interval;
        set => _timer.Interval = value;
    }

    /// <summary>Modifies the speed of the game when set to a value other than 1.</summary>
    public float SpeedFactor { get; set; } = 1;

    private void OnTick()
    {
        UpdateTime();
        _game.OnHeartbeat();
        Tick?.Invoke(_game, System.EventArgs.Empty);
    }

    public void Start()
    {
        Started = true;

        // We need to set the last updated time to now, otherwise the next tick
        // will think the game was never stopped. 
        _lastUpdate = DateTime.UtcNow;
        UpdateTime();
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        Started = false;
    }

    public bool Enabled => _timer.Enabled;
}