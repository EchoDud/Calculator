using Calculator.Models;
using MongoDB.Driver;

public class MongoDBService
{
    private readonly IMongoCollection<Calculation> _calculations;

    public MongoDBService(IConfiguration config)
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("CalculatorDB");
        _calculations = database.GetCollection<Calculation>("Calculations");
    }

    public void AddCalculation(Calculation calculation)
    {
        _calculations.InsertOne(calculation);
    }

    public List<Calculation> GetCalculations()
    {
        return _calculations.Find(calc => true).SortByDescending(calc => calc.CreatedAt).ToList();
    }
}