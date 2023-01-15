using System;
using Kartel.Entities;

namespace Kartel.Activities;

public class Train : Activity
{
	public Train(Person actor, Skill skill) : base(actor) => _skill = skill;

	private readonly Skill _skill;

	protected override void Update(TimeSpan sinceLastUpdate)
	{
		// Increase skill at 1/ms
		var increase = sinceLastUpdate.TotalMilliseconds / 100;

		if (_skill.Value + increase >= byte.MaxValue)
			_skill.Value = byte.MaxValue;
		else
			_skill.Value += (byte) increase;
	}
}