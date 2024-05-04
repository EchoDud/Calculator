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
        private readonly MongoDBService _mongoDBService;

        public HomeController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public IActionResult Index()
        {
            ViewBag.History = _mongoDBService.GetCalculations();
            if (TempData["result"] != null)
            {
                ViewBag.Result = TempData["result"];
            }
            if (TempData["error"] != null)
            {
                ViewBag.Error = TempData["error"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(string expression)
        {
            try
            {
                double result = EvaluateExpression(expression);
                var calculation = new Calculation
                {
                    Expression = expression,
                    Result = result
                };
                _mongoDBService.AddCalculation(calculation);
                TempData["result"] = result.ToString();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Invalid expression or internal error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        private double EvaluateExpression(string expression)
        {
            var evaluator = new NCalc.Expression(expression);
            evaluator.EvaluateFunction += (name, args) =>
            {
                switch (name.ToLower())
                {
                    case "sin":
                        args.Result = Math.Sin(Convert.ToDouble(args.Parameters[0].Evaluate()));
                        break;
                    case "cos":
                        args.Result = Math.Cos(Convert.ToDouble(args.Parameters[0].Evaluate()));
                        break;
                    case "tan":
                        args.Result = Math.Tan(Convert.ToDouble(args.Parameters[0].Evaluate()));
                        break;
                    case "log":
                        args.Result = Math.Log10(Convert.ToDouble(args.Parameters[0].Evaluate()));
                        break;
                    case "ln":
                        args.Result = Math.Log(Convert.ToDouble(args.Parameters[0].Evaluate()));
                        break;
                }
            };
            return Convert.ToDouble(evaluator.Evaluate());
        }
        
    }
}
