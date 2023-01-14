using System;
using Kartel.Entities;
using Kartel.Services;

namespace Kartel.Commands;

public abstract class Activity
{
	protected Activity() { }
		
	protected Activity(Person actor)
	{
		Actor = actor;
		Game = actor.Game;
	}

	public void Start(DateTime startTime)
	{
		// The last update time is the start time, so update calculations
		// calculate from the activity start time and not initialization time.
		StartTime = UpdatedTime = startTime;
		Update();
	}
        
	protected Game Game { get; }

	public DateTime StartTime { get; private set; }
        
	public bool Started => StartTime != default && StartTime <= Now;
        
	protected DateTime Now => Game.Clock.Time;
        
	protected ServiceContainer Services => Game.Services;
        
	protected static Random Random { get; } = new();
        
	public virtual bool IsComplete => EndTime <= Now;
        
	public virtual DateTime EndTime { get; set; } = DateTime.MaxValue;
        
	protected Person Actor { get; }
        
	public DateTime UpdatedTime { get; private set; }
        
	public void Complete() => EndTime = Now;

	protected abstract void Update(TimeSpan sinceLastUpdate);
        
	public void Update()
	{
		if (StartTime == default) Start(Now);                
		else Update(Now - UpdatedTime);

		UpdatedTime = Now;
	}
}