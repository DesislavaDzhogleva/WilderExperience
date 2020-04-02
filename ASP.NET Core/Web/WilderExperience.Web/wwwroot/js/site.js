// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//$(function () {
//    let nav = $(".navbar");
//    $(window).scroll(function () {
//        var scroll = $(window).scrollTop();

//        if (scroll >= nav.height()) {
//            nav.addClass('nav-scrolled').addClass('fixed-top');
//        } else {
//            nav.removeClass('nav-scrolled').removeClass('fixed-top');
//        }
//    });
//});

//$(function () {
//    $("#mdb-lightbox-ui").load("mdb-addons/mdb-lightbox-ui.html");
//});
ClassicEditor
    .create(document.querySelector('#editor'), {height:400})
    .catch(error => {
    });

$(document).ready(function () {
    $("#lightgallery").lightGallery();

    $(document).on("click", ".ajax-submit-btn", function (evt) {
        evt.preventDefault();
        let form = $(this).closest("form");

        if ($(this).hasClass("ajax-submit-btn-collapsable")) {
            form.toggle();
        }
        $.post(form.attr("action"), form.serialize(), function (data) {
            console.log(data);
            $(data).insertAfter(form);
        });
    });

});

