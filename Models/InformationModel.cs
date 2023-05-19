namespace Converter.Models
{
    public class InformationModel
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string EventType { get;set; }
        public DateTime CreatedAt { get; set; }
    }
}
