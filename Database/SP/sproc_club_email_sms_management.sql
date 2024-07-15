USE [CRS.UAT_V2]
GO

/****** Object:  StoredProcedure [dbo].[sproc_club_email_sms_management]    Script Date: 6/19/2024 10:26:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[sproc_club_email_sms_management] @Flag VARCHAR(10)
	,@EmailSendTo VARCHAR(4000) = NULL
	,@EmailSendToCC VARCHAR(4000) = NULL
	,@EmailSendToBCC VARCHAR(4000) = NULL
	,@MobileNumber VARCHAR(15) = NULL
	,@ActionUser NVARCHAR(200) = NULL
	,@ActionIP VARCHAR(200) = NULL
	,@ActionPlatform VARCHAR(20) = NULL
	,@Username NVARCHAR(200) = NULL
	,@AgentId VARCHAR(10) = NULL
	,@UserId VARCHAR(10) = NULL
	,@VerificationCode VARCHAR(6) = NULL
	,@LoginId VARCHAR(200) = NULL
	,@Password VARCHAR(16) = NULL
	,@ExtraDatailId1 VARCHAR(10) = NULL
	,@ResponseCode INT OUTPUT
AS
DECLARE @Sno BIGINT
	,@Sno2 BIGINT
	,@StringSQL VARCHAR(MAX)
	,@StringSQL2 VARCHAR(MAX)
	,@TransactionName VARCHAR(200)
	,@ErrorDesc VARCHAR(MAX);
DECLARE @MessageContent NVARCHAR(MAX)
	,@EmailSendBy VARCHAR(200)
	,@ErrorResponseCode INT = 1
	,@SuccessResponseCode INT = 0;

BEGIN TRY
	IF @Flag = '1' -- customer :- Registration( E-mail & SMS)
	BEGIN
		DECLARE @LoginLink VARCHAR(MAX) = ' https://club.hoslog.jp/ ';

		--		SELECT @MessageContent = ISNULL(@Username, '') + N' 様、
		--ホスログへのご登録が成功し、あなたを迎えられることを嬉しく思います。
		--以下があなたのアカウント詳細です：
		--ユーザー名：' + ISNULL(@LoginId, '') + N'
		--パスワード：' + ISNULL(@Password, '') + N'
		--セキュリティ上の理由から、初めてログインする際にパスワードを変更することをお勧めします。ログイン情報の機密性を保つようにしてください。
		--始めるには、新しい認証情報を使用してログインするために ' + ISNULL(@LoginLink, '') + N' をクリックしてください。
		--ご質問がある場合、問題に遭遇した場合、またはサポートが必要な場合は、サポートチームがお手伝いいたします。[support@hoslog.jp までご連絡ください。
		--ホスログを選んでいただき、誠にありがとうございます。
		--[株式会社システム、ホスログ]
		--'
		SELECT @MessageContent = N'
							<!DOCTYPE html>
							<html lang="en,jp">
							<head>
							  <meta charset="UTF-8" />
							  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
							  <title>Email</title>
							</head>
							<body>
							  <div class="email container" style="
								max-width: 500px;
								margin: 0 auto;
								padding: 32px;
								border-radius: 12px;
								background-color: #faf2f6;
							  ">
								<header class="email-header">
								  <a href="#">
									<img style="height: auto; width: auto" src="https://s3.ap-northeast-1.amazonaws.com/dev-assets.hoslog.jp/DOCUMENTS/hosloglogo.svg" alt="Hoslog" />
								  </a>
								</header>
								<div class="email-body" style="margin-top: 12px">
								  <h3 style="color: #2b2b2b; font-family: ''Times New Roman'', Times, serif"> 様' + ISNULL(@Username, '') + 
			N'、 </h3>
								  <p style="color: #2b2b2b; margin-top: 2px; line-height: 1.35"> ホスログへのご登録が成功し、あなたを迎えられることを嬉しく思います。 以下があなたのアカウント詳細です：</p>
								  <div class="otp-container" style="
									padding: 8px;
									margin-top: 8px;
									background-color: #ffffff;
									border-radius: 5px;
									text-align: center;">
									<p> ユーザー名：<span class="otp" style="font-weight: 700; color: #d75a8b">' + ISNULL(@LoginId, '') + N'</span></p>
									<p> パスワード：<span class="otp" style="font-weight: 700; color: #d75a8b">' + ISNULL(@Password, '') + 
			N'</span></p>
								  </div>
								  <br />
								  <p>セキュリティ上の理由から、初めてログインする際にパスワードを変更することをお勧めします。ログイン情報の機密性を保つようにしてください。</p>
								  <p> 始めるには、新しい認証情報を使用してログインするために <a href="' + ISNULL(@LoginLink, '#') + '" style="color: blue">' + @LoginLink + N'</a> をクリックしてください。 ご質問がある場合、問題に遭遇した場合、またはサポートが必要な場合は、サポートチームがお手伝いいたします。 [support@hoslog.jp までご連絡ください。 ホスログを選んでいただき、誠にありがとうございます。 <span class="bold" style="margin: 4px; font-size: bold; color: #d75a8b "> [株式会社システム、ホスログ] </span></p>
								</div>
							  </div>
							</body>
							</html>'
			,@EmailSendBy = 'info@hoslog.jp';

		IF ISNULL(@EmailSendBy, '') != ''
			AND ISNULL(@EmailSendTo, '') != ''
		BEGIN
			INSERT INTO dbo.tbl_email_request (
				EmailSubject
				,EmailText
				,EmailSendBy
				,EmailSendTo
				,EmailSendToCC
				,EmailSendToBCC
				,EmailSendStatus
				,IsImportant
				,STATUS
				,CreatedBy
				,CreatedDate
				,CreatedIP
				,CretaedPlatform
				)
			VALUES (
				'Club-Registration Confirmation'
				,-- EmailSubject - nvarchar(600)
				@MessageContent
				,-- EmailText - nvarchar(max)
				@EmailSendBy
				,-- EmailSendBy - varchar(256)
				@EmailSendTo
				,-- EmailSendTo - varchar(5000)
				@EmailSendToCC
				,-- EmailSendToCC - varchar(5000)
				@EmailSendToBCC
				,-- EmailSendToBCC - varchar(5000)
				'P'
				,-- EmailSendStatus - char(1)
				'Y'
				,-- IsImportant - char(1)
				'P'
				,-- Status - char(1)
				@ActionUser
				,-- CreatedBy - nvarchar(200)
				GETDATE()
				,-- CreatedDate - datetime
				@ActionIP
				,-- CreatedIP - varchar(50)
				@ActionPlatform -- CretaedPlatform - varchar(20)
				);

			SET @ResponseCode = @SuccessResponseCode;

			SELECT @ResponseCode;

			RETURN;
		END;

		SET @ResponseCode = @SuccessResponseCode;

		SELECT @ResponseCode;

		RETURN;
	END;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION @TransactionName;

	SET @ErrorDesc = 'SQL error found: (' + ERROR_MESSAGE() + ')' + ' at ' + CAST(ERROR_LINE() AS VARCHAR);

	INSERT INTO dbo.tbl_error_log (
		ErrorDesc
		,ErrorScript
		,QueryString
		,ErrorCategory
		,ErrorSource
		,ActionDate
		)
	VALUES (
		@ErrorDesc
		,'sproc_email_sms_management(Flag: ' + ISNULL(@Flag, '') + ')'
		,'SQL'
		,'SQL'
		,'sproc_email_sms_management'
		,GETDATE()
		);

	SET @ResponseCode = @ErrorResponseCode;

	SELECT @ResponseCode;

	RETURN;
END CATCH;
GO


