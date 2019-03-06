USE [ATFIS]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetConsumersData]    Script Date: 03/06/2019 10:49:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Jerry Eddy>
-- Create date: <01-16-2019>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetConsumersData] 
	-- Add the parameters for the stored procedure here
    @StartDateTime nvarchar(50),   
    @EndDateTime nvarchar(50),
    @Site int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
 
SELECT	a.Site,a.MANum, a.FullName,
		max(case when [Seq] = 1 then Ratio end) as Ratio1,
		max(case when [Seq] = 2 then Ratio end) as Ratio2,		
		max(case when [Seq] = 1 then CAST(ISNULL(Units,0) AS int) end) as Units1,
		max(case when [Seq] = 2 then CAST(ISNULL(Units,0) AS int) end) as Units2
FROM 
		(SELECT tim.[Site], 
			tim.[Social Security #]AS MANum,
			SUBSTRING([FullName], 1, CHARINDEX('(', [FullName]) -1) AS FullName,
			ROW_NUMBER()OVER(PARTITION BY tim.[Social Security #] ORDER BY tim.[FullName] ASC) AS Seq,
			RIGHT(FullName, 6) AS Ratio, 
			SUM((ISNULL(DATEDIFF(second, ta.[StartTime],ta.[EndTime]),0) + 
			ISNULL(DATEDIFF(second, ta.[StartTime1],ta.[EndTime1]),0)) / 900) AS Units
		FROM  ([ATFIS].[dbo].[T_Attendance] AS ta 
		LEFT OUTER JOIN [ATFIS].[dbo].[T_Individual Master] AS tim ON ta.[CID] = tim.[IndID]) 
		LEFT OUTER JOIN [ATFIS].[dbo].[T_Site] AS ts  ON tim.[Site]=ts.[S_ID]
		WHERE  (ta.[ADate]>= @StartDateTime AND ta."ADate"<@EndDateTime) AND tim.Status = 'Active' AND tim.Site = @Site
		GROUP BY  tim.[Site],tim.[Social Security #], FullName) a
GROUP BY a.Site,a.MANum, a.FullName
ORDER BY a.[Site],a.FullName   
END
