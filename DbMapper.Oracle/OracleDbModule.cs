namespace DbMapper.Oracle
{
    public class OracleDbModule : DefaultDbModule
    {
        public OracleDbModule()
        {
            //TODO register validators
            //RegisterMappingValidator<>();
        }

        public override IQueryBuilder CreateQueryBuilder()
        {
            return new OracleQueryBuilder();
        }
    }
}
