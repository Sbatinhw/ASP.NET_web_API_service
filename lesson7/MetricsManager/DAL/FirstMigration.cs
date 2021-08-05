using FluentMigrator;

namespace MetricsManager.DAL.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("agents");
            Delete.Table("cpumetrics");
            Delete.Table("rammetrics");
            Delete.Table("hddmetrics");

        }

        public override void Up()
        {

            Create.Table("agents")
                .WithColumn("agentid").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentadress").AsString() ;

            Create.Table("cpumetrics").
                WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64()
                .WithColumn("agentid").AsInt64().ForeignKey("agents", "agentid");

            Create.Table("rammetrics").
                WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt64()
                .WithColumn("Time").AsInt64()
                .WithColumn("agentid").AsInt64().ForeignKey("agents", "agentid");

            Create.Table("hddmetrics").
                WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt64()
                .WithColumn("Time").AsInt64()
                .WithColumn("agentid").AsInt64().ForeignKey("agents", "agentid");
        }
    }
}
