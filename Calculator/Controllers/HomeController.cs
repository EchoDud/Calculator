using Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace Calculator.Controllers
{
    public class HomeController : Controller
    {
        private static List<Calculation> history = new List<Calculation>();
        private static int nextId = 1;

        public IActionResult Index()
        {
            ViewBag.History = history;
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(string expression)
        {
            try
            {
                object computationResult = new DataTable().Compute(expression, null);
                double result = Convert.ToDouble(computationResult, CultureInfo.InvariantCulture);
                var calc = new Calculation
                {
                    Id = nextId++,
                    Expression = expression,
                    Result = result
                };
                history.Add(calc);
            }
            catch (Exception ex)
            {                
                ViewBag.Error = "Invalid expression or internal error.";
            }

            return RedirectToAction("Index");
        }
    }
}
