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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250311105720_Initial Migration'
)
BEGIN
    CREATE TABLE [Difficulties] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Difficulties] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250311105720_Initial Migration'
)
BEGIN
    CREATE TABLE [Regions] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [RegionImageUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Regions] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250311105720_Initial Migration'
)
BEGIN
    CREATE TABLE [Walks] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [LenghtInKm] float NOT NULL,
        [WalkImageUrl] nvarchar(max) NULL,
        [DifficultyId] uniqueidentifier NOT NULL,
        [RegionId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Walks] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Walks_Difficulties_DifficultyId] FOREIGN KEY ([DifficultyId]) REFERENCES [Difficulties] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Walks_Regions_RegionId] FOREIGN KEY ([RegionId]) REFERENCES [Regions] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250311105720_Initial Migration'
)
BEGIN
    CREATE INDEX [IX_Walks_DifficultyId] ON [Walks] ([DifficultyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250311105720_Initial Migration'
)
BEGIN
    CREATE INDEX [IX_Walks_RegionId] ON [Walks] ([RegionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250311105720_Initial Migration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250311105720_Initial Migration', N'9.0.2');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250313064348_Seeding data for difficulties and Regions'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Difficulties]'))
        SET IDENTITY_INSERT [Difficulties] ON;
    EXEC(N'INSERT INTO [Difficulties] ([Id], [Name])
    VALUES (''60e28c55-c78e-418d-a864-741e9fd834eb'', N''Hard''),
    (''6e12cb01-ec23-43f3-be03-95d0a2e681ba'', N''Medium''),
    (''c62caf1c-4b01-4ee9-b01f-22712605dfaa'', N''Easy'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Difficulties]'))
        SET IDENTITY_INSERT [Difficulties] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250313064348_Seeding data for difficulties and Regions'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Name', N'RegionImageUrl') AND [object_id] = OBJECT_ID(N'[Regions]'))
        SET IDENTITY_INSERT [Regions] ON;
    EXEC(N'INSERT INTO [Regions] ([Id], [Code], [Name], [RegionImageUrl])
    VALUES (''3012669d-1fdf-43f8-a390-6d506826a3d1'', N''QT'', N''Queenstown'', N''https://unsplash.com/photos/aerial-view-of-city-near-lake-during-daytime-8T8UCBeWuUs''),
    (''b2964597-9765-4ef2-89e4-7510d4583f3d'', N''AKL'', N''Ackland'', N''https://dynamic-media-cdn.tripadvisor.com/media/photo-o/10/dc/e6/a1/enjoy-beautiful-views.jpg?w=600&h=-1&s=1''),
    (''c2d724d7-83c1-4903-b68b-4d55c88cc2bb'', N''WT'', N''Wellington'', N''https://unsplash.com/photos/aerial-view-of-city-buildings-during-sunset-KRtXOMfS4oA'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Name', N'RegionImageUrl') AND [object_id] = OBJECT_ID(N'[Regions]'))
        SET IDENTITY_INSERT [Regions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250313064348_Seeding data for difficulties and Regions'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250313064348_Seeding data for difficulties and Regions', N'9.0.2');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315104740_Adding Images table'
)
BEGIN
    CREATE TABLE [Images] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(max) NOT NULL,
        [FileDescription] nvarchar(max) NULL,
        [FileExtension] nvarchar(max) NOT NULL,
        [FileSizeInBytes] bigint NOT NULL,
        [FilePath] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Images] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315104740_Adding Images table'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250315104740_Adding Images table', N'9.0.2');
END;

COMMIT;
GO

