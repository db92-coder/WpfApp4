namespace WpfApp4.Entities
{
    public class Unit
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int Coordinator { get; set; }

        public override string ToString()
        {
            return Code + " " + Title;
        }

    }
}
