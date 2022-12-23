using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceApp.Data.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'fd65933c-5611-4655-8239-bf1a29929c7a', N'admin@system.com', N'ADMIN@SYSTEM.COM', N'admin@system.com', N'ADMIN@SYSTEM.COM', 1, N'AQAAAAEAACcQAAAAEJIBzwQ6JekyPzgMxEcK8umtwYHCjRtYH0T35oWbuoka+X8i5QdNMN2yGcyqB719UQ==', N'MQD5B5ATQGBMGIC2LTCSHDTGPHWFD3Y3', N'2cda823e-75a0-4db5-8fa6-9a3e1495bb0d', NULL, 0, 0, NULL, 1, 0)\r\n");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = 'fd65933c-5611-4655-8239-bf1a29929c7a'");

        }
    }
}
