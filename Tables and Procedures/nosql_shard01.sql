USE NoSQLServer 
GO




/* ************************************************************************************************************************
* DROP  TABLES    
************************************************************************************************************************* */

--IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME =  'AggregateIndexes' AND TABLE_SCHEMA  = 'NoSQLServer_01') > 0
--BEGIN
--	DROP TABLE NoSQLServer_01.AggregateIndexes
--END

--IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME =  'AggregateEvents' AND TABLE_SCHEMA  = 'NoSQLServer_01') > 0
--BEGIN
--	DROP TABLE NoSQLServer_01.AggregateEvents
--END

--IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME =  'AggregateEventTypes' AND TABLE_SCHEMA  = 'NoSQLServer_01') > 0
--BEGIN
--	DROP TABLE NoSQLServer_01.AggregateEventTypes
--END
--IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME =  'Aggregates' AND TABLE_SCHEMA  = 'NoSQLServer_01') > 0
--BEGIN
--	DROP TABLE NoSQLServer_01.Aggregates
--END







/* ************************************************************************************************************************
*  CREATE TABLES
************************************************************************************************************************* */

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME =  'Aggregates' AND TABLE_SCHEMA  = 'NoSQLServer_01') =  0
BEGIN
	CREATE TABLE NoSQLServer_01.Aggregates (
		AggregateID					BIGINT NOT NULL IDENTITY PRIMARY KEY ,
		AggregateTypeID				BIGINT NOT NULL ,
		Data						VARBINARY(MAX),
		VersionNumber				INT,
		ObjectTimestamp				DATETIME NOT NULL DEFAULT GETDATE(),
		LookupValue                 VARCHAR(36) NOT NULL
	)
	CREATE UNIQUE INDEX  LookupValue_IDX ON NoSQLServer_01.Aggregates(LookupValue,AggregateTypeID)
END
GO




IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERe TABLE_NAME =  'AggregateIndexes' AND TABLE_SCHEMA  = 'NoSQLServer_01') = 0
BEGIN
	CREATE TABLE NoSQLServer_01.AggregateIndexes (
		AggregateID					BIGINT NOT NULL  ,
		AggregateTypeID				BIGINT NOT NULL ,
		IndexValues					VARCHAR(251)
	)
 
	CREATE INDEX AggregateIndexes_IDX01 ON NoSQLServer_01.AggregateIndexes(IndexValues , AggregateTypeID)
	CREATE INDEX AggregateIndexes_IDX01 ON NoSQLServer_01.AggregateIndexes(AggregateID)
END
GO







/* ************************************************************************************************************************
* DROP/RECREATE PROCEDURES FOR TABLE: Aggregates 
************************************************************************************************************************* */
 
IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'p_InsertAggregate' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_InsertAggregate
END
 
