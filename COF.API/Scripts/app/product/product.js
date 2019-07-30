var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allShops : []
};

var productController = {

    run: function () {
        homeconfig.allShops = allShop;
        homeconfig.shopId = allShop[0].Id;
        productController.loadData(homeconfig.shopId);

        productController.loadCategories(homeconfig.shopId);
        productController.registerEvent();
    },
    // register event for elements
    registerEvent: function () {
        // event for search button,

        $('.shop-tab').off('click').on('click', function () {
            var dataId = $(this).data('id');
            homeconfig.shopId = dataId;
            productController.loadData(dataId);
            productController.loadCategories(homeconfig.shopId);
        });

        $('#btnAdd').off('click').on('click', function () {
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
    },
    loadData: function (shopId) {
        $.ajax({
            url: '/product/getallproducts',
            type: 'get',
            dataType: 'json',
            data: { shopId: shopId },
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = '';
                    for (var i = 0; i < data.length; i++)
                    {
                        var category = data[i];
                        html += '<tr>';
                        html += '<td colspan="4" style="background-color:beige"><b>' + category.Name.toUpperCase() + '</b> - ' + category.Products.length  +' sản phẩm </td>';
                        html += '/<tr>';

                        html += '<tr style="background-color:#ddd">';
                        html += '<td><b>Tên sản phẩm</b></td>';
                        html += '<td><b>Size</b></td>';
                        html += '<td><b>Giá tiền </b></td>';
                        html += '<td>';
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
        if (data.Id == 0) {
            $.ajax({
                url: '/Student/Create',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.Status) {
                        studentController.loadData(true);
                        $('#btnCancel').click();
                        toastr.success("Create successfully");
                    } else {
                        toastr.error(res.ErrorMessage);
                    }
                }
            })
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
    loadDetail: function (id) {
        $.ajax({
            url: '/Student/Detail/' + id,
            type: 'get',
            dataType: 'json',
            success: function (res) {
                if (res.Status) {
                    var data = res.Data;
                    $('#txtHiddenId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtAge').val(data.Age);
                    $('#txtAddress').val(data.Address);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#txtEmail').val(data.Email);
                    studentController.loadPartialView();
                }
                else {
                    toastr.error(res.ErrorMessage);
                }
            }
        })
    },
    loadPartialView: function () {
        $('#searchListSection').hide();
        $('#createUpdateSection').show();
    },
    loadSearchView: function () {
        $('#searchListSection').show();
        $('#createUpdateSection').hide();
    },
    resetParitalView: function () {
        $('#txtHiddenId').val(0);
        $('#txtName').val('');
        $('#txtAge').val('');
        $('#txtAddress').val('');
        $('#txtPhoneNumber').val('');
        $('#txtEmail').val('');
    }
};
productController.run();