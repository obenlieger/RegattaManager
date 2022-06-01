USE [RMDB]
delete from StartboatMembers
delete from StartboatStandbys
delete from Startboats
delete from Races
delete from ReportedStartboatMembers
delete from ReportedStartboatStandbys
delete from ReportedStartboats
delete from ReportedRaces


USE [RMDB]
SET IDENTITY_INSERT [RMDB].[dbo].[ReportedRaces] ON
GO
INSERT INTO [RMDB].[dbo].[ReportedRaces] ([ReportedRaceId]
      ,[CompetitionId]
      ,[modifiedDate]
      ,[OldclassId]
      ,[Gender]
      ,[RaceCode]
      ,[RegattaId]
      ,[Comment]
      ,[isCreated]
      ,[StartboatCount]
      ,[isAbteilungslauf])
(SELECT [ReportedRaceId]
      ,[CompetitionId]
      ,[modifiedDate] = '2020-08-19 00:00:00'
      ,[OldclassId]
      ,[Gender]
      ,[RaceCode]
      ,[RegattaId]
      ,[Comment]
      ,[isCreated] = 0
      ,[StartboatCount] = 0
      ,[isAbteilungslauf] = 0
FROM [RegattaMeldung].[dbo].[ReportedRaces]
WHERE [ReportedRaceId] NOT IN
(SELECT [ReportedRaceId] FROM RMDB.dbo.ReportedRaces))
GO
SET IDENTITY_INSERT [RMDB].[dbo].[ReportedRaces] OFF
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

USE [RMDB]
INSERT INTO [RMDB].[dbo].[ReportedStartboatStandbys] (ReportedStartboatId,MemberId,Standbynumber)
SELECT ReportedStartboatId,MemberId,Standbynumber
FROM [RegattaMeldung].[dbo].[ReportedStartboatStandbys]
GO



  UPDATE ReportedRaces SET isAbteilungslauf = 1 
  WHERE RaceCode IN ('11183E','11023E','11033E', '11043E', '12183E', '12023E', '12033E', '12043E') AND RegattaId = 2
-- bis hier