namespace SolutionDllLoadTest.Entities
{
    public class ForeignKey
    {
        public string Name { get; set; }

        public string Schema { get; set; }

        public TableInfo FromTable { get; set; }

        public string FromColumn { get; set; }

        public TableInfo ToTable { get; set; }

        public string ToColumn { get; set; }
    }
}
