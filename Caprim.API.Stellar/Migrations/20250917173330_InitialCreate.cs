using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Caprim.API.Stellar.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Issuer = table.Column<string>(type: "character varying(56)", maxLength: 56, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bank_account_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_account_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "document_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kyc_levels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LevelName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kyc_levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "exchange_rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseAssetId = table.Column<int>(type: "integer", nullable: false),
                    QuoteAssetId = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    Provider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseAssetId1 = table.Column<int>(type: "integer", nullable: true),
                    QuoteAssetId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exchange_rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exchange_rates_assets_BaseAssetId",
                        column: x => x.BaseAssetId,
                        principalTable: "assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exchange_rates_assets_BaseAssetId1",
                        column: x => x.BaseAssetId1,
                        principalTable: "assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_exchange_rates_assets_QuoteAssetId",
                        column: x => x.QuoteAssetId,
                        principalTable: "assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exchange_rates_assets_QuoteAssetId1",
                        column: x => x.QuoteAssetId1,
                        principalTable: "assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CognitoSub = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserStatusId = table.Column<int>(type: "integer", nullable: false),
                    KycLevelId = table.Column<int>(type: "integer", nullable: false),
                    KycDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_kyc_levels_KycLevelId",
                        column: x => x.KycLevelId,
                        principalTable: "kyc_levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_user_statuses_UserStatusId",
                        column: x => x.UserStatusId,
                        principalTable: "user_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exchange_rates_history",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExchangeRateId = table.Column<int>(type: "integer", nullable: false),
                    OldRate = table.Column<decimal>(type: "numeric(18,8)", nullable: true),
                    NewRate = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exchange_rates_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exchange_rates_history_exchange_rates_ExchangeRateId",
                        column: x => x.ExchangeRateId,
                        principalTable: "exchange_rates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bank_accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    EncryptedAccountNumber = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HolderName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    HolderDocumentId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    HolderDocumentTypeId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bank_accounts_bank_account_types_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "bank_account_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bank_accounts_banks_BankId",
                        column: x => x.BankId,
                        principalTable: "banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bank_accounts_document_types_HolderDocumentTypeId",
                        column: x => x.HolderDocumentTypeId,
                        principalTable: "document_types",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_bank_accounts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stellar_accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PublicKey = table.Column<string>(type: "character varying(56)", maxLength: 56, nullable: false),
                    KmsKeyArn = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stellar_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stellar_accounts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EncryptedFirstName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EncryptedLastName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EncryptedAddress = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EncryptedPhone = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EncryptedDocumentNumber = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: true),
                    Nationality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_profiles_document_types_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "document_types",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_user_profiles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StellarAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    StellarTxHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    AssetId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StellarAccountId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    AssetId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_assets_AssetId1",
                        column: x => x.AssetId1,
                        principalTable: "assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_transactions_stellar_accounts_StellarAccountId",
                        column: x => x.StellarAccountId,
                        principalTable: "stellar_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_stellar_accounts_StellarAccountId1",
                        column: x => x.StellarAccountId1,
                        principalTable: "stellar_accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_AccountTypeId",
                table: "bank_accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_BankId",
                table: "bank_accounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_HolderDocumentTypeId",
                table: "bank_accounts",
                column: "HolderDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_UserId",
                table: "bank_accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_exchange_rates_BaseAssetId",
                table: "exchange_rates",
                column: "BaseAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_exchange_rates_BaseAssetId1",
                table: "exchange_rates",
                column: "BaseAssetId1");

            migrationBuilder.CreateIndex(
                name: "IX_exchange_rates_QuoteAssetId",
                table: "exchange_rates",
                column: "QuoteAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_exchange_rates_QuoteAssetId1",
                table: "exchange_rates",
                column: "QuoteAssetId1");

            migrationBuilder.CreateIndex(
                name: "IX_exchange_rates_history_ExchangeRateId",
                table: "exchange_rates_history",
                column: "ExchangeRateId");

            migrationBuilder.CreateIndex(
                name: "IX_stellar_accounts_UserId",
                table: "stellar_accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_AssetId",
                table: "transactions",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_AssetId1",
                table: "transactions",
                column: "AssetId1");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_StellarAccountId",
                table: "transactions",
                column: "StellarAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_StellarAccountId1",
                table: "transactions",
                column: "StellarAccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_DocumentTypeId",
                table: "user_profiles",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_UserId",
                table: "user_profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_KycLevelId",
                table: "users",
                column: "KycLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_users_UserStatusId",
                table: "users",
                column: "UserStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bank_accounts");

            migrationBuilder.DropTable(
                name: "exchange_rates_history");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "bank_account_types");

            migrationBuilder.DropTable(
                name: "banks");

            migrationBuilder.DropTable(
                name: "exchange_rates");

            migrationBuilder.DropTable(
                name: "stellar_accounts");

            migrationBuilder.DropTable(
                name: "document_types");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "kyc_levels");

            migrationBuilder.DropTable(
                name: "user_statuses");
        }
    }
}
