"use strict"

$(".remove-btn").click(function (e) {
    e.preventDefault();
    
    var that = $(this);
    $.ajax({
        url: $(this).attr("href"),
        method: "GET",
        success: function (res) {
            $(that).parents("tr").first().remove();
            UpdateBasket(res.data);
            $(".basket-count").text(res.data);
            $(".total-product").text("$" + res.totalProduct);
        }
    })
})