using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalHealthFallowUp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalRecordFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoalRecords_Users_UserId",
                table: "GoalRecords");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "GoalRecords");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "GoalRecords");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "GoalRecords",
                newName: "DurationInMinutes");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GoalRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImageBase64",
                table: "GoalRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "GoalRecords",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddForeignKey(
                name: "FK_GoalRecords_Users_UserId",
                table: "GoalRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoalRecords_Users_UserId",
                table: "GoalRecords");

            migrationBuilder.DropColumn(
                name: "ImageBase64",
                table: "GoalRecords");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "GoalRecords");

            migrationBuilder.RenameColumn(
                name: "DurationInMinutes",
                table: "GoalRecords",
                newName: "Duration");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GoalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "GoalRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "GoalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_GoalRecords_Users_UserId",
                table: "GoalRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
