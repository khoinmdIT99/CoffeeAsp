"use strict"

$(".add-ajax-product").click(function (e) {
    e.preventDefault();

    $.ajax({
        url: $(this).attr("href"),
        method: "GET",
        success: function (res) {
            UpdateBasket(res.data);
            $(".basket-count").text(res.data);
        }
    })
})