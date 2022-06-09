DECLARE @resultset TABLE(Member INT, Startboat INT, Seatnumber INT)

;WITH Q AS (
	SELECT CONCAT(CAST(rsbm.ReportedStartboatId AS varchar(5)),'-', CAST(rsbm.MemberId AS varchar(5)),'#', CAST(rsbm.Seatnumber AS varchar(3))) AS result
	,rsbm.ReportedStartboatId, rsbm.MemberId, rsbm.Seatnumber
	FROM ReportedStartboatMembers rsbm
	INNER JOIN Startboats sb ON sb.ReportedStartboatId = rsbm.ReportedStartboatId
	INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
	GROUP BY rsbm.ReportedStartboatId, rsbm.MemberId, rsbm.Seatnumber
), R AS (
	SELECT CONCAT(CAST(sb.ReportedStartboatId AS varchar(5)),'-', CAST(sbm.MemberId AS varchar(5)),'#', CAST(sbm.Seatnumber AS varchar(3))) AS result
	, sb.ReportedStartboatId, sbm.MemberId, sbm.Seatnumber
	FROM ReportedStartboatMembers rsbm
	INNER JOIN Startboats sb ON sb.ReportedStartboatId = rsbm.ReportedStartboatId
	INNER JOIN StartboatMembers sbm ON sbm.StartboatId = sb.StartboatId
	GROUP BY sb.ReportedStartboatId, sbm.MemberId, sbm.Seatnumber
)
INSERT INTO @resultset (Member, Startboat, Seatnumber) (
	SELECT Q.MemberId AS Member, sb.StartboatId AS Startboat, Q.Seatnumber
	FROM Q
	JOIN Startboats sb ON sb.ReportedStartboatId = Q.ReportedStartboatId
	WHERE Q.result NOT IN 
	( SELECT R.result FROM R)
)

UPDATE StartboatMembers SET StartboatMembers.MemberId = rs.Member
FROM @resultset rs
WHERE StartboatMembers.StartboatId = rs.Startboat AND StartboatMembers.Seatnumber = rs.Seatnumber