
USE [ConsumerMaster]
GO

/****** Object:  StoredProcedure [dbo].[[Consumers_CRUD]]     ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Consumers_CRUD]
      @Action VARCHAR(10)
      ,@consumer_internal_number INT = NULL
      ,@consumer_first VARCHAR(50) = NULL
      ,@consumer_last VARCHAR(50) = NULL
      ,@consumer_middle VARCHAR(50) = NULL
      ,@date_of_birth DATETIME = NULL
      ,@address_line_1 VARCHAR(50) = NULL
      ,@address_line_2 VARCHAR(50) = NULL
      ,@city VARCHAR(50) = NULL
      ,@state VARCHAR(3) = NULL
      ,@zip_code VARCHAR(10) = NULL
      ,@identifier VARCHAR(10) = NULL
      ,@gender VARCHAR(2) = NULL
      ,@diagnosis VARCHAR(50) = NULL
      ,@nickname_first VARCHAR(50) = NULL
      ,@nickname_last VARCHAR(50) = NULL

AS
BEGIN
      SET NOCOUNT ON;

      --SELECT
    IF @Action = 'SELECT'
      BEGIN
		SELECT consumer_internal_number, consumer_first, consumer_last, consumer_middle, date_of_birth, address_line_1
                ,address_line_2, city, state, zip_code, identifier, gender, diagnosis, nickname_first, nickname_last
		FROM [ConsumerMaster].[dbo].[Consumers]
      END

      --INSERT
    IF @Action = 'INSERT'
      BEGIN
		INSERT INTO [ConsumerMaster].[dbo].[Consumers](consumer_internal_number, consumer_first, consumer_last, consumer_middle, 
			date_of_birth, address_line_1, address_line_2, city, state, zip_code, identifier, gender, diagnosis, nickname_first, 
			nickname_last)
		VALUES (@consumer_internal_number, @consumer_first, @consumer_last, @consumer_middle, @date_of_birth, @address_line_1, 
			@address_line_2, @city, @state, @zip_code, @identifier, @gender, @diagnosis, @nickname_first, @nickname_last)
      END

      --UPDATE
    IF @Action = 'UPDATE'
      BEGIN
		UPDATE [ConsumerMaster].[dbo].[Consumers]
		SET consumer_first = @consumer_first, 
		consumer_last = @consumer_last, 
		consumer_middle = @consumer_middle, 
		date_of_birth = @date_of_birth, 
		address_line_1 = @address_line_1, 
		address_line_2 = @address_line_2, 
		city = @city, 
		state = @state, 
		zip_code = @zip_code, 
		identifier = @identifier, 
		gender = @gender, 
		diagnosis = @diagnosis, 
		nickname_first = @nickname_first
		WHERE consumer_internal_number = @consumer_internal_number
      END

      --DELETE
    IF @Action = 'DELETE'
      BEGIN
		DELETE FROM [ConsumerMaster].[dbo].[Consumers]
		WHERE consumer_internal_number = @consumer_internal_number
      END
END

GO