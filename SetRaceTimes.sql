UPDATE Races SET Starttime = '2023-06-03 09:00:00' WHERE RaceCode = '11033V1'
UPDATE Races SET Starttime = '2023-06-03 09:02:00' WHERE RaceCode = '11033V2'
UPDATE Races SET Starttime = '2023-06-03 09:04:00' WHERE RaceCode = '11033V3'
UPDATE Races SET Starttime = '2023-06-03 09:06:00' WHERE RaceCode = '11033V4'
--UPDATE Races SET Starttime = '2023-06-03 12:30:00' WHERE RaceCode = '93003V2'
--UPDATE Races SET Starttime = '2023-06-03 12:45:00' WHERE RaceCode = '93003V3'


DECLARE @raceid INT
DECLARE @boatclassid INT
DECLARE @startdate DATETIME2
DECLARE @racecode VARCHAR(50)
DECLARE @lastdate DATETIME2

IF NOT EXISTS (SELECT * FROM Races WHERE Starttime <> '0001-01-01')
BEGIN
	SET @startdate = '2023-06-03 09:00:00'
END

--SET @startdate = '2023-06-03 18:15:00'

DECLARE @zwischenlaeufe TABLE(RaceId INT, BoatclassId INT, Starttime DATETIME2)
DECLARE @hoffnungslaeufe TABLE(RaceId INT, BoatclassId INT, Starttime DATETIME2)
DECLARE @previousMembers TABLE(pmid INT)
DECLARE @previousStartboats TABLE(psid INT)
DECLARE @previousRaces TABLE(prid INT, st DATETIME2)
DECLARE @currentMembers TABLE(cmid INT)
DECLARE @tempvorlaufraceid INT

IF EXISTS (SELECT * FROM Races WHERE Starttime <> '0001-01-01')
BEGIN
	DECLARE cur_race CURSOR FOR
	SELECT RaceId, RaceCode FROM Races
	WHERE RaceTypId = 1 
	AND RacestatusId <> 1006 
	AND Starttime = '0001-01-01'
	AND RaceCode NOT IN ('91003V1','91003V2','91003V3')
	ORDER BY newid()
	--ORDER BY RaceCode
END
ELSE
BEGIN
	DECLARE cur_race CURSOR FOR
	SELECT RaceId, RaceCode FROM Races
	WHERE RaceTypId = 1 
	AND RacestatusId <> 1006 
	AND Starttime = '0001-01-01'
	AND RaceCode NOT IN ('91003V1','91003V2','91003V3')
	--ORDER BY RaceCode
	ORDER BY newid()
END

OPEN cur_race  
FETCH NEXT FROM cur_race INTO @raceid, @racecode

