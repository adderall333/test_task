using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Entities;

namespace BLL.Services
{
    public class FilterQueryParser<T>
        where T : IEntity
    {
        private const char And = '&';
        private const char Or = '|';

        private const char Equal = '=';
        private const char Greater = '>';
        private const char Less = '<';
        
        private readonly Dictionary<string, Func<string, Func<T, bool>>> _specifications;

        public FilterQueryParser(Dictionary<string, Func<string, Func<T, bool>>> specifications)
        {
            _specifications = specifications;
        }
        
        public Func<T, bool> MakeFilter(string queryPart)
        {
            var i = 0;
            while (i < queryPart.Length)
            {
                if (queryPart[i] == '(')
                {
                    i = GoToNextScope(queryPart, i);
                    if (i == -1)
                        return MakeFilter(queryPart.Substring(1, queryPart.Length - 2));
                    if (i >= queryPart.Length)
                        break;
                }

                if (queryPart[i] == Or)
                {
                    var left = MakeFilter(queryPart[..i]);
                    var right = MakeFilter(queryPart[(i + 1)..]);
                    return GetFunc(left, queryPart[i], right);
                }

                i++;
            }

            while (--i >= 0)
            {
                if (queryPart[i] == ')')
                {
                    i = GoToPreviousScope(queryPart, i);
                }
                
                if (queryPart[i] == And)
                {
                    var left = MakeFilter(queryPart[..i]);
                    var right = MakeFilter(queryPart[(i + 1)..]);
                    return GetFunc(left, queryPart[i], right);
                }
            }

            return GetFunc(queryPart);
        }

        private Func<T, bool> GetFunc(string expression)
        {
            var left= expression.Split(Equal, Greater, Less)[0];
            var right= expression.Split(Equal, Greater, Less)[1];
            var operatorName = GetOperatorName(expression);

            var specification = _specifications
                .Where(kvp => 
                    kvp.Key.ToLower().StartsWith(left.ToLower()) &&
                    kvp.Key.ToLower().EndsWith(operatorName.ToLower()))
                .Select(kvp => kvp.Value)
                .First();
            
            var result = specification(right);
            return result;
        }

        private static int GoToNextScope(string str, int i)
        {
            var openScopeIndex = i;
            var openCount = 1;
            var closeCount = 0;
            while (openCount > closeCount)
            {
                i++;
                openCount += str[i] == '(' ? 1 : 0;
                closeCount += str[i] == ')' ? 1 : 0;
            }

            if (i == str.Length - 1 && openScopeIndex == 0) 
                return -1;
            return i + 1;
        }

        private static int GoToPreviousScope(string str, int i)
        {
            var openCount = 0;
            var closeCount = 1;
            while (openCount < closeCount)
            {
                i--;
                openCount += str[i] == '(' ? 1 : 0;
                closeCount += str[i] == ')' ? 1 : 0;
            }
            
            return i - 1;
        }

        private static Func<T, bool> GetFunc(Func<T, bool> left, char operation, Func<T, bool> right)
            => operation switch
            {
                Or => entity => left(entity) || right(entity),
                And => entity => left(entity) && right(entity),
                _ => throw new ArgumentException("There is no such operator")
            };

        private static string GetOperatorName(string expression)
        {
            if (expression.Contains(Equal))
                return "equal";

            if (expression.Contains(Greater))
                return "greater";

            if (expression.Contains(Less))
                return "less";

            throw new ArgumentException("There is no such operator");
        }
    }
}