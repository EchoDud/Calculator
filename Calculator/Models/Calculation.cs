using MongoDB.Bson;

namespace Calculator.Models
{
    public class Calculation
    {
        public ObjectId Id { get; set; }
        public string Expression { get; set; }
        public double Result { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
