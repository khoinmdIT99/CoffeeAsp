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

    var rangeSlider = document.getElementById("range");
    var output = document.getElementsByClassName("from-to")[0];

    output.innerHTML = "$" + rangeSlider.value;

    rangeSlider.oninput = function(){
      output.innerHTML = "$" + this.value;
    }

    // rangeSlider.addEventListener("mousemove", function(){
    //   var color = 'linear-gradient(90deg, rgb(190, 156, 121)' + rangeSlider.value + '%, rgb(204, 204, 204)' + rangeSlider.value + '%)';
    //   rangeSlider.style.background = color;
    // })

})  