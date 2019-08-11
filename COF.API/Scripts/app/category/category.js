var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allShops: [],
    allCategories : []
};

var categoryController = {

    run: function () {
        homeconfig.allShops = allShop;
        homeconfig.shopId = allShop[0].Id;
        categoryController.loadData(homeconfig.shopId);
        categoryController.registerEvent();
    },
    // register event for elements
    registerEvent: function () {
        // event for search button,

        $('.shop-tab').off('click').on('click', function () {
            var dataId = $(this).data('id');
            homeconfig.shopId = dataId;
            categoryController.loadData(homeconfig.shopId);
        });

        $('#btnAdd').off('click').on('click', function () {
            categoryController.resetParitalView();
            var shop = homeconfig.allShops.filter(x => x.Id === homeconfig.shopId);
            var title = shop[0].Name + ' -  Tạo mới danh mục';
            $('#lblTitle').text(title);
            $('#addOrUpdateCateogoryModal').modal('show');
        });

   
        $('#btnSave').off('click').on('click', function () {
            var data = {
                Id: parseInt($('#txtHiddenId').val()),
                ShopId: homeconfig.shopId,
                Name: $('#txtName').val().trim()
            };
            categoryController.saveData(data);
        });

        $('#txtKeySearch').off('keypress').on('keypress', function (e) {
            if (e.keyCode === 13) {
                categoryController.loadData(homeconfig.shopId);
            }
        });

        $('#btnSearch').off('click').on('click', function () {
            categoryController.loadData(homeconfig.shopId);
        });

        $('#btnResetFilter').off('click').on('click', function () {
            $('#txtKeySearch').val('');
            categoryController.loadData(homeconfig.shopId);
        });
 
   
        $('.btnEdit').off('click').on('click', function () {

            categoryController.resetParitalView();
            var shop = homeconfig.allShops.filter(x => x.Id === homeconfig.shopId);
            var title = shop[0].Name + ' - Chỉnh sửa danh mục';
            $('#lblTitle').text(title);
            var id = $(this).data('id');

            var category = homeconfig.allCategories.filter(x => x.Id === id)[0];
            console.log(category);
            categoryController.loadDetail(category);
            $('#addOrUpdateCateogoryModal').modal('show');

        });
    },
    loadData: function (shopId) {
        homeconfig.allCategories = [];
        $.ajax({
            url: '/category/GetAllCategories',
            type: 'get',
            dataType: 'json',
            data: { keyword: $('#txtKeySearch').val(), shopId: shopId },
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = '';
                    html += '<tr style="background-color:#ddd">';
                    html += '<td><b>#</b></td>';
                    html += '<td><b>Tên danh mục</b></td>';

                    if (isAdmin == 'True') {
                        html += '<td><b>Thao tác</b></td>';
                    }
                    
                    html += '</tr>';
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            var category = data[i];


                            html += ' <td>' + (i + 1) + '</td>';
                            html += ' <td>' + (category.Name) + '</td>';

                            if (isAdmin == 'True') {
                                html += '<td>';
                                html += '<button data-id="' + category.Id + '" class="btn btn-primary btnEdit"><i class="fa fa-edit"></i></button> &nbsp;';
                                html += '<button  data-id="' + category.Id + '"  class="btn btn-danger btnRemove"><i class="fa fa-remove"></i></button>';
                                html += '</td>';
                            }
                            html += '</tr>';
                        }
                    }
                    else {
                        html += '<tr>';
                        html += '<td colspan="3">Không tìm thấy kết quả nào</.td>';
                        html += '</tr>';
                    }
                    homeconfig.allCategories = data;
                    $('#tblData').html(html);
                    categoryController.registerEvent();
                }
            }
        });

    },
    saveData: function (data) {
        if (data.Id === 0) {
            $.ajax({
                url: '/category/addcategory',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.status) {
                        $('#addOrUpdateCateogoryModal').modal('hide');
                        toastr.success(res.message, "Kết quả");
                        categoryController.loadData(homeconfig.shopId);

                    } else {
                        toastr.error(res.errorMessage, "Lỗi");
                    }
                }
            });
        } else {
            $.ajax({
                url: '/category/updatecategory',
                type: 'post',
                dataType: 'json',
                data: { model: data },
                success: function (res) {
                    if (res.status) {
                        $('#addOrUpdateCateogoryModal').modal('hide');
                        toastr.success(res.message, "Kết quả");
                        categoryController.loadData(homeconfig.shopId);
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
    },
    loadDetail: function (row) {
        $('#txtHiddenId').val(row.Id);
        $('#txtName').val(row.Name.trim());
    }
};
categoryController.run();