using System;
using System.Collections.Generic;
using System.Linq;
using Kartel.Activities;
using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Commands;

[Verb("Hang out", "Hanging out", "Hung out")]
public class Command
{
    protected Game Game { get; }

    protected Person Actor { get; }

    public DateTime StartTime { get; private set; }
        
    public bool Started => StartTime != default && StartTime <= Now;
        
    //public virtual DateTime EndTime { get; set; } = DateTime.MaxValue;

    public Activity CurrentActivity => Activities.Any() ? Activities.Peek() : default;
        
    public Queue<Activity> Activities { get; } = new();
        
    public DateTime UpdatedTime { get; private set; }

    public Command()
    {
        Name = new VerbName(this);
    }

    public Command(Person actor) : this()
    {
        Game = actor.Game;
        Actor = actor;
    }
        
    public VerbName Name { get; set; }
        
    protected DateTime Now => Game.Clock.Time;
        
    public bool Complete => Activities.Count == 0;

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
            CurrentActivity.Update();
                
            if (CurrentActivity.IsComplete) 
                Activities.Dequeue();
        }
        
        UpdatedTime = Now;
        
        if (!Activities.Any()) 
            EndTime = Now;
    }
}