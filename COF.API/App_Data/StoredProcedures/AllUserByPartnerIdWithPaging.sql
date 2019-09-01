CREATE PROCEDURE [dbo].[AllUserByPartnerIdWithPaging](
	@partnerId INT,
	@pageIndex INT,
	@pageSize INT, 
	@keyword NVARCHAR(MAX)
)
AS
BEGIN



--get key sort
declare @orderBy nvarchar(max) = 'u.username asc'

DECLARE @query NVARCHAR(MAX) ='
;with paging as (
		Select 
		ROW_NUMBER() OVER (ORDER BY ' + @orderBy + ') as RowCounts,
   u.Id as UserId, 
	u.FullName, 
	u.Email,
    u.Address, 
	u.userName, 
	u.Phonenumber,

                        (select STRING_AGG(r.Description, '','') from[Role] r join[UserRole] ur
                        on r.Id = ur.RoleId
                        where ur.UserId = u.Id ) as Roles,
					   (select STRING_AGG(shop.ShopName, '','') from[Shop] shop 
					   join[ShopHasUser] hasUsers
											on hasUsers.ShopId = shop.Id
                        where hasUsers.UserId = u.Id ) as Shop
                        from[User] u
                        where 1 = 1'
	IF NULLIF(@keyword, '') IS NOT NULL
	Begin
	set @query = @query + ' and ( u.Fullname like N''%' + @keyword  + '%'' or
	u.PhoneNumber like ''%' + @keyword  + '%'' or 	
	u.Email like ''%' + @keyword  + '%'' or 	
	u.Address like N''%' + @keyword  + '%'')
	' 
	

	End

	--IF NULLIF(@keyword, '') IS NOT NULL
	--Begin
	set @query = @query + ' and u.partnerId = ' + CONVERT(NVARCHAR(10), @partnerId) 
	--End
	--filter by bin type
	set @query = @query + ') select max(p.RowCounts) RowCounts, 
		null UserId,
		null FullName,
	    null Email,
	    null Address,
	    null UserName,
	    null PhoneNumber,
	    null  Roles,
		null Shop
	from paging p

	union all

	select * from paging p
	where p.RowCounts > ' + CONVERT(NVARCHAR(10), ((@pageIndex - 1) * @pageSize)) + ' and p.RowCounts <= ' + CONVERT(NVARCHAR(10), (@pageIndex * @pageSize)) + ''
	print(@query);
EXECUTE sp_executesql @query

End