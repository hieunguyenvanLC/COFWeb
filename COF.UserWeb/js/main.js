$(document).ready(function () {

    var offset = 200;
    let scrollTo = $(".scroll-to");

    let lastId;
    let cur = [];
    $(window).scroll(function () {
        var pos = $(window).scrollTop();
        var navbar = $('.header');
        var jump = $('.jump-up');
        var menuSection = $(".menu-section");

        if (navbar) {
            if (pos >= 102) {
                navbar.addClass('fixed');
            } else {
                navbar.removeClass('fixed');
            }
        }
        if (jump) {
            if (pos <= $(menuSection).offset().top - offset) {
                $(jump).addClass("_hide");
            } else {
                $(jump).removeClass("_hide");
            }
        }
        var offset2 = 300;

        let fromTop = window.scrollY;
        $(scrollTo).each(function () {
            let section = $(this).attr("target");
            if (
                $(section).offset().top - 200 <= fromTop &&
                $(section).offset().top - 200 + $(section).outerHeight() > fromTop
            ) {
                $(this).addClass("active");
            } else {
                $(this).removeClass("active");
            }
        });
    });
    $(".scroll-to").click(function (e) {
        // e.preventDefault();
        // e.stopPropagation();
        if ($("#header-nav").hasClass("active")) {
            $("#header-nav").removeClass("active");
            $("#close-nav").addClass("_hide");
        }
    });

    // var totalItems = $('.slide-item').length;

    // $('.slide-list').owlCarousel({
    //     items: 1.25,
    //     autoplay: false,
    //     loop: true,
    //     margin: 50,
    //     nav: true,
    //     onTranslate: function () {
    //         var currentIndex = $('.owl-item.active').find(".slide-item").attr("index");
    //         console.log("COUNT" + currentIndex + '/' + totalItems);
    //         $('.slide-count').html('' + currentIndex + '/' + totalItems + '');
    //     }
    // });


    var $tabs = $(".tabs");
    $tabs.each(function () {
        $(this).siblings('.tab-content').addClass("_hide");
        var curentTarget = $(this).find(".active").attr("tab-target");
        $(curentTarget).removeClass("_hide");
    });

    $(".header-tabs").on("click", "li", function (e) {
        if ($("#header-nav").hasClass("active")) {
            $("#header-nav").removeClass("active");
            $("#close-nav").addClass("_hide");
        }
        tabProcess($(this));
    });

    $(".tabs").on("click", "li", function (e) {
        e.preventDefault();
        tabProcess($(this));
    });

    function tabProcess(tab) {
        var target = $(tab).attr("tab-target");
        var $panels = $(target).siblings('.tab-content');
        var latestActive = $(tab).siblings(".item");
        $(latestActive).each(function () {
            if (latestActive.hasClass("active")) {
                $(this).removeClass("active");
            }
        });
        $(tab).addClass("active");

        $panels.each(function () {
            if ($(this) != $(target)) {
                $(this).addClass("_hide");
            }
        });
        $(target).removeClass("_hide");
    }




    $("#close-nav").click(function () {
        $("#header-nav").removeClass("active");
        $(this).addClass("_hide");
    });
    $("#open-nav").click(function () {
        $("#header-nav").addClass("active");

        $("#close-nav").removeClass("_hide");
    });


    $('.dropdown-trigger').click(function (e) {
        var target = $(this).attr('data-target');
        $('.dropdown-content').each(function () {
            var dropdownId = '#' + $(this).attr('id');
            if (dropdownId != target) {
                $(this).removeClass("active");
            }
        });

        $(target).toggleClass("active");

        e.preventDefault();
        e.stopPropagation();
        $(window).click(function (e) {
            if (!$(e.target).closest(target).length) {
                $(target).removeClass("active");
            }
        });
    });

    //MODAL 
    var $modalTrigger = $(".open-modal"),
        $modalClose = $(".close-modal");
    $(document).on("click", ".open-modal", function (e) {
        e.stopPropagation();
        var t = $(this).attr("target");
       
        if(t == "#gallery-modal"){
            var style = $(this).closest('.gallery-figure').css("background-image");
            var src = style.replace("url(\"", "").replace("\")","");
            $(".gallery-img").attr('src', src);
        }
        $(t).addClass("show");

    }), $(document).on("click", ".close-modal", function (e) {
        closeModal()
    }), $(document).on("click", ".modal-content", function (e) {
        e.stopPropagation()
    }), $(document).on("click", ".modal", function (e) {
        closeModal()
    });
    function closeModal() {
        $(".modal").removeClass("show")
    }
});