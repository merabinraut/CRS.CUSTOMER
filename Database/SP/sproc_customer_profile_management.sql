USE [CRS]
GO

/****** Object:  StoredProcedure [dbo].[sproc_profile_management]    Script Date: 10/26/2023 9:33:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sproc_customer_profile_management] (
	@Flag CHAR(5) = NULL
	,@NickName NVARCHAR(400) = NULL
	,@FirstName NVARCHAR(200) = NULL
	,@LastName NVARCHAR(200) = NULL
	,@MobileNumber VARCHAR(15) = NULL
	,@DOB VARCHAR(20) = NULL
	,@EmailAddress VARCHAR(512) = NULL
	,@Gender VARCHAR(10) = NULL
	,@PreferredLocation VARCHAR(10) = NULL
	,@PostalCode VARCHAR(100) = NULL
	,@Prefecture VARCHAR(10) = NULL
	,@City VARCHAR(100) = NULL
	,@Street VARCHAR(100) = NULL
	,@ResidenceNumber VARCHAR(100) = NULL
	,@ProfileImage VARCHAR(500) = NULL
	,@CurrentPassword VARCHAR(16) = NULL
	,@NewPassword VARCHAR(16) = NULL
	,@Session VARCHAR(200) = NULL
	,@UserId VARCHAR(200) = NULL
	,@AgentId VARCHAR(200) = NULL
	,@ActionUser VARCHAR(200) = NULL
	,@ActionIp VARCHAR(50) = NULL
	,@ActionPlatform VARCHAR(20) = NULL
	)
AS
BEGIN
	SET NOCOUNT ON;

	--SELECT USER PROFILE DETAIL
	IF @Flag = 's'
	BEGIN
		SELECT NickName
			,FirstName
			,LastName
			,MobileNumber
			,DOB
			,EmailAddress
			,Gender
			,PreferredLocation
			,PostalCode
			,Prefecture
			,City
			,Street
			,ResidenceNumber
			,ProfileImage
		FROM tbl_customer tc WITH (NOLOCK)
		WHERE tc.AgentId = @AgentId

		RETURN;
	END

	--UPDATE USER PROFILE DETAIL
	IF @Flag = 'u'
	BEGIN
		UPDATE tbl_customer
		SET NickName = ISNULL(@NickName, NickName)
			,FirstName = ISNULL(@FirstName, FirstName)
			,LastName = ISNULL(@LastName, LastName)
			,MobileNumber = ISNULL(@MobileNumber, MobileNumber)
			,DOB = ISNULL(@DOB, DOB)
			,EmailAddress = ISNULL(@EmailAddress, EmailAddress)
			,Gender = ISNULL(@Gender, Gender)
			,PreferredLocation = ISNULL(@PreferredLocation, PreferredLocation)
			,PostalCode = ISNULL(@PostalCode, PostalCode)
			,Prefecture = ISNULL(@Prefecture, Prefecture)
			,City = ISNULL(@City, City)
			,Street = ISNULL(@Street, Street)
			,ResidenceNumber = ISNULL(@ResidenceNumber, ResidenceNumber)
			,ProfileImage = ISNULL(@ProfileImage, ProfileImage)
		WHERE AgentId = @AgentId

		SELECT '0' code
			,'Customer detail updated successfully' message

		RETURN;
	END

	--UPLOAD USER PROFILE IMAGE
	IF @Flag = 'uimg'
	BEGIN
		UPDATE tbl_customer
		SET ProfileImage = ISNULL(@ProfileImage, ProfileImage)
		WHERE AgentId = @AgentId

		SELECT '0' code
			,'Profile picture updated successfully' message

		RETURN;
	END

	--CHANGE USER PASSWORD
	IF @Flag = 'cp'
	BEGIN
		IF NOT EXISTS (
				SELECT 'X'
				FROM tbl_users tu WITH (NOLOCK)
				WHERE AgentId = @AgentId
					AND PWDCOMPARE(@CurrentPassword, tu.Password) = 1
				)
		BEGIN
			SELECT '1' code
				,'Invalid user credentials' message

			RETURN;
		END
		ELSE
		BEGIN
			UPDATE tbl_users
			SET Password = PWDENCRYPT(@NewPassword)
				,LastPasswordChangedDate = GETDATE()
				,Session = @Session
			WHERE AgentId = @AgentId

			SELECT '0' code
				,'Password changed successfully' message

			RETURN;
		END
	END
END
GO


