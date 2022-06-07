USE RMDB
GO

UPDATE Races
SET Races.Starttime = rtt.Starttime
FROM Races r
INNER JOIN RaceTimesTemp rtt ON r.RaceCode = rtt.RaceCode

UPDATE Races SET Comment = 'Abteilungslauf'
  WHERE RaceCode like '11033V%' 
     OR RaceCode like '11043V%'
	 OR RaceCode like '12033V%'
	 OR RaceCode like '12043V%'
	 OR RaceCode like '11173V%'
	 OR RaceCode like '11183V%'
	 OR RaceCode like '12173V%'
    AND RegattaId = 2

UPDATE Races SET Comment = 'Pokallauf'
  WHERE RaceCode = '11033E' 
     OR RaceCode = '11043E'
	 OR RaceCode = '12033E'
	 OR RaceCode = '12043E'
	 OR RaceCode = '11173E'
	 OR RaceCode = '11183E'
	 OR RaceCode = '12173E'
	 AND RegattaId = 2