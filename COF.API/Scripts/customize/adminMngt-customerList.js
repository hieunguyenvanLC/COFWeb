var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    detailCustomerPageIndex: 1,
    detailCustomerPageSize: 10,
    searchBtnClick: true
};
var homeController = {
    init: function () {
        //homeController.loadData();
        homeController.loadCustomer();
        homeController.registerEvent();
        homeController.loadProvince();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var branchCreateDto = {
                BranchId: $('#txtBranchId').val(),
                Name: $('#txtBranchName').val(),
                CityId: $('#ddlProvince').val(),
                DistrictId: $('#ddlDistrict').val(),
                Address: $('#txtAddress').val(),
                Latitude: $('#txtLad').val(),
                Longitude: $('#txtLong').val(),
                GoogleMapSearchKey: $('#pac-input').val()
            }
            if (branchCreateDto.BranchId == 0) {
                $.ajax({
                    url: '/Supplier/Branch/AddBranchAsync',
                    type: 'Post',
                    dataType: 'json',
                    data: branchCreateDto,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            $('#myModal').modal('hide');
                            toastr.success("Tạo mới thành công.");
                            homeController.loadData();
                        }
                    }
                })
            } else {
                $.ajax({
                    url: '/Supplier/Branch/UpdateBranchAsync',
                    type: 'Post',
                    dataType: 'json',
                    data: branchCreateDto,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            toastr.success("Cập nhật thành công.");
                            $('#myModal').modal('hide');
                            homeController.loadData();
                        }
                    }
                })
            }

        });



        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới chi nhánh');
            homeController.resetForm();
            $('#myModal').modal('show');
        });


        $('#btnSearch').off('click').on('click', function () {
            homeconfig.searchBtnClick = true;
            homeController.loadData(true);
        });

        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Chi tiết khách hàng');
            var id = $(this).data('id');
            homeController.resetForm();
            homeController.loadDetail(id);
            $('#myModal').modal('show');
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa chi nhánh trên không?", function (result) {
                if (result) {
                    homeController.deleteBranch(id);
                }

            });
        });


        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                homeController.loadDistrict(parseInt(id));
            }
            else {
                $('#ddlDistrict').html('');
            }
        });

        $('#btnSearchCustomer').off('click').on('click', function () {
            $('#customerModal').modal('show');
        });

        $('#btnSearchCustomerList').off('click').on('click', function () {
            homeconfig.searchBtnClick = true;
            homeController.loadCustomer(true);
        });
        $('#txtCustomerNameSearch').off('change').on('change', function () {

        });

        $('.btn-refer').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thông tin chi tiết khách hàng');
            homeController.resetForm();
            var id = $(this).data('id');
            homeController.loadCustomerDetail(id);
            $('#myModal').modal('show');
        });
        $('#btnClearForm').off('click').on('click', function () {
            $('#txtSearchCustomerHelperName').val('');
            $('#txtSearchCustomerHelperPhoneNumber').val('');
            $('#txtSearchCustomerHelperEmail').val('');
            $('#txtSearchCustomerHelperAddress').val('');
            homeconfig.searchBtnClick = true;
            homeController.loadCustomer(true);
        });

    },
    deleteBranch: function (id) {
        $.ajax({
            url: '/Supplier/Branch/DeleteById',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.success == true) {
                    toastr.success("Xóa thành chi nhánh thành công.");
                    homeController.loadData(true);
                }
                else {
                    toastr.error(response.validation.Errors[0].ErrorMessage);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Supplier/Branch/GetBranchById',
            data: {
                branchId: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.result;
                    $('#txtBranchId').val(data.BranchId),
                        $('#txtBranchName').val(data.Name),
                        $('#ddlProvince').val(data.CityId).change();
                    $('#txtLong').val(data.Longitude);
                    $('#txtLad').val(data.Latitude);
                    $('#pac-input').val(data.GoogleMapSearchKey);


                    setTimeout(function () { $('#ddlDistrict').val(data.DistrictId).change(); }, 1000);

                    $('#txtAddress').val(data.Address);
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    saveData: function () {

    },
    resetForm: function () {
        $('#txtCustomerNameDetail').val('');
        $('#txtCustomerAgeDetail').val('');
        $('#txtPhoneNumberDetail').val('');
        $('#txtAddressDetail').val('');
        $('#txtUsernameDetail').val('');
        $('#txtCreatedDateDetail').val('');
        $('#tblDataOrder').html('');
    },
    loadData: function (changePageSize) {
        var orderSearchDto = {
            CustomerId: $('#txtHiddenCustomerId').val(),
            CustomerName: $('#txtCustomerNameSearch').val(),
            PhoneNumber: $('#txtPhoneNumberSearch').val(),
            Address: "",
            FromDate: "",
            ToDate: "",
            PageIndex: homeconfig.detailCustomerPageIndex,
            PageSize: homeconfig.detailCustomerPageSize

        };
        console.log(orderSearchDto);
        $.ajax({
            url: '/Admin/Customer/SearchCustomerAsync',
            type: 'POST',
            dataType: 'json',
            data: orderSearchDto,
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            BranchId: item.BranchId,
                            CustomerName: item.CustomerName,
                            Address: item.Address,
                            OrderStatus: item.OrderStatus,
                            CreatedDate: item.CreatedDate,
                            PhoneNumber: item.PhoneNumber

                        });

                    });
                    console.log(html);
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    loadProvince: function () {

        $.ajax({
            url: '/Supplier/Branch/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="0">--Chọn tỉnh thành--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    });
                    $('#ddlProvince').html(html);
                }
            }
        });
    },
    loadDistrict: function (id) {
        $.ajax({
            url: '/Supplier/Branch/LoadDistrict',
            type: "POST",
            data: { provinceID: id },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="0">--Chọn quận huyện--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        })
    },
    loadCustomer: function (changePageSize) {
        var searchDto = {
            Name: $('#txtSearchCustomerHelperName').val(),
            PhoneNumber: $('#txtSearchCustomerHelperPhoneNumber').val(),
            Email: $('#txtSearchCustomerHelperEmail').val(),
            Address: $('#txtSearchCustomerHelperAddress').val(),
            PageIndex: homeconfig.detailCustomerPageIndex,
            PageSize: homeconfig.detailCustomerPageSize
        };
        $.ajax({
            url: '/Admin/Customer/SearchCustomerAsync',
            type: 'POST',
            dataType: 'json',
            data: searchDto,
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template-customer').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            CustomerId: item.CustomerId,
                            CustomerName: item.Name,
                            Address: item.Address,
                            PhoneNumber: item.PhoneNumber,
                            Email: item.Email,
                            Age: item.Age
                        });

                    });
                    $('#tblDataCustomer').html(html);
                    homeController.pagingCustomerList(response.total, function () {
                        homeController.loadCustomer();
                    }, changePageSize);
                    homeController.registerEvent();
                    if (homeconfig.searchBtnClick == true) {
                        homeconfig.searchBtnClick = false;
                    }
                }
            }
        });
    },
    pagingCustomerList: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.detailCustomerPageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationCustomerList a').length === 0 || changePageSize === true) {
            $('#paginationCustomerList').empty();
            $('#paginationCustomerList').removeData("twbs-pagination");
            $('#paginationCustomerList').unbind("page");
        }

        $('#paginationCustomerList').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                homeconfig.detailCustomerPageIndex = page;
                if(homeconfig.searchBtnClick != true)
                {
                    setTimeout(callback, 200);
                }
               
                
            }
            
        });
    },
    loadCustomerDetail: function (id) {
        $.ajax({
            url: '/Admin/Customer/GetCustomerByIdAsync',
            type: "GET",
            dataType: "json",
            data: { customerId: id },
            success: function (response) {
                if (response.success == true) {
                    var data = response.data;
                    var customer = data.Customer;
                    $('#txtCustomerNameDetail').val(customer.FullName);
                    $('#txtCustomerAgeDetail').val(customer.Age);
                    $('#txtAddressDetail').val(customer.Address);
                    $('#txtPhoneNumberDetail').val(customer.PhoneNumber);
                    var account = customer.accountDetailDto;
                    $('#txtUsernameDetail').val(account.Username);
                    $('#txtCreatedDateDetail').val(account.CreatedDate);
                    var orders = data.Orders;

                    var html = '';
                    var template = $('#data-template-order').html();
                    $.each(orders, function (i, item) {
                        var orderDetails = "";
                        $.each(item.OrderDetails, function (i, chim) {
                            orderDetails +=  '' + chim.Service  + ": " + chim.Price + "\n";
                        });
                        html += Mustache.render(template, {
                            TotalOrderDetail: item.OrderDetails.length,
                            Index: i + 1,
                            CreatedDate: item.CreatedDate,
                            Status: item.OrderStatus,
                            Supplier: item.SupplierInfo.Name,
                            Total: (item.Total) ? item.Total : "Chưa định giá",
                           // Detail: orderDetails
                   
                        });

                    });
                    $('#tblDataOrder').html(html);

                }
            }
        });
    },
  
};
homeController.init();