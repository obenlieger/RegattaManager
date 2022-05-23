DECLARE @racedate DATETIME2

DECLARE cur_race CURSOR FOR
  SELECT Starttime 
  FROM Races
  WHERE Starttime <> '0001-01-01'
  ORDER BY Starttime

OPEN cur_race  
FETCH NEXT FROM cur_race INTO @racedate  

WHILE @@FETCH_STATUS = 0  
BEGIN  
		USE RMDB
		SELECT sbm.MemberId, COUNT(sbm.MemberId)
		FROM Races r
		INNER JOIN Startboats sb ON sb.RaceId = r.RaceId
		INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
		WHERE Starttime <> '0001-01-01' AND r.Starttime >= @racedate AND r.Starttime <= DATEADD(Minute,15,@racedate) 
		AND sbm.MemberId NOT IN (1,2,3,4,5,6,7,8)
		GROUP BY sbm.MemberId
		HAVING COUNT(sbm.MemberId) > 1

      FETCH NEXT FROM cur_race INTO @racedate 
END 

CLOSE cur_race  
DEALLOCATE cur_race 

