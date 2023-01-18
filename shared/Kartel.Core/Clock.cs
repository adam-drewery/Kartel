using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Kartel;

public class Clock : IClock
{
    private readonly Game _game;
    private DateTime _lastUpdate;
    private Task _task;
    private readonly CancellationTokenSource _cancellationToken = new();
    private TimeSpan _sinceLastUpdate;
    private readonly Stopwatch _stopwatch = new();

    public bool Started { get; private set; }

    public Clock(Game game)
    {
        _game = game;
    }

    /// <summary>Number of milliseconds per tick</summary>
    public short Interval { get; set; }

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

    /// <summary>The actual number of milliseconds per tick</summary>
    public double TickSpeed => _sinceLastUpdate.TotalMilliseconds; 
        
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
        
        _task = Task.Run(async () =>
        {
            while(!_cancellationToken.IsCancellationRequested)
            {
                _stopwatch.Restart();
                OnTick();

                var delayTime = Interval - (int)_stopwatch.ElapsedMilliseconds;
                if (delayTime > 0)
                    await Task.Delay(delayTime);
            }

            _cancellationToken.TryReset();
        });
    }

    public void Stop()
    {
        Started = false;
        _cancellationToken.Cancel();
    }
}