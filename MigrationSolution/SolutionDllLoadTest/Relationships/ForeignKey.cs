using System;

namespace SolutionDllLoadTest.RelationshipDTOs
{
    public class ForeignKey
    {
        public string DatabaseName { get; set; }

        public Type FromEntity { get; set; }

        public string FromColumn { get; set; }

        public Type ToEntity { get; set; }

        public string ToColumn { get; set; }
    }
}
