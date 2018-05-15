USE [RMDB]
delete from StartboatMembers
delete from StartboatStandbys
delete from Startboats
delete from Races
delete from ReportedStartboatMembers
delete from ReportedStartboatStandbys
delete from ReportedStartboats
delete from ReportedRaces
delete from Members
GO

USE [RMDB]
SET IDENTITY_INSERT [RMDB].[dbo].[Members] ON
GO
INSERT INTO [RMDB].[dbo].[Members] (MemberId, Birthyear, ClubId, FirstName, LastName, Gender, RentYear, RentedToClubId, isRented)
SELECT MemberId, Birthyear, ClubId, FirstName, LastName, Gender, RentYear, RentedToClubId, isRented 
FROM [RegattaMeldung].[dbo].[Members]
GO
SET IDENTITY_INSERT [RMDB].[dbo].[Members] OFF
GO

USE [RMDB]
SET IDENTITY_INSERT [RMDB].[dbo].[ReportedRaces] ON
GO
INSERT INTO [RMDB].[dbo].[ReportedRaces] (ReportedRaceId,CompetitionId,Gender,OldclassId,RaceCode,RegattaId)
SELECT ReportedRaceId,CompetitionId,Gender,OldclassId,RaceCode,RegattaId
FROM [RegattaMeldung].[dbo].[ReportedRaces]
GO
SET IDENTITY_INSERT [RMDB].[dbo].[ReportedRaces] OFF
GO

USE [RMDB]
SET IDENTITY_INSERT [RMDB].[dbo].[ReportedStartboats] ON
GO
INSERT INTO [RMDB].[dbo].[ReportedStartboats] (ReportedStartboatId,ClubId,CompetitionId,Gender,RegattaId,ReportedRaceId)
SELECT ReportedStartboatId,ClubId,CompetitionId,Gender,RegattaId,ReportedRaceId
FROM [RegattaMeldung].[dbo].[ReportedStartboats]
GO
SET IDENTITY_INSERT [RMDB].[dbo].[ReportedStartboats] OFF
GO

USE [RMDB]

INSERT INTO [RMDB].[dbo].[ReportedStartboatMembers] (ReportedStartboatId,MemberId,Seatnumber)
SELECT ReportedStartboatId,MemberId,Seatnumber
FROM [RegattaMeldung].[dbo].[ReportedStartboatMembers]
GO

USE [RMDB]

INSERT INTO [RMDB].[dbo].[ReportedStartboatStandbys] (ReportedStartboatId,MemberId,Standbynumber)
SELECT ReportedStartboatId,MemberId,Standbynumber
FROM [RegattaMeldung].[dbo].[ReportedStartboatStandbys]
GO