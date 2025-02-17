using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Account_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Full_Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone_Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Is_external = table.Column<bool>(type: "bit", nullable: true),
                    External_provider = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__B19D418153C218A9", x => x.Account_id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Media_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Media__278CBDD334309D6C", x => x.Media_id);
                });

            migrationBuilder.CreateTable(
                name: "Membership_plan",
                columns: table => new
                {
                    Membership_plan_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__0A7DEA259F8CD130", x => x.Membership_plan_id);
                });

            migrationBuilder.CreateTable(
                name: "Pregnancy_Standard",
                columns: table => new
                {
                    Pregnancy_Standard_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true),
                    Minimum = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Maximum = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pregnanc__41CCD4377106EC7A", x => x.Pregnancy_Standard_id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule_Template",
                columns: table => new
                {
                    Schedule_Template_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__112A047641EEFD5D", x => x.Schedule_Template_id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule_User",
                columns: table => new
                {
                    Schedule_User_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pregnancy_id = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__CEFD2C9D1F53F754", x => x.Schedule_User_id);
                });

            migrationBuilder.CreateTable(
                name: "Blog_post",
                columns: table => new
                {
                    Blog_post_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_id = table.Column<int>(type: "int", nullable: true),
                    author_id = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    published_day = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Blog_pos__3FD703BF2C193ECB", x => x.Blog_post_id);
                    table.ForeignKey(
                        name: "FK__Blog_post__autho__4316F928",
                        column: x => x.author_id,
                        principalTable: "Account",
                        principalColumn: "Account_id");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_id = table.Column<int>(type: "int", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Via = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Transaction_code = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__DA638B192282CC45", x => x.Payment_id);
                    table.ForeignKey(
                        name: "FK__Payment__Account__403A8C7D",
                        column: x => x.Account_id,
                        principalTable: "Account",
                        principalColumn: "Account_id");
                });

            migrationBuilder.CreateTable(
                name: "Pregnancy",
                columns: table => new
                {
                    Pregnancy_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_id = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    End_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pregnanc__9BB559690F4E4C0D", x => x.Pregnancy_id);
                    table.ForeignKey(
                        name: "FK__Pregnancy__Accou__5165187F",
                        column: x => x.Account_id,
                        principalTable: "Account",
                        principalColumn: "Account_id");
                });

            migrationBuilder.CreateTable(
                name: "Account_membership",
                columns: table => new
                {
                    membership_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_id = table.Column<int>(type: "int", nullable: true),
                    Membership_plan_id = table.Column<int>(type: "int", nullable: true),
                    Start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    End_date = table.Column<DateOnly>(type: "date", nullable: true),
                    Payment_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Payment_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Transaction_code = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account___CAE49DDDAE6AC4FE", x => x.membership_id);
                    table.ForeignKey(
                        name: "FK__Account_m__Accou__3C69FB99",
                        column: x => x.Account_id,
                        principalTable: "Account",
                        principalColumn: "Account_id");
                    table.ForeignKey(
                        name: "FK__Account_m__Membe__3D5E1FD2",
                        column: x => x.Membership_plan_id,
                        principalTable: "Membership_plan",
                        principalColumn: "Membership_plan_id");
                });

            migrationBuilder.CreateTable(
                name: "Blog_bookmark",
                columns: table => new
                {
                    Blog_bookmark_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_id = table.Column<int>(type: "int", nullable: true),
                    post_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Blog_boo__43525A619F3E70B6", x => x.Blog_bookmark_id);
                    table.ForeignKey(
                        name: "FK__Blog_book__accou__49C3F6B7",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "Account_id");
                    table.ForeignKey(
                        name: "FK__Blog_book__post___4AB81AF0",
                        column: x => x.post_id,
                        principalTable: "Blog_post",
                        principalColumn: "Blog_post_id");
                });

            migrationBuilder.CreateTable(
                name: "Blog_like",
                columns: table => new
                {
                    Blog_like_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_id = table.Column<int>(type: "int", nullable: true),
                    post_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Blog_lik__3AE47BBD432EF71A", x => x.Blog_like_id);
                    table.ForeignKey(
                        name: "FK__Blog_like__Accou__45F365D3",
                        column: x => x.Account_id,
                        principalTable: "Account",
                        principalColumn: "Account_id");
                    table.ForeignKey(
                        name: "FK__Blog_like__post___46E78A0C",
                        column: x => x.post_id,
                        principalTable: "Blog_post",
                        principalColumn: "Blog_post_id");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account_id = table.Column<int>(type: "int", nullable: true),
                    Blog_post_id = table.Column<int>(type: "int", nullable: true),
                    Reply_id = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comment__99D3E6C3A6984A5C", x => x.Comment_id);
                    table.ForeignKey(
                        name: "FK__Comment__Account__5AEE82B9",
                        column: x => x.Account_id,
                        principalTable: "Account",
                        principalColumn: "Account_id");
                    table.ForeignKey(
                        name: "FK__Comment__Blog_po__5BE2A6F2",
                        column: x => x.Blog_post_id,
                        principalTable: "Blog_post",
                        principalColumn: "Blog_post_id");
                });

            migrationBuilder.CreateTable(
                name: "Fetus_Record",
                columns: table => new
                {
                    Fetus_Record_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pregnancy_id = table.Column<int>(type: "int", nullable: true),
                    Fetus_Id = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true),
                    Input_Period = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    BPD = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    HC = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Fetus_Re__11E3575A507B3844", x => x.Fetus_Record_id);
                    table.ForeignKey(
                        name: "FK__Fetus_Rec__Pregn__5441852A",
                        column: x => x.Pregnancy_id,
                        principalTable: "Pregnancy",
                        principalColumn: "Pregnancy_id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Account__A9D10534C1B03E34",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_membership_Account_id",
                table: "Account_membership",
                column: "Account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Account_membership_Membership_plan_id",
                table: "Account_membership",
                column: "Membership_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_bookmark_account_id",
                table: "Blog_bookmark",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_bookmark_post_id",
                table: "Blog_bookmark",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_like_Account_id",
                table: "Blog_like",
                column: "Account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_like_post_id",
                table: "Blog_like",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_post_author_id",
                table: "Blog_post",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Account_id",
                table: "Comment",
                column: "Account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Blog_post_id",
                table: "Comment",
                column: "Blog_post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Fetus_Record_Pregnancy_id",
                table: "Fetus_Record",
                column: "Pregnancy_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Account_id",
                table: "Payment",
                column: "Account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Pregnancy_Account_id",
                table: "Pregnancy",
                column: "Account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_membership");

            migrationBuilder.DropTable(
                name: "Blog_bookmark");

            migrationBuilder.DropTable(
                name: "Blog_like");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Fetus_Record");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Pregnancy_Standard");

            migrationBuilder.DropTable(
                name: "Schedule_Template");

            migrationBuilder.DropTable(
                name: "Schedule_User");

            migrationBuilder.DropTable(
                name: "Membership_plan");

            migrationBuilder.DropTable(
                name: "Blog_post");

            migrationBuilder.DropTable(
                name: "Pregnancy");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuitionFee = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classrooms_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_SubjectId",
                table: "Classrooms",
                column: "SubjectId");
        }
    }
}
