using System.Linq.Expressions;

namespace RegistryApi.SharedKernel;

internal class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    internal ParameterReplacer(ParameterExpression parameter)
    {
        _parameter = parameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
        => base.VisitParameter(_parameter);
}