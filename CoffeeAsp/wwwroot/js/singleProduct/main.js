"use strict"

$(document).ready(function(){
  window.onload = function(){
    let preloader = document.getElementById("preloader");
    preloader.style.opacity = "0";
    preloader.style.visibility = "hidden";  
  }

    $(window).on('scroll', function(){
      if($(window).scrollTop()){
        $('header').addClass('sticky-nav-menu');
      }
      else{
        $('header').removeClass('sticky-nav-menu');
      }
    })

    $('#top-btn').fadeOut();

    $(window).on('scroll', function(){
      if($(this).scrollTop()>40){
        $('#top-btn').fadeIn();
      }else{
        $('#top-btn').fadeOut();
      }
    })

    $('#top-btn').click(function(){
      $('html, body').animate({scrollTop:0}, 800);  
    })

    $('.resp-li').click(function(){
      $(this).toggleClass('active-nav-li')
    })

    $('.search-btn').click(function(){
        $('.search-area').addClass('active-search');
    })

    $('.search-close-btn').click(function(){
        $('.search-area').removeClass('active-search');
    })

    $('.side-btn').click(function(){
        $('.side').addClass('active-side');
        $('.full-screen-bg').addClass('active-full-screen-bg');
    })

    $('.side-close-btn').click(function(){
        $('.side').removeClass('active-side');
        $('.full-screen-bg').removeClass('active-full-screen-bg');
    })
    
    $('.full-screen-bg').click(function(){
        $('.side').removeClass('active-side');
        $(this).removeClass('active-full-screen-bg');
    })

    $('.responsive-bar').click(function(){
        $('.responsive-menu').toggleClass('active-responsice-menu');
    })

    $(".add-product").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: $(this).attr("href"),
            method: "Get",
            success: function (res) {
                if (res.status == 200) {
                    $("#basket-count").html(res.data);
                    //toastr["success"]("Dear " + "Customer " + res.message);
                    //Toastr();
                }
            }
        })
    })

    function Toastr() {
        toastr.options = {
            "closeButton": true,
            "positionClass": "toast-top-right",
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    }
})