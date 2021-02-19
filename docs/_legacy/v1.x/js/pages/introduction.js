define(["jquery"], function($) {
  "use strict";
  
  function performInitialisation()
  {
    $(".main_page_content section.learn_more li")
      .on("click", function(ev) {
        var item = $(ev.delegateTarget);
        var link = $("a", item);
        
        window.location = link.attr("href");
      });
  }
  
  return {
    initialise: performInitialisation,
  };
});