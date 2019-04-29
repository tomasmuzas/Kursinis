using System.Globalization;

namespace SolutionDllLoadTest.SqlHelpers
{
    public class ForeignKeyInfo
    {
        public string DatabaseName { get; set; }

        public string Schema { get; set; }

        public string FromTable { get; set; }

        public string FromColumn { get; set; }

        public string ToTable { get; set; }

        public string ToColumn { get; set; }
    }
}