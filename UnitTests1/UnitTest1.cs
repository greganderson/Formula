using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic; 

namespace MyUnitTests {
    [TestClass]
    public class UnitTest1 {
        // ************************** TESTS CONSTRUCTORS ************************* //
        [TestMethod]
        public void TestConstructors1() {
            Formula f = new Formula("(1+2)");
        }

        [TestMethod]
        public void TestConstructors2() {
            Formula f = new Formula("(1+2)", s => { string a = s.ToUpper(); return a; }, t => { bool b = 1 == 0; return b; });
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors3() {
            Formula f = new Formula("");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors4() {
            Formula f = new Formula("())");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors5() {
            Formula f = new Formula("(8+5%)");
        }

        [TestMethod]
        public void TestConstructors6() {
            Formula f = new Formula("(7 + 7)");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors7() {
            Formula f = new Formula("(()");
        }

        [TestMethod]
        public void TestConstructors8() {
            Formula f = new Formula("a + 7");
        }

        [TestMethod]
        public void TestConstructors9() {
            Formula f = new Formula("7 + 7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors10() {
            Formula f = new Formula("+ 7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors11() {
            Formula f = new Formula("- 7 +7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors12() {
            Formula f = new Formula("* 7 +7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors13() {
            Formula f = new Formula("/ 7 +7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors14() {
            Formula f = new Formula(") 7 +7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors15() {
            Formula f = new Formula("7 +7 +");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors16() {
            Formula f = new Formula("7 +7 -");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors17() {
            Formula f = new Formula("7 +7 *");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors18() {
            Formula f = new Formula("7 +7 /");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors19() {
            Formula f = new Formula("7 +7 (");
        }

        [TestMethod]
        public void TestConstructors20() {
            Formula f = new Formula("(12 - 4)");
        }

        [TestMethod]
        public void TestConstructors21() {
            Formula f = new Formula("(a7 - 4)");
        }

        [TestMethod]
        public void TestConstructors22() {
            Formula f = new Formula("((4 - 4))");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors23() {
            Formula f = new Formula("( + 4)");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors24() {
            Formula f = new Formula("( - 4)");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors25() {
            Formula f = new Formula("( * 4)");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors26() {
            Formula f = new Formula("( / 4)");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors27() {
            Formula f = new Formula("( $ 4)");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors28() {
            Formula f = new Formula("34.0273 10");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors29() {
            Formula f = new Formula("10 a7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors30() {
            Formula f = new Formula("10 )");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors31() {
            Formula f = new Formula("_7 a7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors32() {
            Formula f = new Formula("u17 )");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors33() {
            Formula f = new Formula("_7 18");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors34() {
            Formula f = new Formula("(7) a7");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors35() {
            Formula f = new Formula("(a7) )");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors36() {
            Formula f = new Formula("(1) 18");
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructors37() {
            Formula f = new Formula(null);
        }

        // ************************** TESTS BASIC METHODS ************************* //

        [TestMethod]
        public void TestToString1() {
            Formula f = new Formula("4+ 7 -2*4");
            Assert.AreEqual("4+7-2*4", f.ToString());
        }

        [TestMethod]
        public void TestToString2() {
            Formula f = new Formula("( a1 - 7 * 0 ) + 7 - 19 * _");
            Assert.AreEqual("(a1-7*0)+7-19*_", f.ToString());
        }

        [TestMethod]
        public void TestEquals1() {
            Formula f = new Formula("( a1 - 7 * 0 ) + 7 - 19 * _");
            Formula g = new Formula("(a1-7*0)+7-19*_");
            Assert.IsTrue(f.Equals(g));
        }

        [TestMethod]
        public void TestEquals2() {
            Formula f = new Formula("1 + 2");
            Formula g = new Formula("1+2");
            Assert.IsTrue(f.Equals(g));
        }

        [TestMethod]
        public void TestEquals3() {
            Formula f = new Formula("17 * 4 -3");
            Formula g = new Formula("4 * 17 - 3");
            Assert.IsFalse(f.Equals(g));
        }

        [TestMethod]
        public void TestEquals4() {
            Formula f = new Formula("a + b");
            Formula g = new Formula("b + a");
            Assert.IsFalse(f.Equals(g));
        }

        [TestMethod]
        public void TestGetVariables1() {
			bool result = true;	// Result to report to Assert()

            Formula f = new Formula("a1 + b2 - 5 + c3");
            HashSet<string> a = (HashSet<string>)f.GetVariables();

            HashSet<string> b = new HashSet<string> { "a1", "b2", "c3" };
            int count = b.Count;	// Used to keep track of tokens in b

            foreach (string s in a) {
				if (!b.Contains(s)){
                    result = false;
                    break;
                }
                count++;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetVariables2() {
			bool result = true;	// Result to report to Assert()

            Formula f = new Formula("(1 + 2) * _ - 4");
            HashSet<string> a = (HashSet<string>)f.GetVariables();

            HashSet<string> b = new HashSet<string> { "_" };
            int count = b.Count;

            foreach (string s in a) {
				if (!b.Contains(s)){
                    result = false;
                    break;
                }
                count++;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetVariables3() {
			bool result = true;	// Result to report to Assert()

            Formula f = new Formula("a1");
            HashSet<string> a = (HashSet<string>)f.GetVariables();

            HashSet<string> b = new HashSet<string> { "a1" };
            int count = b.Count;

            foreach (string s in a) {
				if (!b.Contains(s)){
                    result = false;
                    break;
                }
                count++;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetVariables4() {
			bool result = true;	// Result to report to Assert()

            Formula f = new Formula("x123 - 4 + y123");
            HashSet<string> a = (HashSet<string>)f.GetVariables();

            HashSet<string> b = new HashSet<string> { "x123", "y123" };
            int count = b.Count;

            foreach (string s in a) {
				if (!b.Contains(s)){
                    result = false;
                    break;
                }
                count++;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetVariables5() {
			bool result = true;	// Result to report to Assert()

            Formula f = new Formula("_ * (abc) - j2");
            HashSet<string> a = (HashSet<string>)f.GetVariables();

            HashSet<string> b = new HashSet<string> { "_", "abc", "j2" };
            int count = b.Count;

            foreach (string s in a) {
				if (!b.Contains(s)){
                    result = false;
                    break;
                }
                count++;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestEqualsOperator1() {
            Formula f = new Formula("5 + 7");
            Formula g = new Formula("5+7");
            Assert.IsTrue(f == g);
        }

        [TestMethod]
        public void TestEqualsOperator2() {
            Formula f = new Formula("5 + 7");
            Formula g = new Formula("7+5");
            Assert.IsFalse(f == g);
        }

        [TestMethod]
        public void TestNotEqualsOperator1() {
            Formula f = new Formula("5 + 7");
            Formula g = new Formula("5+7");
            Assert.IsFalse(f != g);
        }

        [TestMethod]
        public void TestNotEqualsOperator2() {
            Formula f = new Formula("5 + 7");
            Formula g = new Formula("7+5");
            Assert.IsTrue(f != g);
        }

        [TestMethod]
        public void TestEquals() {
            Formula f = new Formula("5");
            Assert.IsFalse(f.Equals(null));
        }

        [TestMethod]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestBadTokens1() {
            Formula f = new Formula("5 %");
        }

		// ************************** TESTS EVALUATIONS ************************* //

        [TestMethod]
        public void TestEvaluate1() {
            Formula f = new Formula("7 + 5");
            Assert.AreEqual("12", f.Evaluate(s => { return 0; }).ToString());
        }

        [TestMethod()]
        public void TestEvaluate2() {
            Formula f = new Formula("5");
            Assert.AreEqual(5.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate3() {
            Formula f = new Formula("X5");
            Assert.AreEqual(13.0, f.Evaluate(s => 13));
        }

        [TestMethod()]
        public void TestEvaluate4() {
            Formula f = new Formula("5+3");
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate5() {
            Formula f = new Formula("18-10");
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate6() {
            Formula f = new Formula("2*4");
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate7() {
            Formula f = new Formula("16/2");
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate8() {
            Formula f = new Formula("2+X1");
            Assert.AreEqual(6.0, f.Evaluate(s => 4));
        }

        [TestMethod()]
        public void TestEvaluate9() {
            Formula f = new Formula("2+X1");
            object error = f.Evaluate(s => { throw new ArgumentException("Unknown variable"); });
            Assert.IsTrue(error.GetType() == typeof(FormulaError));
        }

        [TestMethod()]
        public void TestEvaluate10() {
            Formula f = new Formula("2*6+3");
            Assert.AreEqual(15.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate11() {
            Formula f = new Formula("2*6+3");
            Assert.AreEqual(15.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate12() {
            Formula f = new Formula("(2+6)*3");
            Assert.AreEqual(24.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate13() {
            Formula f = new Formula("2*(3+5)");
            Assert.AreEqual(16.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate14() {
            Formula f = new Formula("2+(3+5)");
            Assert.AreEqual(10.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate15() {
            Formula f = new Formula("2+(3+5*9)");
            Assert.AreEqual(50.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate16() {
            Formula f = new Formula("2+3*(3+5)");
            Assert.AreEqual(26.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate17() {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.AreEqual(194.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate18() {
            Formula f = new Formula("5/0");
            object error = f.Evaluate(s => 0);
            Assert.IsTrue(error.GetType() == typeof(FormulaError));
        }

        [TestMethod()]
        public void TestEvaluate19() {
            Formula f = new Formula("xx");
            Assert.AreEqual(0.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestEvaluate20() {
            Formula f = new Formula("5+xx");
            Assert.AreEqual(5.0, f.Evaluate(s => 0));
        }

        [TestMethod()]		[ExpectedException(typeof(FormulaFormatException))]
        public void TestEvaluate21() {
            Formula f = new Formula("5+7+(5)8");
        }

        [TestMethod()]
        public void TestEvaluate22() {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.14285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 0.00000001);
        }

        [TestMethod()]
        public void TestEvaluate23() {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6.0, f.Evaluate(s => 1));
        }

        [TestMethod()]
        public void TestEvaluate24() {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12.0, f.Evaluate(s => 2));
        }

        [TestMethod()]
        public void TestEvaluate25() {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0.0, f.Evaluate(s => 3));
        }

        [TestMethod()]
        public void TestEvaluate26() {
            Formula f = new Formula("(5*4)");
            Assert.AreEqual(20.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
		[ExpectedException(typeof(FormulaFormatException))]
        public void TestEvaluate27() {
            Formula f = new Formula("(5**4)");
        }
    }
}
