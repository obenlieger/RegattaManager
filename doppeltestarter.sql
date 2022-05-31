DECLARE @racedate DATETIME2
DECLARE @racecode VARCHAR(50)

DECLARE cur_race CURSOR FOR
  SELECT Starttime, RaceCode 
  FROM Races
  WHERE Starttime <> '0001-01-01'
  ORDER BY Starttime

OPEN cur_race  
FETCH NEXT FROM cur_race INTO @racedate, @racecode  

WHILE @@FETCH_STATUS = 0  
BEGIN  
		USE RMDB
		;WITH S AS(	
			SELECT sbm.MemberId AS MemberId, COUNT(sbm.MemberId) AS Anzahl
				FROM Races r
				INNER JOIN Startboats sb ON sb.RaceId = r.RaceId
				INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
				WHERE r.Starttime <> '0001-01-01' AND r.Starttime >= @racedate AND r.Starttime <= DATEADD(Minute,15,@racedate) 
				AND sbm.MemberId NOT IN (1,2,3,4,5,6,7,8)
				GROUP BY sbm.MemberId
				HAVING COUNT(sbm.MemberId) > 1
		), R
		AS (
			SELECT r.RaceId AS RaceId, r.RaceCode AS RaceCode, m.LastName AS LastName, m.FirstName AS FirstName, c.ShortName AS ClubName, sbm.MemberId AS MemberId, R.Starttime AS Startime
				FROM Races r
				INNER JOIN Startboats sb ON sb.RaceId = r.RaceId
				INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
				INNER JOIN Members m ON sbm.MemberId = m.MemberId
				INNER JOIN Clubs c ON m.ClubId = c.ClubId
				WHERE r.Starttime <> '0001-01-01' AND r.Starttime >= @racedate AND r.Starttime <= DATEADD(Minute,15,@racedate) 
				AND sbm.MemberId NOT IN (1,2,3,4,5,6,7,8)
		)
		SELECT S.MemberId,
			   R.RaceCode,
			   R.ClubName,
			   R.LastName,
			   R.FirstName,
			   R.Startime
			FROM S
			JOIN R ON S.MemberId = R.MemberId

      FETCH NEXT FROM cur_race INTO @racedate, @racecode
END 

CLOSE cur_race  
DEALLOCATE cur_race 

DECLARE cur_race CURSOR FOR
  SELECT Starttime, RaceCode 
  FROM Races r
  INNER JOIN Oldclasses oc ON r.OldclassId = oc.OldclassId
  WHERE Starttime <> '0001-01-01'
  AND oc.ToAge < 13
  ORDER BY Starttime

OPEN cur_race  
FETCH NEXT FROM cur_race INTO @racedate, @racecode  

WHILE @@FETCH_STATUS = 0  
BEGIN  
		USE RMDB
		;WITH S AS(	
			SELECT sbm.MemberId AS MemberId, COUNT(sbm.MemberId) AS Anzahl
				FROM Races r
				INNER JOIN Startboats sb ON sb.RaceId = r.RaceId
				INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
				WHERE r.Starttime <> '0001-01-01' AND r.Starttime >= @racedate AND r.Starttime <= DATEADD(Minute,30,@racedate) 
				AND sbm.MemberId NOT IN (1,2,3,4,5,6,7,8)
				GROUP BY sbm.MemberId
				HAVING COUNT(sbm.MemberId) > 1
		), R
		AS (
			SELECT r.RaceId AS RaceId, r.RaceCode AS RaceCode, m.LastName AS LastName, m.FirstName AS FirstName, c.ShortName AS ClubName, sbm.MemberId AS MemberId, R.Starttime AS Startime
				FROM Races r
				INNER JOIN Startboats sb ON sb.RaceId = r.RaceId
				INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
				INNER JOIN Members m ON sbm.MemberId = m.MemberId
				INNER JOIN Clubs c ON m.ClubId = c.ClubId
				WHERE r.Starttime <> '0001-01-01' AND r.Starttime >= @racedate AND r.Starttime <= DATEADD(Minute,30,@racedate) 
				AND sbm.MemberId NOT IN (1,2,3,4,5,6,7,8)
		)
		SELECT S.MemberId,
			   R.RaceCode,
			   R.ClubName,
			   R.LastName,
			   R.FirstName,
			   R.Startime
			FROM S
			JOIN R ON S.MemberId = R.MemberId

      FETCH NEXT FROM cur_race INTO @racedate, @racecode
END 

CLOSE cur_race  
DEALLOCATE cur_race 
