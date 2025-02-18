using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Fetus_Rec__Pregn__5441852A",
                table: "Fetus_Record");

            migrationBuilder.DropIndex(
                name: "IX_Fetus_Record_Pregnancy_id",
                table: "Fetus_Record");

            migrationBuilder.DropColumn(
                name: "Pregnancy_id",
                table: "Fetus_Record");

            migrationBuilder.CreateTable(
                name: "Fetus",
                columns: table => new
                {
                    Fetus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pregnancy_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Fetus__F1A3E2A3D3A3D3A3", x => x.Fetus_id);
                    table.ForeignKey(
                        name: "FK__Fetus__Pregnancy__4D94879B",
                        column: x => x.Pregnancy_id,
                        principalTable: "Pregnancy",
                        principalColumn: "Pregnancy_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_User_Pregnancy_id",
                table: "Schedule_User",
                column: "Pregnancy_id");

            migrationBuilder.CreateIndex(
                name: "IX_Fetus_Record_Fetus_Id",
                table: "Fetus_Record",
                column: "Fetus_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Fetus_Pregnancy_id",
                table: "Fetus",
                column: "Pregnancy_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fetus_Record_Fetus_Fetus_Id",
                table: "Fetus_Record",
                column: "Fetus_Id",
                principalTable: "Fetus",
                principalColumn: "Fetus_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_User_Pregnancy_Pregnancy_id",
                table: "Schedule_User",
                column: "Pregnancy_id",
                principalTable: "Pregnancy",
                principalColumn: "Pregnancy_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fetus_Record_Fetus_Fetus_Id",
                table: "Fetus_Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_User_Pregnancy_Pregnancy_id",
                table: "Schedule_User");

            migrationBuilder.DropTable(
                name: "Fetus");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_User_Pregnancy_id",
                table: "Schedule_User");

            migrationBuilder.DropIndex(
                name: "IX_Fetus_Record_Fetus_Id",
                table: "Fetus_Record");

            migrationBuilder.AddColumn<int>(
                name: "Pregnancy_id",
                table: "Fetus_Record",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fetus_Record_Pregnancy_id",
                table: "Fetus_Record",
                column: "Pregnancy_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Fetus_Rec__Pregn__5441852A",
                table: "Fetus_Record",
                column: "Pregnancy_id",
                principalTable: "Pregnancy",
                principalColumn: "Pregnancy_id");
        }
    }
}
