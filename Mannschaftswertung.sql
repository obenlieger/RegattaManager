DECLARE @gesamtsportler TABLE(rc VARCHAR(10),rccount INT)
DECLARE @vereinssportler TABLE(rcv VARCHAR(10),rccountv INT, rcvname VARCHAR(100))
DECLARE @sumverein TABLE(oc VARCHAR(50), c VARCHAR(50), placement INT, score FLOAT) 

INSERT INTO @gesamtsportler(rc,rccount)
(
	SELECT SUBSTRING(r.RaceCode,1,5) AS rc
	, COUNT(sb.StartboatId) AS rccount
	FROM Startboats sb
	INNER JOIN Races r ON r.RaceId = sb.RaceId
	INNER JOIN Boatclasses bc ON bc.BoatclassId = r.BoatclassId
	INNER JOIN Oldclasses oc ON oc.OldclassId = r.OldclassId
	INNER JOIN RaceTyps rt ON rt.RaceTypId = r.RaceTypId
	WHERE 
		(rt.Name = 'Endlauf' AND (r.RaceDrawId = 1 OR r.RaceDrawId = 9)) 
		OR rt.Name = 'Vorlauf'
	GROUP BY SUBSTRING(r.RaceCode,1,5)
)


INSERT INTO @vereinssportler(rcv,rccountv,rcvname)
(
	SELECT SUBSTRING(r.RaceCode,1,5) AS rcv
	, COUNT(sb.StartboatId) AS rccountv
	, c.ShortName
	FROM Startboats sb
	INNER JOIN Races r ON r.RaceId = sb.RaceId
	INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
	INNER JOIN Members m ON m.MemberId = sbm.MemberId
	INNER JOIN Clubs c ON c.ClubId = m.ClubId
	INNER JOIN Boatclasses bc ON bc.BoatclassId = r.BoatclassId
	INNER JOIN Oldclasses oc ON oc.OldclassId = r.OldclassId
	INNER JOIN RaceTyps rt ON rt.RaceTypId = r.RaceTypId
	WHERE 
		(rt.Name = 'Endlauf' AND (r.RaceDrawId = 1 OR r.RaceDrawId = 9)) 
		OR rt.Name = 'Vorlauf'
	GROUP BY SUBSTRING(r.RaceCode,1,5), c.ShortName
)


INSERT INTO @sumverein(oc, c, placement, score)
(
SELECT oc.Name
	, c.ShortName
--	, rccountv
--	, rccount
--	, bc.Seats
	, sb.Placement
--	, r.RaceCode
	, CASE
		WHEN sb.Placement = 1 THEN (CAST(1 AS FLOAT) - (CAST(rccountv AS FLOAT)/CAST(rccount AS FLOAT))) * (CAST(10 AS FLOAT) / CAST(bc.Seats AS FLOAT))
		WHEN sb.Placement = 2 THEN (CAST(1 AS FLOAT) - (CAST(rccountv AS FLOAT)/CAST(rccount AS FLOAT))) * (CAST(8 AS FLOAT) / CAST(bc.Seats AS FLOAT))
		WHEN sb.Placement = 3 THEN (CAST(1 AS FLOAT) - (CAST(rccountv AS FLOAT)/CAST(rccount AS FLOAT))) * (CAST(7 AS FLOAT) / CAST(bc.Seats AS FLOAT))
		WHEN sb.Placement = 4 THEN (CAST(1 AS FLOAT) - (CAST(rccountv AS FLOAT)/CAST(rccount AS FLOAT))) * (CAST(5 AS FLOAT) / CAST(bc.Seats AS FLOAT))
		WHEN sb.Placement = 5 THEN (CAST(1 AS FLOAT) - (CAST(rccountv AS FLOAT)/CAST(rccount AS FLOAT))) * (CAST(4 AS FLOAT) / CAST(bc.Seats AS FLOAT))
		WHEN sb.Placement = 6 THEN (CAST(1 AS FLOAT) - (CAST(rccountv AS FLOAT)/CAST(rccount AS FLOAT))) * (CAST(3 AS FLOAT) / CAST(bc.Seats AS FLOAT))
		WHEN sb.Placement = 7 THEN (CAST(1 AS FLOAT) - (CAST(rccountv AS FLOAT)/CAST(rccount AS FLOAT))) * (CAST(1 AS FLOAT) / CAST(bc.Seats AS FLOAT))
	  END AS 'Bewertung'
FROM Startboats sb
INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
INNER JOIN Members m ON m.MemberId = sbm.MemberId
INNER JOIN Clubs c ON c.ClubId = m.ClubId
INNER JOIN Races r ON r.RaceId = sb.RaceId
INNER JOIN Boatclasses bc ON bc.BoatclassId = r.BoatclassId
INNER JOIN Oldclasses oc ON oc.OldclassId = r.OldclassId
INNER JOIN RaceTyps rt ON rt.RaceTypId = r.RaceTypId
INNER JOIN @gesamtsportler ON rc = SUBSTRING(r.RaceCode,1,5)
INNER JOIN @vereinssportler ON rcv = SUBSTRING(r.RaceCode,1,5)
WHERE 
	r.RaceTypId = 4
)

SELECT c
	, COUNT(score) 
	FROM @sumverein
GROUP BY c
ORDER BY COUNT(score) DESC

SELECT oc
	, c
	, COUNT(score) 
FROM @sumverein
GROUP BY oc, c
ORDER BY oc, c






