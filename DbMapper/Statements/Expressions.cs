namespace DbMapper.Statements
{
    public interface IExpression { }

    public interface IColumnReferenceExpression : IExpression
    {
        IColumnIdentifier Column { get; }
    }

    public interface IConstantExpression : IExpression
    {
        object Value { get; }
    }

    public interface IUnaryExpression : IExpression
    {
        IExpression Operand { get; }
    }

    public interface INotExpression : IUnaryExpression { }

    public interface IMinusExpression : IUnaryExpression { }

    public interface IBinaryExpression : IExpression
    {
        IExpression LeftOperand { get; }

        IExpression RightOperand { get; }
    }

    public interface IEqualsExpression : IBinaryExpression { }

    public interface INotEqualsExpression : IBinaryExpression { }

    public interface IGreaterThanExpression : IBinaryExpression { }

    public interface IGreaterOrEqualsThanExpression : IBinaryExpression { }

    public interface ILessOrEqualsThanExpression : IBinaryExpression { }

    public interface IAndExpression : IBinaryExpression { }

    public interface IOrExpression : IBinaryExpression { }
}