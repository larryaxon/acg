CREATE FUNCTION dbo.fnRemoveLeadingZeros 
(
    -- Add the parameters for the function here
    @Input varchar(256)
)
RETURNS varchar(256)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @Result varchar(max)

    -- Add the T-SQL statements to compute the return value here
    SET @Result = @Input

    WHILE LEFT(@Result, 1) = '0'
    BEGIN
        SET @Result = SUBSTRING(@Result, 2, LEN(@Result) - 1)
    END

    -- Return the result of the function
    RETURN @Result

END

select dbo.fnRemoveLeadingZeros ('00000808')