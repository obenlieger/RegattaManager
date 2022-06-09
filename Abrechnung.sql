

	SELECT c.ShortName
	, 3.5 * COUNT(DISTINCT rsbm.MemberId) AS 'Teilnehmergebühr'
	, COUNT(DISTINCT rsbm.MemberId) AS 'Anzahl Sportler' 
	FROM ReportedStartboatMembers rsbm
	INNER JOIN Members m ON m.MemberId = rsbm.MemberId
	INNER JOIN Clubs c ON c.ClubId = m.ClubId
	WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)
	AND m.LastName NOT LIKE '%(%'
	GROUP BY c.ShortName
	ORDER BY 'Anzahl Sportler' DESC


	SELECT c.ShortName
		, CASE
			WHEN oc.FromAge >= 0 AND oc.ToAge <= 6 THEN 0 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 1 THEN 2 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 1 THEN 2.5 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 1 THEN 3 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 2 THEN 4 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 2 THEN 5 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 2 THEN 6 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 7 AND oc.ToAge <= 14 AND bc.Seats = 4 THEN 6 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 15 AND oc.ToAge <= 18 AND bc.Seats = 4 THEN 8 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN oc.FromAge >= 19 AND oc.ToAge <= 99 AND bc.Seats = 4 THEN 10 * COUNT(DISTINCT rsb.ReportedStartboatId)
			WHEN bc.Seats = 8 THEN 12 * COUNT(DISTINCT rsb.ReportedStartboatId)
		  END AS 'Startgebühr'
		, bc.Name
		, COUNT(DISTINCT rsb.ReportedStartboatId) AS 'Anzahl Boote'
	FROM ReportedStartboats rsb
	INNER JOIN ReportedStartboatMembers rsbm ON rsb.ReportedStartboatId = rsbm.ReportedStartboatId
	INNER JOIN ReportedRaces rr ON rr.ReportedRaceId = rsb.ReportedRaceId
	INNER JOIN Oldclasses oc ON oc.OldclassId = rr.OldclassId
	INNER JOIN Competitions cp ON cp.CompetitionId = rr.CompetitionId
	INNER JOIN Boatclasses bc ON bc.BoatclassId = cp.BoatclassId
	INNER JOIN Members m ON m.MemberId = rsbm.MemberId
	INNER JOIN Clubs c ON c.ClubId = m.ClubId
	WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)
	GROUP BY c.ShortName, bc.Name, oc.FromAge, oc.ToAge, bc.Seats


	SELECT rsb.ReportedStartboatId, bc.Seats, COUNT(*)
	FROM ReportedStartboats rsb
	INNER JOIN ReportedStartboatMembers rsbm ON rsb.ReportedStartboatId = rsbm.ReportedStartboatId
	INNER JOIN ReportedRaces rr ON rr.ReportedRaceId = rsb.ReportedRaceId
	INNER JOIN Oldclasses oc ON oc.OldclassId = rr.OldclassId
	INNER JOIN Competitions cp ON cp.CompetitionId = rr.CompetitionId
	INNER JOIN Boatclasses bc ON bc.BoatclassId = cp.BoatclassId
	INNER JOIN Members m ON m.MemberId = rsbm.MemberId
	INNER JOIN Clubs c ON c.ClubId = m.ClubId
	WHERE m.MemberId NOT IN (1,2,3,4,5,6,7,8)
	AND c.ShortName = 'BW Dresden'
	GROUP BY rsb.ReportedStartboatId, bc.Seats

