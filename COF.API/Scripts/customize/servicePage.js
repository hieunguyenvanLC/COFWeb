var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
        homeController.getAllServiceType();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var serviceCreateDto = {
                ServiceId: $('#txtServiceId').val(),
                Name: $('#txtServiceName').val(),
                ServiceTypeId: $('#ddlType').val(),
                ServiceStatusId: $('#ddlStatusId').val(),
                Price: $('#txtPrice').val()
            }
            if (serviceCreateDto.ServiceId == 0) {
                $.ajax({
                    url: '/Supplier/Service/AddService',
                    type: 'Post',
                    dataType: 'json',
                    data: serviceCreateDto,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            toastr.success("Tạo mới thành công.");
                            $('#myModal').modal('hide');
                            homeController.loadData();
                        }
                    }
                })
            } else {
                $.ajax({
                    url: '/Supplier/Service/UpdateService',
                    type: 'Post',
                    dataType: 'json',
                    data: serviceCreateDto,
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
          
        })



        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới dịch vụ');
            $('#myModal').modal('show');
            homeController.resetForm();
        });

       
        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật dịch vụ');
            $('#myModal').modal('show');
            homeController.resetForm();
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn xóa dịch vụ trên không?", function (result) {
                if (result) {
                    homeController.deleteService(id);
                }
                
            });
        });

    },
    deleteService: function (id) {
        $.ajax({
            url: '/Supplier/Service/DeleteServiceByIdAysnc',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.success == true) {
                    bootbox.alert("Xóa dịch vụ thành công.", function () {
                        homeController.loadData(true);
                    });
                }
                else {
                    if (response.validation) {
                        if (response.validation.Errors[0].Severity == 2) {
                            alertify.alert('Cảnh báo', response.validation.Errors[0].ErrorMessage);
                        }
                        else {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }
                        
                    }
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Supplier/Service/GetServiceById',
            data: {
                serviceId: id
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
        $('#txtServiceId').val('0');
        $('#txtServiceName').val('');
        $('#txtPrice').val('');
        $('#ddlStatusId').val(1).change();
        $('#ddlServiceTypeId').val(1).change();
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Supplier/Service/GetAllService',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, searchKey: $('#txtFilter').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ServiceId: item.ServiceId,
                            Name: item.Name,
                            ServiceType: item.ServiceType,
                            Price : item.Price,
                            //Status: (item.ServiceStatusId === 1) ? "<span class=\"label label-success\">Hoạt động</span>" : "<span class=\"label label-danger\">Tạm ngưng</span>"
                            Status: item.ServiceStatus
                        });

                    });
                    
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
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
    getAllServiceType: function () {
        $.ajax({
            url: '/Supplier/Service/GetAllServiceType',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response;
                var html = '';
                $.each(data, function (i, item) {
                    html += '<option value="' + item.ServiceTypeId + '">' + item.Name + '</option>'
                });
                $('#ddlType').html(html);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
}
homeController.init();