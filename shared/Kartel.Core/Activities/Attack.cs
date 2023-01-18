using System;
using Kartel.Attributes;
using Kartel.Entities;

namespace Kartel.Activities;

[Verb("Attack", "Attacking", "Attacked")]
public class Attack : Activity
{
    private bool _complete;
    private readonly Person _target;
        
    public Attack(Person actor, Person target) : base(actor) => _target = target;

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        if (!_complete && (Now - StartTime).TotalSeconds > 10) 
        {
            _complete = true;
            _target.Health -= byte.MaxValue / 16;
                
            Complete();
        }
    }
}