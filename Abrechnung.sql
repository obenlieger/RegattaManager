

	SELECT c.Name AS 'Verein'
	, COUNT(DISTINCT rsbm.MemberId) AS 'Teilnehmer' 
	, CAST(3.5 AS FLOAT) AS 'Preis'
	, CAST(3.5 AS FLOAT) * COUNT(DISTINCT rsbm.MemberId) AS 'Gesamtpreis'
	FROM ReportedStartboatMembers rsbm
	INNER JOIN Members m ON m.MemberId = rsbm.MemberId
	INNER JOIN Clubs c ON c.ClubId = m.ClubId
	WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)
	AND m.LastName NOT LIKE '%(%' 
	AND m.LastName NOT LIKE '%esucht%'
	AND m.LastName NOT LIKE '%suchen%'
	GROUP BY c.Name
	ORDER BY 'Teilnehmer' DESC


	SELECT c.ShortName AS 'Verein'
		, bc.Name AS 'Bootsklasse'
		, oc.FromAge AS 'Alter Von'
		, oc.ToAge AS 'Alter Bis'
		, COUNT(DISTINCT rsb.ReportedStartboatId) AS 'Anzahl Boote'
		, COUNT(m.MemberId) AS 'Anzahl Starter'
		,((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT)))) AS 'Faktor'
		, CASE
			WHEN oc.FromAge >= 0 AND oc.ToAge <= 6 THEN CAST(0 AS FLOAT)
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 1 THEN CAST(2 AS FLOAT)
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 1 THEN CAST(2.5 AS FLOAT) 
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 1 THEN CAST(3 AS FLOAT)
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 2 THEN CAST(4 AS FLOAT) 
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 2 THEN CAST(5 AS FLOAT)
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 2 THEN CAST(6 AS FLOAT)
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 4 THEN CAST(6 AS FLOAT)
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 4 THEN CAST(8 AS FLOAT)
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 4 THEN CAST(10 AS FLOAT)
			WHEN bc.Seats = 8 THEN CAST(12 AS FLOAT)
		  END AS 'Preis'
		, CASE
			WHEN oc.FromAge >= 0 AND oc.ToAge <= 6 THEN (CAST(0 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 1 THEN (CAST(2 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 1 THEN (CAST(2.5 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 1 THEN (CAST(3 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 2 THEN (CAST(4 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 2 THEN (CAST(5 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 2 THEN (CAST(6 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 4 THEN (CAST(6 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 4 THEN (CAST(8 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 4 THEN (CAST(10 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
			WHEN bc.Seats = 8 THEN (CAST(12 AS FLOAT)*(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT)))) * ((COUNT(CAST(m.MemberId AS FLOAT))/(COUNT(DISTINCT CAST(rsb.ReportedStartboatId AS FLOAT))*CAST(bc.Seats AS FLOAT))))
		  END AS 'Startgebühr'
	FROM ReportedStartboats rsb
	INNER JOIN ReportedStartboatMembers rsbm ON rsb.ReportedStartboatId = rsbm.ReportedStartboatId
	INNER JOIN ReportedRaces rr ON rr.ReportedRaceId = rsb.ReportedRaceId
	INNER JOIN Oldclasses oc ON oc.OldclassId = rr.OldclassId
	INNER JOIN Competitions cp ON cp.CompetitionId = rr.CompetitionId
	INNER JOIN Boatclasses bc ON bc.BoatclassId = cp.BoatclassId
	INNER JOIN Members m ON m.MemberId = rsbm.MemberId
	INNER JOIN Clubs c ON c.ClubId = m.ClubId
	WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8) 
	AND m.LastName NOT LIKE '%suchen%'
	AND m.LastName NOT LIKE '%esucht%'
	GROUP BY c.ShortName, bc.Name, oc.FromAge, oc.ToAge, bc.Seats


	--SELECT rsb.ReportedStartboatId, bc.Seats, COUNT(*)
	--FROM ReportedStartboats rsb
	--INNER JOIN ReportedStartboatMembers rsbm ON rsb.ReportedStartboatId = rsbm.ReportedStartboatId
	--INNER JOIN ReportedRaces rr ON rr.ReportedRaceId = rsb.ReportedRaceId
	--INNER JOIN Oldclasses oc ON oc.OldclassId = rr.OldclassId
	--INNER JOIN Competitions cp ON cp.CompetitionId = rr.CompetitionId
	--INNER JOIN Boatclasses bc ON bc.BoatclassId = cp.BoatclassId
	--INNER JOIN Members m ON m.MemberId = rsbm.MemberId
	--INNER JOIN Clubs c ON c.ClubId = m.ClubId
	--WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)
	--AND c.ShortName = 'BW Dresden'
	--GROUP BY rsb.ReportedStartboatId, bc.Seats

