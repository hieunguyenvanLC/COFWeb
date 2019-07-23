var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    searchBtnClick: true
}
var homeController = {
    init: function () {
        homeController.loadSupplierStatus();
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var supplierId = $('#txtSupplierIdHiddent').val();
            var supplierStatusId = $('#ddlCurrentStatusId').val();

            $.ajax({
                url: '/Admin/Supplier/ChangeSupplierStatus',
                type: 'Post',
                dataType: 'json',
                data: { supplierId: supplierId, supplierStatusId: supplierStatusId, description: $('#txtReason').val() },
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
                    }
                }
            });
        })

        $('#btnExportExcel').off('click').on('click', function () {
            var supplierSearchDto = {
                Name: $('#txtSupplierName').val(),
                Email: $('#txtSupplierEmail').val(),
                PhoneNumber: $('#txtPhoneNumber').val(),
                SupplierStatusId: $('#ddlSupplierStatusId').val(),
                PageSize: homeconfig.pageSize,
                Page: homeconfig.pageIndex

            };
            $('#txtHiddensearchDto').val(JSON.stringify(supplierSearchDto));
            $('#hiddenExportExcelForm').submit();
            
        });

        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới dịch vụ');
            homeController.resetForm();
            $('#myModal').modal('show');
        });


        $('#btnSearch').off('click').on('click', function () {
            homeconfig.searchBtnClick = true;
            homeconfig.pageIndex = 1;
            homeController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            var name = $(this).data('name');
            $('#lblPopupTitle').text('Cập nhật công ty');
           homeController.resetForm();
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadSupplierInfo(id);
            homeController.loadAllBranches(id);       
            homeController.loadAllServices(id);
           
           
        });

        $('#ddlCurrentStatusId').off('change').on('change', function () {
            var currentId = $(this).val();
            if (currentId == 2) {
                $('#divHidden').css('display', '');
            }
            else {
                $('#divHidden').css('display', 'none');
            }
        });

    },
    deleteEmployee: function (id) {
        $.ajax({
            url: '/Home/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success", function () {
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
        $('#txtCurrentStatusDetail').val('');
        $('#txtReason').val('');
        $('#divHidden').css('display', 'none');

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
            url: '/Admin/Supplier/GetAllSuppliers',
            type: 'POST',
            dataType: 'json',
            async: false,
            data: supplierSearchDto,
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SupplierId: item.SupplierId,
                            Name: item.Name,
                            SupplierStatus: item.SupplierStatus,
                            Email: item.Email,
                            PhoneNumber: item.PhoneNumber,
                            CreatedDate: item.CreatedDate
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
                if (homeconfig.searchBtnClick != true)
                {
                    setTimeout(callback, 200);
                }
                
                
            }
        });
    },
    loadSupplierStatus: function () {
        $.ajax({
            url: '/Admin/Supplier/GetAllSupplierStatus',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.success == true) {
                     
                    var html = '<option value="">--Chọn trạng thái--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.Id + '">' + item.Name + '</option>'
                    });
                    $('#ddlSupplierStatusId').html(html);
                    $('#ddlCurrentStatusId').html(html);
                }
            }
        })
    },
    loadSupplierInfo: function (id) {
        $.ajax({
            url: '/Admin/Supplier/GetSupplierInfoById',
            type: "GET",
            data: {
                id: id
            },
            dataType: "json",
            success: function (response) {
                if (response.success == true) {
                    var data = response.data;
                    $('#txtSupplierIdHiddent').val(data.SupplierId);
                    $('#txtNameDetail').val(data.Name);
                    $('#txtEmailDetail').val(data.Email);
                    $('#txtPhoneDetail').val(data.PhoneNumber);
                    $('#txtCreatedDateDetail').val(data.CreatedDate);
                    $('#txtCurrentStatusDetail').val(data.SupplierStatus);
                    if (data.SupplierStatusId == 2) {
                        $('#txtReason').val(data.Description);
                        $('#divHidden').css('display', '');
                    }
                    
                }
            }
        })
    },
    loadAllBranches: function (id) {
        $.ajax({
            url: '/Admin/Supplier/GetSupplierBranchesById',
            type: "GET",
            data: {
                id: id
            },
            dataType: "json",
            success: function (response) {
                if (response.success == true) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template-branch').html();
                    var count = 0;
                    $.each(data, function (i, item) {
                        count++;
                        html += Mustache.render(template, {
                            Index: count,
                            Name: item.Name,
                            Address: item.Address + ", " + item.District + ", " + item.City
                        });

                    });
                   
                    $('#tblDataBranch').html(html);
                }
            }
        })
    },
    loadAllServices: function (id) {
        $.ajax({
            url: '/Admin/Supplier/GetSupplierServiceById',
            type: "GET",
            data: {
                id: id
            },
            dataType: "json",
            success: function (response) {
                if (response.success == true) {
                    var data = response.data;

                    var html = '';
                    var template = $('#data-template-service').html();
                    var count = 0;
                    $.each(data, function (i, item) {
                        count++;
                        html += Mustache.render(template, {
                            Index: count,
                            Name: item.Name,
                            TypeName: item.ServiceType,
                            Status: item.ServiceStatus
                        });

                    });
                    $('#tblDataService').html(html);
                }
            }
        })
    }
}
homeController.init();