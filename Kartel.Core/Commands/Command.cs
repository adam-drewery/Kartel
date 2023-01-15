using System;
using System.Collections.Generic;
using System.Linq;
using Kartel.Activities;
using Kartel.Entities;

namespace Kartel.Commands;

public abstract class Command
{
    protected Game Game { get; }

    protected Person Actor { get; }

    public DateTime StartTime { get; private set; }
        
    public bool Started => StartTime != default && StartTime <= Now;
        
    //public virtual DateTime EndTime { get; set; } = DateTime.MaxValue;

    public Activity CurrentTask => Activities.Any() ? Activities.Peek() : default;
        
    public Queue<Activity> Activities { get; } = new();
        
    public DateTime UpdatedTime { get; private set; }
        
    protected Command() { }
            
    protected Command(Person actor)
    {
        Name = new CommandName(this);
        Game = actor.Game;
        Actor = actor;
    }
        
    public CommandName Name { get; }
        
    protected DateTime Now => Game.Clock.Time;
        
    public bool Complete => Activities.Count == 0;

    public void Start(DateTime startTime)
    {
        // The last update time is the start time, so update calculations
        // calculate from the activity start time and not initialization time.
        StartTime = UpdatedTime = startTime;
        Update();
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
        else if (!Activities.Any()) return;
        else
        {
            CurrentTask.Update();
                
            while(CurrentTask.IsComplete)
            {
                Activities.Dequeue();
                if (!Activities.Any())
                {
                    EndTime = Now;
                    break;
                }

                CurrentTask.Update();
            }
        }

        UpdatedTime = Now;
    }
}