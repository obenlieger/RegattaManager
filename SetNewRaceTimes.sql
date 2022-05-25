DECLARE @raceid INT
DECLARE @boatclassid INT
DECLARE @startdate DATETIME2 = '2022-06-11 09:02:00'

DECLARE cur_race CURSOR FOR
SELECT RaceId, BoatclassId FROM Races
WHERE Starttime > '2022-06-11 09:00:00'
AND Starttime < '2022-06-12'
ORDER BY Starttime

OPEN cur_race  
FETCH NEXT FROM cur_race INTO @raceid, @boatclassid

WHILE @@FETCH_STATUS = 0  
BEGIN
	UPDATE Races SET Starttime = @startdate
	WHERE RaceId = @raceid

	IF @boatclassid = 11
	BEGIN
	  SET @startdate = DATEADD(MINUTE,4,@startdate)
    END
	ELSE IF @boatclassid = 10
	BEGIN
	  SET @startdate = DATEADD(MINUTE,15,@startdate)
	END
	ELSE
	BEGIN
	  SET @startdate = DATEADD(MINUTE,2,@startdate)
	END

	FETCH NEXT FROM cur_race INTO @raceid, @boatclassid
END

CLOSE cur_race  
DEALLOCATE cur_race 