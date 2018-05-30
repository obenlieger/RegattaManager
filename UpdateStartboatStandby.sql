DECLARE @NewMemberId int
DECLARE @OldMemberId int
DECLARE @StartboatId int
DECLARE @StandbyNumber int
DECLARE @RowCount int

CREATE Table #UpdateSBS (NewMemberId int, OldMemberId int, StartboatId int, StandbyNumber int)

;WITH NotEqual AS
(
SELECT rsbs.MemberId AS NewMemberId, sbs.MemberId AS OldMemberId, sbs.StartboatId AS StartboatId, sbs.StandbyNumber AS StandbyNumber, CASE WHEN rsbs.MemberId <> sbs.MemberId THEN '0' ELSE '1' END AS isEqual FROM ReportedStartboatStandbys rsbs
INNER JOIN Startboats sb ON rsbs.ReportedStartboatId = sb.ReportedStartboatId
INNER JOIN StartboatStandbys sbs ON sb.StartboatId = sbs.StartboatId AND rsbs.StandbyNumber = sbs.StandbyNumber
)
INSERT INTO #UpdateSBS
SELECT NewMemberId, OldMemberId, StartboatId, StandbyNumber FROM NotEqual WHERE isEqual = 0

SELECT TOP 1 @NewMemberId = NewMemberId, 
@OldMemberId = OldMemberId, 
@StartboatId = StartboatId,
@StandbyNumber = StandbyNumber
FROM #UpdateSBS

SET @RowCount = @@ROWCOUNT

WHILE @RowCount <> 0
  BEGIN
    
	PRINT @NewMemberId
	PRINT @OldMemberId
	PRINT @StartboatId
	PRINT @StandbyNumber

	UPDATE StartboatStandbys SET MemberId = @NewMemberId WHERE StartboatId = @StartboatId AND StandbyNumber = @StandbyNumber

	DELETE FROM #UpdateSBS
	WHERE NewMemberId = @NewMemberId AND 
	  OldMemberId = @OldMemberId AND
	  StartboatId = @StartboatId AND
	  StandbyNumber = @StandbyNumber
	
	SELECT TOP 1 @NewMemberId = NewMemberId, 
	@OldMemberId = OldMemberId, 
	@StartboatId = StartboatId,
	@StandbyNumber = StandbyNumber
	FROM #UpdateSBS

	SET @RowCount = @@ROWCOUNT
END
DROP TABLE #UpdateSBS