TRUNCATE TABLE RaceTimesTemp

INSERT INTO RaceTimesTemp (RaceCode, Starttime)
(SELECT RaceCode, Starttime FROM RMDB.dbo.Races
WHERE RegattaId = 2)