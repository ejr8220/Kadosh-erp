
namespace Application.Dtos.Request
{
    public class CustomDataManagerRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool RequiresCounts { get; set; }
        public List<SortedField> Sorted { get; set; } = new();
        public List<SearchFilter> Search { get; set; } = new();
        public List<WhereFilter> Where { get; set; } = new();
        public List<AggregateField> Aggregates { get; set; } = new();

        public class SortedField
        {
            public string Name { get; set; } = string.Empty;
            public string Direction { get; set; } = "Ascending";
        }

        public class SearchFilter
        {
            public List<string> Fields { get; set; } = new();
            public string Key { get; set; } = string.Empty;
            public string Operator { get; set; } = "contains";
            public bool IgnoreAccent { get; set; }
        }

        public class WhereFilter
        {
            public string Field { get; set; } = string.Empty;
            public string Operator { get; set; } = "equal";
            public object? Value { get; set; }
            public bool IgnoreCase { get; set; }
            public bool IgnoreAccent { get; set; }
            public string Condition { get; set; } = "and";

            
            public List<WhereFilter> Predicates { get; set; } = new();
        }

        public class AggregateField
        {
            public string Field { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
        }
    }
}