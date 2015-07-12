USE NoSQLServer 
GO



/* ************************************************************************************************************************
* DROP  TABLES    
************************************************************************************************************************* */


 
 if (select count(*) from INFORMATION_SCHEMA.TABLES where TABLE_NAME =  'AggregateTypes' and TABLE_SCHEMA  = 'NoSQLServer_MASTER') > 0
BEGIN
	DROP TABLE NoSQLServer_MASTER.AggregateTypes
END 
 
  
 if (select count(*) from INFORMATION_SCHEMA.TABLES where TABLE_NAME =  'CascadingUpdates' and TABLE_SCHEMA  = 'NoSQLServer_MASTER') > 0
BEGIN
	DROP TABLE NoSQLServer_MASTER.CascadingUpdates
END 
 


/* ************************************************************************************************************************
*  CREATE TABLES
************************************************************************************************************************* */


create table NoSQLServer_MASTER.AggregateTypes (
	AggregateTypeID				bigint			not null identity primary key ,
	FullyQualifiedTypeName		varchar(253)	not null ,
	Description					varchar(500)	null,
	Shard						varchar(20)		not null default 'NoSQLServer_MASTER'
)
GO
Create unique index  AggregateType_IDX on NoSQLServer_MASTER.AggregateTypes(FullyQualifiedTypeName)
GO



create table NoSQLServer_MASTER.CascadingUpdates (
	AggregateTypeID				bigint			not null  ,
	CascadingUpdater			varchar(255)	not null ,
	CONSTRAINT pk_CascadingUpdates PRIMARY KEY (AggregateTypeID,CascadingUpdater)
)
GO
Create  index  UpdateReferences_IDX on NoSQLServer_MASTER.CascadingUpdates(AggregateTypeID)
GO


/* ************************************************************************************************************************
* DROP/RECREATE PROCEDURES FOR TABLE: AggregateTypes 
************************************************************************************************************************* */
if (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_InsertAggregateType' AND ROUTINE_SCHEMA = 'NoSQLServer_MASTER') > 0
begin
	drop procedure NoSQLServer_MASTER.p_InsertAggregateType
end

if (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_FetchAggregateTypeIDAndShardByTypeName' AND ROUTINE_SCHEMA = 'NoSQLServer_MASTER') > 0
begin
	drop procedure NoSQLServer_MASTER.p_FetchAggregateTypeIDAndShardByTypeName
end

if (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_GetAllAggregateTypes' AND ROUTINE_SCHEMA = 'NoSQLServer_MASTER') > 0
begin
	drop procedure NoSQLServer_MASTER.p_GetAllAggregateTypes
end


if (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_InsertCascadingUpdates' AND ROUTINE_SCHEMA = 'NoSQLServer_MASTER') > 0
begin
	drop procedure NoSQLServer_MASTER.p_InsertCascadingUpdates
end

if (select count(*) from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'p_FetchCascadingUpdates' AND ROUTINE_SCHEMA = 'NoSQLServer_MASTER') > 0
begin
	drop procedure NoSQLServer_MASTER.p_FetchCascadingUpdates
end


GO

 
create procedure NoSQLServer_MASTER.p_InsertCascadingUpdates   @AggregateTypeID AS BIGINT, @CascadingUpdater  AS VARCHAR(255)	 
as
begin
	insert into NoSQLServer_MASTER.CascadingUpdates ( AggregateTypeID, CascadingUpdater )
	values (  @AggregateTypeID, @CascadingUpdater )
end 
GO
 
 

create procedure NoSQLServer_MASTER.p_FetchCascadingUpdates @AggregateTypeID AS BIGINT
as
begin
	select   CascadingUpdater from NoSQLServer_MASTER.CascadingUpdates where  AggregateTypeID    = @AggregateTypeID  
end 
GO


  
create procedure NoSQLServer_MASTER.p_InsertAggregateType   @FullyQualifiedTypeName as varchar(253), @Description  as varchar(500)	,  @Shard as char(20),  @AggregateTypeID as bigint OUTPUT
as
begin
	insert into NoSQLServer_MASTER.AggregateTypes (  FullyQualifiedTypeName, Description, Shard)
	values (  @FullyQualifiedTypeName, @Description , @Shard)
	select @AggregateTypeID = SCOPE_IDENTITY()
end 
GO
 
 

create procedure NoSQLServer_MASTER.p_FetchAggregateTypeIDAndShardByTypeName @FullyQualifiedTypeName as varchar(253)
as
begin
	select AggregateTypeID, Shard from NoSQLServer_MASTER.AggregateTypes where FullyQualifiedTypeName = @FullyQualifiedTypeName 
end 
GO
 


insert into NoSQLServer_MASTER.AggregateTypes  (FullyQualifiedTypeName,Description,Shard) values ('NoSQLServer.AggregateType','System catalog object: AggregateType','NoSQLServer_MASTER')
insert into NoSQLServer_MASTER.AggregateTypes  (FullyQualifiedTypeName,Description,Shard) values ('NoSQLServer.CascasingUpdate','System catalog object: AggregateType','NoSQLServer_MASTER')
insert into NoSQLServer_MASTER.AggregateTypes  (FullyQualifiedTypeName,Description,Shard) values ('DomainObjects.People.Person','People','NoSQLServer_01')
insert into NoSQLServer_MASTER.AggregateTypes  (FullyQualifiedTypeName,Description,Shard) values ('DomainObjects.People.Family','Families','NoSQLServer_01')


--insert into NoSQLServer_MASTER.CascadingUpdates (AggregateTypeID, CascadingUpdater) values (5,'DomainUpdaters.Enrollment.UpdateFamiliesWithPerson')
 






