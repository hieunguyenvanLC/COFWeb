var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        homeController.loadData();
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

        })



        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới chi nhánh');
            homeController.resetForm();
            $('#myModal').modal('show');
        });


        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });

        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật chi nhánh');           
            homeController.resetForm();
            var id = $(this).data('id');
            homeController.loadDetail(id);
            $('#myModal').modal('show');
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa chi nhánh trên không?", function (result) {
                if (result) {
                    homeController.deleteEmployee(id);
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

       
    },
    deleteEmployee: function (id) {
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
        $('#txtBranchId').val('0');
        $('#txtAddress').val('');
        $('#txtBranchName').val('');
        $('#txtLong').val('');
        $('#txtLad').val('');
        $('#ddlProvince').val(0);
        $('#ddlDistrict').val(0);
        $('#pac-input').val('');
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Supplier/Branch/GetAllBranches',
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
                            BranchId: item.BranchId,
                            Name: item.Name,
                            Address: item.Address + ", " + item.District + ", " + item.City,
                           // Status: (item.ServiceStatusId === 1) ? "<span class=\"label label-success\">Hoạt động</span>" : "<span class=\"label label-danger\">Tạm ngưng</span>"
                            Status: "Hoạt động"
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
        })
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
    }
}
homeController.init();