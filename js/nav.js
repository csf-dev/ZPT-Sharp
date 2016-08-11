$(window).ready(function() {
  $(".page_header .hamburger_menu").click(function(ev) {
    $(".main_page_area > nav").toggleClass("visible");
  });
});
