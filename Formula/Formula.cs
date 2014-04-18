// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

		// Represents the formula that contains no syntactical errors
        private IEnumerable<string> formula;

		// Normalize function passed to the constructor
        Func<string, string> normalize;

		// Contains the function that maps whether a variable is valid
        Func<string, bool> isValid;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string,string> normalize, Func<string,bool> isValid)
        {
			if (formula == null)
				throw new FormulaFormatException("Exception: no formula passed in");
            this.normalize = normalize;
            this.isValid = isValid;
            this.formula = ValidateTokens(formula);
        }

		/// <summary>
		/// Takes the string that was passed to the constructor and creates a valid formula from it
        /// using the rules stated in the constructor.  If a string is a syntactically incorrect
        /// formula, a FormuleFormatException is thrown with a message saying what the error was.
        /// It returns an IEnumerable<string>.
		/// </summary>
		/// <param name="formula">String to be converted to a valid formula</param>
		/// <returns>IEnumberable<string></returns>
        private IEnumerable<string> ValidateTokens(string formula) {
			
			// Check to make sure there is at least one token
			if (formula == "")
				throw new FormulaFormatException("No tokens found");

			// Get the tokens
            IEnumerable<string> lst = GetTokens(formula);

			// IEnumerable to return
			List<string> formulaList = new List<string>();

			// List of operators
			List<string> operators = new List<string>{"+", "-", "*", "/"};

			// Stack to check for matching parenthesis
            Stack<string> paren = new Stack<string>();
            bool prevWasLeftparenOperator = false;	// True if last token seen was a left paren or operator
            bool prevWasNumVarRightparen = false;	// True if last token seen was a number, variable, or right paren

			// Loop counter
            int count = 0;
            double result;	// Not used, just for parsing (double.TryParse())

			// Loop through tokens
            foreach (string s in lst) {
				
				// Eliminate empty strings
				if (s == "")
					continue;


				// Check to make sure first token is a number, variable, or left paren
                if (count == 0) {
                    if (!double.TryParse(s, out result) && !IsVariable(s) && s != "(") {
                        throw new FormulaFormatException("Formula did not start with a number, variable, or left paren");
                    }
                }

                // Check to make sure last token is a number, variable, or right paren
                else if (count == lst.Count() - 1) {
                    if (!double.TryParse(s, out result) && !IsVariable(s) && s != ")") {
                        throw new FormulaFormatException("Formula did not end with a number, variable, or right paren");
                    }
                }

				// Check if token right after left paren is a number, variable, or left paren
				if (prevWasLeftparenOperator)
					if (!double.TryParse(s, out result) && !IsVariable(s) && s != "(")
						throw new FormulaFormatException("Left paren or operator not followed by a number, variable, or left paren");

				// Check if token right after number, variable, or right paren is a left paren or operator
                if (prevWasNumVarRightparen)
                    if (s != ")" && !operators.Contains(s))
                        throw new FormulaFormatException("Number, variable, or right paren not followed by a right paren or operator");

                prevWasLeftparenOperator = false;
                prevWasNumVarRightparen = false;


				// Go through each option of token to check if token is valid

				// Left paren, just add
                if (s == "(") {
                    paren.Push(s);
                    prevWasLeftparenOperator = true;
                }

                // Right paren, make sure there is a matching left paren
                else if (s == ")") {
                    prevWasNumVarRightparen = true;
                    if (paren.Count > 0)
                        paren.Pop();
                    else
                        throw new FormulaFormatException("Missing parenthesis");
                }

				// Check for operators
                else if (operators.Contains(s)) {
                    prevWasLeftparenOperator = true;
                }

				// Check if s is a valid variable
                else if (IsVariable(s) && isValid(normalize(s))) {
                    prevWasNumVarRightparen = true;
                }

				// Check if s is a number
                else if (double.TryParse(s, out result)) {
                    prevWasNumVarRightparen = true;
                }

				// Must have encountered an invalid token
                else {
                    throw new FormulaFormatException("Token found that is not valid");
                }
                if (double.TryParse(s, out result))
                    formulaList.Add(result.ToString());
				else
                    formulaList.Add(s);
                count++;
            }
			
			// Check to make sure there weren't more left parenthesis than right
			if (paren.Count != 0)
				throw new FormulaFormatException("Missing parenthesis");

			// List passed all tests, return completed formula
			return formulaList;
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<string> operators = new Stack<string>();  // A stack containing the operators encountered
			Stack<double> values = new Stack<double>();       // A stack containing the numbers encountered
			
			HashSet<String> validOperators = new HashSet<String>{"+", "-", "*", "/"};

			double number;

			// Loop through tokens in formula and evaluate
			foreach (string s in formula) {
				
				// Found a number
				if (double.TryParse(s, out number)) {

					values.Push(number);

					// Check if operators is empty
					if (operators.Count == 0)
						continue;

					// * or / on top of operator stack, perform operation.  Otherwise, do nothing.
					if (operators.Peek() == "*" || operators.Peek() == "/"){
						object e = multOrDivide(values, operators);
						// Check if FormulaError was returned, returning it if it was
                        if (e != null)
                            return e;
                    }
				}

				// Found parenthesis
				else if (s == "(" || s == ")") {

					// Opening parenthesis, do nothing
					if (s == "(")
						operators.Push("(");

					// Closing parenthesis
					else {
						// + or - at the top of operator stack, so perform operation
						if (operators.Peek() == "+" || operators.Peek() == "-") {
							addOrSubtract(values, operators);
						}

						// * or / at the top of operator stack, so perform operation
						else if (operators.Peek() == "*" || operators.Peek() == "/") {
							object e = multOrDivide(values, operators);
							// Check if FormulaError was returned, returning it if it was
							if (e != null)
								return e;
						}

                        operators.Pop();

						// Check to see if '*' or '/' was in front of '(', and if so, evaluate it
                        if (operators.Count > 0) {
							if (operators.Peek() == "*" || operators.Peek() == "/") {
								object e = multOrDivide(values, operators);
								// Check if FormulaError was returned, returning it if it was
								if (e != null)
									return e;
							}
						}
					}
				}

				// Found an operator
				else if (validOperators.Contains(s)) {

					// Addition or subtraction
					if (s == "+" || s == "-") {
						
						// Operator stack is empty or only 1 value in values
						if (operators.Count == 0 || values.Count == 1) {
							operators.Push(s);
						}
						
						// + or - at the top of operator stack, so perform operation
						else if (operators.Peek() == "+" || operators.Peek() == "-") {
							addOrSubtract(values, operators);
							operators.Push(s);
						}
						
						// Something else on the top of the operator stack, so do nothing for now
						else {
							operators.Push(s);
						}
					}

					// Multiplication or division
					else if (s == "*" || s == "/") {
						operators.Push(s);
					}
				}

				// Found a variable
				else if (IsVariable(s)) {

                    try {
                        number = lookup(normalize(s));
                    }
                    catch (ArgumentException) {
                        return new FormulaError("error: variable '" + s + "' had no value");
                    }
					values.Push(number);

					// Check if operators is empty
					if (operators.Count == 0)
						continue;

					// If * or / on top of operator stack, perform operation.  Otherwise, do nothing.
                    if (operators.Peek() == "*" || operators.Peek() == "/") {
						object e = multOrDivide(values, operators);
						// Check if FormulaError was returned, returning it if it was
						if (e != null)
							return e;
                    }
				}
			}


			// Everything is completed, return the result
			if (operators.Count == 0)
				return values.Pop();


			// Still one operation left
            if (operators.Count == 1) {
                if (operators.Peek() == "+" || operators.Peek() == "-") {
                    addOrSubtract(values, operators);
                    return values.Pop();
                }
            }

            return 0;
        }

        /// <summary>
        /// Performs either adding or subtracting depending on which operator is on top of 
        /// operators stack.
        /// </summary>
        /// <param name="values">Stack containing the values</param>
        /// <param name="operators">Stack containing the operators</param>
        private static void addOrSubtract(Stack<double> values, Stack<string> operators) {
            double v2 = values.Pop();
            double v1 = values.Pop();
            string op = operators.Pop();
            double result = 0;
            if (op == "+") {
                result = v1 + v2;
            }
            else {
                result = v1 - v2;
            }
            values.Push(result);
        }

        /// <summary>
        /// Performs either multiplication or division depending on which operator is on top of
        /// operators stack.  If division by zero occurs, returns a FormulaError.
        /// </summary>
        /// <param name="values">Stack containing the values</param>
        /// <param name="operators">Stack containing the operators</param>
        private static object multOrDivide(Stack<double> values, Stack<string> operators) {
            double v2 = values.Pop();
            double v1 = values.Pop();
            string op = operators.Pop();
            if (op == "*") {
                double result = v1 * v2;
                values.Push(result);
            }
            else {
                // Divsion by 0
                if (v2 == 0)
                    return new FormulaError("error: division by zero occured");
                double result = v1 / v2;
                values.Push(result);
            }
            return null;
        }

        /// <summary>
        /// Determines whether the given string is a valid variable.  A valid variable is a
        /// letter or underscore followed by 0 or more letters, numbers, or underscores.
        /// </summary>
        /// <param name="s">String representation of a variable</param>
        /// <returns>True if s is a valid variable</returns>
        private bool IsVariable(String s) {
            bool flag = Regex.IsMatch(s, "^[_a-zA-Z][a-zA-Z0-9_]*$");
            if (flag)
                return isValid(normalize(s));
            return false;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
		{
            HashSet<string> list = new HashSet<string>();
            foreach (string s in formula) {
                string a = normalize(s);
                if (IsVariable(a))
                    list.Add(a);
            }
			return list;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string finalString = "";
            foreach (string s in formula)
                finalString += s;
            return finalString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.Equals(""))
                return false;
			Formula f = (Formula) obj;
            return this.ToString() == f.ToString() && this.GetHashCode() == f.GetHashCode();
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null)) {
                if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
                    return true;
                return false;
            }
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

