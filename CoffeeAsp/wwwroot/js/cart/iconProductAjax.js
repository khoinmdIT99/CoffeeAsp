"use strict"


$(".plus-product").click(function (e) {
    e.preventDefault();
    var that = $(this);
    $.ajax({
        url: $(this).attr("href"),
        method: "GET",
        success: function (res) {
            UpdateBasket(res.data);
            $(".basket-count").text(res.data);
            $(that).prev(".basket-quantity").text(res.productQuantity);

            $(that).closest("td").next(".multiple-product").text("$" + res.multipleProduct);

            $(".total-product").text("$" + res.totalProduct);
        }
    })
    console.log("salam")
})

$(".minus-product").click(function (e) {
    e.preventDefault();
    var that = $(this);
    $.ajax({
        url: $(this).attr("href"),
        method: "GET",
        success: function (res) {
            UpdateBasket(res.data);
            $(".basket-count").text(res.data);
            $(that).next(".basket-quantity").text(res.productQuantity);

            $(that).closest("td").next(".multiple-product").text("$" + res.multipleProduct);

            $(".total-product").text("$" + res.totalProduct);
        }
    })
})