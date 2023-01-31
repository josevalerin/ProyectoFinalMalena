if ($(".Prodcto-categorias").length > 3) {
    $(".slider").owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 2000,
        autoplayHoverPause: true,
    });

} else {
    $(".slider").owlCarousel({
        loop: false,
        autoplay: false,
    });
}

//$(".carrusel").owlCarousel({
//    loop: true,
//    autoplay: true,
//    autoplayTimeout: 2000,
//    autoplayHoverPause: true,
//});
