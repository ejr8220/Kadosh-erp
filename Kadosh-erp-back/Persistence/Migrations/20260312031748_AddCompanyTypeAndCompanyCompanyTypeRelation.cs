using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyTypeAndCompanyCompanyTypeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Companies_Persons_PersonId1'
)
BEGIN
    ALTER TABLE [Companies] DROP CONSTRAINT [FK_Companies_Persons_PersonId1];
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Companies_PersonId1' AND object_id = OBJECT_ID('dbo.Companies')
)
BEGIN
    DROP INDEX [IX_Companies_PersonId1] ON [Companies];
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE Name = N'PersonId1' AND Object_ID = Object_ID(N'dbo.Companies')
)
BEGIN
    ALTER TABLE [Companies] DROP COLUMN [PersonId1];
END
");

            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Companies_PersonId' AND object_id = OBJECT_ID('dbo.Companies')
)
BEGIN
    DROP INDEX [IX_Companies_PersonId] ON [Companies];
END
");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CompanyTypeId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PersonType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyTypeId",
                table: "Companies",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PersonId",
                table: "Companies",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTypes_Code",
                table: "CompanyTypes",
                column: "Code");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM [CompanyTypes] WHERE [Id] = 1)
BEGIN
    SET IDENTITY_INSERT [CompanyTypes] ON;

    INSERT INTO [CompanyTypes]
        ([Id], [Code], [Name], [PersonType], [Description], [CreatedAt], [CreatedBy], [ModifiedAt], [ModifiedBy], [IsDeleted], [Status])
    VALUES
        (1, '01', 'SIN CLASIFICAR', 'J', 'Registro base para compatibilidad de datos existentes', GETDATE(), 'migration', NULL, NULL, 0, 'A');

    SET IDENTITY_INSERT [CompanyTypes] OFF;
END
");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CompanyTypes_CompanyTypeId",
                table: "Companies",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CompanyTypes_CompanyTypeId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyTypes");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CompanyTypeId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_PersonId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "CompanyTypeId",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PersonId",
                table: "Companies",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PersonId1",
                table: "Companies",
                column: "PersonId1",
                unique: true,
                filter: "[PersonId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Persons_PersonId1",
                table: "Companies",
                column: "PersonId1",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
