define(function(require) {
  "use strict";
  
  var $ = require("jquery"),
      highlighter = require("highlightCodeBlocks"),
      navMenu = require("navigationMenu"),
      pageDetector = require("pageDetector"),
      analytics = require("analytics");
  
  return {
    start: function() {
      $(function() {
        analytics.initialise();
        pageDetector.initialiseCurrentPage();
        highlighter.initialise();
        navMenu.initialise();
      });
    },
  };
});