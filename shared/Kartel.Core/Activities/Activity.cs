using System;
using Kartel.Entities;
using Kartel.Services;
using Serilog;

namespace Kartel.Activities;

public abstract class Activity
{
	protected ILogger Log { get; } = Serilog.Log.ForContext<Activity>();

	protected Activity(Person actor)
	{
		Actor = actor;
		Game = actor.Game;
		Name = new VerbName(this);
	}

	private void Start(DateTime startTime)
	{
		// The last update time is the start time, so update calculations
		// calculate from the activity start time and not initialization time.
		StartTime = UpdatedTime = startTime;
		Update();
	}
	
	protected IGame Game { get; }

	public VerbName Name { get; }
	
	public DateTime StartTime { get; private set; }
        
	public bool Started => StartTime != default && StartTime <= Now;
        
	protected DateTime Now => Game.Clock.Time;
        
	protected ServiceContainer Services => Game.Services;
        
	protected static Random Random { get; } = new();
        
	public virtual bool IsComplete => EndTime <= Now;
        
	public virtual DateTime EndTime { get; private set; } = DateTime.MaxValue;
        
	protected Person Actor { get; }
        
	public DateTime UpdatedTime { get; private set; }
        
	public void Complete() => EndTime = Now;

	protected abstract void Update(TimeSpan sinceLastUpdate);
	
	public void Update()
	{
		if (StartTime != default)
		{
			try
			{
				Update(Now - UpdatedTime);
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Activity for {Actor} ({ID}) of type {Activity} threw an exception", 
					Actor.Name, 
					Actor.Id, 
					GetType().Name);

				throw;
			}
		}
		else
		{
			Log.Information("{ActorName} ({ActorID}) is {ActivityName}", 
				Actor.Name, 
				Actor.Id, 
				Name.PresentTense.ToLowerInvariant());
			
			Start(Now);
		}

		UpdatedTime = Now;
	}
}