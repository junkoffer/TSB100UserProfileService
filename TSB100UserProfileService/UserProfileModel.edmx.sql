
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/14/2019 10:07:41
-- Generated from EDMX file: C:\Users\Carolina Preuss\Desktop\TSB100\Grupparbete\TSB100UserProfileService\TSB100UserProfileService\UserProfileModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [UserProfile];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserDb]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDb];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserDb'
CREATE TABLE [dbo].[UserDb] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Username] nvarchar(50)  NULL,
    [FirstName] nvarchar(100)  NULL,
    [Surname] nvarchar(100)  NULL,
    [PersonalIdentityNumber] int  NULL,
    [Address] nvarchar(50)  NULL,
    [ZipCode] int  NULL,
    [City] nvarchar(50)  NULL,
    [PhoneNumber] nvarchar(50)  NULL,
    [Email] nvarchar(50)  NULL,
    [PictureUrl] nvarchar(100)  NULL,
    [UserId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserDb'
ALTER TABLE [dbo].[UserDb]
ADD CONSTRAINT [PK_UserDb]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------