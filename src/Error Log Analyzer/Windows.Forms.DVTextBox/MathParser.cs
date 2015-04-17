using System;
using System.Collections;
using System.Collections.Generic;

namespace Windows.Forms
{
    public enum Parameters
    {
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }
    public class MathParser
    {
        private Dictionary<Parameters, decimal> _Parameters = new Dictionary<Parameters, decimal>();
        private List<String> OperationOrder = new List<string>();

        public Dictionary<Parameters, decimal> Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }

        public MathParser()
        {
            OperationOrder.Add("/");
            OperationOrder.Add("*");
            OperationOrder.Add("-");
            OperationOrder.Add("+");
        }
        public decimal Calculate(string Formula)
        {
            try
            {
                var arr = Formula.Split("/+-*()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var de in _Parameters)
                {
                    foreach (var s in arr)
                    {
                        if (s != de.Key.ToString() && s.EndsWith(de.Key.ToString()))
                        {
                            Formula = Formula.Replace(s, (Convert.ToDecimal(s.Replace(de.Key.ToString(), "")) * de.Value).ToString());
                        }
                    }
                    Formula = Formula.Replace(de.Key.ToString(), de.Value.ToString());
                }
                while (Formula.LastIndexOf("(") > -1)
                {
                    var lastOpenPhrantesisIndex = Formula.LastIndexOf("(");
                    var firstClosePhrantesisIndexAfterLastOpened = Formula.IndexOf(")", lastOpenPhrantesisIndex);
                    var result = ProcessOperation(Formula.Substring(lastOpenPhrantesisIndex + 1, firstClosePhrantesisIndexAfterLastOpened - lastOpenPhrantesisIndex - 1));
                    var AppendAsterix = false;
                    if (lastOpenPhrantesisIndex > 0)
                    {
                        if (Formula.Substring(lastOpenPhrantesisIndex - 1, 1) != "(" && !OperationOrder.Contains(Formula.Substring(lastOpenPhrantesisIndex - 1, 1)))
                        {
                            AppendAsterix = true;
                        }
                    }

                    Formula = Formula.Substring(0, lastOpenPhrantesisIndex) + (AppendAsterix ? "*" : "") + result.ToString() + Formula.Substring(firstClosePhrantesisIndexAfterLastOpened + 1);

                }
                return ProcessOperation(Formula);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured While Calculating. Check Syntax", ex);
            }
        }

        private decimal ProcessOperation(string operation)
        {
            var arr = new ArrayList();
            var s = "";
            for (var i = 0; i < operation.Length; i++)
            {
                var currentCharacter = operation.Substring(i, 1);
                if (OperationOrder.IndexOf(currentCharacter) > -1)
                {
                    if (s != "")
                    {
                        arr.Add(s);
                    }
                    arr.Add(currentCharacter);
                    s = "";
                }
                else
                {
                    s += currentCharacter;
                }
            }
            arr.Add(s);
            s = "";
            foreach (var op in OperationOrder)
            {
                while (arr.IndexOf(op) > -1)
                {
                    var operatorIndex = arr.IndexOf(op);
                    var digitBeforeOperator = Convert.ToDecimal(arr[operatorIndex - 1]);
                    decimal digitAfterOperator = 0;
                    if (arr[operatorIndex + 1].ToString() == "-")
                    {
                        arr.RemoveAt(operatorIndex + 1);
                        digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]) * -1;
                    }
                    else
                    {
                        digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]);
                    }
                    arr[operatorIndex] = CalculateByOperator(digitBeforeOperator, digitAfterOperator, op);
                    arr.RemoveAt(operatorIndex - 1);
                    arr.RemoveAt(operatorIndex);
                }
            }
            return Convert.ToDecimal(arr[0]);
        }
        private decimal CalculateByOperator(decimal number1, decimal number2, string op)
        {
            if (op == "/")
            {
                return number1 / number2;
            }
            else if (op == "*")
            {
                return number1 * number2;
            }
            else if (op == "-")
            {
                return number1 - number2;
            }
            else if (op == "+")
            {
                return number1 + number2;
            }
            else
            {
                return 0;
            }
        }
    }
}
