// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    let nav = $(".navbar");
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();

        if (scroll >= nav.height()) {
            nav.addClass('nav-scrolled');
        } else {
            nav.removeClass('nav-scrolled');
        }
    });
});