WHILE @@FETCH_STATUS = 0  
BEGIN

	INSERT INTO @previousRaces (prid, st)
		SELECT RaceId, Starttime
		FROM Races
		WHERE Starttime <= @startdate 
		AND Starttime <> '0001-01-01'
		AND RaceTypId = 1 
		AND RacestatusId <> 1006

	IF ((
		SELECT oc.ToAge FROM Races r
		INNER JOIN Oldclasses oc ON r.OldclassId = oc.OldclassId
		WHERE r.RaceId = @raceid
		) < 13)
	BEGIN
		INSERT INTO @previousStartboats (psid)
		(
			SELECT StartboatId 
			FROM Startboats
			WHERE RaceId IN 
				(
				SELECT prid
				FROM @previousRaces
				WHERE st <= @startdate
				AND st >= DATEADD(Minute, -30, @startdate)
				)
		)

	END
	ELSE
	BEGIN
		INSERT INTO @previousStartboats (psid)
		(
			SELECT StartboatId 
			FROM Startboats
			WHERE RaceId IN 
				(
				SELECT prid
				FROM @previousRaces
				WHERE st <= @startdate
				AND st >= DATEADD(Minute, -30, @startdate)
				)
		)
	END

	--SELECT prid FROM @previousRaces

	--SELECT psid FROM @previousStartboats

	INSERT INTO @previousMembers (pmid)
	(
		SELECT MemberId
		FROM StartboatMembers
		WHERE StartboatId IN
			(
			SELECT psid 
			FROM @previousStartboats
			)
			AND MemberId NOT IN (1,2,3,4,5,6,7,8)
	)

	--SELECT pmid FROM @previousMembers

	INSERT INTO @currentMembers (cmid)
	(
		SELECT MemberId
		FROM StartboatMembers
		WHERE StartboatId IN
			(
			SELECT StartboatId
			FROM Startboats
			WHERE RaceId = @raceid
			)
			AND MemberId NOT IN (1,2,3,4,5,6,7,8)
	)

	IF NOT EXISTS (SELECT cmid FROM @currentMembers WHERE cmid IN (SELECT pmid FROM @previousMembers)) AND (SELECT Starttime FROM Races WHERE RaceId = @raceid) = '0001-01-01'
	BEGIN
		
		SET @lastdate = (SELECT TOP(1) Starttime FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

		SET @boatclassid = (SELECT TOP(1) BoatclassId FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

		IF @boatclassid = 11
		BEGIN
		  SET @startdate = DATEADD(MINUTE,6,@lastdate)
		END
		ELSE IF @boatclassid = 10
		BEGIN
		  SET @startdate = DATEADD(MINUTE,12,@lastdate)
		END
		ELSE
		BEGIN
		  SET @startdate = DATEADD(MINUTE,2,@lastdate)
		END

		IF @startdate >= '2023-06-03 12:00' AND @startdate <= '2023-06-03 12:10'
		BEGIN
		  SET @startdate = '2023-06-03 13:00'
		END

		UPDATE Races SET Starttime = @startdate
		WHERE RaceId = @raceid

		PRINT '1. ' + CAST(@startdate AS VARCHAR(50))

		WHILE	(
				(SELECT COUNT(RaceId) FROM Races
				WHERE SUBSTRING(RaceCode,1,6) = SUBSTRING(@racecode,1,6)
				AND Starttime = '0001-01-01') > 0
				)
		BEGIN	
		
			SET @tempvorlaufraceid = (SELECT TOP(1) RaceId FROM Races
										WHERE SUBSTRING(RaceCode,1,6) = SUBSTRING(@racecode,1,6)
										AND Starttime = '0001-01-01')
			
			PRINT @racecode
			PRINT SUBSTRING(@racecode,1,6)
			PRINT @tempvorlaufraceid
			
			SET @lastdate = (SELECT TOP(1) Starttime FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

			SET @boatclassid = (SELECT TOP(1) BoatclassId FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

			IF @boatclassid = 11
			BEGIN
			  SET @startdate = DATEADD(MINUTE,6,@lastdate)
			END
			ELSE IF @boatclassid = 10
			BEGIN
			  SET @startdate = DATEADD(MINUTE,12,@lastdate)
			END
			ELSE
			BEGIN
			  SET @startdate = DATEADD(MINUTE,2,@lastdate)
			END

			IF @startdate >= '2023-06-03 12:00' AND @startdate <= '2023-06-03 12:10'
			BEGIN
			  SET @startdate = '2023-06-03 13:00'
			END

			UPDATE Races SET Starttime = @startdate
			WHERE RaceId = @tempvorlaufraceid

			PRINT '2. ' + CAST(@startdate AS VARCHAR(50))

		END
	END

	IF EXISTS 
	(
		SELECT * FROM Races 
		WHERE Starttime = '0001-01-01'
		AND RaceTypId = 3
		AND RacestatusId <> 1006
		AND (
			SUBSTRING(RaceCode,1,5) NOT IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime = '0001-01-01'
				AND RaceTypId = 1 
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime >= DATEADD(Minute,-45,@startdate)
				AND Starttime <> '0001-01-01'
				AND RaceTypId = 1 
				AND RacestatusId <> 1006				
				)
			)
	)
	BEGIN
		INSERT INTO @hoffnungslaeufe
		SELECT RaceId, BoatclassId, Starttime
		FROM Races
		WHERE Starttime = '0001-01-01'
		AND RaceTypId = 3
		AND RacestatusId <> 1006
		AND (
			SUBSTRING(RaceCode,1,5) NOT IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime = '0001-01-01'
				AND RaceTypId = 1 
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND Starttime >= DATEADD(Minute,-30,@startdate)
				AND RaceTypId = 1 
				AND RacestatusId <> 1006
				)
			)		
		
		SET @lastdate = (SELECT TOP(1) Starttime FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

		SET @boatclassid = (SELECT TOP(1) BoatclassId FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

		IF @boatclassid = 11
		BEGIN
		  SET @startdate = DATEADD(MINUTE,6,@lastdate)
		END
		ELSE IF @boatclassid = 10
		BEGIN
		  SET @startdate = DATEADD(MINUTE,12,@lastdate)
		END
		ELSE
		BEGIN
		  SET @startdate = DATEADD(MINUTE,2,@lastdate)
		END

		IF @startdate = '2023-06-03 12:00' 
		BEGIN
		  SET @startdate = '2023-06-03 13:00'
		END

		UPDATE Races SET Starttime = @startdate
		WHERE RaceId IN (SELECT TOP (1) RaceId FROM @hoffnungslaeufe)

		PRINT '3. ' + CAST(@startdate AS VARCHAR(50))
	END

	IF EXISTS 
	(
		SELECT * FROM Races 
		WHERE Starttime = '0001-01-01'
		AND RaceTypId = 2
		AND RacestatusId <> 1006
		AND (
			SUBSTRING(RaceCode,1,5) NOT IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime = '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime >= DATEADD(Minute,-60,@startdate)
				AND Starttime <> '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				)
			)
	)
	BEGIN
		INSERT INTO @zwischenlaeufe
		SELECT RaceId, BoatclassId, Starttime
		FROM Races
		WHERE Starttime = '0001-01-01'
		AND RaceTypId = 2
		AND RacestatusId <> 1006
		AND (
			SUBSTRING(RaceCode,1,5) NOT IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime = '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime >= DATEADD(Minute,-30,@startdate)
				AND Starttime <> '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				)
			)		
		
		SET @lastdate = (SELECT TOP(1) Starttime FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

		SET @boatclassid = (SELECT TOP(1) BoatclassId FROM Races WHERE Starttime < '2023-06-04' ORDER BY Starttime DESC)

		IF @boatclassid = 11
		BEGIN
		  SET @startdate = DATEADD(MINUTE,6,@lastdate)
		END
		ELSE IF @boatclassid = 10
		BEGIN
		  SET @startdate = DATEADD(MINUTE,12,@lastdate)
		END
		ELSE
		BEGIN
		  SET @startdate = DATEADD(MINUTE,2,@lastdate)
		END

		IF @startdate = '2023-06-03 12:00' 
		BEGIN
		  SET @startdate = '2023-06-03 13:00'
		END

		UPDATE Races SET Starttime = @startdate
		WHERE RaceId IN (SELECT TOP (1) RaceId FROM @zwischenlaeufe)

		PRINT '4. ' + CAST(@startdate AS VARCHAR(50))
	END

	DELETE FROM @previousRaces
	DELETE FROM @previousStartboats
	DELETE FROM @previousMembers
	DELETE FROM @currentMembers
	DELETE FROM @hoffnungslaeufe
	DELETE FROM @zwischenlaeufe

	FETCH NEXT FROM cur_race INTO @raceid, @racecode
END

CLOSE cur_race  
DEALLOCATE cur_race 