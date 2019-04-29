namespace SolutionDllLoadTest.SqlHelpers
{
    public class DatabaseForeignKeyInfo
    {
        public string Name { get; set; }

        public string Schema { get; set; }

        public string FromTable { get; set; }

        public string FromColumn { get; set; }

        public string ToTable { get; set; }

        public string ToColumn { get; set; }
    }
}