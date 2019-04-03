DECLARE @clubid int
DECLARE @SN NVARCHAR(MAX)

DECLARE id_cursor CURSOR FOR
SELECT ClubId
FROM RegattaMeldung.dbo.Clubs

OPEN id_cursor
FETCH NEXT FROM id_cursor INTO @clubid

WHILE @@FETCH_STATUS = 0
BEGIN

  UPDATE RMDB.dbo.Clubs SET ShortName = 
  (SELECT ShortName FROM RegattaMeldung.dbo.Clubs WHERE ClubId = @clubid)
  WHERE ClubId = @clubid

  FETCH NEXT FROM id_cursor INTO @clubid
END

CLOSE id_cursor
DEALLOCATE id_cursor

