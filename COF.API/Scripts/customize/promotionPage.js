var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
};
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
        homeController.loadAllServices();
        homeController.initW2uiGrid();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            w2ui['myGrid'].save();
            w2ui['myGrid'].refresh();
            var promotionCreateDto = {
                PromotionId: $('#txtPromotionId').val(),
                PromotionTile: $('#txtTitile').val(),
                EffectiveStartDate: $('#txtEffectiveStartDate').val(),
                EffectiveEndDate: $('#txtEffectiveEndDate').val(),
                PromotionDetailDto : homeController.getPromotionServiceList()

            }
            if (promotionCreateDto.PromotionDetailDto.length == 0) {
                toastr.error("Bạn phải chọn giảm giá cho dịch vụ");
                return;
            }
            console.log(promotionCreateDto);
            if (promotionCreateDto.PromotionId == 0) {
                $.ajax({
                    url: '/Supplier/Promotion/AddPromotionAsync',
                    type: 'Post',
                    dataType: 'json',
                    data: promotionCreateDto,
                    success: function (res) {
                        if (!res.success) {
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
                    url: '/Supplier/Promotion/UpdatePromotionAsync',
                    type: 'Post',
                    dataType: 'json',
                    data: promotionCreateDto,
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
                })
            }

        })



        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới ưu đãi');
            homeController.resetForm();
            $('#myModal').modal('show');
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
            homeController.resetForm();
            $('#lblPopupTitle').text('Cập nhật ưu đãi');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn xóa đợt giảm giá trên không?", function (result) {
                if (result) {
                    homeController.deletePromotion(id);
                }

            });
        });

        $('#ddlServices').off('change').on('change', function () {

            var value = $(this).val();
            if ($('#row' + value).length) {
                toastr.warning("Không được nhập trùng dịch vụ.");
                return;
            }
            
            var price = $("#ddlServices option[value='" + value + "']").data('price');

            var text = $("#ddlServices option[value='" + value + "']").text();
            var priceWithFormat = $("#ddlServices option[value='" + value + "']").data('displayprice');
            var newRow = $('#template-row').clone();
            $(newRow).addClass('data-row');
            $(newRow).attr('id', 'row' + value);
            $(newRow).data('id',value);
            $(newRow).find('.colCode').text(value);
            $(newRow).find('.colCurrentPrice').text(priceWithFormat);
            $(newRow).find('.colName').text(text);
            $(newRow).find('.txtDiscount').data('price', price);
            $(newRow).insertAfter('#template-row');
            $(newRow).removeAttr('style');
            $(newRow).find('.txtDiscount').off('change').on('change', function () {
                $.ajax({
                    url: '/Supplier/Promotion/CalculatePromotionPrice',
                    type: 'get',
                    data: {
                        currentPrice: $(this).data('price'),
                        discountPercent: $(this).val()
                    },
                    success: function (res) {
                        $(newRow).find('.colAfterDiscount').text(res);
                    }
                });
            });

            $(newRow).find('.btn-removeDetail').off('click').on('click', function () {
                $(newRow).remove();
            });
            
            
          
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
            url: '/Supplier/Promotion/GetPromotionById',
            data: {
                promotionId: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    $('#txtPromotionId').val(data.PromotionId);
                    $('#txtTitile').val(data.PromotionTile);
                    $('#txtEffectiveStartDate').val(data.EffectiveStartDate);
                    $('#txtEffectiveEndDate').val(data.EffectiveEndDate);
                    var records = data.PromotionDetailDto;
                    $('.data-row').remove();
                    $.each(records, function (i, item) {
                        var newRow = $('#template-row').clone();
                        $(newRow).addClass('data-row');
                        $(newRow).attr('id', 'row' + item.ServiceId);
                        $(newRow).data('id', item.ServiceId);
                        $(newRow).find('.colCode').text(item.ServiceId);
                        $(newRow).find('.colCurrentPrice').text(item.OriginalPricePriceDisplay);
                        $(newRow).find('.colName').text(item.ServiceName);
                        $(newRow).find('.txtDiscount').data('price', item.OriginalPrice);
                        
                        $(newRow).insertAfter('#template-row');
                        $(newRow).removeAttr('style');
                        $(newRow).find('.txtDiscount').off('change').on('change', function () {
                            $.ajax({
                                url: '/Supplier/Promotion/CalculatePromotionPrice',
                                type: 'get',
                                data: {
                                    currentPrice: $(this).data('price'),
                                    discountPercent: $(this).val()
                                },
                                success: function (res) {
                                    $(newRow).find('.colAfterDiscount').text(res);
                                }
                            });
                        });
                        $(newRow).find('.txtDiscount').val(item.PromotionPercent);
                        $(newRow).find('.colAfterDiscount').text(item.PromotionPriceDisplay);
                        $(newRow).find('.btn-removeDetail').off('click').on('click', function () {
                            $(newRow).remove();
                        });

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
        $('#txtPromotionId').val('0');
        $('#txtTitile').val('');
        $('#txtEffectiveStartDate').val('');
        $('#txtEffectiveEndDate').val('');
        $('.data-row').remove();
        $('#txtDescription').val('');
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Supplier/Promotion/GetAllPromtion',
            type: 'POST',
            dataType: 'json',
            data: {
                searchDto: {
                    PromotionName: "",
                    page: homeconfig.pageIndex, pageSize: homeconfig.pageSize
                }
            },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            PromotionId: item.PromotionId,
                            Title: item.Title,
                            EffectiveStartDate: item.EffectiveStartDate,
                            EffectiveEndDate: item.EffectiveEndDate,
                            Description: item.Description
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
    loadAllServices: function () {
        $.ajax({
            url: '/Supplier/Service/GetAllServices',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '<option value="0">-- Chọn dịch vụ --</option>';
                    $.each(data, function (i, item) {
                        html += "<option value='" + item.ServiceId + "' data-price='" + item.Price + "' data-displayprice='" + item.PriceDisplay+  "'>" + item.Name +  "</option>'";
                    });
                    $('#ddlServices').html(html);
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
    initW2uiGrid: function () {
        if (w2ui['myGrid'] == null) {
            $('#myGrid').w2grid({
                name: 'myGrid',
                recordHeight: 40,
                header: 'Test',
                columns: [
                    { field: 'recid', caption: 'ID', size: '60px', attr: 'align=center' ,},
                    { field: 'Index', caption: '#', size: '30px' },
                    { field: 'ServiceName', caption: 'Tên', size: '150px', sizeCorrected : 15 },
                    { field: 'OriginalPrice', caption: 'Giá', size: '90px' },
                    { field: 'PromotionPercent', caption: '% giảm', size: '80px', editable: { type: 'text' } },
                    { field: 'PromotionPrice', caption: 'Giảm còn ', size: '90px' },
                    { field: 'Description', caption: 'Mô tả thêm', size: '200px', editable: { type: 'text' } },
                    { field: 'Delete', caption: 'Xóa', size: '40px'},
                ], onEditField: function (event) {
                    w2ui['myGrid'].save();                 
                    w2ui['myGrid'].refresh();
                    homeController.registerDeleteEvent();
                }
               
            });
            w2ui['myGrid'].hideColumn('recid');
        }


    },
    registerDeleteEvent: function () {
        $('.btnDeletew2ui').off('click').on('click', function () {
            var id = $(this).data('id');
            var records = w2ui['myGrid'].records;
            var tempdata = [];
            $.each(records, function (i, item) {
                console.log(item);
                if (item.ServiceId != id) {
                    tempdata.push(item);
                }
            });
            w2ui['myGrid'].records = tempdata;
            w2ui['myGrid'].refresh();
            if (tempdata.length > 0) {
                w2ui['myGrid'].records = tempdata;
                for (var i = 0; i < w2ui['myGrid'].records.length; i++) {
                    w2ui['myGrid'].records[i].Index = i + 1;
                }
                w2ui['myGrid'].refresh();

                homeController.registerDeleteEvent();
            }
            else {
                $('#grid').prop('hidden', true);
                w2ui['myGrid'].refresh();
            }
            

        });
    },
    getPromotionServiceList: function () {
        var records = [];
        var dataRows = $('.data-row');
        $.each(dataRows, function (i, item) {
            var data = {};
            data.OriginalPrice = $("#ddlServices option[value='" + $(item).data('id') + "']").data('price');
            data.PromotionPercent = $(item).find('.txtDiscount').val();
            data.ServiceId = $(item).data('id');
            records.push(data);
        });
        return records;
    },
    deletePromotion: function (id) {
        $.ajax({
            url: '/Supplier/Promotion/DeletePromotion',
            type: 'Post',
            dataType: 'json',
            data: {
                promotionId: id
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
                    homeController.loadData(true);                  
                }
            }
        });

    }
}
homeController.init();
