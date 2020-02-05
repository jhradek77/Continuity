/*
 * Arithmetic expression evaluator class
 */
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Continuity.Libraries
{
    /// <summary>
    /// The main ArithmeticExpressionEvaluator class. 
    /// Contains methods for evaluating arithmetic expression. 
    /// Expression can contain:
    /// <list type="bullet">
    /// <item>
    /// <term>Numerals</term>
    /// <description>[ '0' .. '9' ]</description>
    /// </item>
    /// <item>
    /// <term>Decimal separator</term>
    /// <description>[ '.' ]</description>
    /// </item>
    /// <item>
    /// <term>Operators</term>
    /// <description>[ '+' , '-' , '*' , '/' , '%' , '^' ]</description>
    /// </item>
    /// <item>
    /// <term>Parenthesis</term>
    /// <description>[ '(' , ')' ]</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>These class can parse arithmetic expression and return numeric result or exception.</para>
    /// </remarks>
    public class ArithmeticExpressionEvaluator
    {
        #region Private members
        private static string number = "";
        
        //characters, which can contain operator
        private static List<char> operatorList = new List<char>(new char[] { '+', '-', '*', '/', '^', '%', '(', ')' });
        
        //characters, which can contain operand
        private static List<char> digitList = new List<char>(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' });

        /// <summary>
        /// Convert string <paramref name="infix"/> to postfix notation.
        /// </summary>
        /// <remarks>
        /// <para>Using Shunting-yard algorithm (Dijkstra)</para>
        /// <para>http://en.wikipedia.org/wiki/Shunting-yard_algorithm</para>
        /// </remarks>
        /// <returns>String in postfix notation (Reverse Polish notation).</returns>
        /// <exception cref="Continuity.Libraries.InvalidSyntaxException">Thrown when invalid syntax founded.</exception>
        /// <param name="infix">An string.</param>
        private static string InfixToPostfix(string infix)
        {
            string result = "";
            char newSymbol, topSymbol;
            Stack<char> operatorStack = new Stack<char>();

            //cycle for whole infix string
            int i = 0;
            while (i < infix.Length)
            {
                newSymbol = infix[i];   //read new symbol

                //operand
                if (digitList.Contains(newSymbol))
                {
                    result += newSymbol;
                    i++;
                    //continue with reading of number (can contain more than 1 character)
                    while (i < infix.Length)
                    {
                        newSymbol = infix[i];
                        if (digitList.Contains(newSymbol))
                        {
                            result += newSymbol;    //number can contain this character
                            i++;
                        }
                        else
                        {
                            break; //stop reading
                        }
                    }
                    result += ';'; //mark the end of number for further processing
                }

                //operator
                if (operatorList.Contains(newSymbol))
                {
                    //there are another operators on the stack
                    if (operatorStack.Count > 0)
                    {
                        //solve precedence
                        topSymbol = operatorStack.Peek();
                        if (Precedence(topSymbol, newSymbol))
                        {
                            if (topSymbol != '(') result += topSymbol;
                            operatorStack.Pop();
                        }
                    }
                    if (newSymbol != ')') operatorStack.Push(newSymbol);
                    else
                    {
                        //process operators from stack until left parenthesis
                        if (operatorStack.Count > 0)
                        {
                            char ch;
                            do
                            {
                                ch = operatorStack.Pop();
                                if (ch != '(') result += ch;
                            }
                            while (ch != '(');
                        }
                        else
                        {
                            //Syntax error
                            throw new InvalidSyntaxException("Left parenthesis is missing.");
                        }
                    }
                }
                i++;
            }

            //Pop leftover operands
            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() != '(')
                {
                    result += operatorStack.Pop();
                }
                else
                {
                    //Syntax error
                    throw new InvalidSyntaxException("Right parenthesis is missing.");
                }
            }

            return result;
        }

        /// <summary>
        /// Evaluate precedence of two operators.
        /// </summary>
        /// <returns>Boolean value (true/false) that indicates precedence of one operator to another.</returns>
        /// <param name="symbol1">An char.</param>        
        /// <param name="symbol2">An char.</param>        
        private static bool Precedence(char symbol1, char symbol2)
        {
            if ((symbol1 == '+' || symbol1 == '-') && (symbol2 == '*' || symbol2 == '/' || symbol2 == '^' || symbol2 == '%')) return false; // operators ['*', '/', '%', '^' has higher precedence than ['+', '-']
            else if ((symbol1 == '*' || symbol1 == '/' || symbol1 == '%') && (symbol2 == '^')) return false;    // '^' has higher precedence than ['*', '/', '%']
            else if (symbol1 == '(' && symbol2 != ')' || symbol2 == '(') return false;
            else return true;
        }
        #endregion

        #region Public members
        
        /// <summary>
        /// Evaluate arithmetic expression <paramref name="expression"/> and return the result.
        /// </summary>
        /// <param name="expression">An exprssion string.</param>
        /// <returns>Result of expression</returns>
        /// <exception cref="Continuity.Libraries.InvalidSyntaxException">Thrown when invalid syntax founded.</exception>
        /// <exception cref="System.DivideByZeroException">Thrown in case that expression contains division by zero.</exception>
        public static double Evaluate(string expression)
        {
            number = "";

            //convert exprssion from infix to postfix notation
            string postfix = InfixToPostfix(expression);

            //new stack for evaluation process
            Stack<double> stack = new Stack<double>();

            //cycle for whole postfix string
            for (int i = 0; i < postfix.Length; i++)
            {
                char ch = postfix[i];
                if (!operatorList.Contains(ch))
                {
                    //part of number
                    if (digitList.Contains(ch)) number += ch; //add character to number character string
                    else
                    {
                        double c = 0;
                        //convert to double (decimal dot/point)
                        if (!double.TryParse(number, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out c)) throw new InvalidSyntaxException("Expression contains number in wrong format.");
                        
                        //end of reading, push to stack
                        stack.Push(c);
                        number = "";
                    }
                }
                else
                {
                    number = "";
                    //every opration needs 2 operands
                    if (stack.Count < 2) throw new InvalidSyntaxException("Operand is missing.");

                    //read operands from stack
                    double operand1 = stack.Pop();
                    double operand2 = stack.Pop();
                    
                    //evaluate opetrator
                    switch (ch)
                    {
                        case '+':
                            stack.Push((operand2 + operand1));
                            break;
                        case '-':
                            stack.Push((operand2 - operand1));
                            break;
                        case '*':
                            stack.Push((operand2 * operand1));
                            break;
                        case '/':
                            {
                                if (operand1 == 0) throw new DivideByZeroException("Division by zero.");
                                else stack.Push((operand2 / operand1));
                            }
                            break;
                        case '^':
                            stack.Push(Math.Pow(operand2, operand1));
                            break;
                        case '%':
                            stack.Push((operand2 % operand1));
                            break;
                    }
                }
            }

            //return last item on stack as result
            return stack.Pop();
        }
        #endregion
    }
}
