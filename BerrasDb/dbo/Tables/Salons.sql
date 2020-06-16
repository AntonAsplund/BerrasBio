CREATE TABLE [dbo].[Salons] (
    [SalonId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Salons] PRIMARY KEY CLUSTERED ([SalonId] ASC)
);

