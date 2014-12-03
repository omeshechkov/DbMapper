using System;

namespace DbMapper.Oracle
{
    public interface IOracleQueryBuilder : IQueryBuilder
    {
        IFunction<TFunction> Function<TFunction>();
        
        IProcedure<TProcedure> Procedure<TProcedure>();
    }

    public interface IFunction<out TFunction>
    {
        TResult Call<TResult>(Func<TFunction, TResult> func);
    }

    public interface IProcedure<out TProcedure>
    {
        void Call(Action<TProcedure> func);
    }
}