CREATE PROCEDURE [dbo].[AllRawMaterialHistoriesWithPaging](
	@id INT,
	@pageIndex INT,
	@pageSize INT, 
	@keyword NVARCHAR(MAX)
)
AS
BEGIN



--get key sort
declare @orderBy nvarchar(max) = 'rmh.Id desc'

DECLARE @query NVARCHAR(MAX) ='
;with paging as (
		Select 
		ROW_NUMBER() OVER (ORDER BY ' + @orderBy + ') as RowCounts,
		rmh.Id,
		rmh.TimeAccess,
		rmh.TransactionTypeId,
		rmh.Quantity,
		rmh.InputTypeId,
		rmh.TotalQtyAtTimeAccess

		from [RawMaterialHistory] rmh
		
	    WHERE 1 = 1 '
		 

	--IF NULLIF(@keyword, '') IS NOT NULL
	--Begin
	set @query = @query + ' and rmh.RawMaterialId = ' + CONVERT(NVARCHAR(10), @id) 
	--End
	--filter by bin type
	set @query = @query + ') select max(p.RowCounts) RowCounts, 
		0 Id,
		getDate() TimeAccess,
		0 TransactionTypeId,
	    0 Quantity,
		0 InputTypeId,
		0 TotalQtyAtTimeAccess
	from paging p

	union all

	select * from paging p
	where p.RowCounts > ' + CONVERT(NVARCHAR(10), ((@pageIndex - 1) * @pageSize)) + ' and p.RowCounts <= ' + CONVERT(NVARCHAR(10), (@pageIndex * @pageSize)) + ''
	print(@query);
EXECUTE sp_executesql @query

End