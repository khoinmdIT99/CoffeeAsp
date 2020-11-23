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

    $('.gallery-slider').slick({
        infinite: true,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        responsive: [
          {
            breakpoint: 1024,
            settings: {
              slidesToShow: 3,
              slidesToScroll: 1,
              infinite: true,
              dots: true
            }
          },
          {
            breakpoint: 768,
            settings: {
              slidesToShow: 2,
              slidesToScroll: 1
            }
          },
          {
            breakpoint: 540,
            settings: {
              slidesToShow: 1,
              slidesToScroll: 1
            }
          }
          // You can unslick at a given breakpoint now by adding:
          // settings: "unslick"
          // instead of a settings object
        ]
    });

    function scrollAppear(){
        let counter_item = document.querySelector('.counter-section');
        let counter_position = counter_item.getBoundingClientRect().top;
        let screen_position = window.innerHeight/2;

        if(counter_position < screen_position){
            const counters = document.querySelectorAll(".count");
            const speed = 2000;
        
            counters.forEach(counter => {
                const updateCount = () => {
                    const target = +counter.getAttribute('data-target');
                    const count = +counter.innerHTML;
                    const inc = target/speed;
                    if(count < target){
                        counter.innerHTML = Math.ceil(count + inc);
                        setTimeout(updateCount, 1);
                    }else{
                        count.innerHTML = target;
                    }
                    console.log(count);
                }
                updateCount();
            });
        };
    }

    window.addEventListener('scroll', scrollAppear);
    
    $('.worker-slider').slick({
        infinite: true,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        responsive: [
          {
            breakpoint: 1024,
            settings: {
              slidesToShow: 3,
              slidesToScroll: 1,
              infinite: true,
            }
          },
          {
            breakpoint: 768,
            settings: {
              slidesToShow: 2,
              slidesToScroll: 1
            }
          },
          {
            breakpoint: 540,
            settings: {
              slidesToShow: 1,
              slidesToScroll: 1
            }
          }
          // You can unslick at a given breakpoint now by adding:
          // settings: "unslick"
          // instead of a settings object
        ]
    });
})