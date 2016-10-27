define("highlightCodeBlocks", ["highlight", "jquery"], function(hljs, $) {
  return {
    initialise: function() {
      $('pre code').each(function(i, block) {
        hljs.highlightBlock(block);
      });
    },
  };
});