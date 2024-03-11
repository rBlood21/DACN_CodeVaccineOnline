
    jQuery(function ($) {
        $(document).ready(function () {

            $(window).scroll(function () {
                if ($(this).scrollTop()) {
                    $('#backTop').fadeIn();
                } else {
                    $('#backTop').fadeOut();
                }
            });
            $("#backTop").click(function () {
                $('html, body').animate({
                    scrollTop: 0
                }, 500);
            });
        });
    });
