/*
 * Custom exception class
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Continuity.Libraries
{
    /// <summary>
    /// Custom InvalidSyntax exception class
    /// </summary>
    /// <remarks>
    /// In this class can be added additional properties and methods for detailed syntax error report (char position etc.) 
    /// </remarks>
    public class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException(string message)
           : base(message)
        {
        }
    }
}
