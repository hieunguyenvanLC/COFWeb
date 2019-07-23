var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    searchBtnClick: true
}
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {

        $('#btnSearch').off('click').on('click', function () {
            homeconfig.searchBtnClick = true;
            homeController.loadData(true);
        });
        $('.btnViewOrder').off('click').on('click', function () {
            var id = $(this).data('id');
            var customerName = $(this).data('customername');
            var phoneNumber = $(this).data('phonenumber');
           
            $.ajax({
                url: '/Supplier/Order/ReferOrderId',
                data: { orderId: id, customerName: customerName, phoneNumber: phoneNumber },
                type: "GET",
                async : true,
                success: function (res) {
                    window.open("/Supplier/Order", "_blank");
                }
            });


        });
     
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Supplier/Service/GetServiceById',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.result;
                    $('#txtServiceId').val(data.ServiceId);
                    $('#txtServiceName').val(data.Name);
                    $('#txtPrice').val(data.Price);
                    $('#ddlType').val(data.ServiceTypeId).change();
                    $('#ddlStatusId').val(data.ServiceStatusId).change();
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
        var name = $('#txtName').val();
        var salary = parseFloat($('#txtSalary').val());
        var status = $('#ckStatus').prop('checked');
        var id = parseInt($('#hidID').val());
        var employee = {
            Name: name,
            Salary: salary,
            Status: status,
            ID: id
        }
        $.ajax({
            url: '/Home/SaveData',
            data: {
                strEmployee: JSON.stringify(employee)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Save Success", function () {
                        $('#modalAddUpdate').modal('hide');
                        homeController.loadData(true);
                    });

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
    resetForm: function () {
        $('#txtNameDetail').val('');
        $('#txtEmailDetail').val('');
        $('#txtPhoneDetail').val('');
        $('#txtCreatedDateDetail').val('');
        $('#tblDataBranch').html('');
        $('#tblDataService').html('');

    },
    loadData: function (changePageSize) {
        var supplierSearchDto = {
            Name: $('#txtSupplierName').val(),
            Email: $('#txtSupplierEmail').val(),
            PhoneNumber: $('#txtPhoneNumber').val(),
            SupplierStatusId: $('#ddlSupplierStatusId').val(),
            CreatedDate: $('#txCreatedDate').val(),
            PageSize: homeconfig.pageSize,
            Page: homeconfig.pageIndex

        };
        $.ajax({
            url: '/Supplier/Feedback/GetAllFeedback',
            type: 'Get',
            dataType: 'json',
            async: false,
            data: { pageIndex: homeconfig.pageIndex, pageSize: homeconfig.pageSize, searchKey: $('#txtFilter').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            CustomerName: item.CustomerName,
                            PhoneNumber: item.PhoneNumber,
                            Address: item.Address,
                            Content: item.FeedbackContent,
                            RatingStar: homeController.renderStarRating(item.NumberOfStart),
                            CreatedDate: item.CreatedDate,
                            HasRelatedOrder: (item.OrderId == null) ? "display:none" : "",
                            OrderId: item.OrderId
                        });

                    });

                    $('#tblData').html(html);

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
                console.log("Test");
                if (homeconfig.searchBtnClick != true) {
                    setTimeout(callback, 200);
                }


            }
        });
    },
    renderStarRating: function (numberOfStar) {
        var html = "";
        for (var i = 0; i < numberOfStar; i++) {
            html += '<span class="fa fa-star checked"></span>';
        };
        for (var j = 0; j < ( 5 - numberOfStar) ; j++) {
            html += '<span class="fa fa-star"></span>';
        }
        return html;
    }
    
  
}
homeController.init();