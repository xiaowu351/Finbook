using Microsoft.EntityFrameworkCore.Migrations;

namespace Projects.API.Migrations
{
    public partial class BoolToZeroOneConverter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Visible",
                table: "projectVisibleRules",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "ShowSecurityInfo",
                table: "projects",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "OnPlatform",
                table: "projects",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "IsCloser",
                table: "projectContributors",
                nullable: false,
                oldClrType: typeof(short));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Visible",
                table: "projectVisibleRules",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "ShowSecurityInfo",
                table: "projects",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "OnPlatform",
                table: "projects",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "IsCloser",
                table: "projectContributors",
                nullable: false,
                oldClrType: typeof(short));
        }
    }
}
