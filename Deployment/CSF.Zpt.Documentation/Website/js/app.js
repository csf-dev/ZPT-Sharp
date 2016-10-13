define(function(require) {
  var $ = require("jquery"),
      highligther = require("highlightCodeBlocks"),
      navMenu = require("navigationMenu");
  
  return {
    start: function() {
      $(function() {
        highligther.initialise();
        navMenu.initialise();
      });
    },
  };
});