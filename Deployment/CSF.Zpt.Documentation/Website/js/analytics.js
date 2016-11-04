define(["ga", "jquery"], function(ga, $) {
  "use strict";
  
  function initialiseAnalytics()
  {
    recordInitialPageview();
    setupOutboundLinkTracking();
  }
  
  function recordInitialPageview()
  {
    ga("send", "pageview");
  }
  
  function setupOutboundLinkTracking()
  {
    $(function() {
      $("a[href^='http://'],a[href^='https://']")
        .on("click", function(ev) {
          var
            $this = $(this),
            url = $this.attr("href");
          
          ga("send", "event", "outbound", "click", url, {
            "transport": "beacon",
            "hitCallback": function() {
              document.location = url;
            }
          });
        });
    });
  }

  return {
    initialise: initialiseAnalytics,
  };
});