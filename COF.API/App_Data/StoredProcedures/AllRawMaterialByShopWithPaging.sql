CREATE PROCEDURE [dbo].[AllRawMaterialByShopWithPaging](
	@shopId INT,
	@pageIndex INT,
	@pageSize INT, 
	@keyword NVARCHAR(MAX)
)
AS
BEGIN



--get key sort
declare @orderBy nvarchar(max) = 'rm.Id desc'

DECLARE @query NVARCHAR(MAX) ='
;with paging as (
		Select 
		ROW_NUMBER() OVER (ORDER BY ' + @orderBy + ') as RowCounts,
		rm.Id,
		rm.Name,
		rm.Description,
		rm.RawMaterialUnitId,
		rmu.Name as RawMaterialUnitName,
		rm.AutoTotalQty
		from [RawMaterial] rm
		inner join [Shop] shop on rm.ShopId = shop.Id	
		inner join [RawMaterialUnit] rmu on rm.RawMaterialUnitId = rmu.Id
	    WHERE 1 = 1 '
		 
	IF NULLIF(@keyword, '') IS NOT NULL
	Begin
	set @query = @query + ' and ( rm.Name like N''%' + @keyword  + '%''  
	
	or rm.Description like N''%' + @keyword  + '%''
	or rmu.Name like N''%' + @keyword  + '%'' )
	'  
	End

	--IF NULLIF(@keyword, '') IS NOT NULL
	--Begin
	set @query = @query + ' and rm.ShopId = ' + CONVERT(NVARCHAR(10), @shopId) 
	--End
	--filter by bin type
	set @query = @query + ') select max(p.RowCounts) RowCounts, 
		0 Id,
		null Name,
		null Description,
		0 RawMaterialUnitId,
	    null RawMaterialUnitName,
		0 AutoTotalQty
	from paging p

	union all

	select * from paging p
	where p.RowCounts > ' + CONVERT(NVARCHAR(10), ((@pageIndex - 1) * @pageSize)) + ' and p.RowCounts <= ' + CONVERT(NVARCHAR(10), (@pageIndex * @pageSize)) + ''
	print(@query);
EXECUTE sp_executesql @query

End