--EXEC LLA_TEMP_UpdateNewFLuentStreamCuxtomerIDs
GO
ALTER  PROCEDURE LLA_TEMP_UpdateNewFLuentStreamCuxtomerIDs AS
DECLARE @EntityID as varchar(50) 
DECLARE @OldID as varchar(50)
DECLARE @NewID as varchar(50)

--Select * INTO EntityBackup from Entity

DECLARE OldIds CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT distinct Entity from LLAFluentStreamCustomerIDs

OPEN OldIds
FETCH NEXT FROM OldIds INTO @EntityID
WHILE @@FETCH_STATUS = 0
BEGIN 
    Select @Oldid = AlternateID from Entity Where Entity = @EntityID
	Select @NewID = [Account ID] from LLAFluentStreamCustomerIDs where Entity = @EntityID
	PRINT 'E:' + @EntityID + ' O:' + @OldID + ' N:' + @NewID
	exec [dbo].CreateAttributeNode @EntityID, 'Entity', 'Customer', 'OldAlternateID',  @Oldid
	Update Entity SET AlternateID = @NewID Where Entity = @EntityID
    FETCH NEXT FROM OldIds INTO @EntityID
END
CLOSE OldIds
DEALLOCATE OldIds
GO