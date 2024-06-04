namespace DllTest
{
    public class Person
    {
        private int Id;
        string Name;
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public void Print() => Console.WriteLine($"Id {Id} Name {Name}");
    }
}
