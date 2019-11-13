var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allShops: [],
    allSizes: [],
    allProducts : []
};

var productController = {

    run: function () {
        homeconfig.allShops = allShop;
        homeconfig.shopId = allShop[0].Id;
        productController.loadData(homeconfig.shopId);

        productController.loadCategories(homeconfig.shopId);
        productController.getAllSizes();
        productController.registerEvent();

        $('#txtProductSizePrice').mask('000.000.000.000.000', { reverse: true });

    },
    // register event for elements
    registerEvent: function () {
        // event for search button,

        $('.shop-tab').off('click').on('click', function () {
            var dataId = $(this).data('id');
            homeconfig.shopId = dataId;
            productController.loadData(homeconfig.shopId);
            productController.loadCategories(homeconfig.shopId);
        });

        $('#btnAdd').off('click').on('click', function () {
            productController.resetParitalView();
            var shop = homeconfig.allShops.filter(x => x.Id === homeconfig.shopId);
            var title = shop[0].Name + ' -  Tạo mới sản phẩm';
            $('#lblTitle').text(title);
            $('#createUpdateSection').show();
            $('#tableContent').hide();
        });

        $('#btnCancel').off('click').on('click', function () {
            $('#createUpdateSection').hide();
            $('#tableContent').show();
        });

        $('#btnSave').off('click').on('click', function () {
            var data = {
                Id: parseInt($('#txtHiddenId').val()),
                Name: $('#txtName').val(),
                CategoryId: $('#ddlCategories').val(),
                ShopId: homeconfig.shopId,
                Description: $('#txtDescription').val(),
                IsActive: $('#chkActive').is(":checked"),
                Image: $('#bigView').attr('src')
            };
            productController.saveData(data);
        });

        $('#txtKeySearch').off('keypress').on('keypress', function (e) {
            if (e.keyCode === 13) {
                productController.loadData(homeconfig.shopId);
            }           
        });

        $('#btnResetFilter').off('click').on('click', function () {
            $('#txtKeySearch').val('');
            productController.loadData(homeconfig.shopId);
        });

        $('#btnSearch').off('click').on('click', function () {
            productController.loadData(homeconfig.shopId);
        });    

        $('#btnSaveDetailPrice').off('click').on('click', function () {
            var productSize = {
                Id: $('#txtProductSizeId').val(),
                SizeId: $('#ddlSizes').val(),
                ProductId: $('#txtHiddenId').val(),
                Price: $('#txtProductSizePrice').val().split('.').join('')
            };
            productController.saveProductSize(productSize);
        });

        $('#btnAddProductSize').off('click').on('click', function () {
            productController.clearProductSizeModel();
            $('#addPriceModal').modal('hide');
           
        });

        $('.btnEditProduct').off('click').on('click', function () {

            productController.resetParitalView();
            var shop = homeconfig.allShops.filter(x => x.Id === homeconfig.shopId);
            var title = shop[0].Name + ' - Chỉnh sửa sản phẩm';
            $('#lblTitle').text(title);


            $('#priceFrm').show();

            var id = $(this).data('id');
            productController.loadProductDetail(id);
            $('#priceFrm').show();

            setTimeout(function () {
                $('#createUpdateSection').show();
                $('#tableContent').hide();
            }, 100);
          
        });

        $('.btnEditProductSize').off('click').on('click', function () {
            productController.clearProductSizeModel();
            $('#addPriceModal').modal('show');
            var id = $(this).data('id');
            var sizeId = $(this).data('sizeid');
            var cost = $(this).data('cost');
            
            $('#ddlSizes').val(sizeId).change();
            $('#txtProductSizeId').val(id);
            $('#txtProductSizePrice').val(cost).trigger('keyup');
        });

        $('.btnRemoveProductSize').off('click').on('click', function () {
            var id = $(this).data('id');
            var productId = parseInt($('#txtHiddenId').val());
            bootbox.confirm({
                message: "Bạn có chắc chắn xóa size trên không ",
                buttons: {
                    confirm: {
                        label: 'Có',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'Không',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {
                    if (result) {
                        productController.removeProductSize(productId,id);
                    }
                }
            });


         
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
                    url: '/Common/UploadImage?type=1',
                    type: "POST",
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {
                        if (result.status) {
                            var data = result.data;
       
                            toastr.success("Upload successfully.");
                            $('#bigView').css('display', '');
                            $('#bigView').attr('src', data.Url);
                            $('#smallView').attr('src', data.Url);
                        }
                        else {
                            toastr.error(result.ErrorMessage);
                        }
                    },
                    error: function (err) {
                        toastr.error(err.statusText);
                    }
                });
            } else {
                alert("FormData is not supported.");
            }
        });
    },
    loadData: function (shopId) {
        homeconfig.allProducts = [];
        $.ajax({
            url: '/product/getallproducts',
            type: 'get',
            dataType: 'json',
            data: { keyword: $('#txtKeySearch').val(), shopId: shopId },
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = '';
                    
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            var category = data[i];
                            html += '<tr>';
                            html += '<td colspan="' +( (isAdmin == 'True') ?5 : 4 )+ '" style="background-color:beige;"><b>' + category.Name.toUpperCase() + '</b> - ' + category.Products.length + ' sản phẩm </td>';
                            html += '/<tr>';

                            html += '<tr style="background-color:#ddd">';
                            html += '<td><b>Tên sản phẩm</b></td>';
                            html += '<td><b>Trạng thái </b></td>';
                            html += '<td><b>Size</b></td>';
                            html += '<td><b>Giá tiền </b></td>';

                            if (isAdmin == 'True') {
                                html += '<td><b>Thao tác</b></td>';
                            }

                            
                            html += '</tr>';

                            var products = category.Products;

                            $.each(products, function (i, item) {
                                homeconfig.allProducts.push(item);
                            });



                            $.each(products, function (i, product) {
                                var sizeCount = product.Sizes.length;
                                if (sizeCount > 0) {
                                    for (var j = 0; j < sizeCount; j++) {
                                        var size = product.Sizes[j];
                                        html += '<tr>';
                                        if (j === 0) {
                                            html += '<td rowspan=" ' + sizeCount + '" style="text-align:center;vertical-align: middle;">' + product.Name.toUpperCase() + '</td>';

                                            if (product.IsActive) {
                                                html += '<td  style="text-align:center;vertical-align: middle;" rowspan=" ' + sizeCount + '"><span class="label label-success">SẴN SÀNG</span></td>';
                                            }
                                            else {
                                                html += '<td style="text-align:center;vertical-align: middle;"  rowspan=" ' + sizeCount + '"><span class="label label-warning">CHƯA SẴN SÀNG</span></td>';
                                            }

                                        }


                                        html += ' <td>' + size.Size + '</td>';
                                        html += ' <td>' + productController.formatMoney(size.Cost) + '</td>';

                                        if (j === 0) {

                                            if (isAdmin == 'True') {
                                                html += '<td rowspan=" ' + sizeCount + '">';
                                                html += '<button data-id="' + product.Id + '" class="btn btn-primary btnEditProduct"><i class="fa fa-edit"></i></button> &nbsp;';
                                               // html += '<button  data-id="' + product.Id + '"  class="btn btn-danger btnRemoveProduct"><i class="fa fa-remove"></i></button>';
                                                html += '</td>';
                                            }
                                           
                                        }


                                        html += '</tr>';
                                    }
                                }
                                else {
                                    html += '<tr>';
                                    html += '<td style="text-align:center;vertical-align: middle;">' + product.Name.toUpperCase() + '</td>';


                                    if (product.IsActive) {
                                        html += '<td style="text-align:center;vertical-align: middle;"><span class="label label-success">SẴN SÀNG</span></td>';
                                    }
                                    else {
                                        html += '<td style="text-align:center;vertical-align: middle;"><span class="label label-warning">CHƯA SẴN SÀNG</span></td>';
                                    }
                                    html += '<td></td>';
                                    html += '<td></td>';
                                   

                                    if (isAdmin == 'True') {
                                        html += '<td>';
                                        html += '<button data-id="' + product.Id + '" class="btn btn-primary btnEditProduct"><i class="fa fa-edit"></i></button> &nbsp;';
                                        //html += '<button  data-id="' + product.Id + '"  class="btn btn-danger btnRemoveProduct"><i class="fa fa-remove"></i></button>';
                                        html += '</td>';
                                    }
                                    
                                    
                                    html += '</tr>';
                                }

                            });
                        }                      
                    }
                    else {
                        html += '<tr>';
                        html += '<td style="text-align:left">Không tìm thấy kết quả nào</.td>';
                        html += '</tr>';
                    }
                    $('#tblData').html(html);
                    productController.registerEvent();
                    
                }
            }
        });

    },
    loadCategories: function (shopId) {
        $.ajax({
            url: '/product/getcategories',
            type: 'get',
            dataType: 'json',
            data: { shopId: shopId },
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = '';
                    $.each(data, function (i, item)
                    {
                        html += '<option value="' + item.CategoryId+'">' + item.Name + '</option>';
                    });
                    $('#ddlCategories').html(html);
                }
            }
        });
    },
    getAllSizes: function () {
        $.ajax({
            url: '/common/getallsizes',
            type: 'get',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    homeconfig.allSizes = data;
                    var html = '';
                    html += '<option value="0"> -- Chọn Size -- </option>';
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.Id + '">' + item.Name + '</option>';
                    });
                    $('#ddlSizes').html(html);
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
            visiblePages: 5,
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    removeData: function (id) {
        $.ajax({
            url: '/Student/Delete',
            type: 'post',
            dataType: 'json',
            data: { id: id },
            success: function (res) {
                if (res.Status) {
                    studentController.loadData();
                    toastr.success("Delete successfully");
                }
                else {
                    toastr.error(res.ErrorMessage);
                }
            }
        })
    },
    saveData: function (data) {
        if (data.Id === 0) {
            $.ajax({
                url: '/product/addproduct',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.status) {
                        $('#txtHiddenId').val(res.data.Id);
                        $('#priceFrm').show();
                        productController.loadData(homeconfig.shopId);
                        toastr.success(res.message, "Kết quả");
                    } else {
                        toastr.error(res.errorMessage, "Lỗi");
                    }
                }
            });
        } else {
            $.ajax({
                url: '/product/updateproduct',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.status) {
                        productController.loadData(homeconfig.shopId);
                       // $('#btnCancel').click();
                        toastr.success(res.message, "Kết quả");
                    } else {
                        toastr.error(res.errorMessage, "Lỗi");
                    }
                }
            })
        }
    },
    resetParitalView: function () {
        $('#txtHiddenId').val(0);
        $('#txtName').val('');
        $('#txtDescription').val('');
        $('#priceFrm').hide();
        $('#chkActive').prop('checked', false);
        productController.loadProductSizes([]);
        $('#bigView').show();
        $('#bigView').attr('src', '');
    },
    saveProductSize: function (data) {
        if (data.Id == 0) {
            $.ajax({
                url: '/product/addproductsize',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.status) {
                        $("#btnCancelPrice").click();
                        toastr.success(res.message, "Kết quả");
                        productController.loadData(homeconfig.shopId);
                        productController.loadProductDetail(data.ProductId);
                        
                    } else {
                        toastr.error(res.errorMessage, "Lỗi");
                    }
                }
            });
        } else {
            $.ajax({
                url: '/product/updateproductsize',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.status) {
                        $("#btnCancelPrice").click();
                        toastr.success(res.message, "Kết quả");
                        productController.loadProductDetail(data.ProductId);
                        productController.loadData(homeconfig.shopId);

                    } else {
                        toastr.error(res.errorMessage);
                    }
                }
            });
        }
    },
    loadProductSizes: function (sizes) {
        var html = '';
        html += '<tr>';

        html += '<td> # </td>';
        html += '<td>Size</td>';
        html += '<td>Giá</td>';
        html += '<td></td>';
        html += '</tr>';
        $.each(sizes, function (i, item) {
            html += '<tr>';
            html += '<td>' + (i + 1 ) + '</td>';
            html += '<td>' + item.Size + '</td>';
            html += '<td>' + productController.formatMoney(item.Cost) + '</td>';
            html += '<td>';
            html += '<button data-id="' + item.Id + '" data-cost= "' + item.Cost + '" data-sizeid ="' + item.SizeId + '" class="btn btn-primary btnEditProductSize"><i class="fa fa-edit"></i></button> &nbsp;';
            html += '<button  data-id="' + item.Id + '" class="btn btn-danger btnRemoveProductSize"><i class="fa fa-remove"></i></button>';
            html += '</td > ';
            html += '</tr>';

        });
        $('#tblProductSizes').html(html);
        productController.registerEvent();
       

       
    }
    ,
    clearProductSizeModel: function () {
        $('#ddlSizes').val('0').change();
        $('#txtProductSizeId').val('0');
        $('#txtProductSizePrice').val('');
        
    },
    editProductSize: function () {
        
    },
    removeProductSize: function (productId,id) {
        $.ajax({
            url: '/product/removeProductSize',
            type: 'post',
            dataType: 'json',
            data: { productId: productId, id: id },
            success: function (res) {
                if (res.status) {
                    toastr.success(res.message, "Kết quả");
                    productController.loadProductDetail(productId);
                    productController.loadData(homeconfig.shopId);
                } else {
                    toastr.error(res.errorMessage, "Lỗi");
                }
            }
        });
    },
    loadProductDetail: function (id) {
         productController.loadAllRms(homeconfig.shopId);
         $.ajax({
             url: '/product/getProductDetail',
                    type: 'get',
                    dataType: 'json',
                    data: { productId: id },
                    success: function (res) {
                        if (res.status) {
                            var row = res.data;
                            $('#txtName').val(row.Name);
                            $('#txtHiddenId').val(row.Id);
                            $('#txtDescription').val(row.Description);
                            $('#chkActive').prop('checked', row.IsActive);
                            $('#ddlCategories').val(row.CategoryId).change();
                            $('#bigView').hide();
                            if (row.Image != null) {
                                $('#bigView').show();
                                $('#bigView').attr('src',  row.Image);
                            }
                            
                            productController.loadProductSizes(row.Sizes);
                        } else {
                            toastr.error(res.errorMessage, "Lỗi");
                        }
                    }
                });
        
        
    },
    formatMoney: function (value) {
        var ressult = accounting.formatMoney(value, "", 0, ".", ","); // €4.999,99
        return ressult;
    },
    loadAllRms: function (shopId) {
        $.ajax({
            url: '/rawmaterial/GetAllRmsAsync',
            type: 'get',
            dataType: 'json',
            data: { shopId: shopId },
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    console.log(data);
                } else {
                    toastr.error(res.errorMessage, "Lỗi");
                }
            }
        });
    }
};
productController.run();