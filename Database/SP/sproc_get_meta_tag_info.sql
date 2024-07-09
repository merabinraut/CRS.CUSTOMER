USE [crs_v2]
GO

CREATE PROC [dbo].[sproc_get_meta_tag_info] @Flag VARCHAR(10)
	,@LocationId VARCHAR(10) = NULL
	,@AgentId VARCHAR(10) = NULL
AS
DECLARE @ClubCount INT = 0
	,@HostCount INT = 0
	,@Count INT = 0;

BEGIN
	IF @Flag = '1' --dashboard
	BEGIN
		SELECT @ClubCount = ISNULL(COUNT(a.AgentId), 0)
		FROM dbo.tbl_club_details a WITH (NOLOCK)
		INNER JOIN dbo.tbl_location b WITH (NOLOCK) ON b.LocationId = a.LocationId
			AND ISNULL(b.STATUS, '') = 'A'
		WHERE ISNULL(a.STATUS, '') = 'A'
			AND (
				b.LocationId IS NULL
				OR b.LocationId = @LocationId
				);

		SELECT @HostCount = ISNULL(COUNT(a.HostId), 0)
		FROM dbo.tbl_host_details a WITH (NOLOCK)
		WHERE a.AgentId IN (
				SELECT a.AgentId
				FROM dbo.tbl_club_details a2 WITH (NOLOCK)
				INNER JOIN dbo.tbl_location b2 WITH (NOLOCK) ON b2.LocationId = a2.LocationId
					AND ISNULL(b2.STATUS, '') = 'A'
				WHERE ISNULL(a2.STATUS, '') = 'A'
					AND (
						b2.LocationId IS NULL
						OR b2.LocationId = @LocationId
						)
				)
			AND ISNULL(a.STATUS, '') = 'A';

		SELECT @ClubCount AS Item1
			,@HostCount AS Item2;

		RETURN;
	END
	ELSE IF @Flag = '2' --club host count
	BEGIN
		SELECT @HostCount = ISNULL(COUNT(a.HostId), 0)
		FROM dbo.tbl_host_details a WITH (NOLOCK)
		WHERE a.AgentId IN (
				SELECT a.AgentId
				FROM dbo.tbl_club_details a2 WITH (NOLOCK)
				INNER JOIN dbo.tbl_location b2 WITH (NOLOCK) ON b2.LocationId = a2.LocationId
					AND ISNULL(b2.STATUS, '') = 'A'
				WHERE ISNULL(a2.STATUS, '') = 'A'
					AND b2.LocationId = @LocationId
					AND a2.AgentId = @AgentId
				)
			AND ISNULL(a.STATUS, '') = 'A';

		SELECT @HostCount AS Item1;

		RETURN;
	END
	ELSE IF @Flag = '3' --club review count
	BEGIN
		SELECT @Count = ISNULL(COUNT(a.ReviewId), 0)
		FROM dbo.tbl_review_and_rating a WITH (NOLOCK)
		INNER JOIN dbo.tbl_club_details b WITH (NOLOCK) ON b.AgentId = a.ClubId
			AND ISNULL(a.STATUS, '') = 'A'
		INNER JOIN dbo.tbl_location c WITH (NOLOCK) ON c.LocationId = b.LocationId
			AND ISNULL(c.STATUS, '') = 'A'
		WHERE b.AgentId = @AgentId
			AND ISNULL(b.STATUS, '') = 'A';

		SELECT @Count AS Item1;

		RETURN;
	END
	ELSE IF @Flag = '4' --club gallery count
	BEGIN
		SELECT @Count = ISNULL(COUNT(a.ImageID), 0)
		FROM dbo.tbl_club_gallery_image a WITH (NOLOCK)
		INNER JOIN dbo.tbl_club_gallery_folder b WITH (NOLOCK) ON b.FolderID = a.FolderID
			AND ISNULL(b.STATUS, '') = 'A'
			AND ISNULL(a.STATUS, '') = 'A'
		WHERE b.ClubID = @AgentId

		SELECT @Count AS Item1;

		RETURN;
	END
END
GO


