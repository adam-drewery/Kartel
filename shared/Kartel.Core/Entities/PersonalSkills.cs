namespace Kartel.Entities;

public class PersonalSkills : TypedList<Skill>
{
    public Skill Strength { get; } = new("Strength");

    public Skill Charisma { get; } = new("Charisma");

    public Skill Finesse { get; } = new("Finesse");

    public Skill Endurance { get; } = new("Endurance");

    public Skill Intelligence { get; } = new("Intelligence");

    public Skill Intimidation { get; } = new("Intimidation");

    public Skill Guns { get; } = new("Guns");

    public Skill Melee { get; } = new("Melee Weapons");
    
    public Skill Chemistry { get; } = new("Chemistry");

    public Skill Horticulture { get; } = new("Horticulture");
}