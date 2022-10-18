
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/17/2022 13:46:19
-- Generated from EDMX file: C:\Users\caela\Documents\2022-FSD04-DotNet-main\2022-FSD-AppDev1-project\SenkouCards\SenkouDbContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [senkoucards];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_attempts_decks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[attempts] DROP CONSTRAINT [FK_attempts_decks];
GO
IF OBJECT_ID(N'[dbo].[FK_attempts_users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[attempts] DROP CONSTRAINT [FK_attempts_users];
GO
IF OBJECT_ID(N'[dbo].[FK_cards_decks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[cards] DROP CONSTRAINT [FK_cards_decks];
GO
IF OBJECT_ID(N'[dbo].[FK_cardsAudios_cards]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[cardsAudios] DROP CONSTRAINT [FK_cardsAudios_cards];
GO
IF OBJECT_ID(N'[dbo].[FK_cardsImages_cards]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[cardsImages] DROP CONSTRAINT [FK_cardsImages_cards];
GO
IF OBJECT_ID(N'[dbo].[FK_decks_users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[decks] DROP CONSTRAINT [FK_decks_users];
GO
IF OBJECT_ID(N'[dbo].[FK_responses_attempts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[responses] DROP CONSTRAINT [FK_responses_attempts];
GO
IF OBJECT_ID(N'[dbo].[FK_responses_cards]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[responses] DROP CONSTRAINT [FK_responses_cards];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[attempts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[attempts];
GO
IF OBJECT_ID(N'[dbo].[cards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[cards];
GO
IF OBJECT_ID(N'[dbo].[cardsAudios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[cardsAudios];
GO
IF OBJECT_ID(N'[dbo].[cardsImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[cardsImages];
GO
IF OBJECT_ID(N'[dbo].[decks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[decks];
GO
IF OBJECT_ID(N'[dbo].[responses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[responses];
GO
IF OBJECT_ID(N'[dbo].[users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'attempts'
CREATE TABLE [dbo].[attempts] (
    [id] int  NOT NULL,
    [userId] int  NOT NULL,
    [deckId] int  NOT NULL,
    [score] decimal(6,2)  NOT NULL,
    [attemptDate] datetime  NOT NULL
);
GO

-- Creating table 'cards'
CREATE TABLE [dbo].[cards] (
    [id] int  NOT NULL,
    [front] varchar(255)  NOT NULL,
    [back] varchar(255)  NOT NULL,
    [points] int  NOT NULL,
    [deckId] int  NOT NULL
);
GO

-- Creating table 'cardsAudios'
CREATE TABLE [dbo].[cardsAudios] (
    [id] int  NOT NULL,
    [cardId] int  NOT NULL,
    [audio] varbinary(max)  NULL
);
GO

-- Creating table 'cardsImages'
CREATE TABLE [dbo].[cardsImages] (
    [id] int  NOT NULL,
    [cardId] int  NOT NULL,
    [image] varbinary(max)  NOT NULL
);
GO

-- Creating table 'decks'
CREATE TABLE [dbo].[decks] (
    [id] int  NOT NULL,
    [name] varchar(100)  NOT NULL,
    [description] varchar(max)  NOT NULL,
    [isOfficial] bit  NOT NULL,
    [ownerId] int  NOT NULL
);
GO

-- Creating table 'responses'
CREATE TABLE [dbo].[responses] (
    [id] int  NOT NULL,
    [attemptId] int  NOT NULL,
    [cardId] int  NOT NULL,
    [isCorrectResponse] bit  NOT NULL
);
GO

-- Creating table 'users'
CREATE TABLE [dbo].[users] (
    [id] int  NOT NULL,
    [username] varchar(50)  NOT NULL,
    [password] varchar(250)  NOT NULL,
    [score] decimal(17,2)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'attempts'
ALTER TABLE [dbo].[attempts]
ADD CONSTRAINT [PK_attempts]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'cards'
ALTER TABLE [dbo].[cards]
ADD CONSTRAINT [PK_cards]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'cardsAudios'
ALTER TABLE [dbo].[cardsAudios]
ADD CONSTRAINT [PK_cardsAudios]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'cardsImages'
ALTER TABLE [dbo].[cardsImages]
ADD CONSTRAINT [PK_cardsImages]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'decks'
ALTER TABLE [dbo].[decks]
ADD CONSTRAINT [PK_decks]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'responses'
ALTER TABLE [dbo].[responses]
ADD CONSTRAINT [PK_responses]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'users'
ALTER TABLE [dbo].[users]
ADD CONSTRAINT [PK_users]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [deckId] in table 'attempts'
ALTER TABLE [dbo].[attempts]
ADD CONSTRAINT [FK_attempts_decks]
    FOREIGN KEY ([deckId])
    REFERENCES [dbo].[decks]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_attempts_decks'
CREATE INDEX [IX_FK_attempts_decks]
ON [dbo].[attempts]
    ([deckId]);
GO

-- Creating foreign key on [userId] in table 'attempts'
ALTER TABLE [dbo].[attempts]
ADD CONSTRAINT [FK_attempts_users]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[users]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_attempts_users'
CREATE INDEX [IX_FK_attempts_users]
ON [dbo].[attempts]
    ([userId]);
GO

-- Creating foreign key on [attemptId] in table 'responses'
ALTER TABLE [dbo].[responses]
ADD CONSTRAINT [FK_responses_attempts]
    FOREIGN KEY ([attemptId])
    REFERENCES [dbo].[attempts]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_responses_attempts'
CREATE INDEX [IX_FK_responses_attempts]
ON [dbo].[responses]
    ([attemptId]);
GO

-- Creating foreign key on [deckId] in table 'cards'
ALTER TABLE [dbo].[cards]
ADD CONSTRAINT [FK_cards_decks]
    FOREIGN KEY ([deckId])
    REFERENCES [dbo].[decks]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cards_decks'
CREATE INDEX [IX_FK_cards_decks]
ON [dbo].[cards]
    ([deckId]);
GO

-- Creating foreign key on [cardId] in table 'cardsAudios'
ALTER TABLE [dbo].[cardsAudios]
ADD CONSTRAINT [FK_cardsAudios_cards]
    FOREIGN KEY ([cardId])
    REFERENCES [dbo].[cards]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cardsAudios_cards'
CREATE INDEX [IX_FK_cardsAudios_cards]
ON [dbo].[cardsAudios]
    ([cardId]);
GO

-- Creating foreign key on [cardId] in table 'cardsImages'
ALTER TABLE [dbo].[cardsImages]
ADD CONSTRAINT [FK_cardsImages_cards]
    FOREIGN KEY ([cardId])
    REFERENCES [dbo].[cards]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_cardsImages_cards'
CREATE INDEX [IX_FK_cardsImages_cards]
ON [dbo].[cardsImages]
    ([cardId]);
GO

-- Creating foreign key on [cardId] in table 'responses'
ALTER TABLE [dbo].[responses]
ADD CONSTRAINT [FK_responses_cards]
    FOREIGN KEY ([cardId])
    REFERENCES [dbo].[cards]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_responses_cards'
CREATE INDEX [IX_FK_responses_cards]
ON [dbo].[responses]
    ([cardId]);
GO

-- Creating foreign key on [ownerId] in table 'decks'
ALTER TABLE [dbo].[decks]
ADD CONSTRAINT [FK_decks_users]
    FOREIGN KEY ([ownerId])
    REFERENCES [dbo].[users]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_decks_users'
CREATE INDEX [IX_FK_decks_users]
ON [dbo].[decks]
    ([ownerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------