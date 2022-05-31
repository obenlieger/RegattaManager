DECLARE @raceid INT
DECLARE @boatclassid INT
DECLARE @startdate DATETIME2

IF EXISTS (SELECT * FROM Races WHERE Starttime <> '0001-01-01')
BEGIN
	SET @startdate = DATEADD(MINUTE,2,(SELECT TOP 1 Starttime FROM Races ORDER BY Starttime DESC))
END
ELSE
BEGIN
	SET @startdate = '2022-06-11 09:00:00'
END

DECLARE @vorlaeufe TABLE(RaceId INT, BoatclassId INT, RaceCode VARCHAR(50), Starttime DATETIME2)
DECLARE @zwischenlaeufe TABLE(RaceId INT, BoatclassId INT, Starttime DATETIME2)
DECLARE @hoffnungslaeufe TABLE(RaceId INT, BoatclassId INT, Starttime DATETIME2)
DECLARE @previousMembers TABLE(MemberId INT)
DECLARE @previousStartboats TABLE(StartboatId INT)
DECLARE @previousRaces TABLE(RaceId INT)
DECLARE @currentMembers TABLE(MemberId INT)


INSERT INTO @vorlaeufe (RaceId, BoatclassId, RaceCode, Starttime)
	SELECT RaceId, BoatclassId, RaceCode, Starttime 
	FROM Races
	WHERE RaceTypId = 1 
	AND RacestatusId <> 1006 
	AND Starttime = '0001-01-01'
	ORDER BY newid()

IF EXISTS (SELECT * FROM Races WHERE Starttime <> '0001-01-01')
BEGIN
	DECLARE cur_race CURSOR FOR
	SELECT RaceId, BoatclassId FROM @vorlaeufe
	ORDER BY newid()
END
ELSE
BEGIN
	DECLARE cur_race CURSOR FOR
	SELECT RaceId, BoatclassId FROM @vorlaeufe
	ORDER BY RaceCode
END
OPEN cur_race  
FETCH NEXT FROM cur_race INTO @raceid, @boatclassid

WHILE @@FETCH_STATUS = 0  
BEGIN

	INSERT TOP (8) INTO @previousRaces (RaceId)
		SELECT RaceId 
		FROM Races
		WHERE Starttime <= @startdate 
		AND Starttime <> '0001-01-01'
		AND RaceTypId = 1 
		AND RacestatusId <> 1006
		ORDER BY Starttime DESC

	INSERT INTO @previousStartboats (StartboatId)
	(
		SELECT StartboatId 
		FROM Startboats
		WHERE RaceId IN 
			(
			SELECT RaceId
			FROM @previousRaces
			)
	)

	INSERT INTO @previousMembers (MemberId)
	(
		SELECT MemberId
		FROM StartboatMembers
		WHERE StartboatId IN
			(
			SELECT StartboatId 
			FROM @previousStartboats
			)
			AND MemberId NOT IN (1,2,3,4,5,6,7,8)
	)

	INSERT INTO @currentMembers (MemberId)
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

	IF NOT EXISTS (SELECT MemberId FROM @currentMembers WHERE MemberId IN (SELECT MemberId FROM @previousMembers))
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

		IF @startdate = '2022-06-11 12:04' 
		BEGIN
		  SET @startdate = '2022-06-11 13:00'
		END
	END

	IF EXISTS 
	(
		SELECT * FROM Races 
		WHERE Starttime = '0001-01-01'
		AND RaceTypId = 3
		AND RacestatusId <> 1006
		AND (
			SUBSTRING(RaceCode,1,5) IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND RaceTypId = 1 
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT TOP (15) SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND RaceTypId = 1 
				AND RacestatusId <> 1006
				ORDER BY Starttime DESC
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
			SUBSTRING(RaceCode,1,5) IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND RaceTypId = 1 
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT TOP (15) SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND RaceTypId = 1 
				AND RacestatusId <> 1006
				ORDER BY Starttime DESC
				)
			)

		UPDATE Races SET Starttime = @startdate
		WHERE RaceId IN (SELECT TOP (1) RaceId FROM @hoffnungslaeufe)

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

		IF @startdate = '2022-06-11 12:04' 
		BEGIN
		  SET @startdate = '2022-06-11 13:00'
		END
	END

	IF EXISTS 
	(
		SELECT * FROM Races 
		WHERE Starttime = '0001-01-01'
		AND RaceTypId = 2
		AND RacestatusId <> 1006
		AND (
			SUBSTRING(RaceCode,1,5) IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT TOP (15) SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				ORDER BY Starttime DESC
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
			SUBSTRING(RaceCode,1,5) IN
				(
				SELECT SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				)
			AND SUBSTRING(RaceCode,1,5) NOT IN
				(			
				SELECT TOP (15) SUBSTRING(RaceCode,1,5)
				FROM Races
				WHERE Starttime <= @startdate 
				AND Starttime <> '0001-01-01'
				AND (RaceTypId = 1 OR RaceTypId = 3)
				AND RacestatusId <> 1006
				ORDER BY Starttime DESC
				)
			)

		UPDATE Races SET Starttime = @startdate
		WHERE RaceId IN (SELECT TOP (1) RaceId FROM @zwischenlaeufe)

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

		IF @startdate = '2022-06-11 12:04' 
		BEGIN
		  SET @startdate = '2022-06-11 13:00'
		END
	END

	DELETE FROM @previousRaces
	DELETE FROM @previousStartboats
	DELETE FROM @previousMembers
	DELETE FROM @currentMembers
	DELETE FROM @hoffnungslaeufe
	DELETE FROM @zwischenlaeufe

	FETCH NEXT FROM cur_race INTO @raceid, @boatclassid
END

CLOSE cur_race  
DEALLOCATE cur_race 