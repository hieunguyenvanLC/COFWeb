var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    searchBtnClick: true
};
var homeController = {
    init: function () {
        homeController.loadSupplierAccount(accountDtoData.data);
        //homeController.loadAllBranches();
        //homeController.loadSupplierDetail();
        homeController.registerEvent();

    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var branchCreateDto = {
                SupplierId: $('#txtSupplierId').val(),
                Name: $('#txtSupplierName').val(),
                Email: $('#txtEmail').val(),
                SupplierStatusId: $('#ddlSupplierStatusId').val(),
                MainBranchId: $('#ddlBranch').val(),
                PhoneNumber: $('#txtPhoneNumber').val()
            }
            console.log(branchCreateDto);
            $.ajax({
                url: '/Supplier/Account/UpdateSupplier',
                type: 'Post',
                dataType: 'json',
                data: branchCreateDto,
                success: function (res) {
                    if (!res.success) {
                        if (res.validation && res.validation.Errors) {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }
                    }
                    else {
                        homeController.loadSupplierDetail();
                        toastr.success("Cập nhật thành công.");
                        
                    }
                }
            })


        });
        $('#inputImage').off('change').on('change', function () {
            // Checking whether FormData is available in browser
            if (window.FormData !== undefined) {

                var fileUpload = $("#inputImage").get(0);
                var files = fileUpload.files;

                // Create FormData object
                var fileData = new FormData();

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }

                // Adding one more key to FormData object


                $.ajax({
                    url: '/Supplier/Account/UploadLogo',
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {
                        if (result.success == true) {
                            var url = result.url;
                            $('#bigView').css('display', '');
                            $('#bigView').attr('src', url);
                            $('#smallView').attr('src', url);
                        }
                        else {
                            $('#lblUploadAlert').text('')
                        }
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            } else {
                alert("FormData is not supported.");
            }
        });
        $('#btnUpdateAvatar').off('click').on('click', function () {
            var imageUrl = $('#bigView').attr('src');
            $.ajax({
                url: '/Supplier/Account/UpdateAvatar',
                type: 'POST',
                dataType: 'json',
                data: { imageUrl: imageUrl },
                success: function (response) {
                    if (response.success) {
                        homeController.loadSupplierDetail();
                        toastr.success("Cập nhật logo thành công.");
                        $('#logoImage').attr('src', imageUrl);
                    }
                    else {
                        toastr.success("Cập nhật logo không thành công.");
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        });
        $('#btnUpdatePaymentEmail').off('click').on('click', function () {
            $('#txtUsername').val('');
            $('#txtPassword').val('');
            $('#passwordConfrim').modal('show');
        });
        $('#btnConfirmChangeEmail').off('click').on('click', function () {
            var username = $('#txtUsername').val();
            var password = $('#txtPassword').val();
            var email = $('#txtPaymentEmail').val();
            var dto = {
                username: username,
                password: password,
                paymentEmail: email
            };
            $.ajax({
                url: '/Supplier/Account/UpdatePaymentEmail',
                type: 'POST',
                dataType: 'json',
                data: { paymentEmailDto: dto },
                success: function (response) {
                    if (response.success) {
                        homeController.loadSupplierDetail();
                        toastr.success("Cập nhật email Ngân Lượng thành công.");
                        $('#passwordConfrim').modal('hide');
                    }
                    else {
                        if (response.validation && response.validation.Errors) {
                            toastr.error(response.validation.Errors[0].ErrorMessage);
                        }
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        });
    },

    loadSupplierDetail: function () {
        $.ajax({
            url: '/Supplier/Account/GetSupplierInfo',
           
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    $('#txtSupplierName').val(data.supplier.Name);
                    $('#txtEmail').val(data.supplier.Email);
                    $('#txtSupplierId').val(data.supplier.SupplierId);
                    $('#txtCurrentStatus').val(data.supplier.SupplierStatusName);
                    $('#txtPhoneNumber').val(data.supplier.PhoneNumber);
                    if (!data.supplier.Avatar) {
                        $('#txtAvatarMessage').html("<b>Bạn nên chọn logo cho công ty mình</b>");
                        $('#bigView').css('display', 'none');
                    } else {
                        $('#bigView').css('display', '');
                        $('#bigView').attr('src', data.supplier.Avatar);
                    }
                    $('#txtPaymentEmail').val(data.supplier.PaymentEmail);
                    if (data.supplier.SupplierStatusId == 2 && !data.supplier.LastUpdateBySupplier) {
                        $('#txtReason').css('display', '');
                        $('#txtReason').html("<b>Bị tạm ngưng vì lí do sau: </b><br> <br>" + data.supplier.Description);
                    }

                    var branchHtml = '<option value="0">--Chọn chi nhánh chính--</option>';;
                    $.each(data.branches, function (i, item) {
                        branchHtml += '<option value="' + item.BranchId + '">' + item.Name + '</option>'
                    });
                    $('#ddlBranch').html(branchHtml);
                    $('#ddlBranch').val(data.supplier.MainBranchId).change();
                    var allStatus = data.supplierStatus;
                    var html = '<option value="">--Chọn trạng thái--</option>';;
                    $.each(allStatus, function (i, item) {
                        html += '<option value="' + item.SupplierStatusId + '">' + item.Name + '</option>'
                    });
                    $('#ddlSupplierStatusId').html(html);
                    
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
    loadAllBranches: function () {
        $.ajax({
            url: '/Supplier/Branch/GetAllBranchAsync',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '<option value="0">--Chọn chi nhánh chính--</option>';;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.BranchId + '">' + item.Name + '</option>'
                    });
                    $('#ddlBranch').html(html);
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
        $('#txtPhoneDetail').val('');
        $('#txtEmailDetail').val('');
        $('#txtCreatedDateDetail').val('');
        $('#txtCurrentStatusDetail').val('');
        $('#txtReason').val('');
        $('#divHidden').css('display', 'none');
        $('#txtSupplierNoteDetail').val('');
        $('#txtTotalMoneyDetail').prop('readonly', true);
    },
    loadSupplierAccount: function (data) {
        $('#txtSupplierName').val(data.supplier.Name);
        $('#txtEmail').val(data.supplier.Email);
        $('#txtSupplierId').val(data.supplier.SupplierId);
        $('#txtCurrentStatus').val(data.supplier.SupplierStatusName);
        $('#txtPhoneNumber').val(data.supplier.PhoneNumber);
        if (!data.supplier.Avatar) {
            $('#txtAvatarMessage').html("<b>Bạn nên chọn logo cho công ty mình</b>");
            $('#bigView').css('display', 'none');
        } else {
            $('#bigView').css('display', '');
            $('#bigView').attr('src', data.supplier.Avatar);
        }
        $('#txtPaymentEmail').val(data.supplier.PaymentEmail);
        if (data.supplier.SupplierStatusId == 2 && !data.supplier.LastUpdateBySupplier) {
            $('#txtReason').css('display', '');
            $('#txtReason').html("<b>Bị tạm ngưng vì lí do sau: </b><br> <br>" + data.supplier.Description);
        }

        var branchHtml = '<option value="0">--Chọn chi nhánh chính--</option>';;
        $.each(data.branches, function (i, item) {
            branchHtml += '<option value="' + item.BranchId + '">' + item.Name + '</option>'
        });
        $('#ddlBranch').html(branchHtml);
        $('#ddlBranch').val(data.supplier.MainBranchId).change();
        var allStatus = data.supplierStatus;
        var html = '<option value="">--Chọn trạng thái--</option>';;
        $.each(allStatus, function (i, item) {
            html += '<option value="' + item.SupplierStatusId + '">' + item.Name + '</option>'
        });
        $('#ddlSupplierStatusId').html(html);
    },
   
}
homeController.init();