if (select count(*) from INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'p_UpdateAggregate' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
begin
	drop procedure NoSQLServer_01.p_UpdateAggregate
end
 
IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'p_FetchAggregateByID' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_FetchAggregateByID
END
 
IF (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_FetchAggregateByUniqueKey' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_FetchAggregateByUniqueKey
END
GO

IF (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_FetchUniqueKeyAggregateID' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_FetchUniqueKeyAggregateID
END

GO

IF (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_DeleteAggregate' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_DeleteAggregate
END


GO

CREATE PROCEDURE NoSQLServer_01.p_InsertAggregate   @AggregateTypeID AS bigint, @Data AS VARBINARY(MAX), @LookupValue AS VARCHAR(36), @AggregateID AS BIGINT OUTPUT
AS
BEGIN
	INSERT INTO NoSQLServer_01.Aggregates (  AggregateTypeID, Data	,  VersionNumber,LookupValue )
	VALUES (  @AggregateTypeID, @Data	,   0, @LookupValue)
	SELECT @AggregateID = SCOPE_IDENTITY()
end 
GO
 
CREATE PROCEDURE NoSQLServer_01.p_UpdateAggregate @AggregateID AS BIGINT,  @Data AS varbinary(max), @LookupValue AS VARCHAR(36)
AS
BEGIN
	UPDATE NoSQLServer_01.Aggregates    SET LookupValue	= @LookupValue,  Data	= @Data,   VersionNumber = (VersionNumber + 1)  WHERE AggregateID = @AggregateID 
END 
GO

CREATE PROCEDURE NoSQLServer_01.p_FetchAggregateByID @AggregateID as bigint, @Data as varbinary(max) OUTPUT 
AS
BEGIN
	SELECT @Data = Data from NoSQLServer_01.Aggregates WHERE AggregateID = @AggregateID 
END 
GO
 
CREATE PROCEDURE NoSQLServer_01.p_FetchAggregateByUniqueKey @AggregateTypeID AS BIGINT, @LookupValue AS VARCHAR(36), @Data AS VARBINARY(MAX) OUTPUT, @AggregateID AS BIGINT OUTPUT 
AS
BEGIN
	SELECT @Data = Data, @AggregateID =  AggregateID 
	FROM  NoSQLServer_01.Aggregates   
	WHERE   AggregateTypeID = @AggregateTypeID AND LookupValue = @LookupValue
END 
GO

CREATE PROCEDURE NoSQLServer_01.p_FetchUniqueKeyAggregateID @AggregateTypeID AS BIGINT, @LookupValue AS VARCHAR(36) ,  @AggregateID AS BIGINT OUTPUT
AS
BEGIN
	SELECT @AggregateID =  IsNull(AggregateID,0) 
	FROM  NoSQLServer_01.Aggregates   
	WHERE   AggregateTypeID = @AggregateTypeID AND LookupValue = @LookupValue
END 
GO 
 
CREATE PROCEDURE NoSQLServer_01.p_DeleteAggregate @AggregateID AS BIGINT 
AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			DELETE FROM  NoSQLServer_01.Aggregates   
			WHERE   AggregateID = @AggregateID  

			DELETE FROM  NoSQLServer_01.AggregateIndexes   
			WHERE   AggregateID = @AggregateID  
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE
		   @ErMessage NVARCHAR(2048),
		   @ErSeverity INT,
		   @ErState INT
 
		 SELECT
		   @ErMessage = ERROR_MESSAGE(),
		   @ErSeverity = ERROR_SEVERITY(),
		   @ErState = ERROR_STATE()
 
		 RAISERROR (@ErMessage,
					 @ErSeverity,
					 @ErState )
	END CATCH
END 
GO






/* ************************************************************************************************************************
* CREATE PROCEDURES FOR TABLE: AggregateIndexes 
************************************************************************************************************************* */
 
IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'p_InsertAggregateIndexes' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_InsertAggregateIndexes 
END

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'p_DeleteAggregateIndexes' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_DeleteAggregateIndexes  
END
 

IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'p_FetchAggregateIDsByIndex' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_FetchAggregateIDsByIndex
END
 
IF (SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'p_FetchAggregatesByIndex' AND ROUTINE_SCHEMA = 'NoSQLServer_01') > 0
BEGIN
	DROP PROCEDURE NoSQLServer_01.p_FetchAggregatesByIndex
END
  
GO

CREATE PROCEDURE NoSQLServer_01.p_InsertAggregateIndexes   @AggregateTypeID AS BIGINT,   @AggregateID AS BIGINT, @ColumnNames	  AS CHAR(253), @DataValues AS CHAR(253)
AS
BEGIN
	INSERT INTO NoSQLServer_01.AggregateIndexes (  AggregateTypeID  ,   AggregateID  ,   IndexValues  )
	VALUES (  @AggregateTypeID  ,   @AggregateID  ,  rtrim(@DataValues) + '::' + rtrim(@ColumnNames)  )
END 
GO

CREATE PROCEDURE NoSQLServer_01.p_DeleteAggregateIndexes  @AggregateID as bigint ,  @AggregateTypeID as bigint 
AS
BEGIN
	DELETE FROM NoSQLServer_01.AggregateIndexes WHERE   AggregateID = @AggregateID and AggregateTypeID = @AggregateTypeID
END 
GO

CREATE PROCEDURE NoSQLServer_01.p_FetchAggregatesByIndex @AggregateTypeID AS BIGINT,  @ColumnNames AS CHAR(253), @DataValues AS char(253)
AS
BEGIN
	SELECT   Data,   b.AggregateID 
	FROM NoSQLServer_01.AggregateIndexes  a 
	INNER JOIN NoSQLServer_01.Aggregates b on a.AggregateID = b.AggregateID 
	WHERE  a.AggregateTypeID = @AggregateTypeID AND IndexValues =  rtrim(@DataValues) + '::' + rtrim(@ColumnNames) 
END 
GO

CREATE PROCEDURE NoSQLServer_01.p_FetchAggregateIDsByIndex @AggregateTypeID AS BIGINT,  @ColumnNames AS CHAR(253), @DataValues AS char(253), @AggregateID AS BIGINT OUTPUT
AS
BEGIN
	
	SELECT   @AggregateID =  IsNull(b.AggregateID , 0)  
	FROM NoSQLServer_01.AggregateIndexes  a 
	INNER JOIN NoSQLServer_01.Aggregates b on a.AggregateID = b.AggregateID 
	WHERE  a.AggregateTypeID = @AggregateTypeID AND IndexValues =  rtrim(@DataValues) + '::' + rtrim(@ColumnNames) 
END 
GO


