UPDATE Races
SET Races.Starttime = rtt.Starttime
FROM Races r
INNER JOIN RaceTimesTemp rtt ON r.RaceCode = rtt.RaceCode