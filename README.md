# ArithmeticInterpreter

Test application for Continuity SK
HTTP web service, that accepts an expr in query string and returns correct result.
Included solution for unit testing.

## Getting Started

Open file ArithmeticInterpreter.sln in VS 2019. Click Run in IIS Express. You can see result in browser window. Predefined expression is located in launchSetting.json in ArithmeticIntepreter project. 

## Running the tests
Unit tests are available in ArithmeticIntepreterUnitTest project.
  - Expression_Empty_ReturnError(): <i>test empty expression input</i>
    
  - Expression_ContainsWhiteSpace_ReturnError(): <i>test white space in expression which is not allowed</i>
  
  - Expression_InvalidCharacter_ReturnError(): <i>test expression with character wchich is not allowed by assignment</i>
  
  - Expression_InvalidNumberFormat_ReturnError(): <i>test correct number format (3.14.5 etc. is wrong)</i>
  
  - Expression_LeftParenthesisMissing_ReturnError(): <i>test syntax error, where left parenthesis is missing in expression</i>
  
  - Expression_RightParenthesisMissing_ReturnError(): <i>test syntax error, where right parenthesis is missing in expression</i>  
  
  - Expression_OperandMissing_ReturnError(): <i>test syntax error, where operand is missing in expression</i>
  
  - Expression_DivisionByZero_ReturnError(): <i>test runtime error where attempts to perform a mathematical division by zero, which is an illegal operation</i>
  
  - Expression_Correct_ReturnResult(): <i>test correct input and correct result of expression</i>
  
  - Expression_PowPrecedence_ReturnResult(): <i>test precedence of pow operation, which is higher than multiplication</i>
  
  - Expression_ModulusRemainder_ReturnResult(): <i>test remainder value after modulus</i>
  
## Built With
.NET Core 3.1

## External resources
* [Shunting-yard algorithm](https://en.wikipedia.org/wiki/Shunting-yard_algorithm) - transform infix notation to postfix notation
* [Inspiration in codeproject.com](https://www.codeproject.com/Tips/370486/Converting-InFix-to-PostFix-using-Csharp-VB-NET) - concrete implementation of infix to postfix notation
