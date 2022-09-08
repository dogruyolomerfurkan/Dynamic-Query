using System.Linq.Expressions;
using System.Reflection;
using WebAPI.Entities;

namespace WebAPI.Builder;

public static class PredicateBuilder<T>
{
    public static Expression<Func<T, bool>>? Predicate(QueryPropertyDto queryPropertyDto)
    {
        var parameterExpression = Expression.Parameter(typeof(T), "person");

        var expressions = new Lazy<List<Expression>>();

        foreach (var queryProperty in queryPropertyDto.QueryProperties)
        {

            var constantExpression = GetConstant(queryProperty.FieldType, queryProperty.Value);

            var memberExpression = Expression.Property(parameterExpression, queryProperty.FieldName);

            Expression binaryExpression = ExpressionBuilder(queryProperty.FieldType, queryProperty.ComparisonOperator, memberExpression, constantExpression);

            expressions.Value.Add(binaryExpression);
        }
        var resultExpression = LogicalOperatorExpression(queryPropertyDto.LogicalOperator, expressions.Value);

        return Expression.Lambda<Func<T, bool>>(resultExpression, parameterExpression);
    }


    private static ConstantExpression GetConstant(string fieldType, string value)
    {
        return fieldType.ToLower() switch
        {
            "int" => Expression.Constant(int.Parse(value)),
            "datetime" => Expression.Constant(DateTime.Parse(value)),
            "guid" => Expression.Constant(Guid.Parse(value)),
            "string" => Expression.Constant(value.ToLower()),
            _ => throw new NotImplementedException()
        };
    }

    private static Expression ExpressionBuilder(string fieldType, string comparisonOperator, MemberExpression memberExpression,
        ConstantExpression constantExpression)
    {
        if (fieldType.ToLower() == "string")
        {
            MethodInfo lowerMethodInfo = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var lowerMethodCallExpression = Expression.Call(memberExpression, lowerMethodInfo);
            MethodInfo containsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var expressionCall = Expression.Call(lowerMethodCallExpression, containsMethodInfo, constantExpression);
            return comparisonOperator == "=" ? expressionCall : Expression.Not(expressionCall);
        }

        return comparisonOperator switch
        {
            ">=" => Expression.GreaterThanOrEqual(memberExpression, constantExpression),
            ">" => Expression.GreaterThan(memberExpression, constantExpression),
            "<=" => Expression.LessThanOrEqual(memberExpression, constantExpression),
            "<" => Expression.LessThan(memberExpression, constantExpression),
            "=" => Expression.Equal(memberExpression, constantExpression),
            "!=" => Expression.NotEqual(memberExpression, constantExpression),
            _ => throw new NotImplementedException(),
        };
    }

    private static Expression LogicalOperatorExpression(string logicalOperator, List<Expression> expressions)
    {
        return logicalOperator.ToLower() switch
        {
            "and" => AndExpression(expressions),
            "or" => OrExpression(expressions),
            _ => throw new NotImplementedException()
        };
    }

    private static Expression AndExpression(List<Expression> expressions)
    {
        Expression binaryExpression = expressions.First();
        if (expressions.Count <= 1) return binaryExpression;
        for (int i = 1; i < expressions.Count; i++)
        {
            binaryExpression = Expression.AndAlso(binaryExpression, expressions[i]);
        }
        return binaryExpression;
    }
    private static Expression OrExpression(List<Expression> binaryExpressions)
    {
        Expression binaryExpression = binaryExpressions.First();
        if (binaryExpressions.Count <= 1) return binaryExpression;
        for (int i = 1; i < binaryExpressions.Count; i++)
        {
            binaryExpression = Expression.OrElse(binaryExpression, binaryExpressions[i]);
        }
        return binaryExpression;
    }
}
