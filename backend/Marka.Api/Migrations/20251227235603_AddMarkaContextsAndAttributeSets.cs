using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marka.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMarkaContextsAndAttributeSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MarkaContextId",
                table: "markas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Attributes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "Attributes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Attributes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Attributes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Attributes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "Attributes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Persist",
                table: "Attributes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReadOnly",
                table: "Attributes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attributes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Attributes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttributeSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeSets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttributeSets_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttributeSets_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarkaContexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DefaultRadius = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkaContexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkaContexts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarkaContexts_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarkaContexts_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttributeSetAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarkaAttributeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeSetAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeSetAttributes_AttributeSets_AttributeSetId",
                        column: x => x.AttributeSetId,
                        principalTable: "AttributeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributeSetAttributes_Attributes_MarkaAttributeId",
                        column: x => x.MarkaAttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkaContextAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MarkaContextId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarkaAttributeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeOrder = table.Column<int>(type: "integer", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "boolean", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    FeaturedOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkaContextAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkaContextAttributes_Attributes_MarkaAttributeId",
                        column: x => x.MarkaAttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarkaContextAttributes_MarkaContexts_MarkaContextId",
                        column: x => x.MarkaContextId,
                        principalTable: "MarkaContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_markas_MarkaContextId",
                table: "markas",
                column: "MarkaContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_CreatedBy",
                table: "Attributes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_UpdatedBy",
                table: "Attributes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeSetAttributes_AttributeSetId_AttributeOrder",
                table: "AttributeSetAttributes",
                columns: new[] { "AttributeSetId", "AttributeOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeSetAttributes_AttributeSetId_MarkaAttributeId",
                table: "AttributeSetAttributes",
                columns: new[] { "AttributeSetId", "MarkaAttributeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttributeSetAttributes_MarkaAttributeId",
                table: "AttributeSetAttributes",
                column: "MarkaAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeSets_CreatedBy",
                table: "AttributeSets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeSets_CustomerId_Name",
                table: "AttributeSets",
                columns: new[] { "CustomerId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeSets_UpdatedBy",
                table: "AttributeSets",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MarkaContextAttributes_MarkaAttributeId",
                table: "MarkaContextAttributes",
                column: "MarkaAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkaContextAttributes_MarkaContextId_AttributeOrder",
                table: "MarkaContextAttributes",
                columns: new[] { "MarkaContextId", "AttributeOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MarkaContextAttributes_MarkaContextId_MarkaAttributeId",
                table: "MarkaContextAttributes",
                columns: new[] { "MarkaContextId", "MarkaAttributeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarkaContexts_CreatedBy",
                table: "MarkaContexts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MarkaContexts_CustomerId_Name",
                table: "MarkaContexts",
                columns: new[] { "CustomerId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_MarkaContexts_UpdatedBy",
                table: "MarkaContexts",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Users_CreatedBy",
                table: "Attributes",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Users_UpdatedBy",
                table: "Attributes",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_markas_MarkaContexts_MarkaContextId",
                table: "markas",
                column: "MarkaContextId",
                principalTable: "MarkaContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Users_CreatedBy",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Users_UpdatedBy",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_markas_MarkaContexts_MarkaContextId",
                table: "markas");

            migrationBuilder.DropTable(
                name: "AttributeSetAttributes");

            migrationBuilder.DropTable(
                name: "MarkaContextAttributes");

            migrationBuilder.DropTable(
                name: "AttributeSets");

            migrationBuilder.DropTable(
                name: "MarkaContexts");

            migrationBuilder.DropIndex(
                name: "IX_markas_MarkaContextId",
                table: "markas");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_CreatedBy",
                table: "Attributes");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_UpdatedBy",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "MarkaContextId",
                table: "markas");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "Persist",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "ReadOnly",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Attributes");
        }
    }
}
