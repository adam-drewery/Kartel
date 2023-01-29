using System;
using System.Collections.Generic;
using System.Linq;
using Kartel.Activities;
using Kartel.Attributes;
using Kartel.Entities;
using Serilog;

namespace Kartel.Commands;

[Verb("Hang out", "Hanging out", "Hung out")]
public class Command
{
    protected ILogger Log { get; } = Serilog.Log.ForContext<Command>();
    
    protected IGame Game { get; }

    protected Person Actor { get; }

    public DateTime StartTime { get; private set; }
    
    public bool IsResolvingNeed { get; set; }
    
    public bool Started => StartTime != default && StartTime <= Now;

    public Activity? CurrentActivity => Activities.Any() ? Activities.Peek() : default;
        
    public Queue<Activity> Activities { get; } = new();
        
    public DateTime UpdatedTime { get; private set; }

    public Command(Person actor)
    {
        Name = new VerbName(this);
        Game = actor.Game;
        Actor = actor;
    }
        
    public VerbName Name { get; set; }
        
    protected DateTime Now => Game.Clock.Time;
        
    public bool IsComplete => Activities.Count == 0;

    public void Start(DateTime startTime)
    {
        // The last update time is the start time, so update calculations
        // calculate from the activity start time and not initialization time.
        StartTime = UpdatedTime = startTime;
    }

    public void Cancel()
    {
        while (Activities.Any())
        {
            var task = Activities.Dequeue();
            task.Complete();
        }

        EndTime = Now;
    }

    public DateTime EndTime { get; set; }

    public void Update()
    {
        if (StartTime == default) Start(Now);
        
        if (CurrentActivity != null)
        {
            try
            {
                CurrentActivity.Update();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Activity for {Actor} ({ID}) of type {Activity} threw an exception", 
                    Actor.Name, 
                    Actor.Id, 
                    GetType().Name);

                Activities.Clear();
                EndTime = Now;
                return;
            }
                
            if (CurrentActivity.IsComplete)
            {
                Log.Information("{Actor} ({ID}) finished {Activity}", Actor, Actor.Id, CurrentActivity.Name.PresentTense);
                Activities.Dequeue();
            }
        }
        
        UpdatedTime = Now;
        
        if (!Activities.Any()) 
            EndTime = Now;
    }
}