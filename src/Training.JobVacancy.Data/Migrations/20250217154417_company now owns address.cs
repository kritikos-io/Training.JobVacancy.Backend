using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adaptit.Training.JobVacancy.Data.Migrations
{
    /// <inheritdoc />
    public partial class companynowownsaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Address_AddressCountry1_AddressCity1_AddressStreet1~",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Address_AddressCountry_AddressCity_AddressStreet_Ad~",
                table: "Company");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Company_AddressCountry_AddressCity_AddressStreet_AddressStr~",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_AddressCountry1_AddressCity1_AddressStreet1_Address~",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AddressCity",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AddressCity1",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AddressCountry",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AddressCountry1",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "AddressPostalCode",
                table: "Company");

            migrationBuilder.RenameColumn(
                name: "AddressStreetNumber1",
                table: "Company",
                newName: "Address_StreetNumber");

            migrationBuilder.RenameColumn(
                name: "AddressStreetNumber",
                table: "Company",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "AddressStreet1",
                table: "Company",
                newName: "Address_PostalCode");

            migrationBuilder.RenameColumn(
                name: "AddressStreet",
                table: "Company",
                newName: "Address_Country");

            migrationBuilder.RenameColumn(
                name: "AddressPostalCode1",
                table: "Company",
                newName: "Address_City");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Company",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_StreetNumber",
                table: "Company",
                newName: "AddressStreetNumber1");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Company",
                newName: "AddressStreetNumber");

            migrationBuilder.RenameColumn(
                name: "Address_PostalCode",
                table: "Company",
                newName: "AddressStreet1");

            migrationBuilder.RenameColumn(
                name: "Address_Country",
                table: "Company",
                newName: "AddressStreet");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "Company",
                newName: "AddressPostalCode1");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Company",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressCity",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressCity1",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressCountry",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressCountry1",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressPostalCode",
                table: "Company",
                type: "text",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Company_AddressCountry_AddressCity_AddressStreet_AddressStr~",
                table: "Company",
                columns: new[] { "AddressCountry", "AddressCity", "AddressStreet", "AddressStreetNumber", "AddressPostalCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Company_AddressCountry1_AddressCity1_AddressStreet1_Address~",
                table: "Company",
                columns: new[] { "AddressCountry1", "AddressCity1", "AddressStreet1", "AddressStreetNumber1", "AddressPostalCode1" });

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Address_AddressCountry1_AddressCity1_AddressStreet1~",
                table: "Company",
                columns: new[] { "AddressCountry1", "AddressCity1", "AddressStreet1", "AddressStreetNumber1", "AddressPostalCode1" },
                principalTable: "Address",
                principalColumns: new[] { "Country", "City", "Street", "StreetNumber", "PostalCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Address_AddressCountry_AddressCity_AddressStreet_Ad~",
                table: "Company",
                columns: new[] { "AddressCountry", "AddressCity", "AddressStreet", "AddressStreetNumber", "AddressPostalCode" },
                principalTable: "Address",
                principalColumns: new[] { "Country", "City", "Street", "StreetNumber", "PostalCode" });
        }
    }
}
