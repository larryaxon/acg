/*
	Update script to add Reset ALL sql
*/

Update dbo.ProcessSteps
SET Sequence = 22
WHERE Step = 'Unpost Invoices'

INSERT INTO dbo.ProcessSteps (Cycle, Sequence, Step, Description, Form, FormParameters, LastModifiedBy, LastModifiedDateTime, IsRequired)
VALUES ('CHSFinancials', 23, 'Reset ALL Data for this Period', 'Reset ALL Data for this Period', 'ResetAllImports', null, 'Larry', getdate(), 0)