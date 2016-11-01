define(["highlight", "jquery"], function(hljs, $) {
  "use strict";
  
  return {
    initialise: function() {
      $('pre code').each(function(i, block) {
        hljs.highlightBlock(block);
      });
    },
  };
});