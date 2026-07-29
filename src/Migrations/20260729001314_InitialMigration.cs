using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnuncieCompre.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AwaitingResponseNodeId = table.Column<string>(type: "text", nullable: false),
                    DateTimeLastMessage = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Attendant = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    User_CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    User_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User_Email_Value = table.Column<string>(type: "text", nullable: true),
                    User_Name_Value = table.Column<string>(type: "text", nullable: true),
                    User_Phone_Value = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email_Value = table.Column<string>(type: "text", nullable: true),
                    Name_Value = table.Column<string>(type: "text", nullable: true),
                    Phone_Value = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    SenderType = table.Column<int>(type: "integer", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    Conversation_Attendant = table.Column<int>(type: "integer", nullable: false),
                    Conversation_AwaitingResponseNodeId = table.Column<string>(type: "text", nullable: false),
                    Conversation_CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Conversation_DateTimeLastMessage = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Conversation_EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Conversation_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Conversation_Status = table.Column<int>(type: "integer", nullable: false),
                    Conversation_UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Conversation_User_CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Conversation_User_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Conversation_User_Email_Value = table.Column<string>(type: "text", nullable: true),
                    Conversation_User_Name_Value = table.Column<string>(type: "text", nullable: true),
                    Conversation_User_Phone_Value = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Category_Value = table.Column<int>(type: "integer", nullable: false),
                    Product_Value = table.Column<string>(type: "text", nullable: true),
                    Quantity_Value = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
