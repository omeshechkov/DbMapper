namespace DbMapper
{
    public interface IGenerator
    {
        object GetNextValue(Connection connection);
    }
}
