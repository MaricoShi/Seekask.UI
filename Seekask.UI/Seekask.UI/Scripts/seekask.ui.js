$(document).ready(function () {
    // === sa_content resize === //
    fnWindowsResize();
    function fnWindowsResize() {
        $('.container-fluid', $('.sa_content'))
            .height($(window).height() - 87 - $('.fixed_bottom').outerHeight());
        $('.sa_content').height($('.container-fluid', $('.sa_content')).height()+10 + $('.fixed_bottom').outerHeight());
    }
    $(window).resize(function () {
        fnWindowsResize();
    });

    var $scrollspyObj = $(".widget-box").find('.widget-title').clone()
        .addClass('widget-fixed')
        .appendTo($(".widget-box"));
    $scrollspyObj.hide();
    //获取要定位元素距离浏览器顶部的距离 
    var navH = $(".widget-box").offset().top;
    //滚动条事件 
    $(window).scroll(function () {
        //获取滚动条的滑动距离 
        var scroH = $(this).scrollTop();
        //滚动条的滑动距离大于等于定位元素距离浏览器顶部的距离，就固定，反之就不固定 
        var scroW = $(".widget-box .table").width();
        $scrollspyObj.css('width', scroW);
        if (scroH >= navH - 77) {
            //$scrollspyObj.show();
        } else if (scroH < navH - 77) {
            $scrollspyObj.hide();
        }
    });
    $(window).resize(function () {
        var scroW = $(".widget-box .table").width();
        $scrollspyObj.css('width', scroW);
    });

    $('body').on('click', 'input[type=checkbox].ace+.lbl', function () {
        var $cbi = $(this).find('i');
        if (!$cbi.is('.fa-check-square-o')) {
            $(this).prev('.ace').prop("checked", true);
            $cbi.attr('class', 'fa fa-check-square-o');
        } else {
            $(this).prev('.ace').prop("checked", false);
            $cbi.attr('class', 'fa fa-square-o');
        }
    });

    // === Sidebar navigation === //
    $('.submenu > a').click(function (e) {
        e.preventDefault();
        var submenu = $(this).siblings('ul');
        var li = $(this).parents('li');
        var submenus = $('.sa_sidebar li.submenu ul');
        var submenus_parents = $('.sa_sidebar li.submenu');
        if (li.hasClass('open')) {
            if (($(window).width() > 768) || ($(window).width() < 479)) {
                submenu.slideUp();
            } else {
                submenu.fadeOut(250);
            }
            li.removeClass('open');
        } else {
            if (($(window).width() > 768) || ($(window).width() < 479)) {
                submenus.slideUp();
                submenu.slideDown();
            } else {
                submenus.fadeOut(250);
                submenu.fadeIn(250);
            }
            submenus_parents.removeClass('open');
            li.addClass('open');
        }
        return false;
    });

    // === Tooltips === //
    $('.tip').tooltip();
    $('.tip-left').tooltip({ placement: 'left' });
    $('.tip-right').tooltip({ placement: 'right' });
    $('.tip-top').tooltip({ placement: 'top' });
    $('.tip-bottom').tooltip({ placement: 'bottom' });


});