using System;
using System.Threading.Tasks;
using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Activities;

[Verb("Socialize", "Socializing", "Socialized")]
public class Socialize : Activity 
{
    public Socialize(Person actor) : base(actor) { }

    private Task<Person> _personTask;
    
    protected override void Update(TimeSpan sinceLastUpdate)
    {
        if (_personTask != null)
        {
            if (_personTask.IsFaulted)
                Game.OnError("Socialization task failed", _personTask.Exception);
                
            else if (_personTask.IsCompleted)
                Actor.Meet(_personTask.Result);

            Actor.Needs.Social.Value = 0;
            Complete();
        }
        else
        {
            for (var i = 0; i < sinceLastUpdate.TotalMilliseconds; i++)
            {
                // Meet approximately 1 person per minute
                // TODO: optimise these based on tick length? 
                if (Random.Next(0, 60000) == 0)
                {
                    _personTask = Person.New(Game);
                }
            }
        }
            
        // Meet new customers?
        // Increase happiness/relaxation stat
    }
}