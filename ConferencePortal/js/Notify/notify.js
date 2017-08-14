Notify = function (text, style, callback, close_callback) {

    var time = '5000';
    var $container = $('#notifications');
    if (style == "success") {
        var icon = '<i class="fa fa-check "></i>';
    }
    else if (style == "danger") {
        var icon = '<i class="fa fa-exclamation-circle "></i>';
    }
    else if (style == "info") {
        var icon = '<i class="fa fa-info-circle "></i>';
    }

    if (typeof style == 'undefined') style = style

    var html = $('<div style="height:80px; width:auto; padding-top:25px;" class="alert alert-' + style + '  hide">' + icon + " " + text + '</div>');

    //$('<a>', {
    //    text: '×',
    //    class: 'button close',
    //    style: 'padding-left: 10px;',
    //    href: '#',
    //    click: function (e) {
    //        e.preventDefault()
    //        close_callback && close_callback()
    //        remove_notice()
    //    }
    //}).prependTo(html)

    $container.prepend(html)
    html.removeClass('hide').hide().fadeIn('slow')

    function remove_notice() {
        html.stop().fadeOut('slow').remove()
    }

    var timer = setInterval(remove_notice, time);

    $(html).hover(function () {
        clearInterval(timer);
    }, function () {
        timer = setInterval(remove_notice, time);
    });

    //html.on('click', function () {
    //    clearInterval(timer)
    //    callback && callback()
    //    remove_notice()
    //});
}