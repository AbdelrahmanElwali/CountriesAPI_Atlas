IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Countries] (
    [Id] int NOT NULL IDENTITY,
    [CommonName] nvarchar(200) NOT NULL,
    [OfficialName] nvarchar(300) NOT NULL,
    [Cca2] nvarchar(2) NOT NULL,
    [Cca3] nvarchar(3) NOT NULL,
    [Region] nvarchar(100) NOT NULL,
    [Subregion] nvarchar(100) NOT NULL,
    [Capital] nvarchar(200) NOT NULL,
    [Population] bigint NOT NULL,
    [FlagEmoji] nvarchar(10) NOT NULL,
    [FlagPng] nvarchar(max) NOT NULL,
    [Lat] float NOT NULL,
    [Lng] float NOT NULL,
    [RawJson] nvarchar(max) NOT NULL,
    [LastSyncedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Countries_Cca2] ON [Countries] ([Cca2]);

CREATE UNIQUE INDEX [IX_Countries_Cca3] ON [Countries] ([Cca3]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260402115602_InitialCreate', N'9.0.0');

COMMIT;
GO

