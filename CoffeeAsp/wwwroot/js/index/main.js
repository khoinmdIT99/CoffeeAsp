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

    var next = document.getElementsByClassName('next-btn ')[0];
    var prev = document.getElementsByClassName('prev-btn ')[0];
    var slide =[...document.querySelectorAll('.slide')];
    let currentSlide = 0;

    function NextSlide(){
        slide[currentSlide].className = "slide";
        currentSlide = (currentSlide + 1 + slide.length)%slide.length;
        slide[currentSlide].className = "slide active-slide";
    }

    function PrevSlide(){
        slide[currentSlide].className = "slide";
        currentSlide = (currentSlide - 1 + slide.length)%slide.length;
        slide[currentSlide].className = "slide active-slide";
    }

    next.onclick = function(){
        NextSlide()
    }

    prev.onclick = function(){
        PrevSlide();
    }

    setInterval(function(){
        NextSlide();
    },8500);

    $('.brand-slider').slick({
        infinite: true,
        speed: 300,
        slidesToShow: 6,
        autoplay: true,
        autoplaySpeed: 2000,
        responsive: [
          {
            breakpoint: 1024,
            settings: {
              slidesToShow: 4,
              infinite: true,
            }
          },
          {
            breakpoint: 600,
            settings: {
              slidesToShow: 2,
              infinite: true,
            }
          },
          {
            breakpoint: 480,
            settings: {
              slidesToShow: 1,
              infinite: true,

            }
          }
          // You can unslick at a given breakpoint now by adding:
          // settings: "unslick"
          // instead of a settings object
        ]
    });
})