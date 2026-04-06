namespace Application.Dtos.Response
{
    public class DataResult
    {
        public object Result { get; set; } = new();
        public int Count { get; set; }
    }
}
