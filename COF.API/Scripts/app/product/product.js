var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allShops: [],
    allSizes : []
};

var productController = {

    run: function () {
        homeconfig.allShops = allShop;
        homeconfig.shopId = allShop[0].Id;
        productController.loadData(homeconfig.shopId);

        productController.loadCategories(homeconfig.shopId);
        productController.getAllSizes();
        productController.registerEvent();
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
                Description: $('#txtDescription').val()
            };
            console.log(data);
            productController.saveData(data);
        });

        $('#txtKeySearch').off('keypress').on('keypress', function (e) {
            if (e.keyCode == 13) {
                productController.loadData(homeconfig.shopId);
            }           
        });

        $('#btnSearch').off('click').on('click', function () {
            productController.loadData(homeconfig.shopId);
        });    

        $('#btnSaveDetailPrice').off('click').on('click', function () {
            var productSize = {
                Id : 0,
                SizeId: $('#ddlSizes').val(),
                ProductId: $('#txtHiddenId').val(),
                Price: $('#txtProductSizePrice').val()
            };
            productController.saveProductSize(productSize);
        });
    },
    loadData: function (shopId) {
        $.ajax({
            url: '/product/getallproducts',
            type: 'get',
            dataType: 'json',
            data: { keyword: $('#txtKeySearch').val(), shopId: shopId },
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = '';
                    for (var i = 0; i < data.length; i++)
                    {
                        var category = data[i];
                        html += '<tr>';
                        html += '<td colspan="5" style="background-color:beige"><b>' + category.Name.toUpperCase() + '</b> - ' + category.Products.length  +' sản phẩm </td>';
                        html += '/<tr>';

                        html += '<tr style="background-color:#ddd">';
                        html += '<td><b>Tên sản phẩm</b></td>';
                        html += '<td><b>Trạng thái </b></td>';
                        html += '<td><b>Size</b></td>';
                        html += '<td><b>Giá tiền </b></td>';

                        html += '<td></td>';
                        html += '</tr>';

                        var products = category.Products;
                        $.each(products,function (i, product) {
                            var sizeCount = product.Sizes.length;
                            if (sizeCount > 0) {
                                for (var j = 0; j < sizeCount; j++)
                                {
                                    var size = product.Sizes[j];
                                    html += '<tr>';
                                    if (j === 0) {
                                        html += '<td rowspan=" ' + sizeCount + '" style="text-align:center">' + product.Name.toUpperCase() + '</td>';

                                        if (product.IsActive) {
                                            html += '<td  rowspan=" ' + sizeCount + '"><span class="label label-success">SẴN SÀNG</span></td>';
                                        }
                                        else {
                                            html += '<td  rowspan=" ' + sizeCount + '"><span class="label label-warning">CHƯA SẴN SÀNG</span></td>';
                                        }

                                    }

                                  
                                    html += ' <td>' + size.Size + '</td>';
                                    html += ' <td>' + size.Cost + '</td>';
                                   
                                    
                                    html += '<td>';
                                    html += '<button class="btn btn-primary"><i class="fa fa-edit"></i></button>  &nbsp;';
                                    html += '<button class="btn btn-danger"><i class="fa fa-remove"></i></button>';
                                    html += '</td>';
                                    html += '</tr>';
                                }
                            }
                             else {
                                html += '<tr>';
                                html += '<td style="text-align:center">' + product.Name.toUpperCase() + '</td>';
                         

                                if (product.IsActive) {
                                    html += '<td><span class="label label-success">SẴN SÀNG</span></td>';
                                }
                                else {
                                    html += '<td><span class="label label-warning">CHƯA SẴN SÀNG</span></td>';
                                }
                                html += '<td></td>';
                                html += '<td></td>';
                                html += '<td>';
                                html += '<button class="btn btn-primary"><i class="fa fa-edit"></i></button> &nbsp;';
                                html += '<button class="btn btn-danger"><i class="fa fa-remove"></i></button>';
                                html += '</td>';
                                html += '</tr>';
                            }
                            
                        });
                    }
                    $('#tblData').html(html);
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
                    console.log(data);
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
                url: '/Student/Edit',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.Status) {
                        studentController.loadData(true);
                        $('#btnCancel').click();
                        toastr.success("Edit successfully");
                    } else {
                        toastr.error(res.ErrorMessage);
                    }
                }
            })
        }
    },
    resetParitalView: function () {
        $('#txtHiddenId').val(0);
        $('#txtName').val('');
        $('#txtDescription').val('');
        $('#priceFrm').hide('');
    },
    saveProductSize: function (data) {
        if (data.Id === 0) {
            $.ajax({
                url: '/product/AddProductSize',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.status) {
                        toastr.success(res.message, "Kết quả");
                        productController.loadProductSizes(res.data);
                    } else {
                        toastr.error(res.errorMessage, "Lỗi");
                    }
                }
            });
        } else {
            $.ajax({
                url: '/Student/Edit',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.Status) {
                        studentController.loadData(true);
                        $('#btnCancel').click();
                        toastr.success("Edit successfully");
                    } else {
                        toastr.error(res.ErrorMessage);
                    }
                }
            });
        }
    },
    loadProductSizes: function (sizes) {
        var html = '';
        html + '<tr>';

        html + '<td> # </td>';
        html + '<td>Size</td>';
        html + '<td>Giá</td>';
        html + '<td></td>';
        html + '</tr>';
        $.each(sizes, function (i, item) {
            html + '<tr>';
            html + '<td>' + (i + 1 ) + '</td>';
            html + '<td>' + item.Size + '</td>';
            html + '<td>' + item.Cost + '</td>';
            html + '<td></td>';
            html + '</tr>';

        });
        $('#tblProductSizes').html(html);
    }
};
productController.run();