using FluentAssertions;
using Kartel.Entities;
using Kartel.Environment;
using Xunit;

namespace Kartel.Core.Tests;

public class PersonTests
{
	public class OnHeartbeat
	{
		[Fact]
		public void Reduces_fatigue_when_an_interval_has_passed()
		{
			var clock = new MockClock();
			var game = new Game { Clock = clock };
			clock.Game = game;
				
			var person = new Person(new House()) { Needs = { Sleep = { Value = 0 } } };
			game.Characters.Add(person);

			var oldFatigue = person.Needs.Sleep.Value;
			clock.IncrementTime(TimeSpan.FromSeconds(300));
			clock.InvokeTick();

			person.Needs.Sleep.Value.Should().BeGreaterThan(oldFatigue);
		}
			
		[Fact]
		public void Reduces_fatigue_when_an_interval_has_passed_and_the_previous_tick_was_the_previous_day()
		{
			var clock = new MockClock();
			var game = new Game { Clock = clock };
			clock.Game = game;
				
			var person = new Person(new House()) { Needs = { Sleep = { Value = 0 } } };
			game.Characters.Add(person);

			var oldFatigue = person.Needs.Sleep.Value;
			clock.IncrementTime(TimeSpan.FromHours(23));
			clock.IncrementTime(TimeSpan.FromHours(2));
			clock.InvokeTick();

			person.Needs.Sleep.Value.Should().BeGreaterThan(oldFatigue);
		}
	}
}