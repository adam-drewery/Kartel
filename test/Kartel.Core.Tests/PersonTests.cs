using FluentAssertions;
using Kartel.Entities;
using Kartel.Environment;
using Kartel.Testing;
using Xunit;

namespace Kartel.Core.Tests;

public class PersonTests : GameTests
{
	public class OnHeartbeat : PersonTests
	{
		[Fact]
		public void Reduces_fatigue_when_an_interval_has_passed()
		{
			var person = new Person(new House(Game)) { Needs = { Sleep = { Value = 0 } } };
			Game.Characters.Add(person);

			var oldFatigue = person.Needs.Sleep.Value;
			Clock.IncrementTime(TimeSpan.FromSeconds(300));
			Clock.InvokeTick();

			person.Needs.Sleep.Value.Should().BeGreaterThan(oldFatigue);
		}
			
		[Fact]
		public void Reduces_fatigue_when_an_interval_has_passed_and_the_previous_tick_was_the_previous_day()
		{
			var person = new Person(new House(Game)) { Needs = { Sleep = { Value = 0 } } };
			Game.Characters.Add(person);

			var oldFatigue = person.Needs.Sleep.Value;
			Clock.IncrementTime(TimeSpan.FromHours(23));
			Clock.IncrementTime(TimeSpan.FromHours(2));
			Clock.InvokeTick();

			person.Needs.Sleep.Value.Should().BeGreaterThan(oldFatigue);
		}
	}
}