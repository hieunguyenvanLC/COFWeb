CREATE PROCEDURE [dbo].[AllCustomerByPartnerIdWithPaging](
	@partnerId INT,
	@pageIndex INT,
	@pageSize INT, 
	@keyword NVARCHAR(MAX)
)
AS
BEGIN



--get key sort
declare @orderBy nvarchar(max) = 'customer.Id desc'

DECLARE @query NVARCHAR(MAX) ='
;with paging as (
		Select 
		ROW_NUMBER() OVER (ORDER BY ' + @orderBy + ') as RowCounts,
	customer.Id,
	customer.FullName,
	customer.PhoneNumber,
	customer.Address,
	customer.CreatedOnUtc as CreatedDate,
	customer.ActiveBonusPoint,
	level.Name as CustomerLevel,
	customer.Code,
	customer.BirthDate
	from [Customer] customer
	join [BonusLevel] level on customer.BonusLevelId = level.Id
			WHERE 1=1 '
		 
	IF NULLIF(@keyword, '') IS NOT NULL
	Begin
	set @query = @query + ' and ( customer.Fullname like N''%' + @keyword  + '%'' or
	customer.PhoneNumber like ''%' + @keyword  + '%'' or 	
	customer.Code like ''%' + @keyword  + '%'' or 	
	customer.Address like N''%' + @keyword  + '%'')
	' 
	

	End

	--IF NULLIF(@keyword, '') IS NOT NULL
	--Begin
	set @query = @query + ' and customer.partnerId = ' + CONVERT(NVARCHAR(10), @partnerId) 
	--End
	--filter by bin type
	set @query = @query + ') select max(p.RowCounts) RowCounts, 
		0 Id,
		null FullName,
	    null PhoneNumber,
	    null Address,
	    null CreatedDate,
	    0 ActiveBonusPoint,
	    null  CustomerLevel,
		null Code,
		null BirthDate
	from paging p

	union all

	select * from paging p
	where p.RowCounts > ' + CONVERT(NVARCHAR(10), ((@pageIndex - 1) * @pageSize)) + ' and p.RowCounts <= ' + CONVERT(NVARCHAR(10), (@pageIndex * @pageSize)) + ''
	print(@query);
EXECUTE sp_executesql @query

End