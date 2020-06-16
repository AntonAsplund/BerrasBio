/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
USE [BerrasBio]
GO



INSERT INTO dbo.Salons([Name])
VALUES ('Drottningen');
INSERT INTO dbo.Salons([Name])
VALUES ('Kungen');



DECLARE @i int = 1;
WHILE @i <= 50 -- insert 100 rows.  change this value to whatever you want.
BEGIN
	INSERT INTO dbo.Seats(SalonId, SeatNumber)
	VALUES (1, @i);
	INSERT INTO dbo.Seats(SalonId, SeatNumber)
	VALUES (2, @i);
	SET @i = @i + 1;
END

DECLARE @y int = 51;
WHILE @y <= 100 -- insert 100 rows.  change this value to whatever you want.
BEGIN
	INSERT INTO dbo.Seats(SalonId, SeatNumber)
	VALUES (1, @y);
	SET @y = @y + 1;
END
GO