define(function(require) {
  "use strict";
  
  var $ = require("jquery"),
      highlighter = require("highlightCodeBlocks"),
      navMenu = require("navigationMenu"),
      pageDetector = require("pageDetector");
  
  return {
    start: function() {
      $(function() {
        pageDetector.initialiseCurrentPage();
        highlighter.initialise();
        navMenu.initialise();
      });
    },
  };
});