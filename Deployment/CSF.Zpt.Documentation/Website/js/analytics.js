define(["ga", "jquery", "jscookie"], function(ga, $, cookie) {
  "use strict";
  
  function initialiseAnalytics()
  {
    showCookieMessage();
    recordInitialPageview();
    setupOutboundLinkTracking();
  }
  
  function showCookieMessage()
  {
    var
      lifetimeDays = 730,
      acceptedVal = "cookiesAccepted",
      accepted = cookie.get(acceptedVal),
      isFileUrl = window.location.protocol == "file:";
      
    if(!accepted !isFileUrl)
    {
      var
        statement = $(".cookie_statement"),
        acceptLink = $(".accept", statement);
      
      acceptLink.on("click", function(ev) {
        var $this = $(this);
        cookie.set(acceptedVal, true, { expires: lifetimeDays });
        statement.hide();
      });
      
      statement.show();
    }
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