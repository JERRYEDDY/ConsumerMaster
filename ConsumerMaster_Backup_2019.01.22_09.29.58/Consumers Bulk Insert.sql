use ConsumerMaster

BULK INSERT dbo.Consumers FROM 'C:\Pathways\ExportConsumerNoTP.csv'
WITH (FIRSTROW = 2, FIELDTERMINATOR = ',', ROWTERMINATOR = '\n');