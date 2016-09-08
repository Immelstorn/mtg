namespace Mtg.Models.DTO
{
    public class Response<T>
    {
        public bool Failed { get; set; }
        public string Error { get; set; }
        public T Result { get; set; }
    }
}