using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("cpumetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AgentId").AsInt64().ForeignKey("agentsinfo", "AgentId")
                .WithColumn("MetricId").AsInt64()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
            Create.Table("hddmetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AgentId").AsInt64().ForeignKey("agentsinfo", "AgentId")
                .WithColumn("MetricId").AsInt64()
                .WithColumn("Value").AsInt64()
                .WithColumn("Time").AsInt64();
            Create.Table("rammetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AgentId").AsInt64().ForeignKey("agentsinfo", "AgentId")
                .WithColumn("MetricId").AsInt64()
                .WithColumn("Value").AsInt64()
                .WithColumn("Time").AsInt64();
            Create.Table("agentsinfo")
                .WithColumn("AgentId").AsInt64().PrimaryKey().Identity()
                .WithColumn("AgentAdress").AsString().NotNullable()
                .WithColumn("Enable").AsBoolean().NotNullable();

            
        }
        public override void Down()
        {
            Delete.Table("cpumetrics");
            Delete.Table("hddmetrics");
            Delete.Table("rammetrics");
            Delete.Table("agentsinfo");
        }

        
    }
}
