using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;

using Continuity.Helpers;
using Continuity.Libraries;

namespace Continuity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComputeController : ControllerBase
    {
        private static readonly string _controlRegEx = @"^[0-9.+\-*/^%()]*$";

        public object Cultureinfo { get; private set; }

        [HttpGet]
        public string Get([RequiredFromQuery]string expr)
        {
            try
            {
                //check for null exprssion
                if (!string.IsNullOrWhiteSpace(expr))
                {
                    //replace spaces by '+' character due to request encoding rules, where space is encoded by '+' character
                    //expr = expr.Replace(' ', '+');

                    //check expression for supported characters
                    Regex r = new Regex(_controlRegEx);
                    if (!r.IsMatch(expr))
                    {
                        return "SyntaxError: Expression contains invald character.";
                    }

                    double result = ArithmeticExpressionEvaluator.Evaluate(expr);
                    return result.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    return "SyntaxError: Cannot evaluate empty expression.";
                }
            }
            catch (InvalidSyntaxException ise)
            {
                return string.Format("SyntaxError: {0}", ise.Message);
            }
            catch (DivideByZeroException dbze)
            {
                return string.Format("RuntimeError: {0}", dbze.Message);
            }
            catch (Exception e)
            {
                return string.Format("UnknownError: {0}", e.Message);
            }
        }
    }
}
