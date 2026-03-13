namespace Smdb.Core.Actors;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string Biography { get; set; } = string.Empty;

    public Actor(int id, string name, int birthYear, string biography)
    {
        Id = id;
        Name = name;
        BirthYear = birthYear;
        Biography = biography;
    }

    public override string ToString()
    {
        return $"Actor[Id={Id}, Name={Name}, BirthYear={BirthYear}, Biography={Biography}]";
    }
}