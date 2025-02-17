using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adaptit.Training.JobVacancy.Data.Migrations
{
    /// <inheritdoc />
    public partial class companyaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    StreetNumber = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => new { x.Country, x.City, x.Street, x.StreetNumber, x.PostalCode });
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Website = table.Column<string>(type: "text", nullable: true),
                    Vat = table.Column<string>(type: "text", nullable: true),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    AddressCountry = table.Column<string>(type: "text", nullable: true),
                    AddressCity = table.Column<string>(type: "text", nullable: true),
                    AddressStreet = table.Column<string>(type: "text", nullable: true),
                    AddressStreetNumber = table.Column<string>(type: "text", nullable: true),
                    AddressPostalCode = table.Column<string>(type: "text", nullable: true),
                    Sponsored = table.Column<bool>(type: "boolean", nullable: false),
                    TotalJobsAdvertised = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AddressCity1 = table.Column<string>(type: "text", nullable: true),
                    AddressCountry1 = table.Column<string>(type: "text", nullable: true),
                    AddressPostalCode1 = table.Column<string>(type: "text", nullable: true),
                    AddressStreet1 = table.Column<string>(type: "text", nullable: true),
                    AddressStreetNumber1 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Address_AddressCountry1_AddressCity1_AddressStreet1~",
                        columns: x => new { x.AddressCountry1, x.AddressCity1, x.AddressStreet1, x.AddressStreetNumber1, x.AddressPostalCode1 },
                        principalTable: "Address",
                        principalColumns: new[] { "Country", "City", "Street", "StreetNumber", "PostalCode" });
                    table.ForeignKey(
                        name: "FK_Company_Address_AddressCountry_AddressCity_AddressStreet_Ad~",
                        columns: x => new { x.AddressCountry, x.AddressCity, x.AddressStreet, x.AddressStreetNumber, x.AddressPostalCode },
                        principalTable: "Address",
                        principalColumns: new[] { "Country", "City", "Street", "StreetNumber", "PostalCode" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_AddressCountry_AddressCity_AddressStreet_AddressStr~",
                table: "Company",
                columns: new[] { "AddressCountry", "AddressCity", "AddressStreet", "AddressStreetNumber", "AddressPostalCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Company_AddressCountry1_AddressCity1_AddressStreet1_Address~",
                table: "Company",
                columns: new[] { "AddressCountry1", "AddressCity1", "AddressStreet1", "AddressStreetNumber1", "AddressPostalCode1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
