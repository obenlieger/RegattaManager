DECLARE @NewMemberId int
DECLARE @OldMemberId int
DECLARE @StartboatId int
DECLARE @SeatNumber int
DECLARE @RowCount int

CREATE Table #UpdateSBM (NewMemberId int, OldMemberId int, StartboatId int, SeatNumber int)

;WITH NotEqual AS
(
SELECT rsbm.MemberId AS NewMemberId, sbm.MemberId AS OldMemberId, sbm.StartboatId AS StartboatId, sbm.SeatNumber AS SeatNumber, CASE WHEN rsbm.MemberId <> sbm.MemberId THEN '0' ELSE '1' END AS isEqual FROM ReportedStartboatMembers rsbm
INNER JOIN Startboats sb ON rsbm.ReportedStartboatId = sb.ReportedStartboatId
INNER JOIN StartboatMembers sbm ON sb.StartboatId = sbm.StartboatId AND rsbm.Seatnumber = sbm.SeatNumber
)
INSERT INTO #UpdateSBM
SELECT NewMemberId, OldMemberId, StartboatId, SeatNumber FROM NotEqual WHERE isEqual = 0

SELECT TOP 1 @NewMemberId = NewMemberId, 
@OldMemberId = OldMemberId, 
@StartboatId = StartboatId,
@SeatNumber = SeatNumber
FROM #UpdateSBM

SET @RowCount = @@ROWCOUNT

WHILE @RowCount <> 0
  BEGIN
    
	PRINT @NewMemberId
	PRINT @OldMemberId
	PRINT @StartboatId
	PRINT @SeatNumber

	UPDATE StartboatMembers SET MemberId = @NewMemberId WHERE StartboatId = @StartboatId AND SeatNumber = @SeatNumber

	DELETE FROM #UpdateSBM
	WHERE NewMemberId = @NewMemberId AND 
	  OldMemberId = @OldMemberId AND
	  StartboatId = @StartboatId AND
	  SeatNumber = @SeatNumber
	
	SELECT TOP 1 @NewMemberId = NewMemberId, 
	@OldMemberId = OldMemberId, 
	@StartboatId = StartboatId,
	@SeatNumber = SeatNumber
	FROM #UpdateSBM

	SET @RowCount = @@ROWCOUNT
END
DROP TABLE #UpdateSBM