define(["jquery"], function($) {
  "use strict";
  
  var collapsedClass = "collapsed", pageNav, toggleButton;

  function init()
  {
    pageNav = $("nav#page_navigation");
    
    if(pageNav.length == 0)
    {
      pageNav = null;
      return;
    }
    
    toggleButton = createToggle(pageNav);
    
    pageNav.prepend(toggleButton);
  }
  
  function createToggle(pageNavElement)
  {
    var toggle = $('<button class="toggle" title="Toggle visibility of the page contents">Hide</button>');
    
    toggle.click(function(ev) {
      if(pageNavElement.hasClass(collapsedClass))
      {
        expandNav();
      }
      else
      {
        collapseNav();
      }
    });
    
    return toggle;
  }
  
  function expandNav()
  {
    if(!pageNav)
    {
      return;
    }
    
    pageNav.removeClass(collapsedClass);
    toggleButton.removeClass(collapsedClass);
    toggleButton.text("Hide");
  }
  
  function collapseNav()
  {
    if(!pageNav)
    {
      return;
    }
    
    pageNav.addClass(collapsedClass);
    toggleButton.addClass(collapsedClass);
    toggleButton.text("Show");
  }

  return {
    initialise: init,
    collapse: collapseNav,
    expand: expandNav,
  };
});