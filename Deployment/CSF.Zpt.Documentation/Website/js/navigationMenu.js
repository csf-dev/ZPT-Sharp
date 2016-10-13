define("navigationMenu", ["jquery"], function($) {
  
  return {
    initialise: function() {
      var
        navHamburger = $(".page_header .hamburger_menu"),
        navMenu = $(".main_page_area > nav");
        
      navHamburger.click(function(ev) {
        navMenu.toggleClass("visible");
      });
    }
  };
});