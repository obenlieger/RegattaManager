USE RegattaMeldung
DELETE RRFreeStartslots

USE RMDB
;WITH Free AS
(
SELECT DISTINCT ReportedRaceId,
6 - COUNT(sb.Startslot) OVER (PARTITION BY r.RaceId) AS FreeStartslots
FROM Startboats sb
INNER JOIN Races r ON r.RaceId = sb.RaceId
)
INSERT INTO [RegattaMeldung].[dbo].[RRFreeStartslots] (ReportedRaceId,FreeStartslots)
SELECT ReportedRaceId, SUM(FreeStartslots) FROM Free GROUP BY ReportedRaceId
GO