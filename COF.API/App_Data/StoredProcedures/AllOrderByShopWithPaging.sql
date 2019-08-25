CREATE PROCEDURE [dbo].[AllOrderByShopWithPaging](
	@shopId INT,
	@pageIndex INT,
	@pageSize INT, 
	@keyword NVARCHAR(MAX)
)
AS
BEGIN



--get key sort
declare @orderBy nvarchar(max) = 'o.Id desc'

DECLARE @query NVARCHAR(MAX) ='
;with paging as (
		Select 
		ROW_NUMBER() OVER (ORDER BY ' + @orderBy + ') as RowCounts,
		o.Id,
		o.OrderCode,
		customer.FullName as CustomerName,
		customer.PhoneNumber,
		customer.Address,
		u.FullName as StaffName,
		o.CreatedOnUtc as CreatedDate,
		o.FinalAmount as TotalCost,
		o.CancelDate,
		o.CancelBy,
		o.OrderStatus

		from [Order] o
		left join [Customer] customer on o.CustomerId = customer.Id
		inner join [User] u on o.UserId = u.Id
			WHERE 1=1 '
		 
	IF NULLIF(@keyword, '') IS NOT NULL
	Begin
	set @query = @query + ' and ( o.OrderCode like N''%' + @keyword  + '%''  
	
	or customer.FullName like N''%' + @keyword  + '%''
	or u.FullName like N''%' + @keyword  + '%'' )
	'  
	End

	--IF NULLIF(@keyword, '') IS NOT NULL
	--Begin
	set @query = @query + ' and o.ShopId = ' + CONVERT(NVARCHAR(10), @shopId) 
	--End
	--filter by bin type
	set @query = @query + ') select max(p.RowCounts) RowCounts, 
		0 Id,
		null OrderCode,
		null CustomerName,
		null PhoneNumber,
		null Address,
	    null StaffName,
		null CreatedDate,
		0 TotalCost,
		null CancelDate,
		null CancelBy,
		0 OrderStatus
	from paging p

	union all

	select * from paging p
	where p.RowCounts > ' + CONVERT(NVARCHAR(10), ((@pageIndex - 1) * @pageSize)) + ' and p.RowCounts <= ' + CONVERT(NVARCHAR(10), (@pageIndex * @pageSize)) + ''
	print(@query);
EXECUTE sp_executesql @query

End