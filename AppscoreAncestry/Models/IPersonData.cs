namespace AppscoreAncestry.Models
{
    public interface IPersonData
    {
        int ID { get; }

        string Name { get; }

        string Gender { get; }

        string BirthPlace { get; }
    }
}