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
delete from RegattaOldclasses
delete from RegattaCompetitions
delete from RegattaClubs
delete from Regattas
GO

USE [RMDB]
SET IDENTITY_INSERT [RMDB].[dbo].[Regattas] ON
GO
INSERT INTO [RMDB].[dbo].[Regattas] ()
(SELECT MemberId, Birthyear, ClubId, FirstName, LastName, Gender, RentYear, RentedToClubId, isRented FROM RegattaMeldung.dbo.Members
WHERE MemberId NOT IN
(SELECT MemberId FROM RMDB.dbo.Regattas))
GO
SET IDENTITY_INSERT [RMDB].[dbo].[Regattas] OFF
GO

USE [RMDB]
SET IDENTITY_INSERT [RMDB].[dbo].[Members] ON
GO
INSERT INTO [RMDB].[dbo].[Members] (MemberId, Birthyear, ClubId, FirstName, LastName, Gender, RentYear, RentedToClubId, isRented)
(SELECT MemberId, Birthyear, ClubId, FirstName, LastName, Gender, RentYear, RentedToClubId, isRented FROM RegattaMeldung.dbo.Members
WHERE MemberId NOT IN
(SELECT MemberId FROM RMDB.dbo.Members))
GO
SET IDENTITY_INSERT [RMDB].[dbo].[Members] OFF
GO

USE [RMDB]
SET IDENTITY_INSERT [RMDB].[dbo].[ReportedStartboats] ON
GO
INSERT INTO [RMDB].[dbo].[ReportedStartboats] (ReportedStartboatId,ClubId,CompetitionId,Gender,RegattaId,ReportedRaceId,isLate,modifiedDate,NoStartslot)
(SELECT ReportedStartboatId,ClubId,CompetitionId,Gender,RegattaId,ReportedRaceId,isLate,modifiedDate,NoStartslot
FROM [RegattaMeldung].[dbo].[ReportedStartboats]
WHERE ReportedStartboatId NOT IN
(SELECT ReportedStartboatId FROM RMDB.dbo.ReportedStartboats))
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

DELETE FROM StartboatMembers 
WHERE StartboatId IN
(
SELECT ssb.StartboatId FROM Startboats ssb
INNER JOIN Races r ON ssb.RaceId = r.RaceId
WHERE StartboatId NOT IN (
SELECT sb.StartboatId
FROM Startboats sb
LEFT JOIN ReportedStartboats rsb ON sb.ReportedStartboatId = rsb.ReportedStartboatId
INNER JOIN ReportedStartboatMembers rsbm ON rsb.ReportedStartboatId = rsbm.ReportedStartboatId
))

DELETE FROM StartboatStandbys
WHERE StartboatId IN
(
SELECT ssb.StartboatId FROM Startboats ssb
INNER JOIN Races r ON ssb.RaceId = r.RaceId
WHERE StartboatId NOT IN (
SELECT sb.StartboatId
FROM Startboats sb
LEFT JOIN ReportedStartboats rsb ON sb.ReportedStartboatId = rsb.ReportedStartboatId
INNER JOIN ReportedStartboatMembers rsbm ON rsb.ReportedStartboatId = rsbm.ReportedStartboatId
))

DELETE FROM RMDB.dbo.Startboats
WHERE StartboatId IN
(
SELECT ssb.StartboatId FROM Startboats ssb
INNER JOIN RMDB.dbo.Races r ON ssb.RaceId = r.RaceId
WHERE StartboatId NOT IN (
SELECT sb.StartboatId
FROM RMDB.dbo.Startboats sb
LEFT JOIN RegattaMeldung.dbo.ReportedStartboats rsb ON sb.ReportedStartboatId = rsb.ReportedStartboatId
INNER JOIN RegattaMeldung.dbo.ReportedStartboatMembers rsbm ON rsb.ReportedStartboatId = rsbm.ReportedStartboatId
))

USE [RMDB]
DELETE FROM RMDB.dbo.ReportedStartboats
WHERE ReportedStartboatId NOT IN
(SELECT ReportedStartboatId FROM RegattaMeldung.dbo.ReportedStartboats)
GO