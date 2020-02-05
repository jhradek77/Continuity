using Microsoft.VisualStudio.TestTools.UnitTesting;

using Continuity.Controllers;
using System;
using System.Globalization;

namespace Continuity.Testing
{
    [TestClass]
    public class ArithmeticInterpreter_ExpressionEvaluate
    {
        private readonly ComputeController _controller;

        public ArithmeticInterpreter_ExpressionEvaluate()
        {
            _controller = new ComputeController();
        }

        [TestMethod]
        public void Expression_Empty_ReturnError()
        {
            var result = _controller.Get("");

            Assert.AreEqual<string>(result, "SyntaxError: Cannot evaluate empty expression.");
        }

        [TestMethod]
        public void Expression_ContainsWhiteSpace_ReturnError()
        {
            var result = _controller.Get(" ");

            Assert.AreEqual<string>(result, "SyntaxError: Cannot evaluate empty expression.");
        }

        [TestMethod]
        public void Expression_InvalidCharacter_ReturnError()
        {
            var result = _controller.Get("abc");

            Assert.AreEqual<string>(result, "SyntaxError: Expression contains invald character.");
        }

        [TestMethod]
        public void Expression_InvalidNumberFormat_ReturnError()
        {
            var result = _controller.Get("1+3.14.5");

            Assert.AreEqual<string>(result, "SyntaxError: Expression contains number in wrong format.");
        }

        [TestMethod]
        public void Expression_LeftParenthesisMissing_ReturnError()
        {
            var result = _controller.Get("(1+2)*3)");

            Assert.AreEqual<string>(result, "SyntaxError: Left parenthesis is missing.");
        }

        [TestMethod]
        public void Expression_RightParenthesisMissing_ReturnError()
        {
            var result = _controller.Get("((1+2)");

            Assert.AreEqual<string>(result, "SyntaxError: Right parenthesis is missing.");
        }

        [TestMethod]
        public void Expression_OperandMissing_ReturnError()
        {
            var result = _controller.Get("1+2*");

            Assert.AreEqual<string>(result, "SyntaxError: Operand is missing.");
        }

        [TestMethod]
        public void Expression_OperandMissing2_ReturnError()
        {
            var result = _controller.Get("1++2");

            Assert.AreEqual<string>(result, "SyntaxError: Operand is missing.");
        }

        [TestMethod]
        public void Expression_DivisionByZero_ReturnError()
        {
            var result = _controller.Get("((1+2)*43)/0");

            Assert.AreEqual<string>(result, "RuntimeError: Division by zero.");
        }

        [TestMethod]

        public void Expression_Correct_ReturnResult()
        {
            var result = _controller.Get("((1+2)*43)/3.14+2^3");

            Assert.AreEqual<string>(result, "49.0828025477707");
        }

        [TestMethod]
        public void Expression_PowPrecedence_ReturnResult()
        {
            var result = _controller.Get("4*2^3");

            Assert.AreEqual<string>(result, "32");
        }

        [TestMethod]
        public void Expression_ModulusRemainder_ReturnResult()
        {
            var result = _controller.Get("5.9%3.1");
            double dresult = 0;
            Assert.IsTrue(double.TryParse(result, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out dresult));
            Assert.AreEqual<double>(Math.Round(dresult,1), 2.8);
        }

    }
}
