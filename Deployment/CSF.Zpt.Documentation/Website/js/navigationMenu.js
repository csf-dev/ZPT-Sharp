define(["jquery"], function($) {
  "use strict";
  
  return {
    initialise: function() {
      var
        navHamburger = $(".page_header .hamburger_menu"),
        navMenu = $(".main_page_area > nav"),
        body = $("body"),
        navVisible = false,
        navMenuWidth = 298,
        pageWidthThreshold = 1059,
        theWindow = $(window);
        
      navHamburger.click(function(ev) {
        if(!navVisible)
        {
          body.css({ overflow: "hidden" });
          navMenu.css({ display: "block" });
          navMenu.animate({ left: "0px" });
        }
        else
        {
          navMenu.animate({ left: "-" + navMenuWidth + "px" }, { complete: function() {
            navMenu.css({ display: "none" });
            body.css({ overflow: "auto" });
          }});
        }
        
        navVisible = !navVisible;
      });
      
      $("body").on("click", function(ev) {
        var evTarget = $(ev.target);
        if(navVisible
           && !evTarget.is(".main_page_area > nav, .main_page_area > nav *, .page_header .hamburger_menu *"))
        {
          navHamburger.click();
        }
      });
      
      function resetNavMenuForLargeScreens()
      {
        navVisible = false;
        navMenu.css({ display: "block", left: "0" });
        body.css({ overflow: "auto" });
      }
      
      function resetNavMenuForSmallScreens()
      {
        navVisible = false;
        navMenu.css({ display: "none", left: "-" + navMenuWidth + "px" });
        body.css({ overflow: "auto" });
      }
      
      theWindow.resize(function(ev) {
        if(theWindow.width() > pageWidthThreshold)
        {
          resetNavMenuForLargeScreens();
        }
        else
        {
          resetNavMenuForSmallScreens();
        }
      });
    }
  };
});