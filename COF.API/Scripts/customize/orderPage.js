var homeconfig = {
    pageSize: 20,
    pageIndex: 1,
    detailCustomerPageIndex: 1,
    detailCustomerPageSize: 10,
    searchBtnClick: true
};
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
        homeController.loadAllOrderStatus();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            
            $.ajax({
                url: '/Supplier/Order/UpdateOrderStatus',
                type: 'Post',
                dataType: 'json',
                data: {
                    OrderStatusId: $('#ddlCurrentStatusId').val(),
                    OrderId: $('#txtOrderIdDetail').val(),
                    IsPaid: $('#rdoIsPaid').prop('checked'),
                    TotalMoney: $('#txtTotalMoneyDetail').prop('readonly') ? null : $('#txtTotalMoneyDetail').text(),
                    Note : $('#txtSupplierNoteDetail').val()
                },
                success: function (res) {
                    if (!res.success) {
                        if (res.validation && res.validation.Errors) {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }
                    }
                    else {
                        toastr.success("Cập nhật thành công.");
                        $('#myModal').modal('hide');
                        homeController.loadData();
                        if ($('#ddlCurrentStatusId').val() == 3) {
                            var orderId = parseInt($('#txtOrderIdDetail').val());
                            var orders = db.collection("booking").where("OrderId", "==", orderId );
                            orders.get().then(function (documentSnapshots) {
                                var doc = documentSnapshots.docs[0];
                                var docData = doc.data();
                                var currentStatus = docData.CurrentStatus;
                                currentStatus.Name = "Cancel";
                                currentStatus.CreatedByCustomer = false;
                                currentStatus.UpdatedDate = new Date();
                                docData.SeenByCustomer = false;
                                db.collection("booking").doc(doc.id).update(docData);
                            });
                        }
                    }
                }
            });

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
            $('#lblPopupTitle').text('Cập nhật hóa đơn');
            homeController.resetForm();
            var id = $(this).data('id');
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
            homeController.loadCustomer(true);
        });
        $('#txtCustomerNameSearch').off('change').on('change', function () {

        });

        $('.btn-refer').off('click').on('click', function () {
            var id = $(this).data('id');
            var name = $(this).data('customername');
            var phoneNumber = $(this).data('phonenumber');
            $('#txtCustomerNameSearch').val(name);
            $('#txtHiddenCustomerId').val(id);
            $('#txtPhoneNumberSearch').val(phoneNumber);
            $('#customerModal').modal('hide');
        });
        $('#btnExportPdf').off('click').on('click', function () {

            $('#txtHiddenOrderhihiId').val($('#txtOrderIdDetail').val());
            $('#frmExportPdf').submit();
            
        });

        $('#btnClearForm').off('click').on('click', function () {
            $('#txtHiddenCustomerId').val();
            $('#txtCustomerNameSearch').val('');
            $('#txtPhoneNumberSearch').val('');
            $('#txtAddress').val('');
            $('#txtCreatedDateFrom').val('');
            $('#txtCreatedDateTo').val('');
            $('#ddlOrderStatusId').val('').change();
            $('#txtOrderIdSearch').val('');
            $('#ddlPaymentStatusId').val('').change();
            $('#rodAll').prop('checked', true);
            homeconfig.searchBtnClick = true;
            homeController.loadData(true);
        });

        $('#btnEditTotalMoney').off('click').on('click', function () {
            $('#txtTotalMoneyDetail').removeAttr('readonly');
        });

        $('#txtTotalMoneyDetail').off('focusout').on('focusout', function () {

            $.ajax({
                url: '/Supplier/Order/FomatCurrency',
                type: "Get",
                data: {
                    input: $('#txtTotalMoneyDetail').text()
                },
                dataType: "json",
                success: function (response) {
                    $('#txtTotalMoneyDetail').val(response);
                }
            });
        });
        $('#txtTotalMoneyDetail').off('keyup').on('keyup', function () {
            $(this).text($('#txtTotalMoneyDetail').val());
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Supplier/Order/GetOrderById',
            data: {
                orderId: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var order = response.order;
                    var customer = response.customerInfo;
                    $('#txtCustomerNameDetail').val(customer.FullName);
                    $('#txtCustomerAgeDetail').val(customer.Age);
                    $('#txtAddressDetail').val(order.Address);
                    $('#txtPhoneNumberDetail').val(order.PhoneNumber);
                    $('#txtOrderCreatedDateDetail').val(order.CreatedDate);
                    $('#txtOrderIdDetail').val(order.OrderId);
                    $('#txtTotalMoneyDetail').val(order.Total);
                    $('#txtTotalMoneyDetail').text(order.PaymentPrice);
                    $('#txtSupplierNoteDetail').val(order.SupplierNote);
                    $('#txtCurrentStatusDetail').val(order.OrderStatus);
                    $('#txtCustomerNoteDetail').val(order.Description);
                    $('#txtOrderId').val(order.OrderId);
                    $('#txtSupplierNoteDetail').val(order.SupplierNote);
                    $('#txtWorkDateDetail').val(order.WorkDate);
                    if (order.OrderStatusId == 3) {
                        $('#btnSave').css('display', 'none');
                        $('#statusDiv').css('display', 'none');
                        $('#rdoIsPaid').prop('disabled', true);
                        $('#rdoNotPaid').prop('disabled', true);
                    }
                    else {
                        $('#btnSave').css('display', '');
                        $('#statusDiv').css('display', '');
                        $('#rdoIsPaid').prop('disabled', false);
                        $('#rdoNotPaid').prop('disabled', false);
                       
                    }
                    if (order.IsPaid) {
                        $('#rdoIsPaid').prop('checked', true);
                    } else {
                        $('#rdoNotPaid').prop('checked', true);
                    }
                    
                    var orderDetai = order.OrderDetails;

                    var html = '';
                    var template = $('#data-template-orderDetail').html();
                    $.each(orderDetai, function (i, item) {
                        html += Mustache.render(template, {
                            Index: i + 1,
                            Service: item.Service,
                            Price: (item.OrignalPrice != 0) ? item.Price : "Định giá tùy tình trạng" ,
                            Quantity : item.Quantity
                            //Description: '<input type="text" class="form-control" value"=' + item.Description +  '" id="usr">'

                        });

                    });
                    console.log(html);
                    $('#tblDataOrderDetail').html(html);

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
        $('#txtAddressDetail').val('');
        $('#txtPhoneNumberDetail').val('');
        $('#txtOrderCreatedDateDetail').val('');
        $('#txtOrderIdDetail').val('');
        $('#lblTotalMoney').text('');
        $('#txtCurrentStatusDetail').val('');
        $('#txtOrderId').val('0');
        $('#txtSupplierNoteDetail').val('');
        $('#ddlCurrentStatusId').val('').change();
        $('#txtTotalMoneyDetail').prop('readonly', true);
        $('#tblDataOrderDetail').html('');
        $('#txtWorkDateDetail').val('');
        $('#txtCustomerNoteDetail').val('');
    },
    loadData: function (changePageSize) {
        //var isPaid = null;
        //if ($('#rdoYes').prop('checked') == true) {
        //    isPaid = true;
        //} else if ($('#rdoNo').prop('checked') == true) {
        //    isPaid = false;
        //} else {
        //    isPaid = null;
        //}
        var orderSearchDto = {
            CustomerId: $('#txtHiddenCustomerId').val(),
            CustomerName: $('#txtCustomerNameSearch').val(),
            PhoneNumber: $('#txtPhoneNumberSearch').val(),
            Address: $('#txtAddress').val(),
            FromDate: $('#txtCreatedDateFrom').val(),
            ToDate: $('#txtCreatedDateTo').val(),
            OrderStatusId: $('#ddlOrderStatusId').val(),
            PageIndex: homeconfig.pageIndex,
            PageSize: homeconfig.pageSize,
            OrderId: $('#txtOrderIdSearch').val(),
            IsPaid: $('#ddlPaymentStatusId').val()
        };
        $.ajax({
            url: '/Supplier/Order/SearchOrder',
            type: 'POST',
            dataType: 'json',
            data: orderSearchDto ,
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
                            OrderStatus: homeController.labelForOrderStatus(item.OrderStatusId, item.OrderStatus),
                            CreatedDate: item.CreatedDate,
                            PhoneNumber: item.PhoneNumber,
                            WorkDate: item.WorkDate,
                            OrderId: item.OrderId,
                            PaymentStatus : item.IsPaid ? "Đã thanh toán" : "Chưa thanh toán",
                            Total: (item.Total != "") ? item.Total : "Chưa định giá"
                        });

                    });
                    console.log(html);
                    $('#tblData').html(html);
                    $('#txtTotalRow').text(response.total);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                    homeController.registerEvent();
                    if (homeconfig.searchBtnClick == true) {
                        homeconfig.searchBtnClick = false;
                    }
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
                if (homeconfig.searchBtnClick != true) {
                    setTimeout(callback, 200);
                }
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
        });
    },
    loadCustomer: function (changePageSize) {
        var searchDto = {
            Name: $('#txtSearchCustomerHelperName').val(),
            PhoneNumber: $('#txtSearchCustomerHelperPhoneNumber').val(),
            Email: $('#txtSearchCustomerHelperEmail').val(),
            Address: $('#txtSearchCustomerHelperAddress').val(),
            PageIndex: homeconfig.detailCustomerPageIndex,

        };
        $.ajax({
            url: '/Supplier/Order/SearchCustomer',
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
                            Age : item.Age
                        });

                    });
                    $('#tblDataCustomer').html(html);
                    homeController.pagingCustomerList(response.total, function () {
                        homeController.loadCustomer();
                    }, changePageSize);
                    homeController.registerEvent();
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
                setTimeout(callback, 200);
            }
        });
    },
    loadAllOrderStatus: function () {
        $.ajax({
            url: '/Supplier/Order/GetAllOrderStatus',
            type: "GET",
            dataType: "json",
            success: function (response) {
                if (response.success == true) {
                    var html = '<option value="">-- Tất cả --</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.OrderStatusId + '">' + item.Name + '</option>'
                    });
                    $('#ddlOrderStatusId').html(html);
                    $('#ddlCurrentStatusId').html(html);
                }
            }
        })
    },
    labelForOrderStatus(statusId, name) {
        if (statusId == 1) {
            return '<span class="label label-warning">' + name + '</span>';
        } else if (statusId == 2) {
            return '<span class="label label-success">' + name + '</span>'
        } else {
            return '<span class="label label-danger">' + name + '</span>'
        }

    } 

};
homeController.init();