namespace TomorrowSoft.Model
{
    public class ContralDataModel
    {
        public ContralDataModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}