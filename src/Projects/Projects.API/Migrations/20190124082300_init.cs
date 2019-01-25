using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projects.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    UserId = table.Column<int>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    OriginBPFile = table.Column<string>(nullable: true),
                    FormatBPFile = table.Column<string>(nullable: true),
                    ShowSecurityInfo = table.Column<short>(nullable: false),
                    ProvinceId = table.Column<int>(nullable: false),
                    Province = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    AreaId = table.Column<int>(nullable: false),
                    Area = table.Column<string>(nullable: true),
                    RegisterTime = table.Column<DateTime>(nullable: false),
                    Introduction = table.Column<string>(nullable: true),
                    FinPercentage = table.Column<string>(nullable: true),
                    FinStage = table.Column<string>(nullable: true),
                    FinMoney = table.Column<int>(nullable: false),
                    Income = table.Column<int>(nullable: false),
                    Revenue = table.Column<int>(nullable: false),
                    Valuation = table.Column<int>(nullable: false),
                    BrokerageOptions = table.Column<int>(nullable: false),
                    OnPlatform = table.Column<short>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    ReferenceId = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    UpdatetTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "projectContributors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsCloser = table.Column<short>(nullable: false),
                    ContributorType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectContributors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectContributors_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectProperties",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 100, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectProperties", x => new { x.ProjectId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_projectProperties_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectViewers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectViewers_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projectVisibleRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ProjectId = table.Column<int>(nullable: false),
                    Visible = table.Column<short>(nullable: false),
                    Tags = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectVisibleRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projectVisibleRules_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_projectContributors_ProjectId",
                table: "projectContributors",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_projectViewers_ProjectId",
                table: "projectViewers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_projectVisibleRules_ProjectId",
                table: "projectVisibleRules",
                column: "ProjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projectContributors");

            migrationBuilder.DropTable(
                name: "projectProperties");

            migrationBuilder.DropTable(
                name: "projectViewers");

            migrationBuilder.DropTable(
                name: "projectVisibleRules");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
