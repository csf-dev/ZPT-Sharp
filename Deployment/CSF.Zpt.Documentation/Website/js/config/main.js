requirejs.config({
  baseUrl: "js",
  paths: {
    jquery: "https://code.jquery.com/jquery-3.1.0.min",
    modernizr: "lib/modernizr.min",
    highlight: "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.4.0/highlight.min"
  },
});

requirejs(["app"], function(app) {
  app.start();
});