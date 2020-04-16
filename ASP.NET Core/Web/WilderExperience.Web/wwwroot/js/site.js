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

    $('#myModal').on('shown.bs.modal', function () {
        $('#myInput').trigger('focus')
    })

    $(document).on("click", ".ajax-submit-btn", function (evt) {
        evt.preventDefault();
        let form = $(this).closest("form");
        
        if ($(this).hasClass("ajax-submit-btn-collapsable")) {
            form.toggle();
        }
        $.post(form.attr("action"), form.serialize(), function (data) {
            $(data).insertAfter(form);
        });
        $(form).find("textarea").each(function () {
            $(this).val('');
        });
    });


    $('.location-search').select2({
        width: 'resolve', // need to override the changed default
        height: 'resolve',
        ajax: {
            url: '/Locations/Search',
            dataType: 'json',
            minimumInputLength: 3,
            processResults: function (data) {
                for (let i = 0; i < data.length; i++) {
                    data[i].text = data[i].name;
                }
                // Transforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: data
                };
            }
        }
    });


    // rating

    $(".newRating").mouseout(function () {
        $(this).children().each(function () {
            $(this).find("span").addClass("fa-star-o").removeClass("fa-star").removeClass("text-warning");
        });
        console.log("a");
    });
    $(".newRating a").mouseover(function () {
        let dataScore = parseInt($(this).attr("data-score"));
        $(this).parent().children().each(function () {
            let currentDataScore = parseInt($(this).attr("data-score"));
            if (currentDataScore <= dataScore) {
                $(this).find("span").removeClass("fa-star-o").addClass("fa-star").addClass("text-warning");
            } else {
                $(this).find("span").addClass("fa-star-o").removeClass("fa-star").removeClass("text-warning");
            }

        });
    });
});



