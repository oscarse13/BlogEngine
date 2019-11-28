using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BlogEngine.Data.Migrations
{
    public partial class AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Guid user1 = new Guid("c20fbfa1-6031-4188-bc9b-40b809479460");
            Guid user2 = new Guid("33e14e91-76b8-4241-a554-50ecb47cdb31");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "AccessFailedCount", "LockoutEnabled", "TwoFactorEnabled", "PhoneNumberConfirmed" },
                values: new object[] { user1.ToString(), "oscarse@gmail.com", "OSCARSE@GMAIL.COM", "oscarse@gmail.com", "OSCARSE@GMAIL.COM", false, "AQAAAAEAACcQAAAAEH8A10v431Ahq7GFYXSuQVrqUAydkcGqPML6vCCNZObSYvoI/xcEiWBFnkNgl2+yww==" , "VXA7MFADQTUUXVHR4RBMZ2XFW276YX54", "52992f68-d6e4-40e4-8188-f7bd4a7a90b0", 0, false, false, false });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "AccessFailedCount", "LockoutEnabled", "TwoFactorEnabled", "PhoneNumberConfirmed" },
                values: new object[] { user2.ToString(), "celesteq@gmail.com", "CELESTEQ@GMAIL.COM", "celesteq@gmail.com", "CELESTEQ@GMAIL.COM", false, "AQAAAAEAACcQAAAAEH8A10v431Ahq7GFYXSuQVrqUAydkcGqPML6vCCNZObSYvoI/xcEiWBFnkNgl2+yww==", "VXA7MFADQTUUXVHR4RBMZ2XFW276YX54", "52992f68-d6e4-40e4-8188-f7bd4a7a90b0", 0, false, false, false });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name" },
                values: new object[] { "1", "Writer" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name" },
                values: new object[] { "2", "Editor" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { user1.ToString(), "1" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { user2.ToString(), "2" });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
